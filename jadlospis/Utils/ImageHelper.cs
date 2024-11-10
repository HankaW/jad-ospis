using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace jadlospis.Utils
{
    // Klasa pomocnicza do ładowania obrazów z zasobów lub z internetu
    public static class ImageHelper
    {
        // Statyczny HttpClient, aby uniknąć wielokrotnego tworzenia nowych instancji
        private static readonly HttpClient httpClient = new HttpClient();

        // Metoda do ładowania obrazu z zasobów osadzonych w aplikacji
        public static Bitmap LoadFromResource(Uri resourceUri)
        {
            try
            {
                // Zwraca nowy obiekt Bitmap załadowany z zasobów aplikacji, używając AssetLoader
                return new Bitmap(AssetLoader.Open(resourceUri));
            }
            catch (Exception ex)
            {
                // Logowanie ewentualnych błędów związanych z ładowaniem zasobów
                Console.WriteLine($"Error loading resource image: {ex.Message}");
                return null;
            }
        }

        // Asynchroniczna metoda do ładowania obrazu z URL (z internetu)
        public static async Task<Bitmap?> LoadFromWeb(Uri url)
        {
            try
            {
                // Wysyłamy zapytanie GET do wskazanego URL
                var response = await httpClient.GetAsync(url);
                // Sprawdzamy, czy odpowiedź jest poprawna (status 200)
                response.EnsureSuccessStatusCode();
                
                // Odczytujemy dane obrazu w postaci tablicy bajtów
                var data = await response.Content.ReadAsByteArrayAsync();
                
                // Tworzymy i zwracamy nowy obiekt Bitmap z odczytanych danych (obraz w pamięci)
                return new Bitmap(new MemoryStream(data));
            }
            catch (HttpRequestException ex)
            {
                // Jeśli wystąpił błąd podczas pobierania obrazu, logujemy go na konsoli
                Console.WriteLine($"An error occurred while downloading image '{url}' : {ex.Message}");
                return null;
            }
        }
    }
}
