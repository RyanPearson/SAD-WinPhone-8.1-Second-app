using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LoginFacebook.views
{
    public partial class viewList : PhoneApplicationPage
    {
        private string UserID = "";
        private string storageGuid = "4997f1f5-930d-46c8-b11e-7d4c62a3075c";
        private List<TaskData> taskInfo = new List<TaskData>();
        public viewList()
        {
            InitializeComponent();
        }
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            string encodedUserId = NavigationContext.QueryString["encodedUserId"];
            UserID = Uri.UnescapeDataString(encodedUserId);
            WebClient taskAPI = new WebClient();
            try
            {
                taskAPI.DownloadStringAsync(new Uri("http://c01e977a8eb345488260c92dd5f2f480.cloudapp.net/Service1.svc/viewlist?format=JSON&guid=" + storageGuid + "&userid=" + UserID), "GET");
                taskAPI.DownloadStringCompleted += new DownloadStringCompletedEventHandler(taskAPI_DownloadStringCompleted);
            }
            catch
            {
                MessageBox.Show("Network Error, please try again when the network has returned.");
            }

        }
        void taskAPI_DownloadStringCompleted(object senders, DownloadStringCompletedEventArgs e)
        {
            //Stores all information from the JSON object returned as a LyricData datatype.
            //checks if the json object is valid
            if (e.Error == null && e.Result.StartsWith("["))
            {
                var json = e.Result;
                taskInfo = JsonConvert.DeserializeObject<List<TaskData>>(json);
            }
            listBox.ItemsSource = taskInfo.ToList();
            
        }

        public class TaskData
        {
            public string guid { get; set;}

            public int id { get; set;}

            public string task { get; set;}

            public string title { get; set; }

            public string userid {get; set;}
        }
    }
}