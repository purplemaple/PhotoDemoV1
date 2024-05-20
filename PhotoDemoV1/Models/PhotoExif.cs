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
    public string? FNumber { get; set; }

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
    public (string? Width, string? Height) Resolution { get; set; }

    /// <summary>
    /// GPS
    /// </summary>
    public (string? Latitude, string? Longitude) GPS { get; set; }
}

public enum PhotoFormat
{
    NEF,

    ARW,

    JPG,

    JPEG,

    PNG,
}
