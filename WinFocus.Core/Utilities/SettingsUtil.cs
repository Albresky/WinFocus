using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq.Expressions;
using Windows.Storage;
using WinFocus.Core.Models;
using static WinFocus.Core.Models.SettingsTypes;

namespace WinFocus.Core.Utilities;

/// <summary>
/// 设置组件
/// </summary>
public class SettingsUtil
{
    private readonly List<ContainerType> containerTypes = new() { ContainerType.GlobalSettings, ContainerType.ImageInfo };

    public ApplicationDataContainer LocalSettings = ApplicationData.Current.LocalSettings;

    public ApplicationDataContainer ImageContainer;

    public SettingsUtil()
    {
        InitContainers();
    }

    /// <summary>
    /// 初始化配置中的默认容器
    /// </summary>
    private void InitContainers()
    {

        foreach (var t_container in containerTypes)
        {
            var key = t_container.ToString();
            if (!LocalSettings.Containers.ContainsKey(key))
            {

                LocalSettings.CreateContainer(key, ApplicationDataCreateDisposition.Always);
            }
        }
    }

    /// <summary>
    /// 初始化各种默认路径
    /// </summary>
    private void InitFirstStart()
    {
        Trace.WriteLine("SettingsUtil.InitFirstStart()");
        foreach (var pathType in Enum.GetNames(typeof(PathType)))
        {
            UpdateSettings(ContainerType.GlobalSettings,
                pathType,
                Path.Combine(CoreEngine.Current.AppRootDirection, pathType));
        }
    }

    /// <summary>
    /// 设置图片保存路径
    /// </summary>
    /// <param name="path"></param>
    public void SetAssetsPath(string path, PathType pathType) => UpdateSettings(ContainerType.GlobalSettings, pathType.ToString(), path);

    /// <summary>
    /// 获取图片保存路径
    /// </summary>
    public string GetAssetsPath(PathType pathType)
    {
        var path = ReadSetting(ContainerType.GlobalSettings, pathType.ToString());
        if (string.IsNullOrEmpty(path))
        {
            return Path.Combine(CoreEngine.Current.AppRootDirection, pathType.ToString());
        }
        else
        {
            return path;
        }
    }

    /// <summary>
    /// 设置关闭按钮是否直接关闭程序
    /// </summary>
    public void SetCloseDirectly(bool flag) => UpdateSettings(ContainerType.GlobalSettings, "CloseDirectly", flag);

    /// <summary>
    /// 获取关闭按钮是否直接关闭程序
    /// </summary>
    public bool GetCloseDirectly() => ReadSettingBoolean(ContainerType.GlobalSettings, "CloseDirectly", true);


    /// <summary>
    /// 设置程序是否开机自动运行
    /// </summary>
    public void SetAutoStart(bool flag) => UpdateSettings(ContainerType.GlobalSettings, "AutoStart", flag);

    /// <summary>
    /// 获取程序是否开机自动运行
    /// </summary>
    public bool GetAutoStart() => ReadSettingBoolean(ContainerType.GlobalSettings, "AutoStart", false);


    /// <summary>
    /// 设置程序主题
    /// </summary>
    /// <param name="theme"></param>
    public void SetAppTheme(string theme) => UpdateSettings(ContainerType.GlobalSettings, "AppTheme", theme);


    /// <summary>
    /// 获取程序主题
    /// </summary>
    public string GetAppTheme() => ReadSetting(ContainerType.GlobalSettings, "AppTheme", "Light");

    /// <summary>
    /// 获取程序是否第一次运行
    /// </summary>
    public bool GetAppFirstStart()
    {
        var result = ReadSettingBoolean(ContainerType.GlobalSettings, "AppFirstStart", true);
        if (result)
        {
            UpdateSettings(ContainerType.GlobalSettings, "AppFirstStart", false);
            InitFirstStart();
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取图片信息
    /// </summary>
    public string GetCopyright() => ReadSetting(ContainerType.ImageInfo, "TodayImageCopyright");

    /// <summary>
    /// 设置图片信息
    /// </summary>
    /// <param name="copyright"></param>
    public void SetCopyright(string copyright) => UpdateSettings(ContainerType.ImageInfo, "TodayImageCopyright", copyright);


    /// <summary>
    /// 获取壁纸尺寸类型
    /// </summary>
    /// <returns>string</returns>
    public string GetBingSizeType()
    {
        return SettingsTypes.ImageSizeTypeConverterEnumToStr((ImageSizeType)ReadSettingInt32(ContainerType.GlobalSettings, "BingSizeType"));
    }


    /// <summary>
    /// 设置壁纸尺寸类型
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public void SetBingSizeType(object type)
    {
        if (type is string strSize)
        {
            UpdateSettings(ContainerType.GlobalSettings, "BingSizeType", (int)SettingsTypes.ImageSizeTypeConverterStrToEnum(strSize));
        }
        else if (type is int intSize)
        {
            UpdateSettings(ContainerType.GlobalSettings, "BingSizeType", intSize);
        }
        else
        {
            throw new ArgumentException("ExceptionInUpdateSettingsBingSizeType");
        }
    }

    /// <summary>
    /// 读取所有配置信息
    /// </summary>
    private void ReadAllSettings()
    {
        try
        {
            if (LocalSettings.Values.Count == 0)
            {
                Trace.WriteLine("AppSettings is empty.");
            }
            else
            {
                foreach (var item in LocalSettings.Values)
                {
                    Trace.WriteLine($"Key:{item.Key},Value:{item.Value}");
                }
            }
        }
        catch (Exception)
        {
            Console.WriteLine("Error reading app settings");
        }
    }

    /// <summary>
    /// 读取单项配置信息
    /// </summary>
    /// <param name="containerType"></param>
    /// <param name="key"></param>
    /// <param name="def"></param>
    /// <returns></returns>
    private string ReadSetting(ContainerType containerType, string key, string def = "")
    {
        try
        {
            var result = LocalSettings.Containers[containerType.ToString()].Values[key].ToString() ?? def;
            return result;
        }
        catch (NullReferenceException)
        {
            return def;
        }
    }

    /// <summary>
    /// 读取单项配置信息（int类型）
    /// </summary>
    /// <param name="containerType"></param>
    /// <param name="key"></param>
    /// <param name="def"></param>
    /// <returns></returns>
    private int ReadSettingInt32(ContainerType containerType, string key, int def = 0)
    {
        var valueStr = ReadSetting(containerType, key);
        try
        {
            var value = int.Parse(valueStr);
            return value;
        }
        catch
        {
            return def;
        }
    }

    /// <summary>
    /// 读取单项配置信息（bool类型）
    /// </summary>
    /// <param name="containerType"></param>
    /// <param name="key"></param>
    /// <param name="def"></param>
    /// <returns></returns>
    private bool ReadSettingBoolean(ContainerType containerType, string key, bool def = true)
    {
        var valueStr = ReadSetting(containerType, key);
        if (valueStr == "true")
        {
            return true;
        }
        else if (valueStr == "false")
        {
            return false;
        }
        else
        {
            return def;
        }
    }

    /// <summary>
    /// 更新配置信息
    /// </summary>
    /// <param name="containerType"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    private void UpdateSettings(ContainerType containerType, string key, string value)
    {
        try
        {
            if (LocalSettings.Containers[containerType.ToString()].Values[key] == null)
            {
                LocalSettings.Containers[containerType.ToString()].Values.Add(key, value);
            }
            else
            {
                LocalSettings.Containers[containerType.ToString()].Values[key] = value;
            }
        }
        catch (Exception)
        {
            Trace.WriteLine("Error writing app settings");
        }
    }

    /// <summary>
    /// 更新配置信息（bool类型）
    /// </summary>
    /// <param name="containerType"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    private void UpdateSettings(ContainerType containerType, string key, bool value)
    {
        UpdateSettings(containerType, key, value ? "true" : "false");
    }

    /// <summary>
    /// 更新配置信息（int类型）
    /// </summary>
    /// <param name="containerType"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    private void UpdateSettings(ContainerType containerType, string key, int value)
    {
        UpdateSettings(containerType, key, value.ToString());
    }
}
