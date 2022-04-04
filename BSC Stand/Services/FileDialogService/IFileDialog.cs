using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Services.FileDialogService
{
    internal interface IFileDialog
    {
        string OpenFileDialog(string Title="Открыть файл");
        string SaveFileDialog(string Title = "Сохранить файл");
    }
}
