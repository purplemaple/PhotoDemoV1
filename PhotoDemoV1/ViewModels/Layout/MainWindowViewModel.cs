using Avalonia;
using Avalonia.Controls;
using PhotoDemoV1.Views;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhotoDemoV1.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private List<NavigationItemData> navigationItemDatas = [];
    public List<NavigationItemData> NavigationItemDatas
    {
        get => navigationItemDatas;
        set => this.RaiseAndSetIfChanged(ref navigationItemDatas, value);
    }

    private MainViewModel mainViewModel;
    public MainViewModel MainViewModel
    {
        get => mainViewModel;
        set => this.RaiseAndSetIfChanged(ref mainViewModel, value);
    }

    public MainWindowViewModel()
    {
        NavigationItemDatas =
        [
            new NavigationItemData
            {
                Text = "主页",
                //NavigateCommand = ReactiveCommand.Create(Navigate),
                Command = Navigate,
                ViewModelType = typeof(HomeViewModel)
            },

            new NavigationItemData
            {
                Text = "设置",
                //NavigateCommand = ReactiveCommand.Create<Unit>(Navigate)
                Command = Navigate,
                ViewModelType = typeof(SettingsViewModel)
            },

            new NavigationItemData
            {
                Text = "关于",
                //NavigateCommand = ReactiveCommand.Create<Unit>(Navigate)
                Command = Navigate,
                ViewModelType = typeof(AboutViewModel)
            },
        ];
        mainViewModel = new();

        Navigate(NavigationItemDatas.First());
    }

    public void Navigate(NavigationItemData itemData)
    {
        itemData.ViewModelType ??= typeof(HomeViewModel);
        if (itemData.ViewModelType == MainViewModel.CurrentViewModelType)
            return;
        MainViewModel.CurrentViewModelType = itemData.ViewModelType;
        MainViewModel.CurrentViewModel = Activator.CreateInstance(itemData.ViewModelType) as ViewModelBase;
    }
}

public class NavigationItemData
{
    public string? Icon { get; set; }

    public string? Text { get; set; }

    public Type? ViewModelType { get; set; }

    public ReactiveCommand<Unit, Unit>? NavigateCommand { get; set; }

    public Action<NavigationItemData>? Command { get; set; }
}
