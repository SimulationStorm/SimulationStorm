using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;

namespace SimulationStorm.GameOfLife.Avalonia.Views;

public partial class MainView : UserControl
{
    public MainView() { }
    
    public MainView(Action shutdownAction)
    {
        InitializeComponent();

        TitleBar.CloseCommand = new RelayCommand(shutdownAction);
    }
}