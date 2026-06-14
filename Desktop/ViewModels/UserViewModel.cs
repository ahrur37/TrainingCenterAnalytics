using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Desktop.Models;
using TCA.Desktop.Services;

namespace TCA.Desktop.ViewModels;

public partial class UserViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableCollection<RequestModel> _requests;
    private List<RequestModel> _allList;
    private readonly ApiService _apiService;
    private readonly INavigator _navigator;
    private readonly SessionService _session;
    public UserViewModel(ApiService apiService, SessionService session, INavigator navigator)
    {
        _apiService = apiService;
        _navigator = navigator;
        _session = session;
        _ = LoadRequestsAsync();
    }

    private async Task LoadRequestsAsync()
    {
        _allList = await _apiService.GetRequests(userid: _session.UserId);
        Requests = new ObservableCollection<RequestModel>(_allList);
    }
}