using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using TCA.Desktop.Models;
using TCA.Desktop.Services;

namespace TCA.Desktop.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    [ObservableProperty] private string _login = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;

    private readonly ApiService _apiservice;
    private readonly INavigator _navigator;

    public LoginViewModel(ApiService apiservice, INavigator navigator)
    {
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
            _navigator.NavigateTo(App.Services.GetRequiredService<LobbyViewModel>());
        }
        else
        {
            ErrorMessage = "Неверный логин или пароль";
        }
    }

    [RelayCommand]
    private void GoToRegistration()
    {
        _navigator.NavigateTo(App.Services.GetRequiredService<RegistrationViewModel>());
    }
}