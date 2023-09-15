using VctoonCore.Libraries.Dtos;
using Volo.Abp.Validation;

namespace VctoonClient.ViewModels.Homes;

public partial class HomeViewModel : ViewModelBase, ITransientDependency
{
    private readonly IObjectValidator _objectValidator;

    [ObservableProperty]
    private LibraryCreateUpdateInput _library = new LibraryCreateUpdateInput();

    public HomeViewModel(IObjectValidator objectValidator)
    {
        _objectValidator = objectValidator;
    }

    public async void Validation()
    {
        try
        {
            await _objectValidator.ValidateAsync(Library);
        }
        catch (AbpValidationException e)
        {
        }
    }

    public async void ShowDialog()
    {
    }

    public async void ShowLoading()
    {
        using var loading = Dialog.ShowLoading();

        await Task.Delay(1000);
    }
}