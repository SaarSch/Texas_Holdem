using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Client.Data;
using System.Web.Script.Serialization;

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

        private RoomFilter SetFilter()
        {
            RoomFilter filter = new RoomFilter();
            filter.User = loggedUser.Username;
            if (PlayerCheckbox.IsChecked != null && PlayerCheckbox.IsChecked.Value == true)
            {
                filter.PlayerName = PlayerNameTxt.Text;
            }
            if (PotCheckbox.IsChecked != null && PotCheckbox.IsChecked.Value == true)
            {
                try
                {
                    int pot = Int32.Parse(PotSizeTxt.Text);
                    filter.PotSize = pot;
                }
                catch
                {
                    MessageBox.Show("Pot size must be a number!", "Error in search", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            if (LeagueCheckbox.IsChecked != null && LeagueCheckbox.IsChecked.Value == true)
            {
                filter.LeagueOnly = true;
            }
            if (GameTypeCheckbox.IsChecked != null && GameTypeCheckbox.IsChecked.Value == true)
            {
                filter.GameType = GameTypeCombobox.Text;
            }
            if (BuyinPolicyCheckbox.IsChecked != null && BuyinPolicyCheckbox.IsChecked.Value == true)
            {
                try
                {
                    int buy = Int32.Parse(BuyinPolicyTxt.Text);
                    filter.BuyInPolicy = buy;
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
                    filter.ChipPolicy = chip;
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
                    filter.MinBet = minB;
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
                    filter.MinPlayers = minP;
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
                    filter.MaxPlayers = maxP;
                }
                catch
                {
                    MessageBox.Show("Maximum number of players must be a number!", "Error in search", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            if (SpectatingCheckbox.IsChecked != null && SpectatingCheckbox.IsChecked.Value == true)
            {
                bool cond = SpectatingCombobox.Text == "Yes";
                filter.SpectatingAllowed = cond;
            }
            return filter;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            RoomFilter filter = SetFilter();

            if (filter == null)
                return;

            string controller = "Search";
            string data = new JavaScriptSerializer().Serialize(filter);

            string ans = RestClient.MakePostRequest(controller, data);
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
                MessageBox.Show("No rooms to show!", "No results", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private Room SetRoom()
        {
            Room room = new Room();
            room.CreatorUserName = loggedUser.Username;
            room.CreatorPlayerName = PlayerNameTxt_Copy.Text;
            room.GameType = GameTypeCombobox_Copy.Text;
            try
            {
                int buy = Int32.Parse(BuyinPolicyTxt_Copy.Text);
                room.BuyInPolicy = buy;
            }
            catch
            {
                MessageBox.Show("Buy in policy must be a number!", "Error in creation", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }
            try
            {
                int chip = Int32.Parse(ChipPolicyTxt_Copy.Text);
                room.ChipPolicy = chip;
            }
            catch
            {
                MessageBox.Show("Chip policy must be a number!", "Error in creation", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }
            try
            {
                int minB = Int32.Parse(MinBetTxt_Copy.Text);
                room.MinBet = minB;
            }
            catch
            {
                MessageBox.Show("Minimum bet must be a number!", "Error in creation", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }
            try
            {
                int minP = Int32.Parse(MinPlayersTxt_Copy.Text);
                room.MinPlayers = minP;
            }
            catch
            {
                MessageBox.Show("Minimum number of players must be a number!", "Error in creation", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }
            try
            {
                int maxP = Int32.Parse(MaxPlayersTxt_Copy.Text);
                room.MaxPlayers = maxP;
            }
            catch
            {
                MessageBox.Show("Maximum number of players must be a number!", "Error in creation", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }

            bool cond = SpectatingCombobox_Copy.Text == "Yes";
            room.SpectatingAllowed = cond;

            room.RoomName = RoomNameTxt.Text;

            return room;
        }

        private void newRoomButton_Click(object sender, RoutedEventArgs e)
        {
            Room room = SetRoom();

            if (room == null)
                return;

            string controller = "Room";
            string data = new JavaScriptSerializer().Serialize(room);

            string ans = RestClient.MakePostRequest(controller, data);
            JObject json = JObject.Parse(ans);
            RoomState roomState = json.ToObject<RoomState>();
            if (roomState.Messege == null)
            {
                    int chip = (int)chipsLabel.Content;
                if (room.ChipPolicy != 0)
                {
                    chipsLabel.Content = chip - room.ChipPolicy;
                }
                else
                {
                    chipsLabel.Content = 0;
                }
                MessageBox.Show("Room created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                GameWindow gameWindow = new GameWindow(PlayerNameTxt_Copy.Text, roomState, true);
                App.Current.MainWindow = gameWindow;
                //this.Close();
                gameWindow.Show();
            }
            else
            {
                MessageBox.Show(roomState.Messege, "Error in creation", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RoomsGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("NOT IMPLEMENTED!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
