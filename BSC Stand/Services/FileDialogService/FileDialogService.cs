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
                Filter = ("(*.json)|*.json"),
                InitialDirectory = Environment.CurrentDirectory + "Файлы пользователя" + "Конфигурация экспериментов",
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
                InitialDirectory = Environment.CurrentDirectory + "Файлы пользователя" + "Конфигурация экспериментов",
                Filter = "(*.json) |*.json"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                filePath = saveFileDialog.FileName;
            }
            return filePath;
        }

    }
}
