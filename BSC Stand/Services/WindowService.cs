using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BSC_Stand.Services
{
    internal class WindowService: IWindowService
    {
     

        public void ShowWindow<T>(object DataContext) where T:Window, new()
        {
            T window = new T();
            window.DataContext = DataContext;
            window.Show();
      
        }
    }
}
