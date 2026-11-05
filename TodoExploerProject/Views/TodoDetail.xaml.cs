using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoExploerProject.Models;
using TodoExploerProject.ViewModels;

namespace TodoExploerProject.Views;

public partial class TodoDetail : ContentPage
{
    public TodoDetail(ITodoDetailViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}