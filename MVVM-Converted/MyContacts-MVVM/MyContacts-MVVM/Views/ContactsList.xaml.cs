using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MyContactsMVVM.Models;
using Xamarin.Forms;

using MyContactsMVVM.Persistance;
using SQLite;
using System.Threading.Tasks;
using MyContactsMVVM.ViewModels;

namespace MyContactsMVVM.Views
{
    public partial class ContactsList : ContentPage
    {
        public ContactsListViewModel ViewModel
        {
            get { return BindingContext as ContactsListViewModel; }
            set { BindingContext = value; }
        }

        public ContactsList()
        {
            var contactStore = new SQliteContactStore(DependencyService.Get<ISQLiteDb>());
            var pageService = new PageService();
            ViewModel = new ContactsListViewModel(contactStore, pageService);

            InitializeComponent();
            //CallHistoryPage.CallHistory = new ObservableCollection<string>();
        }

        protected override void OnAppearing()
        {
            ViewModel.LoadDataCommand.Execute(null);

            base.OnAppearing();
        }



        void OnTextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            //(BindingContext as ContactsListViewModel).SearchContact(e.NewTextValue);
            ViewModel.SearchTextCommand.Execute(e.NewTextValue);
        }

        void Handle_Refreshing(object sender, System.EventArgs e)
        {
            //await LoadData();
            //lsView.ItemsSource = _contacts;

            //lsView.EndRefresh();
            ViewModel._isDataLoaded = false;
            ViewModel.LoadDataCommand.Execute(null);

            //(sender as ListView).ItemsSource = (BindingContext as ContactsListViewModel).Contacts;
            (sender as ListView).EndRefresh();
        }

        void History_Refresh(object sender, System.EventArgs e)
        {
            //historyView.ItemsSource = CallHistory;
            //historyView.EndRefresh();
        }

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
            //if (lsView.SelectedItem == null)
            //    return;

            //var selectedContact = e.SelectedItem as Contact;

            //lsView.SelectedItem = null;

            //var page = new Views.ContactDetailPage(selectedContact);

            //page.ContactUpdated += (source, contact) =>
            //{
            //    selectedContact.Id = contact.Id;
            //    selectedContact.FirstName = contact.FirstName;
            //    selectedContact.LastName = contact.LastName;
            //    selectedContact.ImageUrl = contact.ImageUrl;
            //    selectedContact.PhoneNumber = contact.PhoneNumber;
            //    selectedContact.Email = contact.Email;
            //    selectedContact.IsBlocked = contact.IsBlocked;
            //};

            //await Navigation.PushAsync(page);
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
            //if (await this.DisplayAlert(
            //    "Dial a number",
            //    "Would you like to call " + number + "?",
            //    "Yes",
            //    "No"
            //))
            //{
                //var dialer = DependencyService.Get<IDialer>();
                //if (dialer != null)
                    //dialer.Dial(number);
            }
        }


        //void OnProfile_Clicked(object sender, System.EventArgs e)
        //{
        //    var contact = (sender as Button).CommandParameter as Contact;
        //    Navigation.PushAsync(new ContactProfilePage(contact));
        //}
}