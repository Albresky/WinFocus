namespace WinFocus.Core.Services;
public class VideoService
{
    //async List<uint> GetResulution(StorageFile file)
    //{
    //    return await GetResolutionAsync(file);
    //}
    
    async void GetResolutionAsync(StorageFile file)
    {
        List<string> encodingPropertiesToRetrieve = new List<string>();
        encodingPropertiesToRetrieve.Add("System.Video.FrameHeight");
        encodingPropertiesToRetrieve.Add("System.Video.FrameWidth");
        IDictionary<string, object> encodingProperties = await file.Properties.RetrievePropertiesAsync(encodingPropertiesToRetrieve);
        uint frameHeight = (uint)encodingProperties["System.Video.FrameHeight"];
        uint frameWidth = (uint)encodingProperties["System.Video.FrameWidth"];
        //List<uint> resolution = new List<uint>();
        //resolution.Add(frameWidth);
        //resolution.Add(frameHeight);
        //return resolution;
    } 
  
}
