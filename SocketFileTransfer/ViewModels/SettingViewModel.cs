using SocketFileTransfer.Services;
using System.ComponentModel;

namespace SocketFileTransfer.ViewModels;
public class SettingViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    private int _applicationRequiredPort;
    public int ApplicationRequiredPort
    {
        get => _applicationRequiredPort;
        set
        {
            _applicationRequiredPort = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ApplicationRequiredPort)));
        }
    }

    private string _savePath;
    public string SavePath
    {
        get => _savePath;
        set
        {
            _savePath = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SavePath)));
        }
    }
}
