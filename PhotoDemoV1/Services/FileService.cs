using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoDemoV1.Services;

public sealed class FileService
{
    private static readonly Lazy<FileService> lazy = new(() => new FileService());
    public static FileService Instance => lazy.Value;

    /// <summary>
    /// 以默认选项打开文件夹
    /// </summary>
    /// <param name="view">基点视图</param>
    /// <returns>文件夹路径集合</returns>
    public async Task<IReadOnlyList<IStorageFolder>> OpenFolderAsync(Visual view)
    {
        FolderPickerOpenOptions options = new()
        {
            Title = "选择文件夹"
        };
        return await OpenFolderAsync(view, options);
    }

    /// <summary>
    /// 以自定义选项打开文件夹
    /// </summary>
    /// <param name="view">基点视图</param>
    /// <param name="options">自定义选项</param>
    /// <returns>文件夹路径集合</returns>
    public async Task<IReadOnlyList<IStorageFolder>> OpenFolderAsync(Visual view, FolderPickerOpenOptions options)
    {
        //通过传递过来的控件获取其顶层视图(一般为 MainWindow)
        TopLevel? topLevel = (TopLevel?)view.GetVisualRoot();
        if (topLevel is null)
        {
            throw new InvalidOperationException("找不到顶级视图");
        }
        return await topLevel.StorageProvider.OpenFolderPickerAsync(options);
    }
}
