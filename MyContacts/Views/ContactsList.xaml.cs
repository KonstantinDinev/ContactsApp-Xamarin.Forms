using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MyContacts.Models;
using Xamarin.Forms;

using MyContacts.Persistance;
using SQLite;
using System.Threading.Tasks;

namespace MyContacts
{
    public partial class ContactsList : ContentPage
    {
        //data members
        private ObservableCollection<ContactGroup> _contacts;
        private SQLiteAsyncConnection _connection;
        private bool _isDataLoaded;

        public ContactsList()
        {
            InitializeComponent();

            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
            CallHistoryPage.CallHistory = new ObservableCollection<string>();
        }

        protected override async void OnAppearing()
        {
            if (_isDataLoaded)
                return;

            _isDataLoaded = true;

            await LoadData();
            base.OnAppearing();
        }

        private async Task LoadData()
        {
            await _connection.CreateTableAsync<Contact>();
            var contacts = await _connection.Table<Contact>().ToListAsync();

            _contacts = GetContacts(String.Empty, new ObservableCollection<Contact>(contacts));
            lsView.ItemsSource = _contacts;
        }

        ObservableCollection<ContactGroup> GetContacts(string searchedTxt = null, ObservableCollection<Contact> contacts = null)
        {
            ObservableCollection<ContactGroup> groupContacts;

            if (contacts != null)
            {
                groupContacts = new ObservableCollection<ContactGroup>();

                groupContacts.Add(new ContactGroup("Local Contacts", "1"));
                groupContacts.Add(new ContactGroup("Cloud Contacts", "2"));

                foreach (var contact in contacts)
                {
                    groupContacts[0].Add(new Contact
                    {
                        FirstName = contact.FirstName,
                        LastName = contact.LastName,
                        ImageUrl = contact.ImageUrl,
                        Id = contact.Id,
                        PhoneNumber = contact.PhoneNumber,
                        Email = contact.Email,
                        IsBlocked = contact.IsBlocked,
                        Status = contact.Status
                    });
                }

            }
            else
            {
                groupContacts = new ObservableCollection<ContactGroup> {
                    new ContactGroup("Personal Profiles", "1") {
                        new Contact {FirstName = "Const", ImageUrl="https://secure.gravatar.com/avatar/31893df4323fac99458ed86784e25a77"},
                    },

                    new ContactGroup("Other Profiles", "2") {
                    new Contact {FirstName = "Friend 1", ImageUrl="http://lorempixel.com/100/100/people/9"},
                    }
                };
            }


            if (String.IsNullOrWhiteSpace(searchedTxt)) return groupContacts;
            else
            {
                groupContacts.Clear();
                groupContacts.Add(new ContactGroup("Search Results", "All"));

                foreach (ContactGroup group in _contacts)
                {
                    foreach (Contact item in group)
                    {
                        if (item.FullName.ToLower().IndexOf(searchedTxt.ToLower(), StringComparison.CurrentCulture) > -1)
                        {
                            groupContacts[0].Add(item);
                        }
                    }
                }
                if (groupContacts[0].Count == 0)
                    groupContacts[0].Title = "No matches found!";

                return groupContacts;
            }
        }

        async void OnAddContact(object sender, System.EventArgs e)
        {
            var page = new Views.ContactDetailPage(new Contact());

            page.ContactAdded += (source, contact) =>
            {
                _contacts[0].Add(contact);
            };

            await Navigation.PushAsync(page);
        }

        void OnTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var tempContacts = GetContacts(e.NewTextValue);
            lsView.ItemsSource = tempContacts;

            if (String.IsNullOrEmpty(e.NewTextValue)) lsView.ItemsSource = _contacts;
        }

        async void Handle_Refreshing(object sender, System.EventArgs e)
        {
            await LoadData();
            lsView.ItemsSource = _contacts;

            lsView.EndRefresh();
        }

        //void History_Refresh(object sender, System.EventArgs e)
        //{
        //    historyView.ItemsSource = CallHistory;
        //    historyView.EndRefresh();
        //}

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            //var contact = e.Item as Contact;
            //DisplayAlert("Tapped", contact.FullName, "OK");

            //Deletion on click
            //var item = (Contact)e.Item;
            //var group = (ContactGroup)e.Group;
            //group.Remove(item);
        }

        async void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            if (lsView.SelectedItem == null)
                return;

            var selectedContact = e.SelectedItem as Contact;

            lsView.SelectedItem = null;

            var page = new Views.ContactDetailPage(selectedContact);

            page.ContactUpdated += (source, contact) =>
            {
                selectedContact.Id = contact.Id;
                selectedContact.FirstName = contact.FirstName;
                selectedContact.LastName = contact.LastName;
                selectedContact.ImageUrl = contact.ImageUrl;
                selectedContact.PhoneNumber = contact.PhoneNumber;
                selectedContact.Email = contact.Email;
                selectedContact.IsBlocked = contact.IsBlocked;
            };

            await Navigation.PushAsync(page);
        }

        void Call_Clicked(object sender, System.EventArgs e)
        {
            var menuItem = sender as MenuItem;
            var contact = menuItem.CommandParameter as Contact;

            //DisplayAlert("Call", contact.PhoneNumber, "OK");
            OnCall(contact.PhoneNumber);

            CallHistoryPage.CallHistory.Add(contact.PhoneNumber);
            //historyView.ItemsSource = CallHistory;
        }

        async void OnCall(string number)
        {
            if (await this.DisplayAlert(
                "Dial a number",
                "Would you like to call " + number + "?",
                "Yes",
                "No"
            ))
            {
                var dialer = DependencyService.Get<IDialer>();
                if (dialer != null)
                    dialer.Dial(number);
            }
        }

        async void Delete_Clicked(object sender, System.EventArgs e)
        {
            var contact = (sender as MenuItem).CommandParameter as Contact;

            foreach (ContactGroup group in _contacts.ToList())
            {
                foreach (Contact item in group.ToList())
                {

                    if (await DisplayAlert("Warning", $"Are you sure you want to delete {contact.FullName}?", "Yes", "No"))
                    {
                        _contacts[0].Remove(contact);
                        await _connection.DeleteAsync(contact);
                        break;
                    }
                    else break;
                }
            }
            // Fires the OnTextChanged event
            search.Text = "";
        }

        void OnProfile_Clicked(object sender, System.EventArgs e)
        {
            var contact = (sender as Button).CommandParameter as Contact;
            Navigation.PushAsync(new ContactProfilePage(contact));
        }
    }
}
