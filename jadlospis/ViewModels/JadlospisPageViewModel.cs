using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using jadlospis.interfaces;
using jadlospis.Models;
using jadlospis.Utils;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;


namespace jadlospis.ViewModels
{
    public partial class JadlospisPageViewModel : ViewModelBase
    {

        // public PdfDocument GetInvoce()
        // {
        //     var document = new Document();
        //
        //     BuildDocument(document);
        //
        //     var pdfRenderer = new PdfDocumentRenderer();
        //     pdfRenderer.Document = document;
        //
        //     pdfRenderer.RenderDocument();
        //
        //     return pdfRenderer.PdfDocument;
        // }
        //
        // private void BuildDocument(Document document)
        // {
        //     // Dodanie sekcji
        //     Section section = document.AddSection();
        //
        //     // Ścieżka relatywna do obrazu
        //     string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Images", "zdj.png");
        //
        //     if (!File.Exists(imagePath))
        //     {
        //         Console.WriteLine($"Plik obrazu nie istnieje: {imagePath}");
        //         throw new FileNotFoundException($"Nie znaleziono pliku: {imagePath}");
        //     }
        //
        //     // Załaduj obrazek
        //     XImage image = XImage.FromFile(imagePath);
        //
        //     // Dodanie obrazka do sekcji nagłówka
        //     Paragraph headerImageParagraph = section.Headers.Primary.AddParagraph();
        //     headerImageParagraph.Format.Alignment = ParagraphAlignment.Left;
        //     headerImageParagraph.Format.LeftIndent = 0; // Brak marginesu od lewej
        //     headerImageParagraph.Format.SpaceAfter = "30pt"; // Większy odstęp pod obrazkiem
        //
        //     // Dodanie obrazu i ustawienie jego rozmiaru
        //     Image headerImage = headerImageParagraph.AddImage(imagePath);
        //     headerImage.Width = Unit.FromCentimeter(3); // Ustaw szerokość obrazka na 3 cm
        //     headerImage.Height = Unit.FromCentimeter(3); // Ustaw wysokość obrazka na 3 cm
        //     
        //     // Dodanie nagłówka JADŁOSPIS na środku strony
        //     Paragraph titleParagraph = section.AddParagraph();
        //     titleParagraph.AddFormattedText("JADŁOSPIS", TextFormat.Bold);
        //     titleParagraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 20);
        //     titleParagraph.Format.Alignment = ParagraphAlignment.Center;
        //     titleParagraph.Format.SpaceAfter = "5pt";
        //
        //     // Dodanie nazwy jadłospisu na środku strony pod tytułem
        //     Paragraph nameParagraph = section.AddParagraph();
        //     nameParagraph.AddFormattedText($",, {Name.ToUpper()} ''", TextFormat.Italic);
        //     nameParagraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 16);
        //     nameParagraph.Format.Alignment = ParagraphAlignment.Center;
        //     nameParagraph.Format.SpaceAfter = "20pt";
        //  
        //     
        //     // Dodanie liczby osób i ceny pod obrazkiem
        //     Paragraph imageDetailsParagraph = section.Headers.Primary.AddParagraph();
        //     imageDetailsParagraph.AddText($"Liczba osób: {IloscOsob}\nŁączna cena: {SumaCeny:C}");
        //     imageDetailsParagraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 12);
        //     imageDetailsParagraph.Format.Alignment = ParagraphAlignment.Left;
        //     imageDetailsParagraph.Format.SpaceAfter = "40pt"; // Odstęp pod tekstem "Łączna cena"
        //
        //     // Dodanie pustego akapitu, aby upewnić się, że kolejne elementy nie nachodzą
        //     section.AddParagraph().AddLineBreak();
        //     section.AddParagraph().AddLineBreak();
        //     section.AddParagraph().AddLineBreak();
        //     section.AddParagraph().AddLineBreak();
        //    
        //
        //     // Iteracja przez dania z numeracją i odstępami
        //     int danieNumer = 1;
        //     foreach (var danie in Dania)
        //     {
        //         // Dodanie odstępu przed kolejnym daniem
        //         if (danieNumer > 1)
        //         {
        //             section.AddParagraph().AddLineBreak();
        //         }
        //         else
        //         {
        //             section.AddParagraph().AddLineBreak(); // Dodatkowy odstęp przed pierwszym daniem
        //         }
        //
        //         // Nagłówek dania
        //         Paragraph paragraph = section.AddParagraph();
        //         paragraph.AddFormattedText($"Danie {danieNumer}:", TextFormat.Bold);
        //         paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 12);
        //         paragraph.Format.SpaceAfter = "5pt";
        //         // Szczegóły dania
        //         paragraph = section.AddParagraph();
        //         paragraph.AddText($"{danie.Nazwa}");
        //         paragraph.AddLineBreak();
        //         paragraph.AddText($"Cena: {danie.Cena:C}");
        //         paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 11);
        //         paragraph.Format.SpaceAfter = "10pt"; // Większy odstęp między szczegółami a produktami
        //
        //         // Iteracja przez produkty w daniu
        //         if (danie.Products != null && danie.Products.Any())
        //         {
        //             foreach (var product in danie.Products)
        //             {
        //                 paragraph = section.AddParagraph();
        //                 paragraph.AddText($"- produkt: \"{product.Products.Name}\" (gramatura: {product.Gramatura} g)");
        //                 paragraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 10);
        //                 paragraph.Format.SpaceAfter = "2pt";
        //             }
        //         }
        //
        //         danieNumer++;
        //     }
        //
        //     // Większy odstęp między ostatnim daniem a podsumowaniem kalorycznym
        //     section.AddParagraph().AddLineBreak();
        //
        //     // Podsumowanie kaloryczne jako tabela
        //     Paragraph summaryParagraph = section.AddParagraph();
        //     summaryParagraph.AddFormattedText($"Podsumowanie kaloryczne dla: {TargetGroup}", TextFormat.Bold);
        //     summaryParagraph.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 12);
        //     summaryParagraph.Format.SpaceBefore = "15pt"; // Większy odstęp przed tabelą
        //     summaryParagraph.Format.SpaceAfter = "10pt";
        //
        //     Table table = section.AddTable();
        //     table.Borders.Width = 1.0; // Grubsze obramowanie
        //     table.Borders.Color = MigraDoc.DocumentObjectModel.Color.FromRgb(120, 120, 120);
        //
        //     // Wyśrodkowanie tabeli na stronie
        //     table.Rows.LeftIndent = Unit.FromCentimeter(1); // Ustawienie odpowiedniego marginesu
        //     table.Format.Alignment = ParagraphAlignment.Center;
        //
        //     // Dodanie kolumn do tabeli
        //     Column column1 = table.AddColumn(Unit.FromCentimeter(7)); // Węższe kolumny
        //     Column column2 = table.AddColumn(Unit.FromCentimeter(7));
        //
        //     // Nagłówki tabeli
        //     Row headerRow = table.AddRow();
        //     headerRow.Cells[0].AddParagraph("Minimalne wymagania kaloryczne");
        //     headerRow.Cells[1].AddParagraph("Jadłospis realizuje");
        //     headerRow.Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 11);
        //     headerRow.Format.Alignment = ParagraphAlignment.Center;
        //     headerRow.Shading.Color = MigraDoc.DocumentObjectModel.Color.FromRgb(200, 230, 255);
        //
        //     // Wypełnienie danych w tabeli
        //     foreach (var minNutriment in MinNutriments)
        //     {
        //         Row row = table.AddRow();
        //         row.Cells[0].AddParagraph($"{minNutriment.Key}: {minNutriment.Value} g");
        //         row.Cells[0].Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 9);
        //
        //         var actual = SumNutriments.FirstOrDefault(sn => sn.Key == minNutriment.Key).Value;
        //         var difference = actual - minNutriment.Value;
        //         row.Cells[1].AddParagraph($"{actual} g (różnica: {difference:+0.##;-0.##;0} g)");
        //         row.Cells[1].Format.Font = new MigraDoc.DocumentObjectModel.Font("Arial", 9);
        //     }
        //
        //     // Ustawienie odstępów między wierszami tabeli
        //     foreach (Row row in table.Rows)
        //     {
        //         row.TopPadding = Unit.FromPoint(3); // Mniejsze odstępy w tabeli
        //         row.BottomPadding = Unit.FromPoint(3);
        //     }
        // }
        // [RelayCommand]
        // public void SaveAsPdf()
        // {
        //     try
        //     {
        //         var document = GetInvoce();
        //
        //         // Pobierz ścieżkę do katalogu "Dokumenty" użytkownika
        //         string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        //
        //         // Zastąp niedozwolone znaki w nazwie pliku
        //         string sanitizedFileName =
        //             string.Concat(Name.Select(ch => Path.GetInvalidFileNameChars().Contains(ch) ? '.' : ch));
        //
        //         // Dodaj datę do nazwy pliku w formacie: "yyyy-MM-dd_HH-mm-ss"
        //         string datePart = DateTime.Now.ToString("yyyy.MM.dd H-mm-ss");
        //         string targetFilePath = Path.Combine(documentsPath, $"jadlospis {datePart}.pdf");
        //
        //         // Zapisz dokument PDF
        //         document.Save(targetFilePath);
        //         Debug.WriteLine($"Plik PDF zapisano pomyślnie pod ścieżką: {targetFilePath}");
        //     }
        //     catch (Exception ex)
        //     {
        //         Debug.WriteLine($"Błąd podczas zapisywania pliku PDF: {ex.Message}");
        //     }
        // }
    }
}