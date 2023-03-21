using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using NewHardwareinfo.Contracts.Services;
using NewHardwareinfo.Helpers;
using NewHardwareinfo.Services;
using Windows.ApplicationModel;

namespace NewHardwareinfo.ViewModels;

public class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    private ElementTheme _elementTheme;
    private string _versionDescription;

    public ElementTheme ElementTheme
    {
        get => _elementTheme;
        set => SetProperty(ref _elementTheme, value);
    }

    public string VersionDescription
    {
        get => _versionDescription;
        set => SetProperty(ref _versionDescription, value);
    }

    public ICommand SwitchThemeCommand
    {
        get;
    }
    public ICommand UpdateRateCommand
    {
        get;
    }
    public bool UpdateRateSlow
    {
        get => HardwareInfoService.timer.Interval == TimeSpan.FromSeconds(3);
        set
        {
        }
    }
    public bool UpdateRateNormal
    {
        get => HardwareInfoService.timer.Interval == TimeSpan.FromSeconds(2);
        set
        {
        }
    }
    public bool UpdateRateFast
    {
        get => HardwareInfoService.timer.Interval == TimeSpan.FromSeconds(1);
        set
        {
        }
    }
    public SettingsViewModel(IThemeSelectorService themeSelectorService)
    {
        _themeSelectorService = themeSelectorService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async (param) =>
            {
                if (ElementTheme != param)
                {
                    ElementTheme = param;
                    await _themeSelectorService.SetThemeAsync(param);
                }
            });

        UpdateRateCommand = new RelayCommand<UpdateRate>((param) =>
        {
            HardwareInfoService.SetTimerInterval(((double)param));
        });
    }
    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }
}
public enum UpdateRate
{
    Slow = 3,
    Normal = 2,
    Fast = 1
}
