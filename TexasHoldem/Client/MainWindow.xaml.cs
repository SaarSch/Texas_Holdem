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
            UpdateUserLabels(loggedUser.Username, loggedUser.Chips);
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
        //Room r = WebApiConfig.GameManger.CreateGameWithPreferences(value.RoomName, value.CreatorUserName, value.CreatorPlayerName, value.GameType, value.BuyInPolicy, value.ChipPolicy, value.MinBet, value.MinPlayers, value.MaxPlayers, value.SepctatingAllowed);
        private string SetFilter()
        {
            string filter = "{\"User\":\"" + loggedUser.Username+"\"";
            if (PlayerCheckbox.IsChecked != null && PlayerCheckbox.IsChecked.Value == true)
            {
                filter += ",\"PlayerName\":\"" + PlayerNameTxt.Text+"\"";
            }
            if (PotCheckbox.IsChecked != null && PotCheckbox.IsChecked.Value == true)
            {
                try
                {
                    int pot = Int32.Parse(PotSizeTxt.Text);
                    filter += ",\"PlayerName\":" + pot + "";
                }
                catch
                {
                    MessageBox.Show("Pot size must be a number!", "Error in search", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            if (LeagueCheckbox.IsChecked != null && LeagueCheckbox.IsChecked.Value == true)
            {
                filter += ",\"LeagueOnly\":" + 1;
            }
            if (GameTypeCheckbox.IsChecked != null && GameTypeCheckbox.IsChecked.Value == true)
            {
                filter += ",\"GameType\":\"" + GameTypeCombobox.Text + "\"";
            }
            if (BuyinPolicyCheckbox.IsChecked != null && BuyinPolicyCheckbox.IsChecked.Value == true)
            {
                try
                {
                    int buy = Int32.Parse(BuyinPolicyTxt.Text);
                    filter += ",\"BuyInPolicy\":" + buy + "";
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
                    filter += ",\"ChipPolicy\":" + chip + "";
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
                    filter += ",\"MinBet\":" + minB + "";
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
                    filter += ",\"MinPlayers\":" + minP + "";
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
                    filter += ",\"MaxPlayers\":" + maxP + "";
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
                filter += ",\"SepctatingAllowed\":" + cond + "";
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
            if (roomList.Message == null)
            {
                roomResults = roomList.Rooms.ToList();
                RoomsGrid.ItemsSource = roomResults;
                RoomsGrid.Items.Refresh();
            }
            else
            {
                MessageBox.Show("No rooms to show", "Error in search", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void newRoomButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
