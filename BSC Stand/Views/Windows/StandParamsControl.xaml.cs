using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Extensions.DependencyInjection;
using BSC_Stand.Services;

namespace BSC_Stand.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для StandParamsControl.xaml
    /// </summary>
    public partial class StandParamsControl : Window
    {
        public StandParamsControl()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.Host.Services.GetRequiredService<IModbusService>().ExitCommand();
     }
    }
}
