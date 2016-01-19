using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;


namespace LoginFacebook.views
{
    public partial class menu : PhoneApplicationPage
    {
        private string UserID = "";
        public menu()
        {
            InitializeComponent();
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            string encodedUserId = NavigationContext.QueryString["encodedUserId"];
            UserID = Uri.UnescapeDataString(encodedUserId);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string encodedUserIdToAddList = Uri.EscapeDataString(UserID);
            NavigationService.Navigate(new Uri("/views/addToList.xaml?encodedUserId=" + encodedUserIdToAddList, UriKind.Relative));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string encodedUserIdToViewList = Uri.EscapeDataString(UserID);
            NavigationService.Navigate(new Uri("/views/viewList.xaml?encodedUserId=" + encodedUserIdToViewList, UriKind.Relative));
        }
    }
}