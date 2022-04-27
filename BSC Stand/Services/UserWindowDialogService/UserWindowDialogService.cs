using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
namespace BSC_Stand.Services
{
    class UserWindowDialogService:IUserDialogWindowService
    {
        public bool ShowErrorMessage(string Message, string Caption = "Ошибка")
        {
            MessageBox.Show(Message,Caption,MessageBoxButton.OK, MessageBoxImage.Error);
            return true;
        }





    }
}
