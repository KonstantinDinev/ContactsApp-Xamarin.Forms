using System;
using System.Collections.Generic;
using MyContactsMVVM.Models;
using MyContactsMVVM.Persistance;
using MyContactsMVVM.ViewModels;
using SQLite;

using Xamarin.Forms;

namespace MyContactsMVVM.Views
{
    public partial class ContactDetailPage : ContentPage
    {
        public ContactDetailPage(ContactDetailsViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = viewModel;
        }
    }
}