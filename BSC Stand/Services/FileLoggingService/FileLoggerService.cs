using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CsvHelper;
using BSC_Stand.Models;
using System.Globalization;
using System.Diagnostics;
using CsvHelper.Configuration;
using System.Linq;

namespace BSC_Stand.Services
{
    public class FileLoggerService : IFileLoggerService
    {

        private string FilePath;
        private StreamWriter StreamWriter;

        public void CreateFile()
        {
            FilePath = $@"{Environment.CurrentDirectory}/Файлы пользователя/Записи экспериментов/{DateTime.Now.ToFileTime()}.csv";
            File.CreateText(FilePath).Close();
            //StreamWriter.Close();
         
            
        }

        public void WriteLog(ReadingParams readingParams)
        {

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                // Don't write the header again
                HasHeaderRecord = false,
            };


            using (var stream = File.Open(FilePath, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer, config))
                {

                    csv.WriteRecord(readingParams);

                    csv.NextRecord();                   
               // StreamWriter.Flush();
                }
              

        }

        public async Task<List<ReadingParams>> ReadLogs(string _FilePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };
           return await Task.Run(() =>
            {

                using (var reader = new StreamReader(_FilePath))
                using (var csv = new CsvReader(reader, config))
                {
                    //    var records = await csv.GetRecordsAsync<ReadingParams>();
                    var a = csv.GetRecords<ReadingParams>();
                    return a.ToList();
                }
            });

          
        }

    }



    
}
