using BSC_Stand.Services.FileLoggingService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Services
{
    internal class FileLoggerService : IFileLogger
    {
        private readonly ILogger _logger = null;
        public FileLoggerService(ILoggerFactory factory)
        {
            if (_logger == null)
            {
                factory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
                _logger = factory.CreateLogger($"FileLogger");
            }
        }
     
        void IFileLogger.Log(string message)
        {
            _logger.LogInformation(message);
        }
    }
}
