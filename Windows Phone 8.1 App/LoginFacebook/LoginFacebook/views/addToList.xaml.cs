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
    public partial class addToList : PhoneApplicationPage
    {
        private string UserID = "";
        public addToList()
        {
            InitializeComponent();
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            string encodedUserId = NavigationContext.QueryString["encodedUserId"];
            UserID = Uri.UnescapeDataString(encodedUserId);
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (tbTitle.Text != null && tbTask.Text != null)
            {
                string storageGuid = "4997f1f5-930d-46c8-b11e-7d4c62a3075c";
                // parse guid string to real guid as your database is expecting this type as uniqueidentifier
                Guid storageId = Guid.Parse(storageGuid);
                string title = tbTitle.Text;
                string task = tbTask.Text;
                // Async webclient call to REST service, passing in parameters in the URI
                // Please note the URI called below is the URI of the locally running service, this may be different on your machine
                WebClient addTask = new WebClient();
                addTask.UploadStringAsync(new Uri("http://c01e977a8eb345488260c92dd5f2f480.cloudapp.net/Service1.svc/lists?guid=" + storageGuid + "&userid=" + UserID + "&title=" + title + "&task=" + task), "POST");
                addTask.UploadStringCompleted += addTask_UploadStringCompleted;
            }
            else
            {
                MessageBox.Show("Both text boxes need an input.", "Unsuccessful", MessageBoxButton.OK);
            }
        }

        void addTask_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                MessageBox.Show("Task added!", "Success", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Problem adding task", "Unsuccessful", MessageBoxButton.OK);
                Console.WriteLine("An error occured:" + e.Error);
            }
        }
    }
}