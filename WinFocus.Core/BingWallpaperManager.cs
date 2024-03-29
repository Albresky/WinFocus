﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using static WinFocus.Core.Models.SettingsTypes;

namespace WinFocus.Core;

class BingWallpaperManager
{
    /// <summary>
    /// 获取图片路径
    /// </summary>
    /// <param name="days">倒数第几天</param>
    /// <returns></returns>
    public string GetBingURL(int days = 0, ImageSizeType imageSizeType = ImageSizeType.S_1080P)
    {
        var InfoUrl = $"http://cn.bing.com/HPImageArchive.aspx?idx={days}&n=1";
        CoreEngine.Current.Logger.Info($"Bing查询接口：{InfoUrl}");
        string ImageUrl;
        try
        {
            var request = (HttpWebRequest)WebRequest.Create(InfoUrl);
            request.Method = "GET"; request.ContentType = "text/html;charset=UTF-8";
            string XmlString;
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var myResponseStream = response.GetResponseStream();
                using (var myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8))
                {
                    XmlString = myStreamReader.ReadToEnd();
                }
            }
            // 定义正则表达式用来匹配标签
            var regImg = new Regex("<Url>(?<imgUrl>.*?)</Url>", RegexOptions.IgnoreCase);
            // 搜索匹配的字符串
            var matches = regImg.Matches(XmlString);
            // 取得匹配项列表
            ImageUrl = "http://www.bing.com" + matches[0].Groups["imgUrl"].Value;
            //保存copyright
            if (days == 0)
            {
                CoreEngine.Current.Logger.Info("Bing查询接口：保存Image Copyright");
                // 定义正则表达式用来匹配标签
                var regCopyright = new Regex("<copyright>(?<imgCopyright>.*?)</copyright>", RegexOptions.IgnoreCase);
                // 搜索匹配的字符串
                var matchesCopyright = regCopyright.Matches(XmlString);
                // 取得匹配项列表
                var copyright = matchesCopyright[0].Groups["imgCopyright"].Value;
                CoreEngine.Current.AppSetting.SetCopyright(copyright);
                CoreEngine.Current.Logger.Info($"Bing查询接口：Copyright:{copyright}");
            }
            switch (imageSizeType)
            {
                case ImageSizeType.S_2160P:
                    ImageUrl = ImageUrl.Replace("1920x1080", "UHD");
                    break;
                case ImageSizeType.S_1200P:
                    ImageUrl = ImageUrl.Replace("1920x1080", "1920x1200");
                    break;
                case ImageSizeType.S_720P:
                    ImageUrl = ImageUrl.Replace("1920x1080", "1366x768");
                    break;
                default: break;
            }
            CoreEngine.Current.Logger.Info($"Bing获取到接口的壁纸链接：{ImageUrl}");
            return ImageUrl;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// 从本地或网络设置墙纸
    /// </summary>
    /// <param name="forceFromWeb">强制从网络获取</param>
    /// <returns>是否设置成功</returns>
    public bool SetWallpaper(bool forceFromWeb = false)
    {
        var imageFolderPath = CoreEngine.Current.AppSetting.GetAssetsPath(PathType.BingWallpaperImagePath);
        var imageFilePath = Path.Combine(imageFolderPath, $"bing{DateTime.Now:yyyyMMdd}.jpg");
        CoreEngine.Current.Logger.Info($"从本地或网络设置墙纸——强制从网络获取：{(forceFromWeb ? "是" : "否")}——文件名：bing{DateTime.Now.ToString("yyyyMMdd")}.jpg");
        if (forceFromWeb || !File.Exists(imageFilePath))//本地不存在文件
        {
            CoreEngine.Current.Logger.Info($"本地未检测到图片，启用网络下载");
            var bingUrl = GetBingURL();
            if (string.IsNullOrEmpty(bingUrl))
            {
                return false;
            }
            var webreq = (HttpWebRequest)WebRequest.Create(bingUrl);
            webreq.Method = "Get";
            try
            {
                using (var webres = webreq.GetResponse())//GetResponse
                {
                    using (var stream = webres.GetResponseStream())
                    {
                        using (var bmpWallpaper = (Bitmap)Image.FromStream(stream))
                        {
                            if (!Directory.Exists(imageFolderPath))
                            {
                                Directory.CreateDirectory(imageFolderPath);
                            }
                            bmpWallpaper.Save(imageFilePath, ImageFormat.Jpeg);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                CoreEngine.Current.Logger.Error(e, $"下载壁纸失败：{e.Message}");
                return false;
            }
        }
        try
        {
            WallpaperService.Set(imageFilePath);
        }
        catch (Exception e)
        {
            CoreEngine.Current.Logger.Error(e, $"设置壁纸失败：系统接口调用错误");
            return false;
        }
        return true;
    }

    /// <summary>
    /// 从本地或网络获取当天最新的图片Bitmap
    /// </summary>
    /// <param name="forceFromWeb">强制从网络获取</param>
    /// <returns></returns>
    public Bitmap GetWallpaperImage(bool forceFromWeb = false)
    {
        var imageFolderPath = CoreEngine.Current.AppSetting.GetAssetsPath(PathType.BingWallpaperImagePath);
        var imageFilePath = Path.Combine(imageFolderPath, $"bing{DateTime.Now:yyyyMMdd}.jpg");
        CoreEngine.Current.Logger.Info($"从本地或网络获取当天最新的图片Bitmap——强制从网络获取：{(forceFromWeb ? "是" : "否")}——文件名：bing{DateTime.Now.ToString("yyyyMMdd")}.jpg");
        if (forceFromWeb || !File.Exists(imageFilePath))//不存在文件
        {
            CoreEngine.Current.Logger.Info($"本地未检测到图片，启用网络下载");
            var bingUrl = GetBingURL();
            if (string.IsNullOrEmpty(bingUrl))
            {
                return null;
            }
            var webreq = (HttpWebRequest)WebRequest.Create(bingUrl);
            webreq.Method = "Get";
            try
            {
                using (var webres = webreq.GetResponse())//GetResponse
                {
                    using (var stream = webres.GetResponseStream())
                    {
                        var bmpWallpaper = (Bitmap)Image.FromStream(stream);

                        if (!Directory.Exists(imageFolderPath))
                        {
                            Directory.CreateDirectory(imageFolderPath);
                        }
                        bmpWallpaper.Save(imageFilePath, ImageFormat.Jpeg);
                        return bmpWallpaper;

                    }
                }
            }
            catch (Exception e)
            {
                CoreEngine.Current.Logger.Error(e, $"下载壁纸失败：网络连接失败");
                return null;
            }
        }
        else
        {
            Bitmap bitmap;
            try
            {
                bitmap = new Bitmap(imageFilePath);
            }
            catch (Exception e)
            {
                CoreEngine.Current.Logger.Error(e, $"获取本地Bitmap文件失败");
                return null;//文件读取失败
            }

            return bitmap;
        }
    }

    /// <summary>
    /// 下载壁纸
    /// </summary>
    /// <param name="date"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool DownloadWallpaperImage(DateTime date, out string result)
    {

        var imageFolderPath = CoreEngine.Current.AppSetting.GetAssetsPath(PathType.BingWallpaperImagePath);
        var imageFilePath = Path.Combine(imageFolderPath, $"bing{date.ToString("yyyyMMdd")}.jpg");
        CoreEngine.Current.Logger.Info($"下载壁纸——强制从网络获取——文件名：bing{DateTime.Now.ToString("yyyyMMdd")}.jpg");
        if (File.Exists(imageFilePath))//存在文件
        {
            result = "文件已存在";
            CoreEngine.Current.Logger.Info($"下载壁纸失败：文件已存在");
            return false;
        }
        var interval = new TimeSpan(DateTime.Now.Ticks - date.Ticks).Days;
        var bingUrl = GetBingURL(interval);
        if (string.IsNullOrEmpty(bingUrl))
        {
            result = "接口连接失败";
            CoreEngine.Current.Logger.Info($"下载壁纸失败：接口连接失败");
            return false;
        }
        var webreq = (HttpWebRequest)WebRequest.Create(bingUrl);
        webreq.Method = "Get";
        try
        {
            using (var webres = webreq.GetResponse())//GetResponse
            {
                using (var stream = webres.GetResponseStream())
                {
                    using (var bmpWallpaper = (Bitmap)Image.FromStream(stream))
                    {
                        if (!Directory.Exists(imageFolderPath))
                        {
                            Directory.CreateDirectory(imageFolderPath);
                        }
                        bmpWallpaper.Save(imageFilePath, ImageFormat.Jpeg);
                    }
                }
            }
        }
        catch (Exception e)
        {
            result = "下载壁纸失败";
            CoreEngine.Current.Logger.Error(e, $"下载壁纸失败：网络连接失败");
            return false;
        }
        result = "下载成功";
        CoreEngine.Current.Logger.Info($"下载壁纸成功：bing{DateTime.Now.ToString("yyyyMMdd")}.jpg");
        return true;
    }
}
