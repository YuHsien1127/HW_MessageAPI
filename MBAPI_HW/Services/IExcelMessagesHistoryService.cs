using MBAPI_HW.Dto.Response;

namespace MBAPI_HW.Services
{
    public interface IExcelMessagesHistoryService
    {
        public byte[] ExportMessaagesHistory(int? MBId = null);
    }
}
