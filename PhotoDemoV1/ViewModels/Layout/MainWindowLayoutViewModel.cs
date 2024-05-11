using Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PhotoDemoV1.ViewModels;

public class MainWindowLayoutViewModel : ViewModelBase
{
    private List<NavigationViewModel> navigations = [];
    public List<NavigationViewModel> Navigations
    {
        get => navigations;
        set => this.RaiseAndSetIfChanged(ref navigations, value);
    }



    public MainWindowLayoutViewModel()
    {
        Navigations =
        [
            new NavigationViewModel
            {
                Text = "主页",
                NavigateCommand = ReactiveCommand.Create<Unit>(Navigate),
                Command = Navigate
            },

            new NavigationViewModel
            {
                Text = "设置",
                //NavigateCommand = ReactiveCommand.Create<Unit>(Navigate)
            },

            new NavigationViewModel
            {
                Text = "关于",
                //NavigateCommand = ReactiveCommand.Create<Unit>(Navigate)
            },
        ];
    }

    //public ReactiveCommand<Unit, Unit> NavigateCommand { get; }

    public void Navigate()
    {

    }

    public void Navigate(Unit parameter)
    {

    }
}

public class NavigationViewModel
{
    public string? Icon { get; set; }

    public string? Text { get; set; }

    public ReactiveCommand<Unit, Unit> NavigateCommand { get; set; }

    public Action Command { get; set; }
}
