using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NewHardwareinfo.Contracts.ViewModels;
using NewHardwareinfo.Models;
using NewHardwareinfo.Services;

namespace NewHardwareinfo.ViewModels;

public class HardwareTableViewModel : ObservableRecipient, INavigationAware
{

    public ObservableCollection<HardwareData> Source { get; } = new ObservableCollection<HardwareData>();
    public ICommand ExportDataCommand
    {
        get; set;
    }

    public HardwareTableViewModel()
    {
        ExportDataCommand = new RelayCommand(async() =>
        {
            var t = "";
            foreach (var p in HardwareInfoService.source)
            {
                t += p.Name + "\n=======================================\n" + p.Content + "\n";
            }
            new KSFileService().SaveFile(App.MainWindow, "Export Data", new List<string>() { ".txt" }, "Export Data", t);
            
        });
    }
    public async void OnNavigatedTo(object parameter)
    {
    }

    public void OnNavigatedFrom()
    {
    }


}
