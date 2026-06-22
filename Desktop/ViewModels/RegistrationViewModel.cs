using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TCA.Desktop.Models;
using TCA.Desktop.Services;

namespace TCA.Desktop.ViewModels;

public partial class RegistrationViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _email;
    [ObservableProperty]
    private string _password;
    [ObservableProperty]
    private string _name;
    [ObservableProperty]
    private string _message;
    [ObservableProperty]
    private string _messageColor = "Red";
    
    private readonly ApiService _apiservice;
    private readonly INavigator _navigator;

    public RegistrationViewModel(ApiService apiService, INavigator navigator)
    {
        _apiservice = apiService;
        _navigator = navigator;
    }

    [RelayCommand]
    private async Task Registration()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Name))
        {
            Message = "Заполните все поля";
            MessageColor = "Red";
            return;
        }

        if (Password.Length < 6)
        {
            Message = "Пароль должен быть больше 6 символов";
            MessageColor = "Red";
            return;
        }
        var response = await _apiservice.RegUserASync(new RegUserModel {Name = Name, Email = Email, Password = Password});
        if (response.IsSuccessStatusCode)
        {
            Message = "Успешно";
            MessageColor = "Green";
        }
        else 
        {
            Message = "Ошибка";
            MessageColor = "Red";
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        _navigator.GoBack();
    }
}