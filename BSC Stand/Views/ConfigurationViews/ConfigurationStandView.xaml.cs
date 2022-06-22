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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BSC_Stand.Views
{
    /// <summary>
    /// Interaction logic for ConfigurationStandView.xaml
    /// </summary>
    public partial class ConfigurationStandView : UserControl
    {
        public ConfigurationStandView()
        {
            InitializeComponent();
            
        }

        
        private void StandConfigModules_Initialized_1(object sender, EventArgs e)
        {
         
            foreach (var p in StandConfigModules.Items)
            {
                StandConfigModules.SelectedItem = p;
            }
            


        }

        private void StandConfigModules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("Selection Changed");
        }
    }
}
