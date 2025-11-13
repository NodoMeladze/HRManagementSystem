using HRManagement.WebApp.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HRManagement.WebApp.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ApiService> _logger;
        private readonly string _baseUrl;

        public ApiService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<ApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _baseUrl = configuration["ApiSettings:BaseUrl"]
                ?? throw new InvalidOperationException("API Base URL is not configured");

            var timeout = configuration.GetValue<int>("ApiSettings:Timeout", 30);
            _httpClient.Timeout = TimeSpan.FromSeconds(timeout);
            _httpClient.BaseAddress = new Uri(_baseUrl);
        }

        public void SetAuthToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public void ClearAuthToken()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

        public async Task<TResponse?> GetAsync<TResponse>(string endpoint)
        {
            try
            {
                _logger.LogInformation("GET request to {Endpoint}", endpoint);

                var response = await _httpClient.GetAsync(endpoint);
                return await ProcessResponse<TResponse>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GET request to {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                _logger.LogInformation("POST request to {Endpoint}", endpoint);

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);
                return await ProcessResponse<TResponse>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during POST request to {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            try
            {
                _logger.LogInformation("PUT request to {Endpoint}", endpoint);

                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync(endpoint, content);
                return await ProcessResponse<TResponse>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during PUT request to {Endpoint}", endpoint);
                throw;
            }
        }

        public async Task<TResponse?> DeleteAsync<TResponse>(string endpoint)
        {
            try
            {
                _logger.LogInformation("DELETE request to {Endpoint}", endpoint);

                var response = await _httpClient.DeleteAsync(endpoint);
                return await ProcessResponse<TResponse>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during DELETE request to {Endpoint}", endpoint);
                throw;
            }
        }

        private async Task<TResponse?> ProcessResponse<TResponse>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "API request failed with status {StatusCode}. Response: {Response}",
                    response.StatusCode,
                    content);
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                return default;
            }

            try
            {
                return JsonConvert.DeserializeObject<TResponse>(content);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize response: {Content}", content);
                throw new InvalidOperationException("Failed to parse API response", ex);
            }
        }
    }
}
