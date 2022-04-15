using BSC_Stand.Models;
using BSC_Stand.Models.StandConfigurationModels.ElectronicLoadModels;
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
        Task SaveProjectConfiguration(string filePath, ObservableCollection<ConfigurationMode> V27ConfigurationModes, ObservableCollection<ConfigurationMode> V100ConfigurationModes, int V27ConfigurationModesRepeatCount, int V100ConfigurationModesRepeatCount);

        Task<FileProjectConfigurationModel> GetProjectConfiguration(string FilePath);

    }
}
