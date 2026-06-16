using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TCA.Desktop.Services;

namespace TCA.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, INavigator
{
    [ObservableProperty]
    private ViewModelBase _currentView;

    private readonly Stack<ViewModelBase> _history = new();
    private readonly ApiService _apiService;
    private readonly SessionService _session;
    public MainWindowViewModel(SessionService session, ApiService apiService)
    {
        _session = session;
        _apiService = apiService;
        CurrentView = new LoginViewModel(session, apiService, this);
    }

    public void NavigateTo(ViewModelBase view)
    {
        _history.Push(CurrentView);
        CurrentView = view;
    }

    [RelayCommand]
    public void GoBack()
    {
        if (_history.Count > 0)
        {
            CurrentView = _history.Pop();
            CurrentView.OnNavigatedTo();
        }
    }
}