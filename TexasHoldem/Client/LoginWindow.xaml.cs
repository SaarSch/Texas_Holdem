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

        private bool _loginMode;

        public LoginWindow()
        {
            InitializeComponent();
            _loginMode = true;
        }

        private void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(UsernameTxt.Text) || string.IsNullOrEmpty(PasswordTxt.Password))
            {
                MessageBox.Show("Username and password cannot be empty!", "Error in registration", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var controller = "User?username=" + Crypto.Encrypt(UsernameTxt.Text) + "&passwordOrRank=" + Crypto.Encrypt(PasswordTxt.Password) +
                                "&mode=register&token=nothing";
            var ans = RestClient.MakeGetRequest(controller);
            if (ans != "\"\"")
            {
                MessageBox.Show(ans, "Error in registration", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                UsernameTxt.Text = "";
                PasswordTxt.Password = "";
                MessageBox.Show("User " + UsernameTxt.Text + " registered succefully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ChangeModeHelper();
            }

        }

        private void LogInButtonClick(object sender, RoutedEventArgs e)
        {
            var data = "{\"username\":\"" + Crypto.Encrypt(UsernameTxt.Text) + "\",\"password\":\"" + Crypto.Encrypt(PasswordTxt.Password) + "\"}";
            const string controller = "user";
            var ans = RestClient.MakePostRequest(controller,data);
            try
            {
                var json = JObject.Parse(ans);
                var loggedUser = json.ToObject<UserData>();

                if (loggedUser.Message == null)
                {
                    loggedUser.Username = Crypto.Decrypt(loggedUser.Username);
                    loggedUser.Password = Crypto.Decrypt(loggedUser.Password);
                    loggedUser.Email = Crypto.Decrypt(loggedUser.Email);
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
            catch
            {
                MessageBox.Show("Server is not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeMode_Click(object sender, RoutedEventArgs e)
        {
            ChangeModeHelper();
        }

        private void ChangeModeHelper()
        {
            _loginMode = !_loginMode;
            UsernameTxt.Text = "";
            PasswordTxt.Password = "";

            if (_loginMode)
            {
                ModeLbl.Content = "Login to Texas Holdem!";
                LogInButton.Visibility = Visibility.Visible;
                RegisterButton.Visibility = Visibility.Hidden;
                LoginPic.Visibility = Visibility.Visible;
                RegisterPic.Visibility = Visibility.Hidden;
                ChangeMode.Content = "Join us now!";
            }
            else
            {
                ModeLbl.Content = "Create a New Account";
                LogInButton.Visibility = Visibility.Hidden;
                RegisterButton.Visibility = Visibility.Visible;
                LoginPic.Visibility = Visibility.Hidden;
                RegisterPic.Visibility = Visibility.Visible;
                ChangeMode.Content = "Back";
            }
        }
    }
}
