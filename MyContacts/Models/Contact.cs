using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using SQLite;

namespace MyContacts.Models
{
    public class Contact : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private string _firstName;
        [MaxLength(255)]
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (_firstName == value)
                    return;

                _firstName = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
            }
        }

        private string _lastName;
        [MaxLength(255)]
        public string LastName
        {
            get { return _lastName; }
            set
            {
                if (_lastName == value)
                    return;

                _lastName = value;

                OnPropertyChanged();
                OnPropertyChanged(nameof(FullName));
            }
        }

        [MaxLength(255)]
        public string ImageUrl { get; set; }

        [MaxLength(255)]
        public string PhoneNumber { get; set; }

        [MaxLength(255)]
        public string Email { get; set; }

        private bool _isBlocked;
        [NotNull]
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

        //TODO Bindable Group property

        [Ignore]
        public Color StatusColor
        {
            get { return IsBlocked ? Color.Red : Color.Green; }
        }

        private string _status;
        [MaxLength(255)]
        public string Status
        {
            get
            {
                if (IsBlocked) return "Blocked";
                return "Active";
            }
            set => _status = value;
        }

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}