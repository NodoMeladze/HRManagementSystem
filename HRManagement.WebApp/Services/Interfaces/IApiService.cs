namespace HRManagement.WebApp.Services.Interfaces
{
    public interface IApiService
    {
        Task<TResponse?> GetAsync<TResponse>(string endpoint);
        Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);
        Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest data);
        Task<TResponse?> DeleteAsync<TResponse>(string endpoint);
        void SetAuthToken(string token);
        void ClearAuthToken();
    }
}
