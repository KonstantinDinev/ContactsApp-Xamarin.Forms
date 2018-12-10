using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyContacts
{
    public partial class MainPage : ContentPage
    {
        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ContactsPage());
        }

        public MainPage()
        {
            InitializeComponent();

            imgBackground.Source = ImageSource.FromResource(@"MyContacts.Images.CMS_Creative.jpg");
        }
    }
}
