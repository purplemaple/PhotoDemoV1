using ReactiveUI;
using System;

namespace PhotoDemoV1.ViewModels;

public class MainViewModel : ViewModelBase
{
    private Type? currentViewModelType;
    public Type? CurrentViewModelType
    {
        get => currentViewModelType;
        set => this.RaiseAndSetIfChanged(ref currentViewModelType, value);
    }

    private ViewModelBase? currentViewModel;
    public ViewModelBase? CurrentViewModel
    {
        get => currentViewModel;
        set => this.RaiseAndSetIfChanged(ref currentViewModel, value);
    }
}
