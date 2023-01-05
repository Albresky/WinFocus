﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media.Imaging;

namespace WinFocus.Core.Models;
public class VideoItem
{
    public string VideoName
    {
        get; set;
    }

    public string VideoPath
    {
        get;set;
    }
    
    public int VideoWidth
    {
        get;set;
    }
    
    public int VideoHeight
    {
        get;set;
    }

    public int VideoSize
    {
        get; set;    
    }
    
    public BitmapImage Thumbnail
    {
        get;set;
    }
}
