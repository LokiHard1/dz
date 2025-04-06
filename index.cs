using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetDataFromApiAsync(string url)
    {
        try
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                if ((int)response.StatusCode >= 400 && (int)response.StatusCode < 500)
                {
                    throw new Exception($"Ошибка клиента: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }

            var content = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new Exception("Получен пустой ответ от API.");
            }

            return content;
        }
        catch (HttpRequestException ex)
        {
            throw new Exception("Сетевая ошибка при обращении к API.", ex);
        }
        catch (Exception ex)
        {
            throw new Exception("Произошла ошибка при выполнении запроса к API.", ex);
        }
    }
}