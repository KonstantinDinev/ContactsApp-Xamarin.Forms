using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MyContacts.Models
{
    public class ContactGroup : ObservableCollection<Contact>
    {
        public string Title { get; set; }
        public string ShortTitle { get; set; }

        public ContactGroup (string title, string shortTitle) 
        {
            this.Title = title;
            this.ShortTitle = shortTitle;
        }
    }
}
