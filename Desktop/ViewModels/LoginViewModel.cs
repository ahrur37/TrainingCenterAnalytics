using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TCA.Desktop.Models;
using TCA.Desktop.Services;

namespace TCA.Desktop.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _login;
    [ObservableProperty]
    private string _password;
    [ObservableProperty]
    private string _errorMessage;
    [ObservableProperty]
    private ViewModelBase _currentView;
    
    private readonly ApiService _apiservice;
    private readonly SessionService _session;
    private readonly INavigator _navigator;
    public LoginViewModel(SessionService session, ApiService apiservice, INavigator navigator)
    {
        _session = session;
        _apiservice = apiservice;
        _navigator = navigator;
    }

    [RelayCommand]
    private async Task Authorization()
    {
        var response = await _apiservice.AuthUserAsync(new AuthUserModel
        {
            Email = Login,
            Password = Password,
        });
        if (response.IsSuccessStatusCode)
        {
            _navigator.NavigateTo(new LobbyViewModel(_apiservice, _session, _navigator));
        }
        else
        {
            ErrorMessage = "Incorrect login or password";
        }
    }

    [RelayCommand]
    private async Task GoToRegistration()
    {
        _navigator.NavigateTo(new RegistrationViewModel(_session, _apiservice,  _navigator));
    }
}