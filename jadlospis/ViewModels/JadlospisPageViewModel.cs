using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics; // Dodajemy to, aby używać Debug.WriteLine
using System; // Dodajemy to, aby używać Console.WriteLine

namespace jadlospis.ViewModels
{
    public partial class JadlospisPageViewModel : ViewModelBase
    {
        // Właściwości związane z danymi jadłospisu
        [ObservableProperty]
        private string _jadlospisName;

        [ObservableProperty]
        private int _numberOfPeople;

        [ObservableProperty]
        private string _mealFor; // Może to być np. "Dla dorosłych", "Dla dzieci", itp.

        // Lista możliwych opcji "Dla kogo" w jadłospisie
        public ObservableCollection<string> AvailableMealsFor { get; } = new()
        {
            "Dorosłych (19-59 lat)",
            "Młodzieży (11-19 lat)",
            "Dzieci (do 11 r.ż)",
            "Seniorów (60+)"
        };

        // Komenda do zapisania jadłospisu
        [RelayCommand]
        public void SaveMealPlan()
        {
            // Wypiszemy dane w konsoli debugowania
            Debug.WriteLine($"Jadłospis: {JadlospisName}, Dla: {MealFor}, Liczba osób: {NumberOfPeople}");
            
            // Dodajemy również Console.WriteLine, aby sprawdzić, czy dane będą widoczne w terminalu
            Console.WriteLine($"Jadłospis: {JadlospisName}, Dla: {MealFor}, Liczba osób: {NumberOfPeople}");
        }

        // Konstruktor
        public JadlospisPageViewModel()
        {
            // Ustawiamy domyślne wartości, jeśli to konieczne
            NumberOfPeople = 1;
            MealFor = AvailableMealsFor[0]; // Domyślnie "Dla dorosłych"
        }
    }
}