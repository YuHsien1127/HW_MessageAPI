using Microsoft.AspNetCore.Mvc;
using MBAPI_HW.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MBAPI_HW.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ExcelMessagesHistoryController : ControllerBase
    {
        private readonly IExcelMessagesHistoryService _excelMessagesHistoryService;
        public ExcelMessagesHistoryController(IExcelMessagesHistoryService excelMessagesHistoryService)
        {
            _excelMessagesHistoryService = excelMessagesHistoryService;
        }
        /// <summary>
        /// 匯出留言歷史
        /// MBId 未輸入或為 0 → 匯出全部
        /// MBId 有輸入 → 匯出該 MBId 留言
        /// </summary>
        /// <param name="MBId"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ExportMessagesHistory(int? MBId = null)
        {
            var fileBytes = _excelMessagesHistoryService.ExportMessaagesHistory(MBId);
            string fileName = MBId.HasValue && MBId > 0 ? $"MBId{MBId}MessagesHiatory_{DateTime.Now:yyyyMMddHHmmss}.xlsx" : $"AllMessagesHistory_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
    }
}
