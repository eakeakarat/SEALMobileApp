using System;
using System.Threading.Tasks;

namespace SEALMobile.Services
{
    public interface IQrScanningService
    {
        Task<string> ScanAsync();
    }
}
