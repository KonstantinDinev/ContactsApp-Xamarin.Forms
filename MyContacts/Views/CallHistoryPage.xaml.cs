using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace MyContacts
{
    public partial class CallHistoryPage : ContentPage
    {
        public static ObservableCollection<string> CallHistory { get; set; }

        void History_Refresh(object sender, System.EventArgs e)
        {
            historyView.ItemsSource = CallHistory;
            historyView.EndRefresh();
        }

        protected override void OnAppearing()
        {
            historyView.ItemsSource = CallHistory;

            base.OnAppearing();
        }

        public CallHistoryPage()
        {
            InitializeComponent();
        }
    }
}
