using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using PhotoDemoV1.Services;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoDemoV1.ViewModels;

internal class HomeViewModel : ViewModelBase
{
    private List<string> inputPaths = [];
    public List<string> InputPaths
    {
        get => inputPaths;
        set => this.RaiseAndSetIfChanged(ref inputPaths, value);
    }

    private string outputPath = "";
    public string OutputPath
    {
        get => outputPath;
        set => this.RaiseAndSetIfChanged(ref outputPath, value);
    }

    public async void OpenInputFolderCommand(Control view)
    {
        //通过传递过来的控件获取其顶层视图(一般为 MainWindow)
        TopLevel? topLevel = (TopLevel?)view.GetVisualRoot();
        if(topLevel is null)
        {
            return;
        }
        FolderPickerOpenOptions options = new()
        {
            Title = "选择输入文件夹(多选)",
            AllowMultiple = true,
        };
        //通过顶层视图异步打开窗口
        var paths = await FileService.Instance.OpenFolderAsync(topLevel, options);
        InputPaths = paths.Select(x => x.Path.ToString()).ToList();
    }

    public async void OpenOutputFolderCommand(Control view)
    {
        //通过传递过来的控件获取其顶层视图(一般为 MainWindow)
        TopLevel? topLevel = (TopLevel?)view.GetVisualRoot();
        if (topLevel is null)
        {
            return;
        }
        FolderPickerOpenOptions options = new()
        {
            Title = "选择输出文件夹",
        };
        //通过顶层视图异步打开窗口
        var paths = await FileService.Instance.OpenFolderAsync(topLevel, options);
        if (paths.Any())
            OutputPath = paths.First().Path.ToString();
        OutputPath = Environment.CurrentDirectory + "\\tempOutput";
    }
}
