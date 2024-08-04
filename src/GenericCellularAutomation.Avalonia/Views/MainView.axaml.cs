using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;

namespace GenericCellularAutomation.Avalonia.Views;

public sealed partial class MainView : UserControl
{
    public MainView() { }
    
    public MainView(Action shutdownAction)
    {
        InitializeComponent();

        TitleBar.CloseCommand = new RelayCommand(shutdownAction);
    }
}