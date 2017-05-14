using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Client.Data;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UserData loggedUser;
        public List<Room> roomResults;

        public MainWindow(UserData user)
        {
            roomResults = new List<Room>();
            InitializeComponent();
            RoomsGrid.ItemsSource = roomResults;
            loggedUser = user;
            UpdateUserLabels(loggedUser.username, loggedUser.chips);
        }

        //TODO: add avatar update
        public void UpdateUserLabels(string username, int chips)
        {
            usernameLabel.Content = username;
            chipsLabel.Content = chips;
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

        private string SetFilter()
        {
            string filter = "{\"user\":\"" + loggedUser.username+"\"";
            if (PlayerCheckbox.IsChecked != null && PlayerCheckbox.IsChecked.Value == true)
            {
                filter += ",\"player_name\":\"" + PlayerNameTxt.Text+"\"";
            }
            if (PotCheckbox.IsChecked != null && PotCheckbox.IsChecked.Value == true)
            {
                try
                {
                    int pot = Int32.Parse(PotSizeTxt.Text);
                    filter += ",\"player_name\":" + pot + "";
                }
                catch
                {
                    MessageBox.Show("Pot size must be a number!", "Error in search", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            if (LeagueCheckbox.IsChecked != null && LeagueCheckbox.IsChecked.Value == true)
            {
                filter += ",\"league_only\":" + 1;
            }
            if (GameTypeCheckbox.IsChecked != null && GameTypeCheckbox.IsChecked.Value == true)
            {
                filter += ",\"game_type\":\"" + GameTypeCombobox.Text + "\"";
            }
            if (BuyinPolicyCheckbox.IsChecked != null && BuyinPolicyCheckbox.IsChecked.Value == true)
            {
                try
                {
                    int buy = Int32.Parse(BuyinPolicyTxt.Text);
                    filter += ",\"buy_in_policy\":" + buy + "";
                }
                catch
                {
                    MessageBox.Show("Buy in policy must be a number!", "Error in search", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            if (ChipPolicyCheckbox.IsChecked != null && ChipPolicyCheckbox.IsChecked.Value == true)
            {
                try
                {
                    int chip = Int32.Parse(ChipPolicyTxt.Text);
                    filter += ",\"chip_policy\":" + chip + "";
                }
                catch
                {
                    MessageBox.Show("Chip policy must be a number!", "Error in search", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            if (MinBetCheckbox.IsChecked != null && MinBetCheckbox.IsChecked.Value == true)
            {
                try
                {
                    int minB = Int32.Parse(MinBetTxt.Text);
                    filter += ",\"min_bet\":" + minB + "";
                }
                catch
                {
                    MessageBox.Show("Minimum bet must be a number!", "Error in search", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            if (MinPlayersCheckbox.IsChecked != null && MinPlayersCheckbox.IsChecked.Value == true)
            {
                try
                {
                    int minP = Int32.Parse(MinPlayersTxt.Text);
                    filter += ",\"min_players\":" + minP + "";
                }
                catch
                {
                    MessageBox.Show("Minimum number of players must be a number!", "Error in search", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            if (MaxPlayersCheckbox.IsChecked != null && MaxPlayersCheckbox.IsChecked.Value == true)
            {
                try
                {
                    int maxP = Int32.Parse(MaxPlayersTxt.Text);
                    filter += ",\"max_players\":" + maxP + "";
                }
                catch
                {
                    MessageBox.Show("Maximum number of players must be a number!", "Error in search", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            if (SpectatingCheckbox.IsChecked != null && SpectatingCheckbox.IsChecked.Value == true)
            {
                int cond = 0;
                if (SpectatingCombobox.Text == "Yes")
                    cond = 1;
                filter += ",\"sepctating_allowed\":" + cond + "";
            }
            filter += "}";
            return filter;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string filter = SetFilter();

            if (filter == null)
                return;
            RestClient.SetController("Search");
            string ans = RestClient.MakePostRequest(filter);
            JObject json = JObject.Parse(ans);
            RoomList roomList = json.ToObject<RoomList>();
            if (roomList.message == null)
            {
                roomResults = roomList.rooms.ToList();
                RoomsGrid.ItemsSource = roomResults;
                RoomsGrid.Items.Refresh();
            }
            else
            {
                roomResults.Clear();
                RoomsGrid.ItemsSource = roomResults;
                RoomsGrid.Items.Refresh();
            }
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
            ProfileWindow profileWindow = new ProfileWindow(loggedUser, this);
            App.Current.MainWindow = profileWindow;
            //this.Close();
            profileWindow.Show();
        }

        private void GameTypeCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            GameTypeCombobox.IsEnabled = !GameTypeCombobox.IsEnabled;
        }
    }
}
