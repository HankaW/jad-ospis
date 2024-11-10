using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using jadlospis.Utils;

namespace jadlospis.Views;

public partial class ProduktPageView : UserControl
{
    public ProduktPageView()
    {
        InitializeComponent();
    }
}