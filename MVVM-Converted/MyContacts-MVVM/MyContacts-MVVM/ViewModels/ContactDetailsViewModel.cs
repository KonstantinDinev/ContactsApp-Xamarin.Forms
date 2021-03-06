﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MyContactsMVVM.Models;
using Xamarin.Forms;

namespace MyContactsMVVM.ViewModels
{
    public class ContactDetailsViewModel
    {
        private readonly IContactStore _contactStore;
        private readonly IPageService _pageService;

        public event EventHandler<Contact> ContactAdded;
        public event EventHandler<Contact> ContactUpdated;

        public Contact Contact { get; private set; }

        public ICommand SaveCommand { get; private set; }

        public ContactDetailsViewModel(ContactViewModel viewModel, IContactStore contactStore, IPageService pageService)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));

            _pageService = pageService;
            _contactStore = contactStore;

            SaveCommand = new Command(async () => await Save());

            Contact = new Contact
            {
                Id = viewModel.Id,
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                PhoneNumber = viewModel.PhoneNumber,
                Email = viewModel.Email,
                IsBlocked = viewModel.IsBlocked
            };
        }

        async Task Save()
        {
            if (String.IsNullOrWhiteSpace(Contact.FirstName) &&
                String.IsNullOrWhiteSpace(Contact.LastName))
            {
                await _pageService.DisplayAlert("Error", "Please enter the name.", "OK");
                return;
            }

            if (Contact.Id == 0)
            {
                await _contactStore.AddContact(Contact);
                ContactAdded?.Invoke(this, Contact);
            }
            else
            {
                await _contactStore.UpdateContact(Contact);
                ContactUpdated?.Invoke(this, Contact);
            }

            await _pageService.PopAsync();
        }
    }
}
