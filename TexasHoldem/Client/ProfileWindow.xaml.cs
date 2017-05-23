using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
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

        public ProfileWindow(UserData user, MainWindow mainWindow)
        {
            InitializeComponent();
            _user = user;
            _mainWindow = mainWindow;
            UsernameTxt.Text = user.Username;
            PasswordTxt.Text = user.Password;
            EmailTxt.Text = user.Email;
        }

        private void AvatarButton_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
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
            }
        }

        // POST: api/User?username=elad
        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            var controller = "User?username=" + _user.Username;
            var data = "{\"Username\":\"" + UsernameTxt.Text + "\"," +
                          "\"Password\":\"" + PasswordTxt.Text + "\"," +
                          "\"Email\":\"" + EmailTxt.Text +  "\"," +
                                                    "\"Avatar\":\"" + "Resources/profilePicture.png" + "\"}";
            var ans = RestClient.MakePostRequest(controller, data);
            var json = JObject.Parse(ans);
            var tmpUser = json.ToObject<UserData>();
            if (tmpUser.Message == null)
            {
                _user.Username = tmpUser.Username;
                _user.Password = tmpUser.Password;
                _user.Email = tmpUser.Email;
                _user.AvatarPath = tmpUser.AvatarPath;
                Application.Current.MainWindow = _mainWindow;
                Close();
                _mainWindow.Show();
            }
            else
            {
                MessageBox.Show(tmpUser.Message, "Error in login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow = _mainWindow;
            _mainWindow.Show();
            Close();
        }
    }
}
