using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BSC_Stand.Services
{
    internal interface IWindowService
    {
        public void ShowWindow<T>(object DataContext) where T : Window, new();
    }
}
