using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.ObjectModel;
using BSC_Stand.Models.StandConfigurationModels;
using System.IO;
using BSC_Stand.Models;
using BSC_Stand.ViewModels;
using System.Diagnostics;

namespace BSC_Stand.Services
{
    internal class ProjectConfigurationService:IProjectConfigurationService
    {
        private readonly StatusBarViewModel _statusBarViewModel;

        public ProjectConfigurationService(StatusBarViewModel statusBarViewModel)
        {
            _statusBarViewModel = statusBarViewModel;
        }

        public async Task<FileProjectConfigurationModel> GetProjectConfiguration(string FilePath)
        {
            _statusBarViewModel.SetNewTask(100,"Импорт конфигурации");

            using FileStream openStream = File.OpenRead(FilePath);
            
       
            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
            {
               
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };
            _statusBarViewModel.UpdateTaskProgress(100);
            return await JsonSerializer.DeserializeAsync<FileProjectConfigurationModel>(openStream,jsonSerializerOptions);
        }


        //public async Task SaveProjectConfiguration(string filePath,ObservableCollection<ConfigMode> V27ConfigurationModes, ObservableCollection<ConfigMode>V100ConfigurationModes,int V27ConfigurationModesRepeatCount, int V100ConfigurationModesRepeatCount)
        //{
           
        //    _statusBarViewModel.SetNewTask(100);
        //    FileProjectConfigurationModel projectConfiguration = new FileProjectConfigurationModel (V100ConfigurationModes, V27ConfigurationModes,V27ConfigurationModesRepeatCount,V100ConfigurationModesRepeatCount);
        //    _statusBarViewModel.UpdateTaskProgress(25);

        //    JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        //    {
        //        WriteIndented = true
        //    };
        //    _statusBarViewModel.UpdateTaskProgress(50);
        //    using FileStream createStream = File.Create(filePath);

        //    await JsonSerializer.SerializeAsync(createStream, projectConfiguration,jsonSerializerOptions);
        //    await createStream.DisposeAsync();
        //    _statusBarViewModel.UpdateTaskProgress(100);
        //    _statusBarViewModel.SetNewTask();
        //}

        public async Task SaveProjectConfiguration(string filePath, ObservableCollection<ElectronicConfigMode> V27ConfigurationModes, ObservableCollection<ElectronicConfigMode> V100ConfigurationModes, ObservableCollection<PowerSupplyConfigMode> powerSupplyConfigModes,   int V27ConfigurationModesRepeatCount, int V100ConfigurationModesRepeatCount, int PowerSupplyRepeatCount)
        {
            _statusBarViewModel.SetNewTask(100, "Экспорт конфигурации");
            FileProjectConfigurationModel projectConfiguration = new FileProjectConfigurationModel(V100ConfigurationModes, V27ConfigurationModes, powerSupplyConfigModes, V27ConfigurationModesRepeatCount, V100ConfigurationModesRepeatCount,PowerSupplyRepeatCount);
            _statusBarViewModel.UpdateTaskProgress(25);

            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
            {
                WriteIndented = true,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
            };
            _statusBarViewModel.UpdateTaskProgress(50);
            using FileStream createStream = File.Create(filePath);

            await JsonSerializer.SerializeAsync(createStream, projectConfiguration, jsonSerializerOptions);
            await createStream.DisposeAsync();
            _statusBarViewModel.UpdateTaskProgress(100);
         
        }
    }
}
