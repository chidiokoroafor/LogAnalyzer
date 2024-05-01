using LogAnalyzerApi.Contracts;
using LogAnalyzerApi.Models;
using LogAnalyzerLibrary;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace LogAnalyzerApi.Controllers
{
    [Route("api/LogAnalyzer")]
    [ApiController]
    public class LogAnalyzerController : ControllerBase
    {
        private readonly ILogAnalyzerService _logAnalyzerService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LogAnalyzerController(ILogAnalyzerService logAnalyzerService, IWebHostEnvironment webHostEnvironment)
        {
            _logAnalyzerService = logAnalyzerService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("GetNumberOfUniqueErrors")]
        public IActionResult GetUniqueErrors(string directory)
        {
            var uniquErrors = Analyzer.GetNumberOfUniqueErrors(directory);
            return Ok(uniquErrors);
        }

        [HttpGet("GetNumberOfDuplicateErrors")]
        public IActionResult GetDuplicateErrors(string directory)
        {
            var duplicateErrors = Analyzer.GetNumberOfDuplicateErrors(directory);
            return Ok(duplicateErrors);
        }

        [HttpGet("SearchLogsPerSize")]
        public IActionResult SearchLogsPerSize(string directory, int minSize, int maxSize)
        {
            var searchedLogs = Analyzer.SearchLogsPerSize(directory, minSize, maxSize);
            return Ok(searchedLogs);
        }

        [HttpGet("ArchiveLogsFromPeriod")]
        public IActionResult ArchiveLogsFromPeriod(string directory, DateTime? startDate, DateTime? endDate)
        {
            Analyzer.ArchiveLogsFromPeriod(directory, startDate, endDate);
            return Ok();
        }

        [HttpPost("UploadLogFile")]
        public IActionResult UploadLogFile(IFormFile logFile)
        {
            if (logFile != null)
            {
                string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "LogFiles");
                string fileName = logFile.FileName;
                string filePath = Path.Combine(uploadDir, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    logFile.CopyTo(fileStream);
                }
            }
            return Ok();
        }
    }
}
