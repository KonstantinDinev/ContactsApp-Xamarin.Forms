using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MyContacts
{
    public partial class ContactProfilePage : ContentPage
    {
        public ContactProfilePage()
        {
            InitializeComponent();
        }

        public ContactProfilePage(Models.Contact contact)
        {
            BindingContext = new Models.Contact()
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                ImageUrl = contact.ImageUrl,
                PhoneNumber = contact.PhoneNumber,
                Email = contact.Email,
                IsBlocked = contact.IsBlocked
            };

            InitializeComponent();
        }
    }
}
