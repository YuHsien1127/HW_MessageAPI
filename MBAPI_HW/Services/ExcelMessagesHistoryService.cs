using MBAPI_HW.Dto.Response;
using MBAPI_HW.Repositorys;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace MBAPI_HW.Services
{
    public class ExcelMessagesHistoryService : IExcelMessagesHistoryService
    {
        private readonly ILogger<ExcelMessagesHistoryService> _logger;
        private readonly IMessagesBoradRepository _messagesBoradRepository;
        public ExcelMessagesHistoryService(ILogger<ExcelMessagesHistoryService> logger, IMessagesBoradRepository messagesBoradRepository)
        {
            _logger = logger;
            _messagesBoradRepository = messagesBoradRepository;
        }
        
        public byte[] ExportMessaagesHistory(int? MBId = null)
        {
            _logger.LogInformation("開始匯出留言，MBId：{MBId}", MBId);
            var query = _messagesBoradRepository.GetAllMessagesHistory();

            if (MBId.HasValue && MBId.Value > 0)
            {
                _logger.LogDebug("只匯出 MBId = {MBId} 的資料", MBId);
                query = query.Where(h => h.Mbid == MBId.Value);
            }
            else
                _logger.LogDebug("未輸入 MBId，將匯出全部留言資料");

            var excelHistorys = query.ToList();
            var messagesBorad = MBId.HasValue ? _messagesBoradRepository.GetMessagesBoradById(MBId.Value) : null;

            // 產生 Excel
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("MessagesHistory");

            // 標題列
            IRow headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Id");
            headerRow.CreateCell(1).SetCellValue("MBId");
            headerRow.CreateCell(2).SetCellValue("Message");
            headerRow.CreateCell(3).SetCellValue("IsDel");
            headerRow.CreateCell(4).SetCellValue("IsMark");
            headerRow.CreateCell(5).SetCellValue("CreateDate");
            headerRow.CreateCell(6).SetCellValue("CreateUserId");

            if (excelHistorys.Count == 0 && messagesBorad == null)
            {
                _logger.LogWarning("匯出失敗：找不到MBId = {MBId} 的資料", MBId);
                return Array.Empty<byte>(); // 無資料就回傳空檔
            }
            else if (excelHistorys.Count == 0 && messagesBorad != null)
                _logger.LogWarning("匯出失敗：找不到MBId = {MBId} 的資料", MBId);
            else
            {
                // 填資料
                var rowIndex = 1;
                foreach (var historys in excelHistorys)
                {
                    var row = sheet.CreateRow(rowIndex++);
                    row.CreateCell(0).SetCellValue(historys.Id);
                    row.CreateCell(1).SetCellValue(historys.Mbid);
                    row.CreateCell(2).SetCellValue(historys.Message);
                    row.CreateCell(3).SetCellValue(historys.IsDel ? "T" : "F");
                    row.CreateCell(4).SetCellValue(historys.IsMark ? "T" : "F");
                    row.CreateCell(5).SetCellValue(historys.CreateDate.ToString("yyyy/MM/dd-HH:mm:ss"));
                    row.CreateCell(6).SetCellValue(historys.CreateUserId);
                }
            }

                // 自動調整欄寬
                for (int col = 0; col < 6; col++)
                    sheet.AutoSizeColumn(col);

                // 轉成 byte[]
                using var ms = new MemoryStream();
                workbook.Write(ms, leaveOpen: true); // 避免 Stream 被關閉
                _logger.LogInformation("匯出完成，MBId：{MBId}，筆數：{Count}，檔案大小：{Size} bytes"
                                   , MBId, excelHistorys.Count, ms.Length);
                return ms.ToArray();
            }
        }
    }
