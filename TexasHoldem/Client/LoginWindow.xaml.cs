using System.Windows;
using Newtonsoft.Json.Linq;

namespace Client
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {

        private bool _login_mode;

        public LoginWindow()
        {
            InitializeComponent();
            _login_mode = true;
        }

        private void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameTxt.Text) || string.IsNullOrEmpty(PasswordTxt.Password))
            {
                MessageBox.Show("Username and password cannot be empty!", "Error in registration", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string controller = "User?username=" + UsernameTxt.Text + "&passwordOrRank=" + PasswordTxt.Password +
                                "&mode=register";
            string ans = RestClient.MakeGetRequest(controller);
            if (ans != "\"\"")
            {
                MessageBox.Show(ans, "Error in registration", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                UsernameTxt.Text = "";
                PasswordTxt.Password = "";
                MessageBox.Show("User " + UsernameTxt.Text + " registered succefully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void LogInButtonClick(object sender, RoutedEventArgs e)
        {
            string data = "{\"username\":\"" + UsernameTxt.Text + "\",\"password\":\"" + PasswordTxt.Password + "\"}";
            string controller = "user";
            string ans = RestClient.MakePostRequest(controller,data);
            JObject json = JObject.Parse(ans);
            UserData loggedUser = json.ToObject<UserData>();
            if (loggedUser.Message == null)
            {
                MainWindow main = new MainWindow(loggedUser);
                App.Current.MainWindow = main;
                this.Close();
                main.Show();
            }
            else
            {
                MessageBox.Show(loggedUser.Message, "Error in login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeMode_Click(object sender, RoutedEventArgs e)
        {
            _login_mode = !_login_mode;
            UsernameTxt.Text = "";
            PasswordTxt.Password = "";

            if (_login_mode)
            {
                ModeLbl.Content = "Log In to Texas Holdem!";
                LogInButton.Visibility = Visibility.Visible;
                RegisterButton.Visibility = Visibility.Hidden;
                ChangeMode.Content = "Join us now!";
            }
            else
            {
                ModeLbl.Content = "Create a New Account";
                LogInButton.Visibility = Visibility.Hidden;
                RegisterButton.Visibility = Visibility.Visible;
                ChangeMode.Content = "Back";
            }
        }
    }
}
