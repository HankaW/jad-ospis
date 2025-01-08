using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Timers;
using jadlospis.Models;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using PdfSharp.Drawing;


namespace jadlospis.ViewModels
{
    public partial class JadlospisPageViewModel : ViewModelBase
    {
        
        private Jadlospis _jadlospis;

        [ObservableProperty] private string _fileName;
        
        private Timer _timer;

        public ObservableCollection<DanieViewModel> Dania { get; set; }
        public ObservableCollection<KeyValuePair<string, double>> SumNutriment { get; set; }
        public ObservableCollection<KeyValuePair<string, double>>? MinNutriment { get; set; }

        public ObservableCollection<string> AvailableMealsFor { get; } = new()
        {
            "Dzieci (do 11 r.ż)",
            "Młodzieży (11-19 lat)",
            "Dorosłych (19-59 lat)",
            "Seniorów (60+)"
        };
        
        private string? _targetGroup = "Młodzieży (11-19 lat)";
        public string? TargetGroup
        {
            get => _targetGroup;
            set
            {
                _targetGroup = value;
                _jadlospis.TargetGroup = value;
                _jadlospis.UstawMinNutriments();
                UpdateDictionary(this.MinNutriment, _jadlospis.MinNutriments);
            }
        }
        
        private int _iloscOsob = 1;

        public int IloscOsob
        {
            get => _iloscOsob;
            set
            {
                _iloscOsob = value;
                _jadlospis.IloscOsob = value;
                ObliczSumaCeny();
            }
        }
        
        [ObservableProperty] private double _sumaCeny;
        
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                _jadlospis.Name = value;
            }
        }

        public JadlospisPageViewModel()
        {
            
            _jadlospis = new Jadlospis();
            TargetGroup = _jadlospis.TargetGroup;
            Dania = new ObservableCollection<DanieViewModel>(); 
            SumNutriment = new ObservableCollection<KeyValuePair<string, double>>();
            MinNutriment = new ObservableCollection<KeyValuePair<string, double>>();
            if (_jadlospis.SumNutriments != null)
                foreach (var item in _jadlospis.SumNutriments)
                    SumNutriment.Add(new KeyValuePair<string, double>(item.Key, item.Value));

            if (_jadlospis.MinNutriments != null)
                foreach (var item in _jadlospis.MinNutriments)
                    MinNutriment.Add(new KeyValuePair<string, double>(item.Key, item.Value));

            SumaCeny = _jadlospis.SumaCeny;
            IloscOsob = _jadlospis.IloscOsob;
            Name = _jadlospis.Name;
            FileName = _jadlospis.FileName;

            _timer = new Timer(300000);
            _timer.Elapsed += (sender, e) => SaveJadlospis();
            _timer.Start();
            
            SaveJadlospis();
        }

        public JadlospisPageViewModel(Jadlospis jadlospis)
        {
            _jadlospis = jadlospis;
            TargetGroup = _jadlospis.TargetGroup;
            Dania = new ObservableCollection<DanieViewModel>();
            SumNutriment = new ObservableCollection<KeyValuePair<string, double>>();
            MinNutriment = new ObservableCollection<KeyValuePair<string, double>>();
            
            if (_jadlospis.SumNutriments != null)
                foreach (var item in _jadlospis.SumNutriments)
                    SumNutriment.Add(new KeyValuePair<string, double>(item.Key, item.Value));
            if (_jadlospis.MinNutriments != null)
                foreach (var item in _jadlospis.MinNutriments)
                    MinNutriment.Add(new KeyValuePair<string, double>(item.Key, item.Value));
                

            SumaCeny = _jadlospis.SumaCeny;
            IloscOsob = _jadlospis.IloscOsob;
            Name = _jadlospis.Name;
            FileName = _jadlospis.FileName;
            
            _timer = new Timer(300000);
            _timer.Elapsed += (sender, e) => SaveJadlospis();
            _timer.Start();
            
            ReadDania();
        }
        
        public void ObliczSumaCeny()
        {
            _jadlospis.ObliczSumaCeny();
            SumaCeny = _jadlospis.SumaCeny;
        }
        
        public void ObliczSumaNutriments()
        {
            _jadlospis.ObliczSumaNutriments();
            UpdateDictionary(this.SumNutriment, _jadlospis.SumNutriments);
        }

        [RelayCommand]
        public void SaveJadlospis()
        {
            _jadlospis.SaveToJson();
        }
        
        [RelayCommand]
        public void AddDanie()
        {
            _jadlospis.AddDanie();
            ReadDania();
        }
        
        public void DeleteDanie(Danie danie)
        {
            _jadlospis.DeleteDanie(danie);
            
            ReadDania();
        }

        public void ReadDania()
        {
            this.Dania.Clear();
            foreach (var d in _jadlospis.Dania)
            {
                DanieViewModel danieModel = new DanieViewModel(d, this);
                danieModel.Produkty?.Clear();
                int i = 0;
                foreach (var p in d.Produkty)
                {
                    ProduktWDaniuViewModel temp = new ProduktWDaniuViewModel(danieModel, i);
                    temp.Produkty = p;
                    temp.ProduktView.Add(new ProduktWJadlospisViewModel(p));
                    temp.Name = p.Name;
                    temp.Gramatura = p.ProductsGram;
                    temp.IsVisible = true;
                    danieModel.Produkty?.Add(temp);
                    i++;
                }
                this.Dania.Add(danieModel);
            }
        }
        
        void UpdateDictionary( 
            ObservableCollection<KeyValuePair<string, double>>? collection, 
            Dictionary<string, double>? dictionary)
        {
            if(collection == null || dictionary == null) return;
            collection.Clear();
            foreach (var item in dictionary)
            {
                collection.Add(new KeyValuePair<string, double>(item.Key, Math.Round(item.Value,2)));
            }
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
        
            // Ścieżka relatywna do obrazu
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Images", "zdj.png");
        
            if (!File.Exists(imagePath))
            {
                Console.WriteLine($"Plik obrazu nie istnieje: {imagePath}");
                throw new FileNotFoundException($"Nie znaleziono pliku: {imagePath}");
            }
        
            // Załaduj obrazek
            XImage image = XImage.FromFile(imagePath);
        
            // Dodanie obrazka do sekcji nagłówka
            Paragraph headerImageParagraph = section.Headers.Primary.AddParagraph();
            headerImageParagraph.Format.Alignment = ParagraphAlignment.Left;
            headerImageParagraph.Format.LeftIndent = 0; // Brak marginesu od lewej
            headerImageParagraph.Format.SpaceAfter = "30pt"; // Większy odstęp pod obrazkiem
        
            // Dodanie obrazu i ustawienie jego rozmiaru
            Image headerImage = headerImageParagraph.AddImage(imagePath);
            headerImage.Width = Unit.FromCentimeter(3); // Ustaw szerokość obrazka na 3 cm
            headerImage.Height = Unit.FromCentimeter(3); // Ustaw wysokość obrazka na 3 cm
            
            // Dodanie nagłówka JADŁOSPIS na środku strony
            Paragraph titleParagraph = section.AddParagraph();
            titleParagraph.AddFormattedText("JADŁOSPIS", TextFormat.Bold);
            titleParagraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 20);
            titleParagraph.Format.Alignment = ParagraphAlignment.Center;
            titleParagraph.Format.SpaceAfter = "5pt";
        
            // Dodanie nazwy jadłospisu na środku strony pod tytułem
            Paragraph nameParagraph = section.AddParagraph();
            nameParagraph.AddFormattedText($"\"{_jadlospis.Name.ToUpper()}\"", TextFormat.Italic);  
            nameParagraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 16);
            nameParagraph.Format.Alignment = ParagraphAlignment.Center;
            nameParagraph.Format.SpaceAfter = "20pt";
         
            
            // Dodanie liczby osób i ceny pod obrazkiem
            Paragraph imageDetailsParagraph = section.Headers.Primary.AddParagraph();
            imageDetailsParagraph.AddText($"Liczba osób: {_jadlospis.IloscOsob}\nŁączna cena: {_jadlospis.SumaCeny:C}");
            imageDetailsParagraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 12);
            imageDetailsParagraph.Format.Alignment = ParagraphAlignment.Left;
            imageDetailsParagraph.Format.SpaceAfter = "40pt"; // Odstęp pod tekstem "Łączna cena"
        
            // Dodanie pustego akapitu, aby upewnić się, że kolejne elementy nie nachodzą
            section.AddParagraph().AddLineBreak();
            section.AddParagraph().AddLineBreak();
            section.AddParagraph().AddLineBreak();
            section.AddParagraph().AddLineBreak();
           
        
            // Iteracja przez dania z numeracją i odstępami
            int danieNumer = 1;
            foreach (var danie in _jadlospis.Dania)
            {
                // Dodanie odstępu przed kolejnym daniem
                if (danieNumer > 1)
                {
                    section.AddParagraph().AddLineBreak();
                }
                else
                {
                    section.AddParagraph().AddLineBreak(); // Dodatkowy odstęp przed pierwszym daniem
                }
        
                // Nagłówek dania
                Paragraph paragraph = section.AddParagraph();
                paragraph.AddFormattedText($"Danie {danieNumer}:", TextFormat.Bold);
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 12);
                paragraph.Format.SpaceAfter = "5pt";
                // Szczegóły dania
                paragraph = section.AddParagraph();
                paragraph.AddText($"{danie.Nazwa}");
                paragraph.AddLineBreak();
                paragraph.AddText($"Cena: {danie.Cena:C}");
                paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 11);
                paragraph.Format.SpaceAfter = "10pt"; // Większy odstęp między szczegółami a produktami
        
                // Iteracja przez produkty w daniu
                if (danie.Produkty.Any())
                {
                    foreach (var product in danie.Produkty)
                    {
                        paragraph = section.AddParagraph();
                        paragraph.AddText($"- produkt: \"{product.Name}\" (gramatura: {product.ProductsGram} g)");
                        paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 10);
                        paragraph.Format.SpaceAfter = "2pt";
                    }
                }
        
                danieNumer++;
            }
        
            // Większy odstęp między ostatnim daniem a podsumowaniem kalorycznym
            section.AddParagraph().AddLineBreak();
        
            // Podsumowanie kaloryczne jako tabela
            Paragraph summaryParagraph = section.AddParagraph();
            summaryParagraph.AddFormattedText($"Podsumowanie kaloryczne dla: {_jadlospis.TargetGroup}", TextFormat.Bold);
            summaryParagraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 12);
            summaryParagraph.Format.SpaceBefore = "15pt"; // Większy odstęp przed tabelą
            summaryParagraph.Format.SpaceAfter = "10pt";
        
            Table table = section.AddTable();
            table.Borders.Width = 1.0; // Grubsze obramowanie
            table.Borders.Color = MigraDoc.DocumentObjectModel.Color.FromRgb(120, 120, 120);
        
            // Wyśrodkowanie tabeli na stronie
            table.Rows.LeftIndent = Unit.FromCentimeter(1); // Ustawienie odpowiedniego marginesu
            table.Format.Alignment = ParagraphAlignment.Center;
        
            // Dodanie kolumn do tabeli
            Column column1 = table.AddColumn(Unit.FromCentimeter(7)); // Węższe kolumny
            Column column2 = table.AddColumn(Unit.FromCentimeter(7));
        
            // Nagłówki tabeli
            Row headerRow = table.AddRow();
            headerRow.Cells[0].AddParagraph("Minimalne wymagania kaloryczne");
            headerRow.Cells[1].AddParagraph("Jadłospis realizuje");
            headerRow.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 11);
            headerRow.Format.Alignment = ParagraphAlignment.Center;
            headerRow.Shading.Color = MigraDoc.DocumentObjectModel.Color.FromRgb(200, 230, 255);
        
            // Wypełnienie danych w tabeli
            foreach (var minNutriment in this.MinNutriment)
            {
                Row row = table.AddRow();
                row.Cells[0].AddParagraph($"{minNutriment.Key}: {minNutriment.Value} g");
                row.Cells[0].Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 9);
        
                var actual = this.SumNutriment.FirstOrDefault(sn => sn.Key == minNutriment.Key).Value;
                var difference = actual - minNutriment.Value;
                row.Cells[1].AddParagraph($"{actual} g (różnica: {difference:+0.##;-0.##;0} g)");
                row.Cells[1].Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 9);
            }
        
            // Ustawienie odstępów między wierszami tabeli
            foreach (Row row in table.Rows)
            {
                row.TopPadding = Unit.FromPoint(3); // Mniejsze odstępy w tabeli
                row.BottomPadding = Unit.FromPoint(3);
            }
        }
        [RelayCommand]
        public void SaveAsPdf()
        {
            try
            {
                var document = GetInvoce();
        
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string pdfName = Name + ".pdf";
                string targetDirectory = Path.Combine(documentsPath, pdfName);
        
                // Zapisz dokument PDF
                document.Save(targetDirectory);
                Console.WriteLine($"Plik PDF zapisano pomyślnie pod ścieżką: {targetDirectory}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd podczas zapisywania pliku PDF: {ex.Message}");
            }
        }
    }
}