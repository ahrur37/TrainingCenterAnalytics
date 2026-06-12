using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TCA.Desktop.Models;

namespace TCA.Desktop.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private string? _token;
    public ApiService()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7279") };
    }
    public async Task<HttpResponseMessage> AuthUserAsync(AuthUserModel model)                                                                                        
    {                                                                                                                                                                
        var response =  await _httpClient.PostAsJsonAsync("AuthUser", model);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthResponseModel>();
            _token = result!.Token;
            _httpClient.DefaultRequestHeaders.Authorization =                                                                                                                             
                new AuthenticationHeaderValue("Bearer", _token);
        }
        return response;
    }

    public async Task<HttpResponseMessage> RegUserASync(RegUserModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("RegUser", model);
        return response;
    }
}