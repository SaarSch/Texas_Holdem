using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using System.Windows.Media.Imaging;
using Client.Data;
using Newtonsoft.Json.Linq;

namespace Client
{
    /// <summary>
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow
    {
        private UserData _user;
        private readonly MainWindow _mainWindow;
        private string Picture;

        public ProfileWindow(UserData user, MainWindow mainWindow)
        {
            InitializeComponent();
            _user = user;
            _mainWindow = mainWindow;
            UsernameTxt.Text = user.Username;
            PasswordTxt.Text = "";
            EmailTxt.Text = user.Email;
            Picture = user.AvatarPath;
            ProfilePic.Dispatcher.Invoke(() => ProfilePic.Source = new BitmapImage(new Uri(@Picture, UriKind.Relative)));
        }

        private void AvatarButton_Click(object sender, RoutedEventArgs e)
        {
            Picture = _user.AvatarPath;
            ProfilePic.Dispatcher.Invoke(() => ProfilePic.Source = new BitmapImage(new Uri(@Picture, UriKind.Relative)));
            /* Create OpenFileDialog 
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".png",
                Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg"
            };

            // Set filter for file extension and default file extension 


            // Display OpenFileDialog by calling ShowDialog method 
            var result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                var avatarStream = dlg.OpenFile();
                var content = new StreamContent(avatarStream);
                content.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg"); // TODO: change jpeg
          //      RestClient.MakePostRequest("somecontroller", content.ToString());
            }
            else
            {
                // handle
            } */
        }

        // POST: api/User?username=elad
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var controller = "User?username=" + Crypto.Encrypt(_user.Username) + "&token=" +_user.token;
            var data = "{\"Username\":\"" + Crypto.Encrypt(UsernameTxt.Text) + "\"," +
                          "\"Password\":\"" + Crypto.Encrypt(PasswordTxt.Text) + "\","
                          + "\"AvatarPath\":\"" + Picture + "\"," +
                       "\"Email\":\"" + Crypto.Encrypt(EmailTxt.Text) + "\"}";
            var ans = RestClient.MakePostRequest(controller, data);
            var json = JObject.Parse(ans);
            var tmpUser = json.ToObject<UserData>();
            if (tmpUser.Message == null)
            {
                _user.Username = Crypto.Decrypt(tmpUser.Username);
                _user.Password = Crypto.Decrypt(tmpUser.Password);
                _user.Email = Crypto.Decrypt(tmpUser.Email);
                _user.AvatarPath = tmpUser.AvatarPath;
                _mainWindow.UpdateAvatar(tmpUser.AvatarPath);
                Close();
            }
            else
            {
                MessageBox.Show(tmpUser.Message, "Error in edit profile", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {   
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.MainWindow = _mainWindow;
            _mainWindow.Show();
        }

        private void Opt1_Click(object sender, RoutedEventArgs e)
        {
            Picture = "Resources/avatar1.png";
            ProfilePic.Dispatcher.Invoke(() => ProfilePic.Source = new BitmapImage(new Uri(@Picture, UriKind.Relative)));
        }

        private void Opt2_Click(object sender, RoutedEventArgs e)
        {
            Picture = "Resources/avatar2.png";
            ProfilePic.Dispatcher.Invoke(() => ProfilePic.Source = new BitmapImage(new Uri(@Picture, UriKind.Relative)));
        }

        private void Opt3_Click(object sender, RoutedEventArgs e)
        {
            Picture = "Resources/avatar3.png";
            ProfilePic.Dispatcher.Invoke(() => ProfilePic.Source = new BitmapImage(new Uri(@Picture, UriKind.Relative)));
        }

        private void Opt4_Click(object sender, RoutedEventArgs e)
        {
            Picture = "Resources/avatar4.png";
            ProfilePic.Dispatcher.Invoke(() => ProfilePic.Source = new BitmapImage(new Uri(@Picture, UriKind.Relative)));
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ChangePsswd.IsChecked == true)
            {
                PasswordLbl.Visibility = Visibility.Visible;
                PasswordTxt.Visibility = Visibility.Visible;
            }
            if (ChangePsswd.IsChecked == false)
            {
                PasswordLbl.Visibility = Visibility.Hidden;
                PasswordTxt.Visibility = Visibility.Hidden;
            }
        }
    }
}
