using System;
using MyContactsMVVM.Models;
using Xamarin.Forms;

namespace MyContactsMVVM.ViewModels
{
    public class ContactViewModel : BaseViewModel
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public ContactViewModel() {  }

        public ContactViewModel(Contact contact) 
        {
            Id = contact.Id;
            _firstName = contact.FirstName;
            _lastName = contact.LastName;
            PhoneNumber = contact.PhoneNumber;
            Email = contact.Email;
            IsBlocked = contact.IsBlocked;
            Status = contact.Status;
        }

        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                SetValue(ref _firstName, value);
                OnPropertyChanged(nameof(FullName));
            }
        }

        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                SetValue(ref _lastName, value);
                OnPropertyChanged(nameof(FullName));
            }
        }

        public string _fullname;
        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
            set => _fullname = value;
        }

        private bool _isBlocked;
        public bool IsBlocked 
        {
            get { return _isBlocked; }
            set 
            {
                if (_isBlocked == value)
                    return;

                _isBlocked = value;

                OnPropertyChanged(nameof(Status));
                OnPropertyChanged(nameof(StatusColor));
            }
        }

        public Color StatusColor
        {
            get { return IsBlocked ? Color.Red : Color.Green; }
        }

        private string _status;
        public string Status
        {
            get
            {
                if (IsBlocked) return "Blocked";
                return "Active";
            }
            set => _status = value;
        }

    }
}
