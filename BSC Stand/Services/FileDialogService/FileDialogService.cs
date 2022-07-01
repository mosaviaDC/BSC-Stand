using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BSC_Stand.Services
{
    internal class FileDialogService:IFileDialog
    {
        public string OpenFileDialog(string Title="Открыть файл")
        {
            string filePath = null;
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = ("(*.json) |*.json"),
                InitialDirectory = $@"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ЭО БСК\Конфигурация экспериментов",
                Title = Title
            };

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
            }
            return filePath;
        }


        public string OpenCSVFileDialog(string Title = "Открыть файл")
        {
            string filePath = null;
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = ("(*.csv)|*.csv"),
                InitialDirectory = @$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ЭО БСК\Отчеты\CSV",
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
                InitialDirectory = @$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ЭО БСК\Конфигурация экспериментов",
                Filter = "(*.json) |*.json"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
            }
            return filePath;
        }


        public string SavePDFFileDialog(string Title = "Сохранить файл", string nameOfFile = "")
        {
            if (nameOfFile.Length > 4)
            {
                nameOfFile = nameOfFile.Substring(0, nameOfFile.Length - 4);
                nameOfFile += ".pdf";
            }
            string filePath = null;
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Title = Title,
                InitialDirectory = @$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ЭО БСК\Отчеты\PDF",
                FileName = nameOfFile,
                Filter = "(*.pdf) |*.pdf"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
            }
            return filePath;
        }

        public string SaveXLSXileDialog(string Title = "Сохранить файл", string nameOfFile = "")
        {
            if (nameOfFile.Length > 4)
            {
                nameOfFile = nameOfFile.Substring(0, nameOfFile.Length - 4);
                nameOfFile += ".xlsx";
            }
            string filePath = null;
            SaveFileDialog saveFileDialog = new SaveFileDialog()
            {
                Title = Title,
                InitialDirectory = @$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\ЭО БСК\Отчеты\XLSX",
                FileName = nameOfFile,
                Filter = "(*.XLSX) |*.XLSX"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
            }
            return filePath;

        }
    }
}
