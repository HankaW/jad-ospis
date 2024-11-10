using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace jadlospis.ViewModels;

// Główna klasa ViewModelu okna aplikacji, dziedziczy po ViewModelBase
public partial class MainWindowViewModel : ViewModelBase
{
    // Właściwość kontrolująca stan otwarcia panelu bocznego
    [ObservableProperty]
    private bool _isPaneOpen = true;

    // Właściwość przechowująca aktualny ViewModel strony
    [ObservableProperty]
    private ViewModelBase _currentPage = new HomePageViewModel();

    // Właściwość przechowująca wybrany element listy
    [ObservableProperty]
    private ListItemTemplate? _selectedListItem;

    // Kolekcja elementów listy służących do nawigacji
    public ObservableCollection<ListItemTemplate> Items { get; } = new()
    {
        new ListItemTemplate(typeof(HomePageViewModel), "Główna", "HomeRegular"),
        new ListItemTemplate(typeof(ProduktPageViewModel), "Produkt", "NotebookRegular"),
        new ListItemTemplate(typeof(JadlospisPageViewModel), "Jadłospis", "DocumentEditRegular"),
    };

    // Metoda wywoływana przy zmianie wybranego elementu listy
    partial void OnSelectedListItemChanged(ListItemTemplate? value)
    {
        // Jeśli wartość jest nullem, nic nie robimy
        if (value is null) return;

        // Tworzymy instancję ViewModelu odpowiadającego wybranemu typowi
        var instance = Activator.CreateInstance(value.ModelType);
        if (instance is null) return;

        // Ustawiamy bieżący ViewModel na nowo utworzony
        CurrentPage = (ViewModelBase)instance;
    }

    // Komenda służąca do przełączania stanu otwarcia panelu
    [RelayCommand]
    public void TrigerPane()
    {
        // Zmiana stanu otwarcia panelu
        IsPaneOpen = !IsPaneOpen;
    }
}

// Klasa reprezentująca szablon elementu listy
public class ListItemTemplate
{
    // Konstruktor inicjujący właściwości elementu listy
    public ListItemTemplate(Type type, string label, string iconKey)
    {
        ModelType = type;
        Label = label;

        // Próbujemy znaleźć zasób ikony na podstawie klucza
        Application.Current!.TryFindResource(iconKey, out var res);
        // Ustawiamy ikonę elementu na znalezioną geometrię strumieniową
        ListItemIcon = (StreamGeometry)res!;
    }

    // Typ ViewModelu powiązanego z danym elementem listy
    public Type ModelType { get; set; }

    // Etykieta wyświetlana dla elementu listy
    public string Label { get; set; }

    // Ikona skojarzona z elementem listy
    public StreamGeometry ListItemIcon { get; set; }
}
