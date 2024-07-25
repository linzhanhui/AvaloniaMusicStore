using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using ReactiveUI;
using MusicStore.Models;
using Splat;

namespace MusicStore.ViewModels;

public partial class MusicStoreViewModel: ViewModelBase
{
    [ObservableProperty]
    private string? _searchText;
    
    [ObservableProperty] 
    private bool _isBusy;
    
    
    public ObservableCollection<AlbumViewModel> SearchResults { get; } = new();

    [ObservableProperty] 
    private AlbumViewModel? _selectedAlbum;
    
    private CancellationTokenSource? _cancellationTokenSource;

    public ReactiveCommand<Unit, AlbumViewModel?> BuyMusicCommand { get; }

    public MusicStoreViewModel()
    {
        this.WhenAnyValue(x => x.SearchText)
            .Throttle(TimeSpan.FromMilliseconds(400))
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(DoSearch!);
        BuyMusicCommand = ReactiveCommand.Create(() =>
        {
            return SelectedAlbum;
        });
    }
    

    
    private async void DoSearch(string? s)
    {
        IsBusy = true;
        SearchResults.Clear();
        
        _cancellationTokenSource?.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;

        if (!string.IsNullOrWhiteSpace(s))
        {
            var albums = await Album.SearchAsync(s);

            foreach (var album in albums)
            {
                var vm = new AlbumViewModel(album);
                SearchResults.Add(vm);
            }
            
            if (!cancellationToken.IsCancellationRequested)
            {
                LoadCovers(cancellationToken);
            }
        }
        IsBusy = false;
    }
    
    private async void LoadCovers(CancellationToken cancellationToken)
    {
        foreach (var album in SearchResults.ToList())
        {
            await album.LoadCover();

            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
        }
    }
    

}