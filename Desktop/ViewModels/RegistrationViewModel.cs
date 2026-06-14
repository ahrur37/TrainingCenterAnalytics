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
    private readonly Action _onSuccess; 
    private readonly Action _onBack; 
    public RegistrationViewModel(ApiService apiService, Action onBack)
    {
        _apiservice = apiService;
        _onBack = onBack;
    }

    [RelayCommand]
    private async Task Registration()
    {
        var response = await _apiservice.RegUserASync(new RegUserModel {Name = Name, Email = Email, Password = Password});
        if (response.IsSuccessStatusCode)
        {
            Message = "Registration successful!";
            MessageColor = "Green";
        }
        else 
        {
            Message = "Error occured while registering!";
            MessageColor = "Red";
        }
    }

    [RelayCommand]
    private async Task GoBack()
    {
        _onBack();
    }
}