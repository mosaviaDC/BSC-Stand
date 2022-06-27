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
        public string FilePath { get; private set; }

        public void CreateFile()
        {
            FilePath = $@"{Environment.CurrentDirectory}/Файлы пользователя/Отчеты/CSV/Отчет от {DateTime.Now.Day}.{DateTime.Now.Month}.{DateTime.Now.Year} {DateTime.Now.Hour}.{DateTime.Now.Minute}.csv";
            File.CreateText(FilePath).Close();
            using (var stream = File.Open(FilePath, FileMode.Append))
            using (var writer = new StreamWriter(stream))
            using (var csv = new CsvWriter(writer,CultureInfo.InvariantCulture))
            {

                csv.WriteHeader<ReadingParams>();
                csv.NextRecord();
                // StreamWriter.Flush();
            }

        }

        public void WriteLog(ReadingParams readingParams)
        {
          
                lock (this)
                {


                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        // Don't write the header again
                        HasHeaderRecord = true
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


        }

        public async Task<List<ReadingParams>> ReadLogs(string _FilePath)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true
            };
           return await Task.Run(() =>
            {

                using (var reader = new StreamReader(_FilePath))
                using (var csv = new CsvReader(reader, config))
                {
                    var a = csv.GetRecords<ReadingParams>();
                    return a.ToList();
                }
            });
        }

    }



    
}
