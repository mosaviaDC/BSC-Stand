using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.ObjectModel;
using BSC_Stand.Models.StandConfigurationModels.ElectronicLoadModels;
using System.IO;
using BSC_Stand.Models;

namespace BSC_Stand.Services
{
    internal class ProjectConfigurationService:IProjectConfigurationService
    {

        public async Task<FileProjectConfigurationModel> GetProjectConfiguration(string FilePath)
        {

            using FileStream openStream = File.OpenRead(FilePath);
            return await JsonSerializer.DeserializeAsync<FileProjectConfigurationModel>(openStream);
        }


        public async Task SaveProjectConfiguration(string filePath,ObservableCollection<ConfigurationMode> V27ConfigurationModes, ObservableCollection<ConfigurationMode>V100ConfigurationModes,int V27ConfigurationModesRepeatCount, int V100ConfigurationModesRepeatCount)
        {

            FileProjectConfigurationModel projectConfiguration = new FileProjectConfigurationModel (V100ConfigurationModes, V27ConfigurationModes,V27ConfigurationModesRepeatCount,V100ConfigurationModesRepeatCount);


            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            using FileStream createStream = File.Create(filePath);
            await JsonSerializer.SerializeAsync(createStream, projectConfiguration,jsonSerializerOptions);
            await createStream.DisposeAsync();
        }


    
    









    }
}
