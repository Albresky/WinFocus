using System;
using System.ComponentModel;
using System.Drawing;
using WinFocus.Core.Utilities;

namespace WinFocus.Core;
public class CoreEngine
{
    #region ����ģʽ
    private static readonly object _lockObj = new();
    private static CoreEngine _current;
    /// <summary>
    /// ����ģʽ
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
    /// APP����
    /// </summary>
    public string AppName => System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

    
    /// <summary>
    /// Ӧ������
    /// </summary>
    //public AppSettingOperation AppSetting { get; private set; } = new AppSettingOperation();
    public SettingsUtil AppSetting { get; private set; } = new();

    /// <summary>
    /// �����Ŀ¼
    /// </summary>
    public string AppRootDirection { get; set; } = AppDomain.CurrentDomain.BaseDirectory;

    /// <summary>
    /// ��־������
    /// </summary>
    public readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

    /// <summary>
    /// �첽���ñ�ֽ
    /// </summary>
    /// <param name="forceFromWeb">ǿ�ƴ������ȡ</param>
    public void SetWallpaperAsync(bool forceFromWeb = false)
    {
        Current.Logger.Info($"���������ֽ���첽��");
        var locker = new object();
        var isSuccess = false;
        using (var work = new BackgroundWorker())
        {
            work.RunWorkerCompleted += new RunWorkerCompletedEventHandler((object work_sender, RunWorkerCompletedEventArgs work_e) =>
            {
                if (isSuccess)
                {
                    Current.Logger.Info($"���������ֽ���첽���ɹ�");
                }
                else
                {
                    Current.Logger.Info($"���������ֽ���첽��ʧ��");
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
    /// ���ñ�ֽ
    /// </summary>
    /// <param name="forceFromWeb">ǿ�ƴ������ȡ</param>
    public void SetWallpaper(bool forceFromWeb = false)
    {
        Current.Logger.Info($"���������ֽ");
        new BingWallpaperManager().SetWallpaper(forceFromWeb);
    }

    /// <summary>
    /// ��ȡ��ֽͼƬ
    /// </summary>
    /// <param name="forceFromWeb">ǿ�ƴ������ȡ</param>
    /// <returns></returns>
    public Bitmap GetWallpaperImage(bool forceFromWeb = false)
    {
        Current.Logger.Info($"��ȡ�����ֽBitmap");
        return new BingWallpaperManager().GetWallpaperImage(forceFromWeb);
    }

    /// <summary>
    /// ���ر�ֽͼƬ
    /// </summary>
    /// <param name="date"></param>
    /// <param name="result"></param>
    /// <returns></returns>
    public bool DownloadWallpaperImage(DateTime date, out string result)
    {
        Current.Logger.Info($"���������ֽͼƬ");
        return new BingWallpaperManager().DownloadWallpaperImage(date, out result);
    }
}
