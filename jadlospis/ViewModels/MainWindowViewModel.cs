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
    // Property to control the Pane's open state
    [ObservableProperty]
    private bool _isPaneOpen = true;

    // Holds the current page view model
    [ObservableProperty] 
    private ViewModelBase _currentPage = new HomePageViewModel();
    
    // Holds the selected list item
    [ObservableProperty]
    private ListItemTemplate? _selectedListItem;

    // Method called when the selected list item changes
    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        if(value is null) return;

        // Create an instance of the selected view model type
        var instance = Activator.CreateInstance(value.ModelType);
        if (instance is null) return;

        // Set the current page to the newly created instance
        CurrentPage = (ViewModelBase)instance;
    }

    // Collection of list items for navigation
    public ObservableCollection<ListItemTemplate> Items { get; } = new()
    {
        new ListItemTemplate(typeof(HomePageViewModel), "Główna", "HomeRegular"),
        new ListItemTemplate(typeof(ProduktPageViewModel), "Produkt", "NotebookRegular"),
        new ListItemTemplate(typeof(JadlospisPageViewModel), "Jadłospis", "DocumentEditRegular"),
    };

    // Command to toggle the Pane's open state
    [RelayCommand]
    public void TrigerPane()
    {
        IsPaneOpen = !IsPaneOpen;
    }
    
}

// Class representing a template for list items
public class ListItemTemplate
{
    // Constructor to initialize list item properties
    public ListItemTemplate(Type type, string label, string iconKey)
    {
        ModelType = type;
        Label = label;

        // Attempt to find the specified resource for icon
        Application.Current!.TryFindResource(iconKey, out var res);
        ListItemIcon = (StreamGeometry)res!;
    }

    // Icon associated with the list item
    public StreamGeometry ListItemIcon { get; set; }

    // Label displayed for the list item
    public string Label { get; set; }

    // Type of the view model associated with the list item
    public Type ModelType { get; set; }
}