﻿using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using WinFocus.Core.Models;

namespace WinFocus.Core.Contracts.Services;
public interface IVideoDataService
{
    Task<IList<VideoItem>> GetVideoDataAsync();
    Task<IEnumerable<uint>> GetResolutionAsync(StorageFile videoFile);
    Task<ImageStream> GetThumbnailFromVideo(StorageFile videoFile,int width,int height);
}
