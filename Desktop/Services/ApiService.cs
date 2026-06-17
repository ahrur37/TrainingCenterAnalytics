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
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", _session.Token);
        }
        return response;
    }

    public async Task<HttpResponseMessage> RegUserASync(RegUserModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("RegUser", model);
        return response;
    }
    
    public async Task<HttpResponseMessage> CreateReqAsync(CreateRequestModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("CreateRequest", model);
        return response;
    }

    public async Task<List<RequestModel>> GetRequests(int? userid = null, int? statusid = null, int? directionid = null)
    {
        var query = new List<string>();
        if (userid.HasValue)      query.Add($"authorId={userid}");                                                                                                                
        if (statusid.HasValue)    query.Add($"statusId={statusid}");   
        if (directionid.HasValue)  query.Add($"directionId={directionid}");
        
        var url = query.Count > 0 ? $"GetRequests?{string.Join("&", query)}" : "GetRequests";
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) return [];
        return await response.Content.ReadFromJsonAsync<List<RequestModel>>(_jsonOptions) ?? [];
    }

    public async Task<List<DirectionModel>> GetDirections()
    {
        var response = await _httpClient.GetAsync("GetDirections");
        return await response.Content.ReadFromJsonAsync<List<DirectionModel>>(_jsonOptions) ?? [];
    }
    
    public async Task<List<TrainingFormatModel>> GetTrainingFormats()
    {
        var response = await _httpClient.GetAsync("GetTrainingFormats");
        return await response.Content.ReadFromJsonAsync<List<TrainingFormatModel>>(_jsonOptions) ?? [];
    }

    public async Task<List<StatusModel>> GetStatuses()
    {
        var response = await _httpClient.GetAsync($"GetStatuses");
        return await response.Content.ReadFromJsonAsync<List<StatusModel>>(_jsonOptions) ?? [];
    }
    public async Task<List<CommentModel>> GetComments(int requestId)
    {
        var response = await _httpClient.GetAsync($"GetCommentsByRequestId/{requestId}");
        return await response.Content.ReadFromJsonAsync<List<CommentModel>>(_jsonOptions) ?? [];
    }

    public async Task<HttpResponseMessage> ChangeStatus(ChangeStatusModel model)
    {
        var url = $"ChangeStatus?requestId={model.RequestId}&newStatusId={model.NewStatusId}&currentUserId={model.CurrentUserId}";                                                                                      
        var response = await _httpClient.PostAsync(url, null);      
        return response;
    }

    public async Task<HttpResponseMessage> AddComment(CreateCommentModel model)
    {
        var response = await _httpClient.PostAsJsonAsync("AddComment", model);
        return response;
    }
    
    public async Task<List<UserModel>> GetAllUsers()
    {
        var response = await _httpClient.GetAsync("GetAllUsers");
        return await response.Content.ReadFromJsonAsync<List<UserModel>>(_jsonOptions) ?? [];
    }

    public async Task<HttpResponseMessage> ChangeAssignManager(AssignManagerModel model)
    {
        var response = await _httpClient.PostAsync($"AssignManager?requestId={model.RequestId}&managerId={model.ManagerId}", null);
        return response;
    }
}