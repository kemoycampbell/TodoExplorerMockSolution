using TodoExploerProject.ViewModels;

namespace TodoExploerProject;

public partial class MainPage : ContentPage
{
    private IMainPageViewModel viewModel;

    public MainPage(IMainPageViewModel viewModel)
    {
        this.viewModel = viewModel;
        InitializeComponent();
        BindingContext = viewModel;
        
    }
    
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await viewModel.LoadTodos();
        });
    }
}