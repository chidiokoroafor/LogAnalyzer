using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.IO.Compression;
using System.Text.RegularExpressions;


namespace LogAnalyzerLibrary
{
    public static class Analyzer
    {
        public static List<int> GetNumberOfUniqueErrors(string directory)
        {
            string[] logFiles = Directory.GetFiles(@$"C:\{directory}");

            int numberOfUniqueErrors = 0;
            List<int> uniqueErrors = new List<int>();
            List<string> logs = new List<string>();
            foreach (string filePath in logFiles)
            {
                if (File.Exists(filePath))
                {
                    string[] fileContent = File.ReadAllLines(filePath);
                    fileContent = fileContent.Where(x => !x.StartsWith("__") && x != "").ToArray();
                    foreach (string item in fileContent)
                    {
                        if (item.Split(" : ").Length > 1)
                        {
                            string logMessage = item.Split(" : ")[1];
                            logs.Add(logMessage);
                        }

                    }

                    logs = logs.Distinct().ToList();

                   numberOfUniqueErrors = logs.Count;
                    uniqueErrors.Add(numberOfUniqueErrors);
                }
            }

            return uniqueErrors;
        }

        public static List<int> GetNumberOfDuplicateErrors(string directory)
        {
            string[] logFiles = Directory.GetFiles(@$"C:\{directory}");

            int numberOfDuplicateErrors = 0;
            List<int> dupErrors = new List<int>();
            List<string> logs = new List<string>();
            foreach (string filePath in logFiles)
            {
                if (File.Exists(filePath))
                {
                    string[] fileContent = File.ReadAllLines(filePath);
                    fileContent = fileContent.Where(x => !x.StartsWith("__") && x != "").ToArray();
                    foreach (string item in fileContent)
                    {
                        if (item.Split(" : ").Length > 1)
                        {
                            string logMessage = item.Split(" : ")[1];
                            logs.Add(logMessage);
                        }

                    }
                    var duplicateErrors = logs.GroupBy(x => x).Where(g => g.Count() > 1).ToList();

                    numberOfDuplicateErrors = duplicateErrors.Count();
                    dupErrors.Add(numberOfDuplicateErrors);
                }
            }

            return dupErrors;
        }

        public static void DeleteArchiveFromPeriod(string directory, DateTime period)
        {
            string[] logFiles = Directory.GetFiles(@$"C:\{directory}");

            foreach (string filePath in logFiles)
            {
                if (File.Exists(filePath))
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    if (fileInfo.LastAccessTime < period)
                    {
                        fileInfo.Delete();
                    }
                }
            }

        }
        
        public static void ArchiveLogsFromPeriod(string directory, DateTime? startDate, DateTime? endDate)
        {
            string[] logFiles = Directory.GetFiles(@$"C:\{directory}");

            List<string> filesToZip = new List<string>();
            foreach (string file in logFiles)
            {
                FileInfo fileInfo = new FileInfo(file);
               
                if (fileInfo.LastAccessTime >= startDate && fileInfo.LastAccessTime <= endDate)
                {
                    filesToZip.Add(file);
                }
            }
           
            string directoryPath = Path.GetDirectoryName(filesToZip[0]);

            string zipFileName = $"{startDate}_{endDate}.zip";
            string zipFilePath = Path.Combine(directoryPath, zipFileName);

            if (Directory.Exists(directoryPath))
            {
                    using (ZipArchive archive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create))
                    {
                        foreach (string fileToZip in filesToZip)
                        {
                            string filePath = Path.Combine(directoryPath, fileToZip);
                            if (File.Exists(filePath))
                            {
                                archive.CreateEntryFromFile(filePath, fileToZip);
                            }
                           
                        }
                    }
               
            }

        }

        public static int CountTotalAvailableLogsInAPeriod(string directory, DateTime? startDate, DateTime? endDate)
        {
            string[] logFiles = Directory.GetFiles(@$"C:\{directory}");
            var filesToCount = new List<string>();

            foreach (string file in logFiles)
            {
                //var fileName = Path.GetFileName(file).Split('.')[0];
                //var fileDate = new DateTime((int)fileName[0], (int)fileName[1], (int)fileName[2]);
                FileInfo fileInfo = new FileInfo(file);
                if (fileInfo.LastAccessTime >= startDate && fileInfo.LastAccessTime <= endDate)
                {
                    filesToCount.Add(file);
                }
            }
            return filesToCount.Count;
        }

        public static List<String> SearchLogsPerSize(string directory, int minSize, int maxSize)
        {
            string[] logFiles = Directory.GetFiles(@$"C:\{directory}");

            var filesToSearch = new List<String>();

            foreach (var log in logFiles)
            {
                FileInfo fileInfo = new FileInfo(log);
                if ((decimal)fileInfo.Length/1024 >= minSize && (decimal)fileInfo.Length / 1024 <= maxSize)
                {
                    filesToSearch.Add(Path.GetFileName(log));
                }
            }

            return filesToSearch;
        }

    }
}
