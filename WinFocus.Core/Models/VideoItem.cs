using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    
    public double VideoWidth
    {
        get;set;
    }
    
    public double VideoHeight
    {
        get;set;
    }

    public int VideoSize
    {
        get; set;    
    }
    
    public string ThumbnailPath
    {
        get;set;
    }
}
