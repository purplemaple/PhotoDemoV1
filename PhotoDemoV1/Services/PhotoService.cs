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
using ImageMagick;

namespace PhotoDemoV1.Services;

public sealed class PhotoService
{
    private static readonly Lazy<PhotoService> lazy = new(() => new PhotoService());
    public static PhotoService Instance => lazy.Value;

    public Dictionary<string, PhotoExif> PhotoExifs { get; set; } = [];

    public Task ReadExifData(string inputFoldrPath, string outputFoldrPath)
    {
        List<string>? imagePaths = System.IO.Directory.EnumerateFiles(inputFoldrPath, ".", SearchOption.AllDirectories).ToList();

        //多线程处理
        imagePaths.AsParallel().ForAll(imagePath =>
        {
            PhotoExif photo = new();
            try
            {
                //读取图片的Exif信息
                List<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(imagePath).ToList();

                photo.Make = directories[0].GetDescription(ExifDirectoryBase.TagMake);
                photo.Model = directories[0].GetDescription(ExifDirectoryBase.TagModel);

                photo.Resolution = (directories[2].GetDescription(ExifDirectoryBase.TagImageWidth), directories[2].GetDescription(ExifDirectoryBase.TagImageHeight));

                photo.LensMake = directories[5].GetDescription(ExifDirectoryBase.TagLensMake);
                photo.LensModel = directories[5].GetDescription(ExifDirectoryBase.TagLensModel);
                photo.FocalLength = directories[5].GetDescription(ExifDirectoryBase.TagFocalLength);
                photo.FNumber = directories[5].GetDescription(ExifDirectoryBase.TagFNumber);
                photo.ExposureTime = directories[5].GetDescription(ExifDirectoryBase.TagExposureTime);
                photo.ISOEquivalent = directories[5].GetDescription(ExifDirectoryBase.TagIsoEquivalent);
                photo.DateTimeOriginal = directories[5].GetDescription(ExifDirectoryBase.TagDateTimeOriginal);
                photo.ExposureProgram = directories[5].GetDescription(ExifDirectoryBase.TagExposureProgram);
                photo.WhiteBalance = directories[5].GetDescription(ExifDirectoryBase.TagWhiteBalance);

                PhotoExifs.Add(imagePath, photo);
            }
            catch (ImageProcessingException e)
            {
                Console.WriteLine("不支持的图片格式或损坏的文件：" + imagePath);
                throw new ImageProcessingException(e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生错误：" + ex.Message);
            }
            AddBorder(imagePath, outputFoldrPath);
        });

        return Task.CompletedTask;
    }

    public Task AddBorder(string inputPath, string outputPath)
    {
        string fileName = Path.GetFileName(inputPath);

        int leftPx = 120, rightPx = 120, topPx = 120;
        int bottomPx = 360;

        using var image = new MagickImage(inputPath, MagickFormat.Jpg);

        // 不压缩
        image.SetCompression(CompressionMethod.NoCompression);
        image.Quality = 100;

        //保存EXIF信息
        IExifProfile? originalExif = image.GetExifProfile();

        // 计算新的宽度和高度
        int newWidth = image.Width + leftPx + rightPx;
        int newHeight = image.Height + topPx + bottomPx;

        // 创建新的画布
        using var newImage = new MagickImage(MagickColors.White, newWidth, newHeight);

        // 将原始图片绘制到新画布上
        newImage.Composite(image, leftPx, topPx, CompositeOperator.Over);

        // 如果原始图像有EXIF信息，将其添加到处理后的图像
        if (originalExif != null)
        {
            newImage.SetProfile(originalExif);
        }

        // 保存或处理新图片
        newImage.Write(Path.Combine(outputPath, fileName), MagickFormat.Jpg);

        return Task.CompletedTask;
    }
}
