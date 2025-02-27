using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;

namespace BitfinexUI.Views;

public partial class RestView : UserControl
{
    public RestView()
    {
        InitializeComponent();
    }

    private void OnRequestDataClick(object sender, RoutedEventArgs e)
    {
        var selectedCurrencyPair = (CurrencyPairsComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();

        if (string.IsNullOrEmpty(selectedCurrencyPair))
        {
            return;
        }

        var currencyData = new List<string>
        {
                $"Курс {selectedCurrencyPair}: 50000",
                $"Объем {selectedCurrencyPair}: 1000",
                $"Изменение {selectedCurrencyPair}: +2%"
        };
    }
}