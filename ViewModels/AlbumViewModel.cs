using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MusicStore.ViewModels;
using MusicStore.Models;
using Avalonia.Media.Imaging;

public partial class AlbumViewModel (Album album): ViewModelBase
{
    private readonly Album _album = album;
    public string Artist => _album.Artist;
    public string Title => _album.Title;
    
    [ObservableProperty] 
    private Bitmap? _cover;
    
    
    public async Task LoadCover()
    {
        await using (var imageStream = await _album.LoadCoverBitmapAsync())
        {
            Cover = await Task.Run(() => Bitmap.DecodeToWidth(imageStream, 400));
        }
    }
    
    public async Task SaveToDiskAsync()
    {
        await _album.SaveAsync();

        if (Cover != null)
        {
            var bitmap = Cover;

            await Task.Run(() =>
            {
                using (var fs = _album.SaveCoverBitmapStream())
                {
                    bitmap.Save(fs);
                }
            });
        }
    }
}