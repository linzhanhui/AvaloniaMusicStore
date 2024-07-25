using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;

namespace MusicStore.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        // Initialize the ViewModel
    }

    [RelayCommand]
    private void BuyMusic()
    {
        Console.WriteLine("Music bought!");
    }
}