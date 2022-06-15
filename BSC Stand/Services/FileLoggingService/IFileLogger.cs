using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using BSC_Stand.Models;
using System.Globalization;

namespace BSC_Stand.Services
{
    public interface IFileLoggerService
    {

        public void CreateFile();

        public void WriteLog(ReadingParams readingParams);
        public Task<List<ReadingParams>> ReadLogs(string _FilePath);
    }
}
