using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoExploerProject.Models;
using TodoExploerProject.ViewModels;

namespace TodoExploerProject.Views;

public partial class Favorites : ContentPage
{
    private IFavoriteViewModel viewModel;
    public Favorites(
        IFavoriteViewModel viewModel
        )
    {
        this.viewModel = viewModel;
        InitializeComponent();
        BindingContext = viewModel;
    }
}