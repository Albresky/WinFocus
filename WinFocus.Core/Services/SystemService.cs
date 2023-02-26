using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace WinFocus.Core.Services;


public class SystemService
{
    public enum PickerType
    {
        SingleFile,
        SingleFolder,
        MuliFile,
        MultiFolder
    }

    private static IntPtr _hwnd = Process.GetCurrentProcess().MainWindowHandle;

    public struct ScreenSize
    {
        public uint Width;
        public uint Height;
    }

    public static async Task<IReadOnlyList<StorageFile>> GetFilesInFilePickerAsync(string[] typeFilter)
    {
        var picker = GetFileOpenPicker(typeFilter);
        return await picker.PickMultipleFilesAsync();
    }

    public static async Task<StorageFolder> GetFolderPickerAsync(string[] typeFilter)
    {
        var picker = GetFolderOpenPicker(typeFilter);
        return await picker.PickSingleFolderAsync();
    }

    private static FileOpenPicker GetFileOpenPicker(string[] typeFilter)
    {
        var picker = new FileOpenPicker();
        if (typeFilter != null)
        {
            foreach (var t_filter in typeFilter)
            {
                picker.FileTypeFilter.Add(t_filter);
            }
        }

        WinRT.Interop.InitializeWithWindow.Initialize(picker, _hwnd);
        picker.ViewMode = PickerViewMode.Thumbnail;
        picker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
        return picker;
    }

    private static FolderPicker GetFolderOpenPicker(string[] typeFilter)
    {
        var picker = new FolderPicker();
        foreach (var t_filter in typeFilter)
        {
            picker.FileTypeFilter.Add(t_filter);
        }
        WinRT.Interop.InitializeWithWindow.Initialize(picker, _hwnd);
        picker.ViewMode = PickerViewMode.List;
        picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
        return picker;
    }
}
