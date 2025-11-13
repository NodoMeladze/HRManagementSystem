using HRManagement.Web.Constants;
using HRManagement.Web.Models.API;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HRManagement.Web.Services
{
    public class ApiClient(
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor,
        ILogger<ApiClient> logger)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogger<ApiClient> _logger = logger;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new DateOnlyJsonConverter(),
                new NullableDateOnlyJsonConverter()
            }
        };

        private void SetAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Request.Cookies[CookieNames.AuthToken];

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
        {
            return await ExecuteRequestAsync<T>(
                async () => await _httpClient.GetAsync(endpoint),
                "GET",
                endpoint);
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object data)
        {
            var content = CreateJsonContent(data);
            return await ExecuteRequestAsync<T>(
                async () => await _httpClient.PostAsync(endpoint, content),
                "POST",
                endpoint);
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object data)
        {
            var content = CreateJsonContent(data);
            return await ExecuteRequestAsync<T>(
                async () => await _httpClient.PutAsync(endpoint, content),
                "PUT",
                endpoint);
        }

        public async Task<ApiResponse<bool>> DeleteAsync(string endpoint)
        {
            return await ExecuteRequestAsync<bool>(
                async () => await _httpClient.DeleteAsync(endpoint),
                "DELETE",
                endpoint);
        }

        private async Task<ApiResponse<T>> ExecuteRequestAsync<T>(
            Func<Task<HttpResponseMessage>> httpRequest,
            string method,
            string endpoint)
        {
            try
            {
                SetAuthorizationHeader();
                _logger.LogDebug("{Method} request to {Endpoint}", method, endpoint);

                var response = await httpRequest();
                return await HandleResponseAsync<T>(response);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed for {Method} {Endpoint}", method, endpoint);
                return CreateErrorResponse<T>(ConnectionMessages.Error_Connection_Failed);
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Request timeout for {Method} {Endpoint}", method, endpoint);
                return CreateErrorResponse<T>(ConnectionMessages.Error_Timeout);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error for {Method} {Endpoint}", method, endpoint);
                return CreateErrorResponse<T>(ConnectionMessages.Error_Unexpected);
            }
        }

        private static StringContent CreateJsonContent(object data)
        {
            var json = JsonSerializer.Serialize(data, JsonOptions);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        private async Task<ApiResponse<T>> HandleResponseAsync<T>(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("Received 401 Unauthorized response");
                ClearAuthCookies();
                return CreateErrorResponse<T>(ConnectionMessages.Error_Session_Expired);
            }

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("API returned status code {StatusCode}", response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                _logger.LogWarning("API returned empty response");
                return CreateErrorResponse<T>(ConnectionMessages.Error_Empty_Response);
            }

            return DeserializeResponse<T>(content);
        }

        private ApiResponse<T> DeserializeResponse<T>(string content)
        {
            try
            {
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(content, JsonOptions);

                if (apiResponse == null)
                {
                    _logger.LogError("Failed to deserialize API response");
                    return CreateErrorResponse<T>(ConnectionMessages.Error_Invalid_Format);
                }

                return apiResponse;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize response: {Content}",
                    content.Length > 200 ? content[..200] + "..." : content);
                return CreateErrorResponse<T>(ConnectionMessages.Error_Invalid_Format);
            }
        }

        private void ClearAuthCookies()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                httpContext.Response.Cookies.Delete(CookieNames.AuthToken);
                httpContext.Response.Cookies.Delete("UserEmail");
                httpContext.Response.Cookies.Delete("UserName");
                httpContext.Response.Cookies.Delete("UserId");
            }
        }

        private static ApiResponse<T> CreateErrorResponse<T>(string message)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = message
            };
        }
    }

    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
            {
                throw new JsonException("DateOnly value cannot be null or empty");
            }

            return DateOnly.ParseExact(stringValue, Format);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(Format));
        }
    }

    public class NullableDateOnlyJsonConverter : JsonConverter<DateOnly?>
    {
        private const string Format = "yyyy-MM-dd";

        public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            if (string.IsNullOrEmpty(stringValue))
            {
                return null;
            }

            return DateOnly.ParseExact(stringValue, Format);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
        {
            if (value.HasValue)
            {
                writer.WriteStringValue(value.Value.ToString(Format));
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}