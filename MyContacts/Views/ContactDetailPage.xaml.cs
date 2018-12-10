using System;
using System.Collections.Generic;

using MyContacts.Models;
using MyContacts.Persistance;
using SQLite;

using Xamarin.Forms;

namespace MyContacts.Views
{
    public partial class ContactDetailPage : ContentPage
    {
        public event EventHandler<Contact> ContactAdded;
        public event EventHandler<Contact> ContactUpdated;

        private SQLiteAsyncConnection _connection;

        //default ctor
        public ContactDetailPage() { InitializeComponent(); }

        //ctor with params
        public ContactDetailPage(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            InitializeComponent();

            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();

            BindingContext = new Contact
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                ImageUrl = contact.ImageUrl,
                PhoneNumber = contact.PhoneNumber,
                Email = contact.Email,
                IsBlocked = contact.IsBlocked
            };
        }

        async void OnSaveBtn(object sender, System.EventArgs e)
        {
            var contact = BindingContext as Contact;

            if (String.IsNullOrWhiteSpace(contact.FullName))
            {
                await DisplayAlert("Error", "Please enter a name.", "OK");
                return;
            }

            if (contact.Id == 0)
            {
                await _connection.InsertAsync(contact);

                ContactAdded?.Invoke(this, contact);
            }
            else
            {
                await _connection.UpdateAsync(contact);

                ContactUpdated?.Invoke(this, contact);
            }

            await Navigation.PopAsync();
        }
    }
}
