using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Services
{
   public interface IExperimentLogginsService 
    {

        public Task<string> CreateLogFile();
        /// <summary>
        /// Добавление текущих показаний в Log файл
        /// </summary>
        /// <returns></returns>
        public Task LogCurrentData();

        /// <summary>
        /// Экспорт лог файлов формата .csv в XLS
        /// </summary>
        /// <returns></returns>
        public Task<string> ExportLogFileToXls();



    }
}
