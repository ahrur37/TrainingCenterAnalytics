using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using TCA.Desktop.Services;

namespace TCA.Desktop.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, INavigator
{
    [ObservableProperty]
    private ViewModelBase _currentView;

    private readonly Stack<ViewModelBase> _history = new();

    public MainWindowViewModel()
    {
    }

    public void NavigateTo(ViewModelBase view)
    {
        _history.Push(CurrentView);
        CurrentView = view;
        _ = CurrentView.OnNavigatedTo();
    }

    [RelayCommand]
    public void GoBack()
    {
        if (_history.Count > 0)
        {
            CurrentView = _history.Pop();
            _ = CurrentView.OnNavigatedTo();
        }
    }
}