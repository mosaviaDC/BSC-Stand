using BSC_Stand.Models;
using BSC_Stand.Models.StandConfigurationModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSC_Stand.Services
{
    internal interface IProjectConfigurationService
    {
        Task SaveProjectConfiguration(string filePath, ObservableCollection<ElectronicConfigMode> V27ConfigurationModes, ObservableCollection<ElectronicConfigMode> V100ConfigurationModes, ObservableCollection<PowerSupplyConfigMode> powerSupplyConfigModes, int V27ConfigurationModesRepeatCount, int V100ConfigurationModesRepeatCount, int PowerSupplyRepeatCount);

        Task<FileProjectConfigurationModel> GetProjectConfiguration(string FilePath);

    }
}
