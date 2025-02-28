using Avalonia.Controls;
using BitfinexUI.ViewModels;
using System;

namespace BitfinexUI.Views;

public partial class CandlesView : UserControl
{
    private CandlesViewModel vm;
    public CandlesView()
    {
        InitializeComponent();
        this.DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        var viewModel = DataContext as CandlesViewModel;

        if (viewModel != null)
        {
            this.vm = viewModel;
        }
    }
    private void OnButtonSpinnerSpin(object? sender, SpinEventArgs e)
    {
        if (e.Direction == SpinDirection.Increase)
        {
            vm.IncreaseCandlesCount();
        }

        if (e.Direction == SpinDirection.Decrease)
        {
            vm.DecreaseCandlesCount();
        }
    }
}