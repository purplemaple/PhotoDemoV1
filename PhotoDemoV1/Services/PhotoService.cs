using Avalonia.Media.Imaging;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor;
using PhotoDemoV1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PhotoDemoV1.Services;

public sealed class PhotoService
{
    private static readonly Lazy<PhotoService> lazy = new(() => new PhotoService());
    public static PhotoService Instance => lazy.Value;

    public Dictionary<string, PhotoExif> PhotoExifs { get; set; } = [];

    public Task ReadExifData(string foldrPath)
    {
        var imagePaths = System.IO.Directory.EnumerateFiles(foldrPath, ".", SearchOption.AllDirectories);

        foreach (var imagePath in imagePaths)
        {
            PhotoExif photo = new();
            try
            {
                //读取图片的Exif信息
                List<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(imagePath).ToList();

                photo.Make = directories[0].GetDescription(ExifDirectoryBase.TagMake);
                photo.Model = directories[0].GetDescription(ExifDirectoryBase.TagModel);

                photo.LensMake = directories[5].GetDescription(ExifDirectoryBase.TagLensMake);
                photo.LensModel = directories[5].GetDescription(ExifDirectoryBase.TagLensModel);
                photo.FocalLength = directories[5].GetDescription(ExifDirectoryBase.TagFocalLength);
                photo.FNumber = directories[5].GetDescription(ExifDirectoryBase.TagFNumber);
                photo.ExposureTime = directories[5].GetDescription(ExifDirectoryBase.TagExposureTime);
                photo.ISOEquivalent = directories[5].GetDescription(ExifDirectoryBase.TagIsoEquivalent);
                photo.DateTimeOriginal = directories[5].GetDescription(ExifDirectoryBase.TagDateTimeOriginal);
                photo.ExposureProgram = directories[5].GetDescription(ExifDirectoryBase.TagExposureProgram);
                photo.WhiteBalance = directories[5].GetDescription(ExifDirectoryBase.TagWhiteBalance);

                //photo.Resolution = (directories[5].GetDescription(ExifDirectoryBase.TagImageHeight), directories[5].GetDescription(ExifDirectoryBase.TagImageWidth));
                PhotoExifs.Add(imagePath, photo);
            }
            catch (ImageProcessingException)
            {
                Console.WriteLine("不支持的图片格式或损坏的文件：" + imagePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生错误：" + ex.Message);
            }
        }
        return Task.CompletedTask;
    }

    public void PhotoProcessor(string foldPath)
    {
        //1. 在照片的原路径下创建新文件夹
        string copyFolderPath = Path.Combine(foldPath, "CopiedPhotos");
        System.IO.Directory.CreateDirectory(copyFolderPath);

        //2. 将照片复制到新文件夹中
        var imageFiles = System.IO.Directory.EnumerateFiles(foldPath, "*.*", SearchOption.AllDirectories);
        foreach (var image in imageFiles)
        {
            string fileName = Path.GetFileName(image) + ".Copy";
            string destFile = Path.Combine(copyFolderPath, fileName);
            File.Copy(image, destFile, true);
        }

        //3.添加边框并显示Exif信息
        var copyImageFiles = System.IO.Directory.EnumerateFiles(copyFolderPath, "*.*", SearchOption.AllDirectories);
        foreach (var image in copyImageFiles)
        {

        }
    }

    RenderTargetBitmap AddBorder(Bitmap bitmap, (string? Height, string? Width) Resolution)
    {
        double borderScale = 1.0;
        double bottomBorderScale = 1.0;

        //计算边框宽度，使其与原始图片的比例保持一致
        int borderWidth = (int)(bitmap.PixelSize.Width * borderScale);
        int bottomBorderWidth = (int)(bitmap.PixelSize.Height * bottomBorderScale);

        //创建一个新的RenderTargetBitmap, 考虑边框的大小
        var renderTarget = new RenderTargetBitmap(
            new Avalonia.PixelSize(bitmap.PixelSize.Width + 2 * borderWidth,
                                           bitmap.PixelSize.Height + 2 * borderWidth + bottomBorderWidth));

        using var ctx = renderTarget.CreateDrawingContext();
        //绘制原始图片
        Rect sourRect = new(0, 0, bitmap.PixelSize.Width, bitmap.PixelSize.Height);
        Rect targetRect = new(borderWidth, borderWidth, bitmap.PixelSize.Width, bitmap.PixelSize.Height);
        ctx.DrawImage(bitmap, sourRect, targetRect);

        //绘制边框
        SolidColorBrush borderBrush = new(Colors.Black);

        // 上边框
        ctx.FillRectangle(borderBrush, new Rect(0, 0, renderTarget.PixelSize.Width, borderWidth));

        // 左边框
        ctx.FillRectangle(borderBrush, new Rect(0, 0, borderWidth, renderTarget.PixelSize.Height));

        // 右边框
        ctx.FillRectangle(borderBrush, new Rect(renderTarget.PixelSize.Width - borderWidth, 0, borderWidth, renderTarget.PixelSize.Height));

        // 下边框
        ctx.FillRectangle(borderBrush, new Rect(0, renderTarget.PixelSize.Height - bottomBorderWidth, renderTarget.PixelSize.Width, bottomBorderWidth));

        return renderTarget;
    }

    void DrawOnBorder(RenderTargetBitmap framedPhoto, Dictionary<string, PhotoExif> photoExifs)
    {


    }

    async Task SaveToFileAsync(IBitmapImpl bitmap, string filePath)
    {

    }
}
