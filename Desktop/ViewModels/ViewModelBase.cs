using CommunityToolkit.Mvvm.ComponentModel;

namespace TCA.Desktop.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    public virtual void OnNavigatedTo() { }
}