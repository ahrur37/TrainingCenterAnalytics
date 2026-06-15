using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Models;
using TCA.Desktop.Enums;
using TCA.Desktop.Services;

namespace TCA.Desktop.ViewModels;

public partial class LobbyViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<RequestModel> _requests;
    [ObservableProperty]
    private ObservableCollection<DirectionModel> _listdirections;
    [ObservableProperty]
    private ObservableCollection<StatusModel> _liststatuses;
    [ObservableProperty]
    private ObservableCollection<CommentModel> _listcomments;
    [ObservableProperty]
    private DirectionModel? _selectedDirection;
    [ObservableProperty]
    private StatusModel? _selectedStatus;
    [ObservableProperty]
    private ChangeStatusModel? _selectedNewStatus;
    [ObservableProperty]
    private RequestModel? _selectedRequest;
    [ObservableProperty] 
    private bool _canView = false;
    [ObservableProperty]                                                                                                                                                          
    private string _statusMessage = string.Empty;                                                                                                                                 
    [ObservableProperty]                                                                                                                                                          
    private string _statusMessageColor = "Green"; 
    [ObservableProperty]
    private string _newComment = string.Empty;

    private bool _isResetting; 
    private readonly ApiService _apiService;
    private readonly INavigator _navigator;
    private readonly SessionService _session;

    public LobbyViewModel(ApiService apiService, SessionService session, INavigator navigator)
    {
        _apiService = apiService;
        _navigator = navigator;
        _session = session;
        CanView = (Roles)_session.RoleId != Roles.User;
        _ = LoadRequestsAsync();
        _ = LoadComboboxesAsync();
    }

    private async Task LoadComboboxesAsync()
    {
        var directions = _apiService.GetDirections();
        var statuses = _apiService.GetStatuses();
        await Task.WhenAll(directions, statuses);
        Listdirections = new ObservableCollection<DirectionModel>(directions.Result);
        Liststatuses = new ObservableCollection<StatusModel>(statuses.Result);
    }

    private async Task LoadRequestsAsync()
    {
        await ApplyFilters(SelectedStatus?.Id, SelectedDirection?.Id);
    }

    private async Task LoadCommentsAsync()
    {
        var comments = await _apiService.GetComments(SelectedRequest.Id);
        Listcomments = new ObservableCollection<CommentModel>(comments);

    }

    private async Task ApplyFilters(int? statusId, int? directionId)
    {
        try
        {
            var role = (Roles)_session.RoleId;
            List<RequestModel> list;

            if (role == Roles.Admin || role == Roles.Director)
                list = await _apiService.GetRequests(statusid: statusId, directionid: directionId);
            else if (role == Roles.User)
                list = await _apiService.GetRequests(userid: _session.UserId);
            else if (role == Roles.Manager)
                list = await _apiService.GetRequests(statusid: statusId ?? (int)Statuses.New, directionid: directionId);
            else
                list = [];

            Requests = new ObservableCollection<RequestModel>(list);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки заявок: {ex.Message}");
        }
    }

    partial void OnSelectedStatusChanged(StatusModel? value)
    {
        if (!_isResetting) _ = ApplyFilters(value?.Id, SelectedDirection?.Id);
    }

    partial void OnSelectedDirectionChanged(DirectionModel? value)
    {
        if (!_isResetting) _ = ApplyFilters(SelectedStatus?.Id, value?.Id);
    }

    [RelayCommand]
    private async Task ResetFilters()
    {
        _isResetting = true;
        SelectedStatus = null;
        SelectedDirection = null;
        _isResetting = false;
        await ApplyFilters(null, null);
    }

    [RelayCommand]
    private async Task ChangeStatus()
    {
        var response = await _apiService.ChangeStatus(SelectedNewStatus);
        if (response.IsSuccessStatusCode)
        {
            StatusMessage = "Success";
            StatusMessageColor = "Green";
        }
        else
        {
            StatusMessage = "Error";
            StatusMessageColor = "Red";
        }
    }

    [RelayCommand]
    private async Task AddComment()
    {
        var response = await 
    }

    [RelayCommand]
    private void Logout()
    {
        _navigator.NavigateTo(new LoginViewModel(_session, _apiService, _navigator));
    }
}
