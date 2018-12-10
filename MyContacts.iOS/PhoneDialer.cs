using Foundation;
using MyContacts.iOS;
using Xamarin.Forms;
using UIKit;

[assembly: Dependency(typeof(PhoneDialer))]
namespace MyContacts.iOS
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
