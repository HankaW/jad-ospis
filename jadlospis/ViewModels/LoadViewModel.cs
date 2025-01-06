using System.Collections.ObjectModel;

namespace jadlospis.ViewModels;

public class LoadViewModel: ViewModelBase
{
    public ObservableCollection<WczytaneJadlospisyViewModel> Jadlospisy { get; set; } = new();
}