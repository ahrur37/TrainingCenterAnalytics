using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Models;
using Microsoft.Extensions.DependencyInjection;
using TCA.Desktop.Services;

namespace TCA.Desktop.ViewModels;

public partial class DictionariesViewModel : ViewModelBase
{
    private readonly ApiService _apiService;
    private readonly INavigator _navigator;

    // Directions
    [ObservableProperty] private ObservableCollection<DirectionModel> _directions = [];
    [ObservableProperty] private DirectionModel? _selectedDirection;
    [ObservableProperty] private string _directionName = string.Empty;
    [ObservableProperty] private string _directionMessage = string.Empty;
    [ObservableProperty] private string _directionMessageColor = "Green";

    // Statuses
    [ObservableProperty] private ObservableCollection<StatusModel> _statuses = [];
    [ObservableProperty] private StatusModel? _selectedStatus;
    [ObservableProperty] private string _statusName = string.Empty;
    [ObservableProperty] private string _statusMessage = string.Empty;
    [ObservableProperty] private string _statusMessageColor = "Green";

    // Training Formats
    [ObservableProperty] private ObservableCollection<TrainingFormatModel> _trainingFormats = [];
    [ObservableProperty] private TrainingFormatModel? _selectedTrainingFormat;
    [ObservableProperty] private string _trainingFormatName = string.Empty;
    [ObservableProperty] private string _trainingFormatMessage = string.Empty;
    [ObservableProperty] private string _trainingFormatMessageColor = "Green";

    public DictionariesViewModel(ApiService apiService, INavigator navigator)
    {
        _apiService = apiService;
        _navigator = navigator;
    }

    public override async Task OnNavigatedTo()
    {
        await LoadAllAsync();
    }

    private async Task LoadAllAsync()
    {
        var d = _apiService.GetDirections();
        var s = _apiService.GetStatuses();
        var f = _apiService.GetTrainingFormats();
        await Task.WhenAll(d, s, f);
        Directions = new ObservableCollection<DirectionModel>(d.Result);
        Statuses = new ObservableCollection<StatusModel>(s.Result);
        TrainingFormats = new ObservableCollection<TrainingFormatModel>(f.Result);
    }

    partial void OnSelectedDirectionChanged(DirectionModel? value) =>
        DirectionName = value?.Name ?? string.Empty;

    partial void OnSelectedStatusChanged(StatusModel? value) =>
        StatusName = value?.Name ?? string.Empty;

    partial void OnSelectedTrainingFormatChanged(TrainingFormatModel? value) =>
        TrainingFormatName = value?.Name ?? string.Empty;

    [RelayCommand]
    private async Task SaveDirection()
    {
        if (string.IsNullOrWhiteSpace(DirectionName)) return;

        var response = SelectedDirection == null
            ? await _apiService.CreateDirection(DirectionName)
            : await _apiService.UpdateDirection(SelectedDirection.Id, DirectionName);

        if (response.IsSuccessStatusCode)
        {
            DirectionMessage = SelectedDirection == null ? "Создано" : "Обновлено";
            DirectionMessageColor = "Green";
            DirectionName = string.Empty;
            SelectedDirection = null;
            Directions = new ObservableCollection<DirectionModel>(await _apiService.GetDirections());
        }
        else
        {
            DirectionMessage = "Ошибка";
            DirectionMessageColor = "Red";
        }
    }

    [RelayCommand]
    private async Task DeleteDirection()
    {
        if (SelectedDirection == null) return;

        var response = await _apiService.DeleteDirection(SelectedDirection.Id);
        if (response.IsSuccessStatusCode)
        {
            DirectionMessage = "Удалено";
            DirectionMessageColor = "Green";
            SelectedDirection = null;
            DirectionName = string.Empty;
            Directions = new ObservableCollection<DirectionModel>(await _apiService.GetDirections());
        }
        else
        {
            DirectionMessage = "Ошибка удаления";
            DirectionMessageColor = "Red";
        }
    }

    [RelayCommand]
    private async Task SaveStatus()
    {
        if (string.IsNullOrWhiteSpace(StatusName)) return;

        var response = SelectedStatus == null
            ? await _apiService.CreateStatus(StatusName)
            : await _apiService.UpdateStatus(SelectedStatus.Id, StatusName);

        if (response.IsSuccessStatusCode)
        {
            StatusMessage = SelectedStatus == null ? "Создано" : "Обновлено";
            StatusMessageColor = "Green";
            StatusName = string.Empty;
            SelectedStatus = null;
            Statuses = new ObservableCollection<StatusModel>(await _apiService.GetStatuses());
        }
        else
        {
            StatusMessage = "Ошибка";
            StatusMessageColor = "Red";
        }
    }

    [RelayCommand]
    private async Task DeleteStatus()
    {
        if (SelectedStatus == null) return;

        var response = await _apiService.DeleteStatus(SelectedStatus.Id);
        if (response.IsSuccessStatusCode)
        {
            StatusMessage = "Удалено";
            StatusMessageColor = "Green";
            SelectedStatus = null;
            StatusName = string.Empty;
            Statuses = new ObservableCollection<StatusModel>(await _apiService.GetStatuses());
        }
        else
        {
            StatusMessage = "Ошибка удаления";
            StatusMessageColor = "Red";
        }
    }

    [RelayCommand]
    private async Task SaveTrainingFormat()
    {
        if (string.IsNullOrWhiteSpace(TrainingFormatName)) return;

        var response = SelectedTrainingFormat == null
            ? await _apiService.CreateTrainingFormat(TrainingFormatName)
            : await _apiService.UpdateTrainingFormat(SelectedTrainingFormat.Id, TrainingFormatName);

        if (response.IsSuccessStatusCode)
        {
            TrainingFormatMessage = SelectedTrainingFormat == null ? "Создано" : "Обновлено";
            TrainingFormatMessageColor = "Green";
            TrainingFormatName = string.Empty;
            SelectedTrainingFormat = null;
            TrainingFormats = new ObservableCollection<TrainingFormatModel>(await _apiService.GetTrainingFormats());
        }
        else
        {
            TrainingFormatMessage = "Ошибка";
            TrainingFormatMessageColor = "Red";
        }
    }

    [RelayCommand]
    private async Task DeleteTrainingFormat()
    {
        if (SelectedTrainingFormat == null) return;

        var response = await _apiService.DeleteTrainingFormat(SelectedTrainingFormat.Id);
        if (response.IsSuccessStatusCode)
        {
            TrainingFormatMessage = "Удалено";
            TrainingFormatMessageColor = "Green";
            SelectedTrainingFormat = null;
            TrainingFormatName = string.Empty;
            TrainingFormats = new ObservableCollection<TrainingFormatModel>(await _apiService.GetTrainingFormats());
        }
        else
        {
            TrainingFormatMessage = "Ошибка удаления";
            TrainingFormatMessageColor = "Red";
        }
    }

    [RelayCommand]
    private async Task NewStatus()
    {
        if (string.IsNullOrEmpty(StatusName)) return;
        var response = await _apiService.CreateStatus(StatusName);
        if (response.IsSuccessStatusCode)
        {
            StatusMessage = "Создано";
            StatusMessageColor = "Green";
            StatusName = string.Empty;
            Statuses = new ObservableCollection<StatusModel>(await _apiService.GetStatuses());
        }
        else
        {
            StatusMessage = "Ошибка";
            StatusMessageColor = "Red";
        }
    }

    [RelayCommand]
    private async Task NewDirection()
    {
        if (string.IsNullOrEmpty(DirectionName)) return;
        var response = await _apiService.CreateDirection(DirectionName);
        if (response.IsSuccessStatusCode)
        {
            DirectionMessage = "Создано";
            DirectionMessageColor = "Green";
            DirectionName = string.Empty;
            Directions = new ObservableCollection<DirectionModel>(await _apiService.GetDirections());
        }
        else
        {
            DirectionMessage = "Ошибка";
            DirectionMessageColor = "Red";
        }
    }

    [RelayCommand]
    private async Task NewTrainingFormat()
    {
        if (string.IsNullOrEmpty(TrainingFormatName)) return;
        var response = await _apiService.CreateTrainingFormat(TrainingFormatName);
        if (response.IsSuccessStatusCode)
        {
            TrainingFormatMessage = "Создано";
            TrainingFormatMessageColor = "Green";
            TrainingFormatName = string.Empty;
            TrainingFormats = new ObservableCollection<TrainingFormatModel>(await _apiService.GetTrainingFormats());
        }
        else
        {
            TrainingFormatMessage = "Ошибка";
            TrainingFormatMessageColor = "Red";
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        _navigator.NavigateTo(App.Services.GetRequiredService<LobbyViewModel>());
    }
}
