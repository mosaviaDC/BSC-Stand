using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Services
{
    internal interface IFileDialog
    {
        string OpenFileDialog(string Title="Открыть файл");
        string SaveFileDialog(string Title = "Сохранить файл");
        string SavePDFFileDialog(string Title = "Сохранить файл", string nameOfFile = "");

        string SaveXLSXileDialog(string Title = "Сохранить файл", string nameOfFile = "");
        string OpenCSVFileDialog(string Title = "Открыть файл");
    }
}
