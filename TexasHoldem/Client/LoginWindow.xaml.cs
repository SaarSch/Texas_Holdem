using System.Windows;
using Client.Data;
using Newtonsoft.Json.Linq;

namespace Client
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameTxt.Text) || string.IsNullOrEmpty(PasswordTxt.Password))
            {
                MessageBox.Show("Username and password cannot be empty!", "Error in registration", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var controller = "User?username=" + UsernameTxt.Text + "&passwordOrRank=" + PasswordTxt.Password +
                                "&mode=register";
            var ans = RestClient.MakeGetRequest(controller);
            if (ans != "\"\"")
            {
                MessageBox.Show(ans, "Error in registration", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("User " + UsernameTxt.Text + " registered succefully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void LogInButtonClick(object sender, RoutedEventArgs e)
        {
            var data = "{\"username\":\"" + UsernameTxt.Text + "\",\"password\":\"" + PasswordTxt.Password + "\"}";
            const string controller = "user";
            var ans = RestClient.MakePostRequest(controller,data);
            var json = JObject.Parse(ans);
            var loggedUser = json.ToObject<UserData>();
            if (loggedUser.Message == null)
            {
                var main = new MainWindow(loggedUser);
                Application.Current.MainWindow = main;
                Close();
                main.Show();
            }
            else
            {
                MessageBox.Show(loggedUser.Message, "Error in login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
