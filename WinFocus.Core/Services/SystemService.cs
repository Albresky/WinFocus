using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace WinFocus.Core.Services;


public class SystemService
{
    public struct ScreenSize
    {
        public uint Width;
        public uint Height;
    }

    public static async Task<IReadOnlyList<StorageFile>> GetFilesInFilePickerAsync(string[] typeFilter)
    {
        var picker = GetPicker(typeFilter);
        return await picker.PickMultipleFilesAsync();
    }

    private static FileOpenPicker GetPicker(string[] typeFilter)
    {
        var picker = new FileOpenPicker();
        foreach (var t_filter in typeFilter)
        {
            picker.FileTypeFilter.Add(t_filter);
        }
        var hwnd = Process.GetCurrentProcess().MainWindowHandle;
        WinRT.Interop.InitializeWithWindow.Initialize(picker, hwnd);
        picker.ViewMode = PickerViewMode.Thumbnail;
        picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
        return picker;
    }
}
