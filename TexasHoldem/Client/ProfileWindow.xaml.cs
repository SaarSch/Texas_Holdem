using System.Net.Http;
using System.Net.Http.Headers;
using System.Windows;
using Client.Data;

namespace Client
{
    /// <summary>
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow
    {
        private readonly UserData _user;
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
            var data = "{\"username\":\"" + UsernameTxt.Text + "\"," +
                          "\"password\":\"" + PasswordTxt.Text + "\"," +
                          "\"email\":\"" + EmailTxt.Text + "\"}" /*"\"," +
                                                    "\"avatar\":\"" + EmailTxt.Text + "\"}"*/;
            var ans = RestClient.MakePostRequest(controller, data);
            if (ans == "\"\"")
            {
                MessageBox.Show("User edited succesfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _mainWindow.UpdateUserLabels(UsernameTxt.Text, _user.Chips); //TODO: add avatar update
            }
            else
            {
                MessageBox.Show(ans, "Error in edit", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
