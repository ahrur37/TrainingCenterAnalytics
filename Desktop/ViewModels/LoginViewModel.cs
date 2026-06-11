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
    private readonly Action _onSuccess; 
    private readonly Action _onGoToRegistration; 
    public LoginViewModel(ApiService apiservice,  Action onSuccess, Action onGoToRegistration)
    {
        _apiservice = apiservice;
        _onSuccess = onSuccess;
        _onGoToRegistration  = onGoToRegistration;
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
            _onSuccess();
        }
        else
        {
            ErrorMessage = "Incorrect login or password";
        }
    }

    [RelayCommand]
    private async Task GoToRegistration()
    {
        _onGoToRegistration();
    }
}