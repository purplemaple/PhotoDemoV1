using Avalonia;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
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
        Navigations = new()
        {
            new NavigationViewModel
            {
                Text = "主页",
            },

            new NavigationViewModel
            {
                Text = "设置"
            },

            new NavigationViewModel
            {
                Text = "关于"
            },
        };
    }

    public void Navigate()
    {

    }
}

public class NavigationViewModel
{
    public string? Icon { get; set; }

    public string? Text { get; set; }

    public ICommand Command { get; set; }
}
