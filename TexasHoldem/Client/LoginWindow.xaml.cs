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
            RestClient.SetController("Registration");
            string ans = RestClient.MakePostRequest("{\"username\":\""+UsernameTxt.Text+"\",\"password\":\""+ PasswordTxt.Password+"\"}");
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
            RestClient.SetController("Login");
            string ans = RestClient.MakePostRequest("{\"username\":\"" + UsernameTxt.Text + "\",\"password\":\"" + PasswordTxt.Password + "\"}");
            JObject json = JObject.Parse(ans);
            UserData loggedUser = json.ToObject<UserData>();
            if (loggedUser != null)
            {
                MainWindow main = new MainWindow(loggedUser);
                App.Current.MainWindow = main;
                this.Close();
                main.Show();
            }
            else
            {
                MessageBox.Show(ans, "Error in login", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
