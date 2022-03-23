using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BSC_Stand.Services.FileLoggingService
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private string _fileName;

        public FileLoggerProvider(string fileName)
        {
            _fileName = fileName;
        }

        public  ILogger CreateLogger(string categoryName)
        {
            return new FileLogger(categoryName);
        }
        public void Dispose()
        {

        }

    }
}
