using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Desktop.Models;
using TCA.Desktop.Models;

namespace TCA.Desktop.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;
    private readonly SessionService _session;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ApiService(SessionService session)
    {
        _session = session;
        _httpClient = new HttpClient { BaseAddress = new Uri("http://161.104.32.25") };
    }
    public async Task<HttpResponseMessage> AuthUserAsync(AuthUserModel model)                                                                                        
    {                                                                                                                                                                
        var response =  await _httpClient.PostAsJsonAsync("AuthUser", model);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<AuthResponseModel>();
            _session.Token = result!.Token;
            _session.UserId = result.UserId;
            _session.RoleId = result.RoleId;
            _httpClient.DefaultRequestHeaders.Authorization =                                                                                                                             
                new AuthenticationHeaderValue("Bearer", _session.Token);
            Console.WriteLine(result.Token);
        }
        return response;
    }

    public async Task<HttpResponseMessage> RegUserASync(RegUserModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("RegUser", model);
        return response;
    }

    public async Task<List<RequestModel>> GetRequests(int userid)
    {
        var response = await _httpClient.GetAsync($"GetRequests?authorId={userid}");
        return await response.Content.ReadFromJsonAsync<List<RequestModel>>(_jsonOptions) ?? [];
        
    }
}