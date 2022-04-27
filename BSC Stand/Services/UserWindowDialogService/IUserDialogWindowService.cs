using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Services
{
    interface IUserDialogWindowService
    {
        public bool ShowErrorMessage(string Message, string Caption = "Ошибка");
    }
}
