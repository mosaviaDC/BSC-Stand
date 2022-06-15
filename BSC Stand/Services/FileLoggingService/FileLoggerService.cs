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

namespace BSC_Stand.Services
{
    public class FileLoggerService : IFileLoggerService
    {

        private string FilePath;
        private StreamWriter StreamWriter;

        public void CreateFile()
        {
            FilePath = $@"{Environment.CurrentDirectory}/Файлы пользователя/Записи экспериментов/{DateTime.Now.ToFileTime()}.csv";
            StreamWriter = File.CreateText(FilePath);
         
            
        }

        public void WriteLog(ReadingParams readingParams)
        {

            StreamWriter.WriteLine("readingParams");


            //var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            //{
            //    // Don't write the header again
            //    HasHeaderRecord = false,
            //};


            //using (var stream = File.Open(FilePath, FileMode.Append))
            //{
            //    using (var writer = new StreamWriter("FilePath"))
            //    {
            //        using (var csv = new CsvWriter(writer, config))
            //        {
                      
            //            csv.WriteHeader<ReadingParams>();
            //            csv.WriteRecord(readingParams);
            //            csv.WriteComment("аывы");
            //            csv.NextRecord();
            //            Debug.WriteLine(readingParams.AKIPAValue);
                      
            //        }

            //    }
            //}

        }



    }
}
