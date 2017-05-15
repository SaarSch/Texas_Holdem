using System.Windows;
using Newtonsoft.Json.Linq;

namespace Client
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            string controller = "User?username=" + UsernameTxt.Text + "&passwordOrRank=" + PasswordTxt.Password +
                                "&mode=register";
            string ans = RestClient.MakeGetRequest(controller);
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
    }
}
