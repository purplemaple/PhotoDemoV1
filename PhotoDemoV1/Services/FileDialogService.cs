using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoDemoV1.Services;

public class FileService
{
    private static readonly Lazy<FileService> lazy = new(() => new FileService());
    public static FileService Instance => lazy.Value;

    /// <summary>
    /// 以默认选项打开文件夹
    /// </summary>
    /// <param name="view">基点视图</param>
    /// <returns>文件夹路径集合</returns>
    public async Task<IReadOnlyList<IStorageFolder>> OpenFolderAsync(TopLevel view)
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
    public async Task<IReadOnlyList<IStorageFolder>> OpenFolderAsync(TopLevel view, FolderPickerOpenOptions options)
    {
        return await view.StorageProvider.OpenFolderPickerAsync(options);
    }
}
