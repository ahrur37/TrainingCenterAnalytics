using TCA.Desktop.ViewModels;

namespace TCA.Desktop.Services;

public interface INavigator
{
    void NavigateTo(ViewModelBase viewModel);
    void GoBack();
}