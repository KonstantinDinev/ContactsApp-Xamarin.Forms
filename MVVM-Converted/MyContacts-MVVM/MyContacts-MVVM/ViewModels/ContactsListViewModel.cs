using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MyContactsMVVM.Views;
using Xamarin.Forms;

namespace MyContactsMVVM.ViewModels
{
    public class ContactsListViewModel : BaseViewModel
    {
        private ContactViewModel _selectedContact;
        private IContactStore _contactStore;
        private IPageService _pageService;
        public bool _isDataLoaded;

        public ObservableCollection<ContactViewModel> Contacts { get; private set; } =
            new ObservableCollection<ContactViewModel>();

        public ContactViewModel SelectedContact
        {
            get { return _selectedContact; }
            set { SetValue(ref _selectedContact, value); }
        }
         
        public ICommand LoadDataCommand { get; private set; }
        public ICommand AddContactCommand { get; private set; }
        public ICommand SelectContactCommand { get; private set; }
        public ICommand DeleteContactCommand { get; private set; }
        public ICommand SearchTextCommand { get; private set; }

        public ContactsListViewModel(IContactStore contactStore, IPageService pageService)
        {
            _contactStore = contactStore;
            _pageService = pageService;


            LoadDataCommand = new Command(async () => await LoadData());
            AddContactCommand = new Command(async () => await AddContact());
            //DeleteContactCommand = new Command(async () => await DeleteContact());
            SearchTextCommand = new Command<String>((searchTxt) => SearchContact(searchTxt));
        }

        private ObservableCollection<ContactViewModel> SearchContact(string searchedTxt)
        {
            var tempContacts = new ObservableCollection<ContactViewModel>();
            if (String.IsNullOrWhiteSpace(searchedTxt)) return this.Contacts;
            else
            {
                foreach (ContactViewModel cs in this.Contacts)
                {
                    if (cs.FullName.ToLower().IndexOf(searchedTxt.ToLower(), StringComparison.CurrentCulture) > -1)
                    {
                        tempContacts.Add(cs);
                    }
                }
            }
            if (tempContacts.Count == 0)
            {
                this.Contacts.Clear();
                tempContacts.Add(new ContactViewModel() { FirstName = "No matches found!" });
                this.Contacts.Add(tempContacts[0]);
            }
            //if (String.IsNullOrEmpty(e.NewTextValue)) lsView.ItemsSource = _contacts;

            this.Contacts.Clear();
            foreach (var it in tempContacts) this.Contacts.Add(it);

            return tempContacts;
        }

        private async Task LoadData()
        {
            if (_isDataLoaded)
                return;

            _isDataLoaded = true;

            var contacts = await _contactStore.GetContactsAsync();

            foreach (var c in contacts)
                Contacts.Add(new ContactViewModel(c));
        }

        private async Task SelectContact(ContactViewModel contact)
        {
            if (contact == null)
                return;

            SelectedContact = null;

            var viewModel = new ContactDetailsViewModel(contact, _contactStore, _pageService);
            viewModel.ContactUpdated += (ImageSource, updatedContact) =>
            {
                contact.Id = updatedContact.Id;
                contact.FirstName = updatedContact.FirstName;
                contact.LastName = updatedContact.LastName;
                contact.PhoneNumber = updatedContact.PhoneNumber;
                contact.Email = updatedContact.Email;
                contact.IsBlocked = updatedContact.IsBlocked;
            };

            await _pageService.PushAsync(new ContactDetailPage(viewModel));
        }

        private async Task AddContact()
        {
            var viewModel = new ContactDetailsViewModel(new ContactViewModel(), _contactStore, _pageService);
            viewModel.ContactAdded += (ImageSource, contact) =>
            {
                Contacts.Add(new ContactViewModel(contact));
            };

            await _pageService.PushAsync(new ContactDetailPage(viewModel));
        }

        private async Task DeleteContact(ContactViewModel contactViewModel)
        {
            if (await _pageService.DisplayAlert("Warning", $"Are you sure you want to delete {contactViewModel.FullName}?", "Yes", "No"))
            {
                Contacts.Remove(contactViewModel);

                var contact = await _contactStore.GetContact(contactViewModel.Id);
                await _contactStore.DeleteContact(contact);
            }
        }
    }
}
