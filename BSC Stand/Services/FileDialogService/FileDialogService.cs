using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Services.FileDialogService
{
    internal class FileDialogService:IFileDialog
    {
        public string OpenFileDialog(string Title="Открыть файл")
        {
            string filePath = null;
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                // Filter = ("CsvFiles (*.csv)|*.csv"),
                InitialDirectory = Environment.CurrentDirectory,
                Title = Title


            };

            if (openFileDialog.ShowDialog() == true)
            {

                filePath = openFileDialog.FileName;

            }
            return filePath;


        }
        public string SaveFileDialog(string Title="Сохранить файл")
        {
            string filePath = null;
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Title = Title,
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "(*.cs) |*.cs"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
            }
            return filePath;
        }

    }
}
