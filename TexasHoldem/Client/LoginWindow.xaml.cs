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
            RestClient.SetController("User?username="+ UsernameTxt.Text+"&passwordOrRank="+ PasswordTxt.Password + "&mode=register");
            string ans = RestClient.MakeGetRequest();
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
            RestClient.SetController("User");
            string ans = RestClient.MakePostRequest("{\"username\":\"" + UsernameTxt.Text + "\",\"password\":\"" + PasswordTxt.Password + "\"}");
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
