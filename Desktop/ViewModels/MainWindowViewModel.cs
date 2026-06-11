using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TCA.Desktop.Services;

namespace TCA.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentView;

    private readonly Stack<ViewModelBase> _history = new();
    private readonly ApiService _apiService;
    public MainWindowViewModel(ApiService apiService)
    {
        _apiService = apiService;
        CurrentView = new LoginViewModel(apiService, () => NavigateTo(new MenuViewModel()), 
            () => NavigateTo(new RegistrationViewModel(apiService, GoBack)));
    }

    public void NavigateTo(ViewModelBase view)
    {
        _history.Push(CurrentView);
        CurrentView = view;
    }

    [RelayCommand]
    private void GoBack()
    {
        if (_history.Count > 0)
            CurrentView = _history.Pop();
    }
}