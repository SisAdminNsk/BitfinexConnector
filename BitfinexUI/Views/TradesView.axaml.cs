using Avalonia.Controls;
using Avalonia.Interactivity;
using BitfinexUI.ViewModels;
using System;

namespace BitfinexUI.Views;

public partial class TradesView : UserControl
{
    private TradesViewModel vm;

    public TradesView()
    {
        InitializeComponent();
        this.DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        var viewModel = DataContext as TradesViewModel;

        if (viewModel != null)
        {
            this.vm = viewModel;
        }
    }

    private void OnLoadDataClick(object sender, RoutedEventArgs e)
    {
        vm.LoadTrades();
    }
}