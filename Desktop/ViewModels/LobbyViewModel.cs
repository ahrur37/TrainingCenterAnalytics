using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Models;
using Microsoft.Extensions.DependencyInjection;
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
    private ObservableCollection<CommentModel> _listcomments = [];
    [ObservableProperty]
    private DirectionModel? _selectedDirection;
    [ObservableProperty]
    private StatusModel? _selectedStatus;
    [ObservableProperty]
    private StatusModel? _selectedNewStatus;
    [ObservableProperty]
    private RequestModel? _selectedRequest;
    [ObservableProperty]
    private bool _canView = false;
    [ObservableProperty]
    private bool _canViewAdm = false;
    [ObservableProperty]
    private bool _canManageDictionaries = false;
    [ObservableProperty]
    private bool _canEdit = false;
    [ObservableProperty]
    private bool _canChangeStatus = false;
    [ObservableProperty]
    private bool _canEditRequest = false;
    [ObservableProperty]
    private bool _canViewAnalytics = false;
    [ObservableProperty]                                                                                                                                                          
    private string _statusMessage = string.Empty;                                                                                                                                 
    [ObservableProperty]                                                                                                                                                          
    private string _messageColor = "Green"; 
    [ObservableProperty]                                                                                                                                                          
    private string _shareMessageColor = "Green"; 
    [ObservableProperty]
    private string _newComment = string.Empty;
    [ObservableProperty]
    private string _shareStatusMessage = string.Empty;
    [ObservableProperty]
    private ObservableCollection<StatusModel> _activeStatuses = [];
    [ObservableProperty]
    private ObservableCollection<UserModel> _listManagers = [];
    [ObservableProperty]
    private UserModel? _selectedManager;
    [ObservableProperty]
    private string _assignMessage = string.Empty;
    [ObservableProperty]
    private string _assignMessageColor = "Green";
    
    private bool _isResetting;
    private int? _currentRequestId;
    private readonly ApiService _apiService;
    private readonly INavigator _navigator;
    private readonly SessionService _session;
    private readonly SignalRService _signalR;

    public LobbyViewModel(ApiService apiService, SessionService session, INavigator navigator, SignalRService signalR)
    {
        _apiService = apiService;
        _navigator = navigator;
        _session = session;
        _signalR = signalR;
        CanView = (Roles)_session.RoleId != Roles.User;
        CanViewAdm = (Roles)_session.RoleId == Roles.Admin;
        CanManageDictionaries = (Roles)_session.RoleId == Roles.Admin || (Roles)_session.RoleId == Roles.Director;
        CanViewAnalytics = _session.RoleId == 4;
        _signalR.CommentAdded += OnCommentAdded;
    }

    public override async Task OnNavigatedTo()
    {
        await _signalR.StartAsync(_apiService.BaseUrl);
        await Task.WhenAll(LoadRequestsAsync(), LoadComboboxesAsync());
    }

    private async Task LoadComboboxesAsync()
    {
        var directions = _apiService.GetDirections();
        var statuses = _apiService.GetStatuses();
        var users = _apiService.GetAllUsers();
        await Task.WhenAll(directions, statuses, users);
        Listdirections = new ObservableCollection<DirectionModel>(directions.Result);
        Liststatuses = new ObservableCollection<StatusModel>(statuses.Result);
        ActiveStatuses = new ObservableCollection<StatusModel>(statuses.Result.Where(s => s.IsActive));
        ListManagers = new ObservableCollection<UserModel>(users.Result.Where(u => u.RoleId == (int)Roles.Manager));
    }

    private async Task LoadRequestsAsync()
    {
        await ApplyFilters(SelectedStatus?.Id, SelectedDirection?.Id);
    }

    private async Task LoadCommentsAsync()
    {
        if (SelectedRequest == null) return; 
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
                list = await _apiService.GetRequests(statusid: statusId, directionid: directionId);
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
    
    private void UpdatePermissions(RequestModel value)
    {
        var role = (Roles)_session.RoleId;
        CanEdit = role == Roles.Admin || role == Roles.Director
                  || (role == Roles.Manager && value.AssigneeId == _session.UserId)
                  || (role == Roles.User && value.AuthorId == _session.UserId);
        CanChangeStatus = role == Roles.Admin || role == Roles.Director
                  || (role == Roles.Manager && value.AssigneeId == _session.UserId);
        CanEditRequest = role == Roles.Admin || role == Roles.Director 
                                             || (role == Roles.Manager && value.AssigneeId == _session.UserId)
                                             || (value.StatusId == 1 && role == Roles.User && value.AuthorId == _session.UserId);
    }

    partial void OnSelectedRequestChanged(RequestModel? value)
    {
        if (_currentRequestId.HasValue)
            _ = _signalR.LeaveRequest(_currentRequestId.Value);

        if (value != null)
        {
            _currentRequestId = value.Id;
            _ = _signalR.JoinRequest(value.Id);

            SelectedNewStatus = Liststatuses?.FirstOrDefault(s => s.Id == value.StatusId);
            SelectedManager = ListManagers.FirstOrDefault(s => s.Id == value.AssigneeId);
            _ = LoadCommentsAsync();
            UpdatePermissions(value);
            StatusMessage = string.Empty;
            ShareStatusMessage = string.Empty;
            AssignMessage = string.Empty;
        }
        else
        {
            _currentRequestId = null;
            CanEdit = false;
            CanChangeStatus = false;
            CanEditRequest = false;
        }
    }

    private void OnCommentAdded(CommentModel comment)
    {
        Avalonia.Threading.Dispatcher.UIThread.Post(() =>
        {
            if (comment.RequestId == _currentRequestId)
                Listcomments.Add(comment);
        });
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
        if (SelectedRequest == null || SelectedNewStatus == null) return; 
        
        var response = await _apiService.ChangeStatus(new ChangeStatusModel()
        {
            RequestId = SelectedRequest.Id,
            NewStatusId = SelectedNewStatus.Id,
        });
        if (response.IsSuccessStatusCode)
        {
            StatusMessage = "Success";
            MessageColor = "Green";
            await LoadRequestsAsync();    
        }
        else
        {                                                                                                      
            StatusMessage = "Error";
            MessageColor = "Red";   
        }
    }

    [RelayCommand]
    private async Task AddComment()
    {
        if (SelectedRequest == null || string.IsNullOrWhiteSpace(NewComment)) return;                                                                                             

        var response = await _apiService.AddComment(new CreateCommentModel()
        {
            Content = NewComment,
            AuthorId =  _session.UserId,
            RequestId = SelectedRequest.Id
        });
        
        if (response.IsSuccessStatusCode)                                                                                                                                         
        {                                                                                                                                                                         
            NewComment = string.Empty;    
            ShareStatusMessage = "Success";
            ShareMessageColor = "Green";
            await LoadCommentsAsync();                                                                                                                                            
        }
        else
        {
            ShareStatusMessage = "Fail";
            ShareMessageColor = "Read";
        }
    }

    [RelayCommand]
    private async Task AssignManager()
    {
        if (SelectedRequest == null || SelectedManager == null) return;

        var response = await _apiService.ChangeAssignManager(new AssignManagerModel
        {
            RequestId = SelectedRequest.Id,
            ManagerId = SelectedManager.Id
        });

        if (response.IsSuccessStatusCode)
        {
            var requestId = SelectedRequest.Id;
            await LoadRequestsAsync();
            var updated = Requests?.FirstOrDefault(r => r.Id == requestId);
            if (updated != null)
            {
                SelectedRequest = updated;
                UpdatePermissions(updated);
            }
            AssignMessage = "Назначен";
            AssignMessageColor = "Green";
        }
        else
        {
            AssignMessage = await response.Content.ReadAsStringAsync();
            AssignMessageColor = "Red";
        }
    }

    [RelayCommand]
    private void GoToCreateRequest()
    {
        _navigator.NavigateTo(App.Services.GetRequiredService<CreateRequestViewModel>());
    }

    [RelayCommand]
    private void Logout()
    {
        _navigator.NavigateTo(App.Services.GetRequiredService<LoginViewModel>());
    }

    

    [RelayCommand]
    private void GoToDictionaries()
    {
        _navigator.NavigateTo(App.Services.GetRequiredService<DictionariesViewModel>());
    }

    [RelayCommand]
    private void GoToEditRequest()
    {
        if (SelectedRequest == null) return;
        var vm = App.Services.GetRequiredService<EditRequestViewModel>();
        vm.LoadRequest(SelectedRequest);
        _navigator.NavigateTo(vm);
    }
    
    [RelayCommand]
    private void GoToDashboard()
    {
        _navigator.NavigateTo(App.Services.GetRequiredService<DashboardViewModel>());
    }
}
