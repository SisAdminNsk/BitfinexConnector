using Avalonia.Controls;
using BitfinexUI.ViewModels;
using System;

namespace BitfinexUI.Views;

public partial class TickerView : UserControl
{
    private TickerViewModel vm;

    public TickerView()
    {
        InitializeComponent();
        this.DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        var viewModel = DataContext as TickerViewModel;

        if (viewModel != null)
        {
            this.vm = viewModel;
        }
    }

}