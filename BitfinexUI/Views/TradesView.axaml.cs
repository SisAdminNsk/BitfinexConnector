using Avalonia.Controls;
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
    private void OnButtonSpinnerSpin(object? sender, SpinEventArgs e)
    {
        if(e.Direction == SpinDirection.Increase)
        {
            vm.IncreaseTradesCount();
        }

        if(e.Direction == SpinDirection.Decrease)
        {
            vm.DecreaseTradesCount(); 
        }       
    }
}