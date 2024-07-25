using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Microsoft.VisualBasic;
using ReactiveUI;
using MusicStore.Models; 
using System.Reactive.Concurrency;

namespace MusicStore.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    
    public Interaction<MusicStoreViewModel, AlbumViewModel?> ShowDialog { get;  }
    
    public ObservableCollection<AlbumViewModel> Albums { get; } = new();

    public MainWindowViewModel()
    {
        // Initialize the ViewModel
        ShowDialog = new Interaction<MusicStoreViewModel, AlbumViewModel?>();
        RxApp.MainThreadScheduler.Schedule(LoadAlbums);
    }
    
    [RelayCommand]
    private async Task BuyMusic()
    {
        var store = new MusicStoreViewModel();

        var result = await ShowDialog.Handle(store);
        
        if (result != null)
        {
            Albums.Add(result);
            await result.SaveToDiskAsync();
        }
        
    }
    
    private async void LoadAlbums()
    {
        var albums = (await Album.LoadCachedAsync()).Select(x => new AlbumViewModel(x));

        foreach (var album in albums)
        {
            Albums.Add(album);
        }

        foreach (var album in Albums.ToList())
        {
            await album.LoadCover();
        }
    }




}