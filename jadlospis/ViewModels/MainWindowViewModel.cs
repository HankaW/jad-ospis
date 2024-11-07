using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace jadlospis.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _isPaneOpen = true;

    [ObservableProperty] 
    private ViewModelBase _currentPage = new HomePageViewModel();
    
    [ObservableProperty]
    private ListItemTemplate? _selectedListItem;

    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if(value is null) return;
        var instance = Activator.CreateInstance(value.ModelType);
        if (instance is null) return;
        CurrentPage = (ViewModelBase)instance;
    }

    public ObservableCollection<ListItemTemplate> Items { get; } = new()
    {
        new ListItemTemplate(typeof(HomePageViewModel), "Główna", "HomeRegular"),
        new ListItemTemplate(typeof(ProduktPageViewModel), "Produkt", "NotebookRegular"),
        new ListItemTemplate(typeof(JadlospisPageViewModel), "Jadłospis", "DocumentEditRegular"),
    };
    
    

    [RelayCommand]
    public void TrigerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
    
}

public class ListItemTemplate
{
    public ListItemTemplate(Type type, string label, string iconKey)
    {
        ModelType = type;
        Label = label;

        Application.Current!.TryFindResource(iconKey, out var res);
        ListItemIcon = (StreamGeometry)res!;
    }

    public StreamGeometry ListItemIcon { get; set; }

    public string Label { get; set; }
    public Type ModelType { get; set; }
}