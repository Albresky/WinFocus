using System;
using System.ComponentModel;
using System.Drawing;
using WinFocus.Core.Utilities;

namespace WinFocus.Core;
public class CoreEngine
{
    #region 单例模式
    private static readonly object _lockObj = new();
    private static CoreEngine _current;
    /// <summary>
    /// 单例模式
    /// </summary>
    public static CoreEngine Current
    {
        get
        {
            lock (_lockObj)
            {
                _current ??= new CoreEngine();
                return _current;
            }
        }
    }

    #endregion

    /// <summary>
    /// APP名称
    /// </summary>
    public string AppName => System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

    
    /// <summary>
    /// 应用设置
    /// </summary>
    //public AppSettingOperation AppSetting { get; private set; } = new AppSettingOperation();
    public SettingsUtil AppSetting { get; private set; } = new();

    /// <summary>
    /// 程序根目录
    /// </summary>
    public string AppRootDirection { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

    /// <summary>
    /// 日志管理器
    /// </summary>
    public readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    /// <summary>
    /// 异步设置壁纸
    /// </summary>
    /// <param name="forceFromWeb">强制从网络获取</param>
    public void SetWallpaperAsync(bool forceFromWeb = false)
    {
        Current.Logger.Info($"设置桌面壁纸（异步）");
        var locker = new object();
        var isSuccess = false;
        using (var work = new BackgroundWorker())
        {
            work.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object work_sender, RunWorkerCompletedEventArgs work_e) =>
            {
                if (isSuccess)
                {
                    Current.Logger.Info($"设置桌面壁纸（异步）成功");
                }
                else
                {
                    Current.Logger.Info($"设置桌面壁纸（异步）失败");
                }
            });
            work.DoWork += new DoWorkEventHandler((object work_sender, DoWorkEventArgs work_e) =>
            {
                lock (locker)
                {
                    isSuccess = new BingWallpaperManager().SetWallpaper(forceFromWeb);
                }
            });
            work.RunWorkerAsync();
        }
    }

    /// <summary>
    /// 设置壁纸
    /// </summary>
    /// <param name="forceFromWeb">强制从网络获取</param>
    public void SetWallpaper(bool forceFromWeb = false)
    {
        Current.Logger.Info($"设置桌面壁纸");
        new BingWallpaperManager().SetWallpaper(forceFromWeb);
    }

    /// <summary>
    /// 获取壁纸图片
    /// </summary>
    /// <param name="forceFromWeb">强制从网络获取</param>
    /// <returns></returns>
    public Bitmap GetWallpaperImage(bool forceFromWeb = false)
    {
        Current.Logger.Info($"获取桌面壁纸Bitmap");
        return new BingWallpaperManager().GetWallpaperImage(forceFromWeb);
    }

    /// <summary>
    /// 下载壁纸图片
    /// </summary>
    /// <param name="date"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool DownloadWallpaperImage(DateTime date, out string result)
    {
        Current.Logger.Info($"下载桌面壁纸图片");
        return new BingWallpaperManager().DownloadWallpaperImage(date, out result);
    }
}
