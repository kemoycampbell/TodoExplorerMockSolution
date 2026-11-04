using Microsoft.Extensions.DependencyInjection;
using TodoExploerProject.Services;

namespace TodoExploerProject;

public partial class App : Application
{
    private MainPage mainPage;
    private readonly INavigationService navigationService;
    
    
    public App(
        INavigationService navigationService,
        MainPage mainPage
            )
    {
        InitializeComponent();
        this.mainPage = mainPage;
        this.navigationService = navigationService;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        //return new Window(new AppShell());
        NavigationPage navigationPage = new NavigationPage(mainPage);
        this.navigationService.setNavigation(navigationPage.Navigation);
        return new Window(navigationPage);
    }
}