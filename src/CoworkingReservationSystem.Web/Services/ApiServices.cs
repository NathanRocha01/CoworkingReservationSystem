using CoworkingReservationSystem.Web.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public interface IApiService
{
    Task<Result<T>> GetAsync<T>(string endpoint);
    Task<Result<T>> PostAsync<T>(string endpoint, object data);
    Task<Result> PostAsync(string endpoint, object data);
    Task<Result<T>> PutAsync<T>(string endpoint, object data);
    Task<Result> DeleteAsync(string endpoint);
    Task<Result<T>> PatchAsync<T>(string endpoint, object data);
    Task<Result> PatchAsync(string endpoint, object data);
    void SetAuthToken(string token);
}

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<T>> GetAsync<T>(string endpoint)
    {
        try
        {
            await SetAuthHeaderAsync();
            var response = await _httpClient.GetAsync(endpoint);
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = JsonSerializer.Deserialize<Result>(content);
                return Result<T>.Failure(errorResult?.Error ?? "Erro desconhecido");
            }

            var data = JsonSerializer.Deserialize<T>(content);
            return Result<T>.Success(data);
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(ex.Message);
        }
    }

    public async Task<Result<T>> PostAsync<T>(string endpoint, object data)
    {
        try
        {
            await SetAuthHeaderAsync();
            var content = CreateJsonContent(data);
            var response = await _httpClient.PostAsync(endpoint, content);
            var responseStream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = await JsonSerializer.DeserializeAsync<Result>(responseStream);
                return Result<T>.Failure(errorResult?.Error ?? "Erro desconhecido");
            }

            var result = await JsonSerializer.DeserializeAsync<T>(responseStream);
            return Result<T>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(ex.Message);
        }
    }

    public async Task<Result> PostAsync(string endpoint, object data)
    {
        try
        {
            await SetAuthHeaderAsync();
            var content = CreateJsonContent(data);
            var response = await _httpClient.PostAsync(endpoint, content);
            var responseStream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = await JsonSerializer.DeserializeAsync<Result>(responseStream);
                return Result.Failure(errorResult?.Error ?? "Erro desconhecido");
            }

            var result = await JsonSerializer.DeserializeAsync<Result>(responseStream);
            return result.IsSuccess ? Result.Success() : Result.Failure(result.Error);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result<T>> PutAsync<T>(string endpoint, object data)
    {
        try
        {
            await SetAuthHeaderAsync();
            var content = CreateJsonContent(data);
            var response = await _httpClient.PutAsync(endpoint, content);
            var responseStream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = await JsonSerializer.DeserializeAsync<Result>(responseStream);
                return Result<T>.Failure(errorResult?.Error ?? "Erro desconhecido");
            }

            var result = await JsonSerializer.DeserializeAsync<T>(responseStream);
            return Result<T>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(ex.Message);
        }
    }

    public async Task<Result> DeleteAsync(string endpoint)
    {
        try
        {
            await SetAuthHeaderAsync();
            var response = await _httpClient.DeleteAsync(endpoint);
            var responseStream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = await JsonSerializer.DeserializeAsync<Result>(responseStream);
                return Result.Failure(errorResult?.Error ?? "Erro desconhecido");
            }

            var result = await JsonSerializer.DeserializeAsync<Result>(responseStream);
            return result.IsSuccess ? Result.Success() : Result.Failure(result.Error);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }


    public async Task<Result<T>> PatchAsync<T>(string endpoint, object data)
    {
        try
        {
            await SetAuthHeaderAsync();
            var content = CreateJsonContent(data);

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);
            var responseStream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = await JsonSerializer.DeserializeAsync<Result>(responseStream);
                return Result<T>.Failure(errorResult?.Error ?? "Erro desconhecido");
            }

            var result = await JsonSerializer.DeserializeAsync<T>(responseStream);
            return Result<T>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<T>.Failure(ex.Message);
        }
    }

    public async Task<Result> PatchAsync(string endpoint, object data)
    {
        try
        {
            await SetAuthHeaderAsync();
            var content = CreateJsonContent(data);

            var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint)
            {
                Content = content
            };

            var response = await _httpClient.SendAsync(request);
            var responseStream = await response.Content.ReadAsStreamAsync();

            if (!response.IsSuccessStatusCode)
            {
                var errorResult = await JsonSerializer.DeserializeAsync<Result>(responseStream);
                return Result.Failure(errorResult?.Error ?? "Erro desconhecido");
            }

            var result = await JsonSerializer.DeserializeAsync<Result>(responseStream);
            return result.IsSuccess ? Result.Success() : Result.Failure(result.Error);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public void SetAuthToken(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }

    private async Task SetAuthHeaderAsync()
    {
        var token = _httpContextAccessor.HttpContext?.Session.GetString("JwtToken");
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    private static StringContent CreateJsonContent(object data)
    {
        return new StringContent(
            JsonSerializer.Serialize(data),
            Encoding.UTF8,
            "application/json");
    }
}
