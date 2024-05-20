using Avalonia.Controls;
using Avalonia.Platform.Storage;
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
    private List<string> inputPaths =
    [
        //占个位
        new("")
    ];
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

    //默认展开第一个
    public bool IsFirstItem { get; set; } = true;

    public async void OpenInputFolderCommand(Control view)
    {
        FolderPickerOpenOptions options = new()
        {
            Title = "选择输入文件夹(多选)",
            AllowMultiple = true,
        };
        //通过顶层视图异步打开窗口
        var paths = await FileService.Instance.OpenFolderAsync(view, options);
        if (paths.Any())
            InputPaths = paths.Select(x => x.Path.ToString()[8..]).ToList();
    }

    public async void OpenOutputFolderCommand(Control view)
    {
        FolderPickerOpenOptions options = new()
        {
            Title = "选择输出文件夹",
        };
        //通过顶层视图异步打开窗口
        var paths = await FileService.Instance.OpenFolderAsync(view, options);
        if (paths.Any())
        {
            //因为输出目录只有一个，因此直接取索引0。从第8个字符开始往后截取，以去掉 file:///
            OutputPath = paths[0].Path.ToString()[8..];
        }
        else if (OutputPath == "")
            OutputPath = Environment.CurrentDirectory + "\\tempOutput";
    }

    public async void StartCommand()
    {
        await PhotoService.Instance.ReadExifData(InputPaths[0], OutputPath);
    }
}
