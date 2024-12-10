using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Timers;
using jadlospis.interfaces;
using jadlospis.Models;
using jadlospis.Utils;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using PdfSharp.Pdf;


namespace jadlospis.ViewModels
{
    public partial class JadlospisPageViewModel : ViewModelBase, IJadlospis
    {

        private string _fileName = "";

        public string FileName
        {
            get => _fileName;
            set => _fileName = value;
        }

        private static Timer _timer = null!;

        public Timer Timer
        {
            get => _timer;
            set => _timer = value;
        }

        public ObservableCollection<KeyValuePair<string, double>> SumNutriments { get; set; }
        public ObservableCollection<KeyValuePair<string, double>> MinNutriments { get; set; }

        public ObservableCollection<DanieViewModel> Dania { get; set; }

        public ObservableCollection<string> AvailableMealsFor { get; } = new()
        {
            "Dorosłych (19-59 lat)",
            "Młodzieży (11-19 lat)",
            "Dzieci (do 11 r.ż)",
            "Seniorów (60+)"
        };

        private string _targetGroup = "Młodzieży (11-19 lat)";

        public string TargetGroup
        {
            get => _targetGroup;
            set
            {
                _targetGroup = value;
                UstawMinNutriments();
            }
        }

        private int _iloscOsob = 1;

        public int IloscOsob
        {
            get => _iloscOsob;
            set
            {
                _iloscOsob = value;
                ObliczSumaCeny();
            }
        }

        [ObservableProperty] private double _sumaCeny;
        public string Name { get; set; }
        public DateTime Data { get; set; }

        public JadlospisPageViewModel()
        {
            _targetGroup = "Młodzieży (11-19 lat)";
            SumNutriments = InitNutrimentsCollection();
            MinNutriments = InitNutrimentsCollection();

            var values = new[] { 260, 50, 2500, 0, 70, 25, 60, 5 };
            var keys = MinNutriments.Select(kv => kv.Key).ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                MinNutriments[i] = new KeyValuePair<string, double>(key, values[i]);
            }

            Dania = new();
            Name = $"Jadłospis {DateTime.Now}";
            Data = DateTime.Now;

            FileName = Name.Replace(" ", "_") + "_" + Data.ToString("yyyy-MM-dd") + ".json";
            
            _timer = new Timer(300000); // 5 min
            _timer.Elapsed += (sender, e) => ZapiszJadlospis();
            _timer.Start();
        }

        public JadlospisPageViewModel(JadlospisPageViewModel jadlospis)
        {
            SumNutriments = jadlospis.SumNutriments;
            MinNutriments = jadlospis.MinNutriments;
            Dania = jadlospis.Dania;
            TargetGroup = jadlospis.TargetGroup;
            IloscOsob = jadlospis.IloscOsob;
            SumaCeny = jadlospis.SumaCeny;
            Name = jadlospis.Name;
            Data = jadlospis.Data;
            FileName = jadlospis.FileName;
            _timer = jadlospis.Timer;
        }

        public void RemoveDanie(DanieViewModel danie)
        {
            Dania.Remove(danie);
            ObliczSumaCeny();
            ObliczSumaNutriments();
        }

        private ObservableCollection<KeyValuePair<string, double>> InitNutrimentsCollection()
        {
            return new ObservableCollection<KeyValuePair<string, double>>
            {
                new("Węglowodany", 0),
                new("Cukier", 0),
                new("Energia", 0),
                new("Kalorie", 0),
                new("Tłuszcz", 0),
                new("Tłuszcze nasycone", 0),
                new("Białko", 0),
                new("Sól", 0)
            };
        }

        public void ObliczSumaCeny()
        {
            SumaCeny = 0;
            foreach (var danie in Dania)
            {
                SumaCeny += danie.Cena;
            }

            SumaCeny *= IloscOsob;
            SumaCeny = Math.Round(SumaCeny, 2);
        }

        public void ObliczSumaNutriments()
        {
            var keys = SumNutriments.Select(kv => kv.Key).ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                var currentValue = SumNutriments.First(kv => kv.Key == key);
                SumNutriments.Remove(currentValue);
                SumNutriments.Add(new KeyValuePair<string, double>(key, 0));
            }

            foreach (var danie in Dania)
            {
                if (danie.Products != null)
                {
                    foreach (var product in danie.Products)
                    {
                        if (product.Products.Nutriments != null)
                        {
                            foreach (var nutriment in new[]
                                     {
                                         ("Węglowodany", product.Products.Nutriments.Carbs),
                                         ("Cukier", product.Products.Nutriments.Sugar),
                                         ("Energia", product.Products.Nutriments.Energy),
                                         ("Kalorie", product.Products.Nutriments.EnergyKcal),
                                         ("Tłuszcz", product.Products.Nutriments.Fat),
                                         ("Tłuszcze nasycone", product.Products.Nutriments.SaturatedFat),
                                         ("Białko", product.Products.Nutriments.Protein),
                                         ("Sól", product.Products.Nutriments.Salt)
                                     })
                            {
                                var current = SumNutriments.First(kv => kv.Key == nutriment.Item1);
                                SumNutriments.Remove(current);
                                SumNutriments.Add(new KeyValuePair<string, double>(
                                    nutriment.Item1,
                                    current.Value + nutriment.Item2
                                ));
                            }
                        }
                    }
                }
            }
        }

        public void UstawMinNutriments()
        {
            var values = TargetGroup switch
            {
                "Dzieci (do 11 r.ż)" => new[] { 200, 40, 0, 1500, 50, 15, 30, 3 },
                "Młodzieży (11-19 lat)" => new[] { 260, 50, 0, 2500, 70, 25, 60, 5 },
                "Dorosłych (19-59 lat)" => new[] { 260, 50, 0, 2000, 60, 20, 50, 5 },
                "Seniorów (60+)" => new[] { 200, 40, 0, 1500, 50, 15, 30, 3 },
                _ => throw new InvalidOperationException()
            };

            var keys = MinNutriments.Select(kv => kv.Key).ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                var key = keys[i];
                var currentValue = MinNutriments.First(kv => kv.Key == key);
                MinNutriments.Remove(currentValue);
                MinNutriments.Add(new KeyValuePair<string, double>(key, values[i]));
            }
        }


        [RelayCommand]
        public void ZapiszJadlospis()
        {
            try
            {
                // Pobierz ścieżkę do katalogu "Dokumenty" użytkownika
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // Utwórz podkatalog "jadlospisy"
                string targetDirectory = Path.Combine(documentsPath, "jadłospisy");
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                // Utwórz nazwę pliku na podstawie nazwy jadłospisu i daty
                string fileName = $"{FileName}";
                string filePath = Path.Combine(targetDirectory, fileName);

                // Serializuj obiekt do JSON
                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                var jadlospisData = new
                {
                    Name,
                    Data,
                    IloscOsob,
                    SumaCeny,
                    TargetGroup,
                    Dania = Dania.Select(d => new
                    {
                        d.Nazwa,
                        d.Cena,
                        Produkty = d.Products?.Select(p => new
                        {
                            p.Products.Name,
                            p.Products.ImageUrl,
                            Nutriments = new
                            {
                                p.Products.Nutriments.Carbs,
                                p.Products.Nutriments.Sugar,
                                p.Products.Nutriments.Energy,
                                p.Products.Nutriments.EnergyKcal,
                                p.Products.Nutriments.Fat,
                                p.Products.Nutriments.SaturatedFat,
                                p.Products.Nutriments.Protein,
                                p.Products.Nutriments.Salt
                            }
                        })
                    }),
                    SumNutriments = SumNutriments.ToDictionary(kv => kv.Key, kv => kv.Value),
                    MinNutriments = MinNutriments.ToDictionary(kv => kv.Key, kv => kv.Value)
                };

                string jsonData = JsonSerializer.Serialize(jadlospisData, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(filePath, jsonData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Błąd podczas zapisu do pliku JSON: {ex.Message}");
            }
        }
        
        // Konwersja Nutriments na ObservableCollection<KeyValuePair<string, double>>
        ObservableCollection<KeyValuePair<string, double>> ConvertNutrimentsToCollection(Nutriments nutriments)
        {
            return new ObservableCollection<KeyValuePair<string, double>>
            {
                new KeyValuePair<string, double>("carbs", nutriments.Carbs),
                new KeyValuePair<string, double>("sugar", nutriments.Sugar),
                new KeyValuePair<string, double>("energy", nutriments.Energy),
                new KeyValuePair<string, double>("energyKcal", nutriments.EnergyKcal),
                new KeyValuePair<string, double>("fat", nutriments.Fat),
                new KeyValuePair<string, double>("saturatedFat", nutriments.SaturatedFat),
                new KeyValuePair<string, double>("protein", nutriments.Protein),
                new KeyValuePair<string, double>("salt", nutriments.Salt)
            };
        }

        public void LoadFromJson(string filePath)
        {
            string jsonContent = File.ReadAllText(filePath);
            var deserializedData = JsonSerializer.Deserialize<DeserializedJadlospis>(jsonContent);

            if (deserializedData == null)
                throw new InvalidDataException("Nie udało się wczytać danych JSON.");


            this.Name = deserializedData.Name;
            this.Data = deserializedData.Data;
            this.IloscOsob = deserializedData.IloscOsob;
            this.SumaCeny = deserializedData.SumaCeny;
            this.TargetGroup = deserializedData.TargetGroup;
            this.Dania = new ObservableCollection<DanieViewModel>();

            int nrDania = 1;
            foreach (var danieData in deserializedData.Dania)
            {
                var danieViewModel = new DanieViewModel(new Danie(nrDania)
                {
                    Nazwa = danieData.Nazwa,
                    Cena = danieData.Cena,
                    Products = new ObservableCollection<ProduktWDaniuViewModel>()
                }, this);

                foreach (var productData in danieData.Produkty)
                {
                    var productViewModel = new ProduktWDaniuViewModel(danieViewModel)
                    {
                        Name = productData.Name,
                        Gramatura = "100",
                        ProduktView = new ObservableCollection<ProduktWJadlospisViewModel>
                        {
                            new ProduktWJadlospisViewModel(new Products
                            {
                                Name = productData.Name,
                                ImageUrl = productData.ImageUrl,
                                Nutriments = new Nutriments
                                {
                                    Carbs = productData.Nutriments.Węglowodany,
                                    Sugar = productData.Nutriments.Cukier,
                                    Energy = productData.Nutriments.Energia,
                                    EnergyKcal = productData.Nutriments.Kalorie,
                                    Fat = productData.Nutriments.Tłuszcz,
                                    SaturatedFat = productData.Nutriments.TłuszczeNasycone,
                                    Protein = productData.Nutriments.Białko,
                                    Salt = productData.Nutriments.Sól
                                }
                            })
                        }
                    };
                    danieViewModel.Products?.Add(productViewModel);
                }

                this.Dania.Add(danieViewModel);
                nrDania++;
            }

            this.ObliczSumaCeny();
            this.ObliczSumaNutriments();
        }

        [RelayCommand]
        public void AddDanie()
        {
            Danie newDanie = new(Dania.Count + 1);
            DanieViewModel daniaV = new DanieViewModel(newDanie, this);
            Dania.Add(daniaV);
            ObliczSumaNutriments();
            ObliczSumaCeny();
        }

        public PdfDocument GetInvoce()
        {
            var document = new Document();

            BuildDocument(document);

            var pdfRenderer = new PdfDocumentRenderer();
            pdfRenderer.Document = document;

            pdfRenderer.RenderDocument();

            return pdfRenderer.PdfDocument;
        }

        private void BuildDocument(Document document)
        {
            // Dodanie sekcji
            Section section = document.AddSection();

           //TODO Dodać informacje o jadłospisie i sformatowac je w odpowiedniej formacie
            Paragraph paragraph = section.AddParagraph();
            ;
            paragraph.AddText($"Jadłospis dla {Name} na dzien {Data}");
            paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 12);
            paragraph.AddLineBreak();

            paragraph = section.AddParagraph();
            paragraph.AddText($"Liczba osob: {IloscOsob}. Łączna cen: {SumaCeny}");
            paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 12);
            paragraph.AddLineBreak();

            foreach (var danie in Dania)
            {
                paragraph = section.AddParagraph();
                paragraph.AddFormattedText($"Danie: {danie.Nazwa}, Cena: {danie.Cena:C}", TextFormat.Bold);
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 11);
                paragraph.AddLineBreak();


                if (danie.Products != null && danie.Products.Any())
                {
                    foreach (var product in danie.Products)
                    {
                        paragraph = section.AddParagraph();
                        paragraph.AddText($"- Produkt: {product.Products.Name}");
                        paragraph.AddLineBreak();
                       
                    }
                }
            }
            // Podsumowanie kaloryczne
    paragraph = section.AddParagraph();
    paragraph.AddFormattedText("Podsumowanie kaloryczne", TextFormat.Bold);
    paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 12);
    paragraph.AddLineBreak();

    paragraph = section.AddParagraph();
    paragraph.AddFormattedText($"Minimalne wymagania kaloryczne dla: {TargetGroup}");
    paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 11);
    paragraph.AddLineBreak();

    foreach (var minNutriment in MinNutriments)
    {
        paragraph = section.AddParagraph();
        paragraph.AddText($"- {minNutriment.Key}: {minNutriment.Value} g");
        paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 10);
    }

    paragraph.AddLineBreak();
    paragraph = section.AddParagraph();
    paragraph.AddText("Rzeczywista wartość wszystkich składników odżywczych z jadłospisu:");
    paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 11);
    paragraph.AddLineBreak();

    foreach (var sumNutriment in SumNutriments)
    {
        paragraph = section.AddParagraph();
        paragraph.AddText($"- {sumNutriment.Key}: {sumNutriment.Value} g");
        paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 10);
    }

    paragraph.AddLineBreak();
    paragraph = section.AddParagraph();
    paragraph.AddFormattedText("Porównanie zapotrzebowania z rzeczywistością:", TextFormat.Bold);
    paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 11);
    paragraph.AddLineBreak();

    foreach (var minNutriment in MinNutriments)
    {
        var actual = SumNutriments.FirstOrDefault(sn => sn.Key == minNutriment.Key).Value;
        var difference = actual - minNutriment.Value;
        paragraph = section.AddParagraph();
        paragraph.AddText($"- {minNutriment.Key}: {actual} g (wymagane: {minNutriment.Value} g, różnica: {difference:+0.##;-0.##;0} g)");
        paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 10);
    }

        }

        [RelayCommand]
        public void SaveAsPdf()
        {
            try
            {
                var document = GetInvoce();

                // Pobierz ścieżkę do katalogu "Dokumenty" użytkownika
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // Utwórz podkatalog "jadłospisy"
                string targetDirectory = Path.Combine(documentsPath, "jadłospisy");
                if (!Directory.Exists(targetDirectory))
                {
                    Directory.CreateDirectory(targetDirectory);
                }

                // Zastąp niedozwolone znaki w nazwie pliku
                string sanitizedFileName =
                    string.Concat(Name.Select(ch => Path.GetInvalidFileNameChars().Contains(ch) ? '.' : ch));

                // Utwórz pełną ścieżkę pliku
                string targetFilePath = Path.Combine(targetDirectory, $"{sanitizedFileName}.pdf");

                // Zapisz dokument PDF
                document.Save(targetFilePath);
                Debug.WriteLine($"Plik PDF zapisano pomyślnie pod ścieżką: {targetFilePath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Błąd podczas zapisywania pliku PDF: {ex.Message}");
            }
        }
    }
}