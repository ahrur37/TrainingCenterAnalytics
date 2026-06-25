using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using TCA.Desktop.Models;
using TCA.Desktop.Services;
using Microsoft.Extensions.DependencyInjection;

namespace TCA.Desktop.ViewModels;

public partial class DashboardViewModel : ViewModelBase
{
    private readonly ApiService _apiService;
    private readonly INavigator _navigator;
    
    private int _totalRequests;
    private int _closedRequests;
    private double _closureRate;

    public int TotalRequests
    {
        get => _totalRequests;
        set => SetProperty(ref _totalRequests, value);
    }

    public int ClosedRequests
    {
        get => _closedRequests;
        set => SetProperty(ref _closedRequests, value);
    }

    public double ClosureRate
    {
        get => _closureRate;
        set => SetProperty(ref _closureRate, value);
    }

    [ObservableProperty] private ISeries[] _dynamicsSeries = Array.Empty<ISeries>();
    [ObservableProperty] private Axis[] _dynamicsXAxes = new[] { new Axis { Labels = new List<string>() } };

    [ObservableProperty] private ISeries[] _popularDirectionsSeries = Array.Empty<ISeries>();
    [ObservableProperty] private Axis[] _directionsXAxes = new[] { new Axis { Labels = new List<string>() } };

    [ObservableProperty] private ISeries[] _statusesSeries = Array.Empty<ISeries>();

    public DashboardViewModel(ApiService apiService, INavigator navigator)
    {
        _apiService = apiService;
        _navigator = navigator;
    }

    public override async Task OnNavigatedTo()
    {
        await LoadAnalyticsDataAsync();
    }

    [RelayCommand]
    private void GoBack()
    {
        _navigator.NavigateTo(App.Services.GetRequiredService<LobbyViewModel>());
    }

    private async Task LoadAnalyticsDataAsync()
    {
        try
        {
            var summary = await _apiService.GetAnalyticsSummary();
            if (summary != null)
            {
                TotalRequests = summary.TotalRequests;
                ClosedRequests = summary.ClosedRequests;
                ClosureRate = summary.ClosureRate;
            }

            var dynamics = await _apiService.GetAnalyticsDynamics();
            if (dynamics != null && dynamics.Any())
            {
                DynamicsXAxes = new[] { new Axis { Labels = dynamics.Select(d => d.Date).ToList() } };
                DynamicsSeries = new ISeries[]
                {
                    new LineSeries<int>
                    {
                        Values = dynamics.Select(d => d.Count).ToArray(),
                        Name = "Новые заявки",
                        GeometrySize = 10,
                        LineSmoothness = 0.5,
                        Stroke = new SolidColorPaint(SKColors.CornflowerBlue) { StrokeThickness = 3 },
                        Fill = new SolidColorPaint(SKColors.CornflowerBlue.WithAlpha(50))
                    }
                };
            }

            var directions = await _apiService.GetPopularDirections();
            if (directions != null && directions.Any())
            {
                DirectionsXAxes = new[] 
                { 
                    new Axis 
                    { 
                        Labels = directions.Select(d => d.DirectionName).ToList(),
                        LabelsRotation = -45,
                        MinStep = 1
                    } 
                };
    
                PopularDirectionsSeries = new ISeries[]
                {
                    new ColumnSeries<int>
                    {
                        Values = directions.Select(d => d.Count).ToArray(),
                        Name = "Заявки",
                        Fill = new SolidColorPaint(SKColors.MediumPurple)
                    }
                };
            }

            var statuses = await _apiService.GetStatusesDistribution();
            if (statuses != null && statuses.Any())
            {
                var pieSeries = new List<ISeries>();
                foreach (var status in statuses)
                {
                    pieSeries.Add(new PieSeries<int>
                    {
                        Values = new[] { status.Count },
                        Name = status.StatusName,
                        DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer,
                        DataLabelsFormatter = point => $"{point.Context.Series.Name}: {point.Coordinate.PrimaryValue}"
                    });
                }
                StatusesSeries = pieSeries.ToArray();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[DashboardViewModel] Error loading analytics: {ex.Message}");
        }
    }
}