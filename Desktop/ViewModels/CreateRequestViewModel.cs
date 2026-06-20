using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Models;
using TCA.Desktop.Services;

namespace TCA.Desktop.ViewModels;

public partial class CreateRequestViewModel : ViewModelBase
{
    [ObservableProperty] private string _topic = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private string? _contactInfo;
    [ObservableProperty] private ObservableCollection<DirectionModel> _directions = [];
    [ObservableProperty] private ObservableCollection<TrainingFormatModel> _formats = [];
    [ObservableProperty] private DirectionModel? _selectedDirection;
    [ObservableProperty] private TrainingFormatModel? _selectedFormat;
    [ObservableProperty] private string _errorMessage = string.Empty;
    [ObservableProperty] private string _errorMessageColor = string.Empty;

    private readonly ApiService _apiService;
    private readonly SessionService _sessionService;
    private readonly INavigator _navigator;

    public CreateRequestViewModel(SessionService sessionService, ApiService apiService, INavigator navigator)
    {
        _apiService = apiService;
        _sessionService = sessionService;
        _navigator = navigator;
    }

    public override async Task OnNavigatedTo()
    {
        var directions = _apiService.GetDirections();
        var formats = _apiService.GetTrainingFormats();
        await Task.WhenAll(directions, formats);
        Directions = new ObservableCollection<DirectionModel>(directions.Result.Where(d => d.IsActive));
        Formats = new ObservableCollection<TrainingFormatModel>(formats.Result.Where(f => f.IsActive));
    }

    [RelayCommand]
    private async Task CreateRequest()
    {
        if (string.IsNullOrWhiteSpace(Topic) || string.IsNullOrWhiteSpace(Description) ||
            string.IsNullOrWhiteSpace(ContactInfo) || SelectedDirection == null || SelectedFormat == null)
        {
            ErrorMessage = "Заполните все поля";
            ErrorMessageColor = "Red";
            return;
        }
        var response = await _apiService.CreateReqAsync(new CreateRequestModel
        {
            Topic = Topic,
            Description = Description,
            ContactInfo = ContactInfo,
            DirectionId = SelectedDirection?.Id ?? 0,
            TrainingFormatId = SelectedFormat?.Id ?? 0,
            AuthorId = _sessionService.UserId
        });

        if (response.IsSuccessStatusCode)
        {
            ErrorMessage = "Заявка успешно создана";
            ErrorMessageColor = "Green";
        }
        else
        {
            ErrorMessage = "Ошибка";
            ErrorMessageColor = "Red";
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        _navigator.GoBack();
    }
}
