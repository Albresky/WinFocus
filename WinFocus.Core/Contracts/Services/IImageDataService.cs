using System.Collections.Generic;
using System.Threading.Tasks;
using WinFocus.Core.Models;

namespace WinFocus.Core.Contracts.Services;
public interface IImageDataService
{
    Task<IEnumerable<ImageItem>> GetImageGridDataAsync();
}
