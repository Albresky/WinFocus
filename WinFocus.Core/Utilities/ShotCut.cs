using System;
using System.IO;
using System.Reflection;
using IWshRuntimeLibrary;

namespace WinFocus.Core.Utilities;


/// <summary>
/// 创建桌面快捷方式
/// </summary>
public class ShortCut
{
    /// <summary>
    /// 快速创建
    /// </summary>
    /// <returns></returns>
    public static bool FastCreate(bool forceCreate = false)
    {
        CoreEngine.Current.Logger.Info($"创建桌面快捷方式");

        var shortCut = new ShortCut();
        var AppName = CoreEngine.Current.AppName;
        var AppRootDir = CoreEngine.Current.AppRootDirection;
        var b1 = shortCut.CreateDeskTopLik(AppName, "获取必应每日壁纸作为电脑桌面壁纸，并支持开机自动设置。", Path.Combine(AppRootDir, $"{AppName}.exe"), "logo", forceCreate);
        var b2 = shortCut.CreateDeskTopLik("一键设置壁纸", "一键设置必应每日壁纸", Path.Combine(AppRootDir, "AutoRunning.exe"), "autologo", forceCreate);
        return b1 || b2;
    }
    /// <summary>
    /// 创建桌面快捷方式
    /// </summary>
    /// <param name="name">目标名称</param>
    /// <param name="description">目标描述</param>
    /// <param name="targetFilePath">源文件路径</param>
    /// <param name="logoName">logo名称</param>
    /// <returns></returns>
    public bool CreateDeskTopLik(string name, string description, string targetFilePath, string logoName, bool forceCreate)
    {
        #region 创建桌面快捷方式
        var deskTop = System.Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        var dirPath = CoreEngine.Current.AppRootDirection;
        var exePath = Assembly.GetExecutingAssembly().Location;
        if (!forceCreate && System.IO.File.Exists($@"{deskTop}\{name}.lnk"))
        {
            return false;
        }
        var shell = new WshShell();
        var shortcut = (IWshShortcut)shell.CreateShortcut(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + $"{name}.lnk");
        shortcut.TargetPath = targetFilePath;         //目标文件
        shortcut.WorkingDirectory = dirPath;    //目标文件夹
        shortcut.WindowStyle = 1;               //目标应用程序的窗口状态分为普通、最大化、最小化【1,3,7】
        shortcut.Description = description;   //描述
        shortcut.IconLocation = $@"{dirPath}\Assets\{logoName}.ico";  //快捷方式图标
        shortcut.Arguments = "";
        shortcut.Hotkey = "ALT+X";              // 快捷键
        shortcut.Save();
        return true;
        #endregion
    }
}
