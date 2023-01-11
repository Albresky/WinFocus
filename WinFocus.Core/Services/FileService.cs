using System;
using System.IO;
using System.Text;

using Newtonsoft.Json;

using WinFocus.Core.Contracts.Services;

namespace WinFocus.Core.Services;

public class FileService : IFileService
{
    public T Read<T>(string folderPath, string fileName)
    {
        var path = Path.Combine(folderPath, fileName);
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }

        return default;
    }

    public void Save<T>(string folderPath, string fileName, T content)
    {
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        var fileContent = JsonConvert.SerializeObject(content);
        File.WriteAllText(Path.Combine(folderPath, fileName), fileContent, Encoding.UTF8);
    }

    public void Delete(string folderPath, string fileName)
    {
        if (fileName != null && File.Exists(Path.Combine(folderPath, fileName)))
        {
            File.Delete(Path.Combine(folderPath, fileName));
        }
    }

    public static bool OpenFolder(string dir)
    {
        if (Directory.Exists(dir))
        {
            return true;
        }
        else
        {
            try
            {
                if (Directory.CreateDirectory(dir) != null)
                {
                    return true;
                }
                else { return false; }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public static bool FileCopy(string srcFile, string destPath, string destfileName)
    {
        if (File.Exists(srcFile)&&OpenFolder(destPath))
        {
            var fullDestPath = destPath + "\\"+destfileName;
            try { File.Copy(srcFile, fullDestPath); return true; } catch (Exception) { return false; }
        }
        else
        {
            return false;
        }
    }
}
