using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
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
    public string BaseUrl { get; } 
    public ApiService(SessionService session)
    {
        _session = session;
        var config = new ConfigurationBuilder()                                                                                           
            .SetBasePath(AppContext.BaseDirectory)                                                         
            .AddJsonFile("appsettings.json", optional: true)                                                                              
            .Build();    

        BaseUrl = config["ApiUrl"] ?? "http://161.104.32.25";

        _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
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
        var url = $"ChangeStatus?requestId={model.RequestId}&newStatusId={model.NewStatusId}";                                                                                      
        var response = await _httpClient.PostAsync(url, null);      
        return response;
    }

    public async Task<HttpResponseMessage> UpdateRequest(int requestId, UpdateRequestModel model) =>
        await _httpClient.PutAsJsonAsync($"UpdateRequest/{requestId}", model);

    public async Task<HttpResponseMessage> DeleteRequest(int requestId) =>
        await _httpClient.DeleteAsync($"DeleteRequest/{requestId}");

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

    // Directions
    public async Task<HttpResponseMessage> CreateDirection(string name) =>
        await _httpClient.PostAsJsonAsync("CreateDirection", new { name });

    public async Task<HttpResponseMessage> UpdateDirection(int id, string name) =>
        await _httpClient.PutAsJsonAsync($"UpdateDirection/{id}", new { name });

    public async Task<HttpResponseMessage> DeleteDirection(int id) =>
        await _httpClient.DeleteAsync($"DeleteDirection/{id}");

    // Statuses
    public async Task<HttpResponseMessage> CreateStatus(string name) =>
        await _httpClient.PostAsJsonAsync("CreateStatus", new { name });

    public async Task<HttpResponseMessage> UpdateStatus(int id, string name) =>
        await _httpClient.PutAsJsonAsync($"UpdateStatus/{id}", new { name });

    public async Task<HttpResponseMessage> DeleteStatus(int id) =>
        await _httpClient.DeleteAsync($"DeleteStatus/{id}");

    // Training Formats
    public async Task<HttpResponseMessage> CreateTrainingFormat(string name) =>
        await _httpClient.PostAsJsonAsync("CreateTrainingFormat", new { name });

    public async Task<HttpResponseMessage> UpdateTrainingFormat(int id, string name) =>
        await _httpClient.PutAsJsonAsync($"UpdateTrainingFormat/{id}", new { name });

    public async Task<HttpResponseMessage> DeleteTrainingFormat(int id) =>
        await _httpClient.DeleteAsync($"DeleteTrainingFormat/{id}");
    
// Analytics
    public async Task<AnalyticsSummaryDto?> GetAnalyticsSummary()
    {
        try 
        {
            var response = await _httpClient.GetAsync("summary");
        
            // Логируем статус ответа (например, 200, 404, 500)
            System.Diagnostics.Debug.WriteLine($"[API Debug] GetAnalyticsSummary Status: {response.StatusCode}");

            if (!response.IsSuccessStatusCode) 
            {
                // Логируем содержимое ошибки, если бэкенд что-то вернул
                var errorContent = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine($"[API Debug] Error Content: {errorContent}");
                return null;
            }

            // Логируем сам JSON для проверки структуры
            var json = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine($"[API Debug] Response JSON: {json}");

            // Десериализуем и возвращаем
            return JsonSerializer.Deserialize<AnalyticsSummaryDto>(json, _jsonOptions);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[API Debug] Exception: {ex.Message}");
            return null;
        }
    }
    public async Task<List<AnalyticsDynamicsDto>> GetAnalyticsDynamics()
    {
        var response = await _httpClient.GetAsync("dynamics");
        if (!response.IsSuccessStatusCode) return [];
        return await response.Content.ReadFromJsonAsync<List<AnalyticsDynamicsDto>>(_jsonOptions) ?? [];
    }

    public async Task<List<PopularDirectionDto>> GetPopularDirections()
    {
        var response = await _httpClient.GetAsync("popular-directions");
        if (!response.IsSuccessStatusCode) return [];
        return await response.Content.ReadFromJsonAsync<List<PopularDirectionDto>>(_jsonOptions) ?? [];
    }

    public async Task<List<StatusDistributionDto>> GetStatusesDistribution()
    {
        var response = await _httpClient.GetAsync("statuses-distribution");
        if (!response.IsSuccessStatusCode) return [];
        return await response.Content.ReadFromJsonAsync<List<StatusDistributionDto>>(_jsonOptions) ?? [];
    }
}