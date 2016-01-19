using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.WindowsAzure.MobileServices;
using LoginFacebook.Resources;
using Windows.UI.Popups;
using System.Linq;
using Windows.Security.Credentials;


namespace LoginFacebook
{
    public partial class MainPage : PhoneApplicationPage
    {
        // new instance of AMS authenticated user
        private MobileServiceUser user;
        async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            await AuthenticateAsync();
           // NavigationService.Navigate(new Uri("/views/menu.xaml", UriKind.Relative));
            string encodedUserId = Uri.EscapeDataString(user.UserId);
            NavigationService.Navigate(new Uri("/views/menu.xaml?encodedUserId=" + encodedUserId, UriKind.Relative));
        }
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += MainPage_Loaded;
        }
        // Define a method that performs the authentication process
        // using a Facebook sign-in. 
        private async System.Threading.Tasks.Task AuthenticateAsync()
        {
            while (user == null)
            {
                string message = "";
                // This sample uses the Facebook provider.
                var provider = "Facebook";

                // Use the PasswordVault to securely store and access credentials.
                PasswordVault vault = new PasswordVault();
                PasswordCredential credential = null;

                while (credential == null)
                {
                    try
                    {
                        // Try to get an existing credential from the vault.
                        credential = vault.FindAllByResource(provider).FirstOrDefault();
                    }
                    catch (Exception)
                    {
                        // When there is no matching resource an error occurs, which we ignore.
                    }

                    if (credential != null)
                    {
                        // Create a user from the stored credentials.
                        user = new MobileServiceUser(credential.UserName);
                        credential.RetrievePassword();
                        user.MobileServiceAuthenticationToken = credential.Password;

                        // Set the user from the stored credentials.
                        App.MobileService.CurrentUser = user;

                        try
                        {
                            // Try to return an item now to determine if the cached credential has expired.
                            //await App.MobileService.GetTable<TodoItem>().Take(1).ToListAsync();
                            message = "HI";
                        }
                        catch (MobileServiceInvalidOperationException ex)
                        {
                            if (ex.Response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                            {
                                // Remove the credential with the expired token.
                                vault.Remove(credential);
                                credential = null;
                                continue;
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            // Login with the identity provider.
                            user = await App.MobileService
                                .LoginAsync(provider);
                            message = string.Format("You are now signed in - {0}", user.UserId);
                            // Create and store the user credentials.
                            credential = new PasswordCredential(provider,
                                user.UserId, user.MobileServiceAuthenticationToken);
                            vault.Add(credential);
                        }
                        catch (MobileServiceInvalidOperationException ex)
                        {
                            message = "You must log in. Login Required";
                        }
                    }

                    MessageBox.Show(message);
                }
            }
        }
    }
}