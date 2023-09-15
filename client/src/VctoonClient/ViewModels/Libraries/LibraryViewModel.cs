using System.Collections.Generic;
using System.Collections.ObjectModel;
using VctoonClient.Navigations.Query;
using VctoonCore.Libraries.Dtos;
using VctoonCore.Resources;
using VctoonCore.Resources.Dtos;

namespace VctoonClient.ViewModels.Libraries;

[QueryProperty(nameof(Library))]
public partial class LibraryViewModel : ViewModelBase, ITransientDependency, IQueryAttributable
{
    private readonly IComicAppService _comicAppService;
    private readonly IVideoAppService _videoAppService;

    [ObservableProperty]
    private LibraryDto library;

    [ObservableProperty]
    private GetComicListInput comicListInput = new GetComicListInput()
    {

    };

    public ObservableCollection<ComicDto> Comics = new ObservableCollection<ComicDto>();

    public ObservableCollection<VideoDto> Videos = new ObservableCollection<VideoDto>();





    public LibraryViewModel(IComicAppService comicAppService, IVideoAppService videoAppService)
    {
        _comicAppService = comicAppService;
        _videoAppService = videoAppService;
    }


    public async void LoadData()
    {
        // if (Library.LibraryType == LibraryType.Comic)
        // {
        //      = await _comicAppService.GetListAsync(ComicListInput);
        // }
        // else
        // {
        //
        // }
    }



    public void ApplyQueryAttributes(Dictionary<string, object>? paras, bool isBack)
    {
    }
}