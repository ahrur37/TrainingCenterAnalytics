using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Desktop.Models;
using TCA.Desktop.Services;

namespace TCA.Desktop.ViewModels;

public partial class EditRequestViewModel : ViewModelBase
{
    [ObservableProperty] private int _requestId;
    [ObservableProperty] private string _topic = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private string _contactInfo = string.Empty;
    [ObservableProperty] private ObservableCollection<DirectionModel> _directions = [];
    [ObservableProperty] private ObservableCollection<TrainingFormatModel> _formats = [];
    [ObservableProperty] private DirectionModel? _selectedDirection;
    [ObservableProperty] private TrainingFormatModel? _selectedFormat;
    [ObservableProperty] private string _message = string.Empty;
    [ObservableProperty] private string _messageColor = string.Empty;

    private readonly ApiService _apiService;
    private readonly INavigator _navigator;

    public EditRequestViewModel(ApiService apiService, INavigator navigator)
    {
        _apiService = apiService;
        _navigator = navigator;
    }

    private int _initialDirectionId;
    private int _initialFormatId;

    public void LoadRequest(RequestModel request)
    {
        RequestId = request.Id;
        Topic = request.Topic;
        Description = request.Description;
        ContactInfo = request.ContactInfo ?? string.Empty;
        _initialDirectionId = request.DirectionId;
        _initialFormatId = request.TrainingFormatId;
    }

    public override async Task OnNavigatedTo()
    {
        var directions = _apiService.GetDirections();
        var formats = _apiService.GetTrainingFormats();
        await Task.WhenAll(directions, formats);
        Directions = new ObservableCollection<DirectionModel>(directions.Result.Where(d => d.IsActive));
        Formats = new ObservableCollection<TrainingFormatModel>(formats.Result.Where(f => f.IsActive));

        SelectedDirection = Directions.FirstOrDefault(d => d.Id == _initialDirectionId);
        SelectedFormat = Formats.FirstOrDefault(f => f.Id == _initialFormatId);
    }

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Topic) || string.IsNullOrWhiteSpace(Description) ||
            string.IsNullOrWhiteSpace(ContactInfo) || SelectedDirection == null || SelectedFormat == null)
        {
            Message = "Заполните все поля";
            MessageColor = "Red";
            return;
        }

        var response = await _apiService.UpdateRequest(RequestId, new UpdateRequestModel
        {
            Topic = Topic,
            Description = Description,
            ContactInfo = ContactInfo,
            DirectionId = SelectedDirection.Id,
            TrainingFormatId = SelectedFormat.Id
        });

        if (response.IsSuccessStatusCode)
        {
            Message = "Успешно";
            MessageColor = "Green";
        }
        else
        {
            Message = await response.Content.ReadAsStringAsync();
            MessageColor = "Red";
        }
    }

    [RelayCommand]
    private async Task Delete()
    {
        var response = await _apiService.DeleteRequest(RequestId);
        if (response.IsSuccessStatusCode)
        {
            Message = "Успешно";
            MessageColor = "Green";
        }
        else
        {
            Message = await response.Content.ReadAsStringAsync();
            MessageColor = "Red";
        }
    }

    [RelayCommand]
    private void GoBack()
    {
        _navigator.GoBack();
    }
}
