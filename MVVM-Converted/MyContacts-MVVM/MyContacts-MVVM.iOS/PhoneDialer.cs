using Foundation;
using Xamarin.Forms;
using UIKit;
using MyContacts_MVVM.iOS;
using MyContactsMVVM;

[assembly: Dependency(typeof(PhoneDialer))]
namespace MyContacts_MVVM.iOS
{
    public class PhoneDialer : IDialer
    {

        public bool Dial(string number)
        {
            return UIApplication.SharedApplication.OpenUrl(
                new NSUrl("tel:" + number));
        }
    }
}
