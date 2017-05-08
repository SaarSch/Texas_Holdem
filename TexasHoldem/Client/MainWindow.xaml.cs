using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //RoomsGrid.ItemsSource = 
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void PlayerCheckBoxChecked(object sender, RoutedEventArgs e)
        {
            PlayerNameTxt.IsEnabled = !PlayerNameTxt.IsEnabled;
        }

        private void PotCheckboxChecked(object sender, RoutedEventArgs e)
        {
            PotSizeTxt.IsEnabled = !PotSizeTxt.IsEnabled;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BuyInPolicyChecked(object sender, RoutedEventArgs e)
        {
            BuyinPolicyTxt.IsEnabled = !BuyinPolicyTxt.IsEnabled;
        }

        private void ChipPolicyCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            ChipPolicyTxt.IsEnabled = !ChipPolicyTxt.IsEnabled;
        }

        private void MinBetCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            MinBetTxt.IsEnabled = !MinBetTxt.IsEnabled;
        }

        private void MinPlayersCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            MinPlayersTxt.IsEnabled = !MinPlayersTxt.IsEnabled;
        }

        private void MaxPlayersCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            MaxPlayersTxt.IsEnabled = !MaxPlayersTxt.IsEnabled;
        }

        private void SpectatingCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            SpectatingCombobox.IsEnabled = !SpectatingCombobox.IsEnabled;
        }

        private void EditProfileButton_Click(object sender, RoutedEventArgs e)
        {
            ProfileWindow main = new ProfileWindow();
            App.Current.MainWindow = main;
            this.Close();
            main.Show();
        }

        private void GameTypeCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            GameTypeCombobox.IsEnabled = !GameTypeCombobox.IsEnabled;
        }
    }
}
