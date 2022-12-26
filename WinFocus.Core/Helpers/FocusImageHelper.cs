using System;
using System.IO;

namespace WinFocus.Core.Helpers;
public static class FocusImageHelper
{
    private static readonly string FOCUS_IMG_DIR;
    private static readonly string FOCUS_IMG_DIR_RELATIVE = "\\AppData\\Local\\Packages\\Microsoft.Windows.ContentDeliveryManager_cw5n1h2txyewy\\LocalState\\Assets";
    private static readonly List<string> imagesPath;

    static FocusImageHelper()
    {
        // Get Focus img dir
        var prefix = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        FOCUS_IMG_DIR = prefix + FOCUS_IMG_DIR_RELATIVE;
        Console.WriteLine(FOCUS_IMG_DIR);

        // Get the images' path from FOCUS_IMG_DIR
        if (FOCUS_IMG_DIR != null)
        {
            try
            {
                var IMGS_PATH = Directory.GetFiles(FOCUS_IMG_DIR);
                imagesPath = new List<string>(IMGS_PATH);
                Console.WriteLine(IMGS_PATH);

                //foreach(var path in IMGS_PATH)
                //{
                    
                //}
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Access to " + FOCUS_IMG_DIR + " is denied");
            }
        }
        else
        {
            Console.WriteLine("FOCUS_IMG_DIR is null.");
        }
    }

    private static string GetFocusImgDir()
    {
        return FOCUS_IMG_DIR;
    }

    public static List<string> GetImgPath()
    {
        return imagesPath;
    }
}
