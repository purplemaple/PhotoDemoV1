using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoDemoV1.Models;

public sealed class PhotoExifReader
{
    public Dictionary<string, PhotoExif> PhotoExifs { get; set; } = [];

    public void ReadExifData(string foldrPath)
    {
        var imagesFiles = System.IO.Directory.EnumerateFiles(foldrPath, ".", SearchOption.AllDirectories);

        foreach(var image in imagesFiles)
        {
            try
            {
                //读取图片的Exif信息
                var exifs = ImageMetadataReader.ReadMetadata(image);

                foreach( var exif in exifs)
                {
                    PhotoExifs.Add(image, new PhotoExif()
                    {
                        Make = exif.GetDescription(ExifDirectoryBase.TagMake),
                        Model = exif.GetDescription(ExifDirectoryBase.TagModel),
                        LensMake = exif.GetDescription(ExifDirectoryBase.TagLensMake),
                        LensModel = exif.GetDescription(ExifDirectoryBase.TagLensModel),
                        FocalLength = exif.GetDescription(ExifDirectoryBase.TagFocalLength),
                        Aperture = exif.GetDescription(ExifDirectoryBase.TagAperture),
                        ExposureTime = exif.GetDescription(ExifDirectoryBase.TagExposureTime),
                        ISOEquivalent = exif.GetDescription(ExifDirectoryBase.TagIsoEquivalent),
                        DateTimeOriginal = exif.GetDescription(ExifDirectoryBase.TagDateTimeOriginal),
                        ExposureProgram = exif.GetDescription(ExifDirectoryBase.TagExposureProgram),
                        WhiteBalance = exif.GetDescription(ExifDirectoryBase.TagWhiteBalance),
                        Resolution = (exif.GetDescription(ExifDirectoryBase.TagImageHeight), exif.GetDescription(ExifDirectoryBase.TagImageHeight)),
                        //GPS = ???
                    });
                }
            }
            catch(ImageProcessingException)
            {
                Console.WriteLine("不支持的图片格式或损坏的文件：" + image);
            }
            catch(Exception ex)
            {
                Console.WriteLine("发生错误：" + ex.Message);
            }
        }
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

public sealed class PhotoExif
{
    /// <summary>
    /// 相机品牌
    /// </summary>
    public string? Make { get; set; }

    /// <summary>
    /// 相机型号
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// 镜头品牌
    /// </summary>
    public string? LensMake { get; set; }

    /// <summary>
    /// 镜头型号
    /// </summary>
    public string? LensModel { get; set; }

    /// <summary>
    /// 所用焦距
    /// </summary>
    public string? FocalLength { get; set; }

    /// <summary>
    /// 光圈值
    /// </summary>
    public string? Aperture { get; set; }

    /// <summary>
    /// 快门速度
    /// </summary>
    public string? ExposureTime { get; set; }

    /// <summary>
    /// ISO
    /// </summary>
    public string? ISOEquivalent { get; set; }

    /// <summary>
    /// 拍摄时间
    /// </summary>
    public string? DateTimeOriginal { get; set; }

    /// <summary>
    /// 曝光模式
    /// </summary>
    public string? ExposureProgram { get; set; }

    /// <summary>
    /// 白平衡
    /// </summary>
    public string? WhiteBalance { get; set; }

    /// <summary>
    /// 分辨率
    /// </summary>
    public (string? Height, string? Width) Resolution { get; set; }

    /// <summary>
    /// GPS
    /// </summary>
    public (string? Latitude, string? Longitude) GPS { get; set; }
}
