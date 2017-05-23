using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Client.Data;
using System.Web.Script.Serialization;
using System.Windows.Media.Imaging;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public UserData LoggedUser;
        public List<Room> RoomResults;
        private bool _loggedIn;

        public MainWindow(UserData user)
        {
            RoomResults = new List<Room>();
            InitializeComponent();
            RoomsGrid.ItemsSource = RoomResults;
            LoggedUser = user;
            _loggedIn = true;
            DataContext = LoggedUser;
            if (LoggedUser.AvatarPath == "default.png")
            {
                LoggedUser.AvatarPath = "Resources/profilePicture.png";
            }
            UpdateAvatar(LoggedUser.AvatarPath);
        }

        public void UpdateAvatar(string path)
        {
            ProfilePic.Dispatcher.Invoke(() => ProfilePic.Source = new BitmapImage(new Uri(@path, UriKind.Relative)));
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
            var filter = new RoomFilter { User = LoggedUser.Username };
            if (PlayerCheckbox.IsChecked != null && PlayerCheckbox.IsChecked.Value)
            {
                filter.PlayerName = PlayerNameTxt.Text;
            }
            if (PotCheckbox.IsChecked != null && PotCheckbox.IsChecked.Value)
            {
                try
                {
                    var pot = int.Parse(PotSizeTxt.Text);
                    filter.PotSize = pot;
                }
                catch
                {
                    MessageBox.Show("Pot size must be a number!", "Error in search", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            if (LeagueCheckbox.IsChecked != null && LeagueCheckbox.IsChecked.Value)
            {
                filter.LeagueOnly = true;
            }
            if (GameTypeCheckbox.IsChecked != null && GameTypeCheckbox.IsChecked.Value)
            {
                filter.GameType = GameTypeCombobox.Text;
            }
            if (BuyinPolicyCheckbox.IsChecked != null && BuyinPolicyCheckbox.IsChecked.Value)
            {
                try
                {
                    var buy = int.Parse(BuyinPolicyTxt.Text);
                    filter.BuyInPolicy = buy;
                }
                catch
                {
                    MessageBox.Show("Buy in policy must be a number!", "Error in search", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            if (ChipPolicyCheckbox.IsChecked != null && ChipPolicyCheckbox.IsChecked.Value)
            {
                try
                {
                    var chip = int.Parse(ChipPolicyTxt.Text);
                    filter.ChipPolicy = chip;
                }
                catch
                {
                    MessageBox.Show("Chip policy must be a number!", "Error in search", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            if (MinBetCheckbox.IsChecked != null && MinBetCheckbox.IsChecked.Value)
            {
                try
                {
                    var minB = int.Parse(MinBetTxt.Text);
                    filter.MinBet = minB;
                }
                catch
                {
                    MessageBox.Show("Minimum bet must be a number!", "Error in search", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            if (MinPlayersCheckbox.IsChecked != null && MinPlayersCheckbox.IsChecked.Value)
            {
                try
                {
                    var minP = int.Parse(MinPlayersTxt.Text);
                    filter.MinPlayers = minP;
                }
                catch
                {
                    MessageBox.Show("Minimum number of players must be a number!", "Error in search", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            if (MaxPlayersCheckbox.IsChecked != null && MaxPlayersCheckbox.IsChecked.Value)
            {
                try
                {
                    var maxP = int.Parse(MaxPlayersTxt.Text);
                    filter.MaxPlayers = maxP;
                }
                catch
                {
                    MessageBox.Show("Maximum number of players must be a number!", "Error in search", MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            }
            if (SpectatingCheckbox.IsChecked != null && SpectatingCheckbox.IsChecked.Value)
            {
                var cond = SpectatingCombobox.SelectedIndex<=0;
                filter.SpectatingAllowed = cond;
            }
            return filter;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var filter = SetFilter();

            if (filter == null)
                return;

            const string controller = "Search";
            var data = new JavaScriptSerializer().Serialize(filter);
            var ans = RestClient.MakePostRequest(controller, data);
            var json = JObject.Parse(ans);
            var roomList = json.ToObject<RoomList>();
            if (roomList.Message == null)
            {
                RoomResults = roomList.Rooms.ToList();
                RoomsGrid.ItemsSource = RoomResults;
                RoomsGrid.Items.Refresh();
            }
            else
            {
                MessageBox.Show("No rooms to show!", "No results", MessageBoxButton.OK, MessageBoxImage.Information);
                RoomResults.Clear();
                RoomsGrid.ItemsSource = RoomResults;
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
             var profile = new ProfileWindow(LoggedUser, this);
            Application.Current.MainWindow = profile;
            profile.Show();
            Hide();
        }

        private void GameTypeCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            GameTypeCombobox.IsEnabled = !GameTypeCombobox.IsEnabled;
        }

        private Room SetRoom()
        {
            var room = new Room
            {
                CreatorUserName = LoggedUser.Username,
                CreatorPlayerName = PlayerNameTxt_Copy.Text,
                GameType = GameTypeCombobox_Copy.Text
            };
            try
            {
                var buy = int.Parse(BuyinPolicyTxt_Copy.Text);
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
                var chip = int.Parse(ChipPolicyTxt_Copy.Text);
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
                var minB = int.Parse(MinBetTxt_Copy.Text);
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
                var minP = int.Parse(MinPlayersTxt_Copy.Text);
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
                var maxP = int.Parse(MaxPlayersTxt_Copy.Text);
                room.MaxPlayers = maxP;
            }
            catch
            {
                MessageBox.Show("Maximum number of players must be a number!", "Error in creation", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return null;
            }

            var cond = SpectatingCombobox_Copy.SelectedIndex <= 0;
            room.SpectatingAllowed = cond;

            room.RoomName = RoomNameTxt.Text;

            return room;
        }

        private void newRoomButton_Click(object sender, RoutedEventArgs e)
        {
            var room = SetRoom();

            if (room == null)
                return;

            const string controller = "Room";
            var data = new JavaScriptSerializer().Serialize(room);
            var ans = RestClient.MakePostRequest(controller, data);
            var json = JObject.Parse(ans);
            var roomState = json.ToObject<RoomState>();
            if (roomState.Messege == null)
            {
                var chip = (int)chipsLabel.Content;
                if (room.ChipPolicy != 0)
                {
                   LoggedUser.Chips = chip - room.ChipPolicy;
                }
                else
                {
                    LoggedUser.Chips = 0;
                }
                MessageBox.Show("Room created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                var gameWindow = new GameWindow(LoggedUser, PlayerNameTxt_Copy.Text, roomState, true, this);
                Application.Current.MainWindow = gameWindow;

                gameWindow.Show();
            }
            else
            {
                MessageBox.Show(roomState.Messege, "Error in creation", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RoomsGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoomsGrid.SelectedIndex >= 0)
            {
                Join.IsEnabled = true;
                JoinNameLbl.Visibility = Visibility.Visible;
                JoinNameTxt.Visibility = Visibility.Visible;
            }
            else
            {
                Join.IsEnabled = false;
                JoinNameLbl.Visibility = Visibility.Hidden;
                JoinNameTxt.Visibility = Visibility.Hidden;
            }
        }

        private void Join_Click(object sender, RoutedEventArgs e)
        {
            Room room = RoomResults[RoomsGrid.SelectedIndex];
            if (string.IsNullOrEmpty(JoinNameTxt.Text))
            {
                MessageBox.Show("Player name cannot be empty!", "Error in join", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                var controller = "Room?userName=" + LoggedUser.Username + "&gameName=" + room.RoomName +
                                 "&playerName=" + JoinNameTxt.Text +"&option=join";
                var ans = RestClient.MakeGetRequest(controller);
                var json = JObject.Parse(ans);
                var roomState = json.ToObject<RoomState>();
                if (roomState.Messege == null)
                {
                    var chip = (int)chipsLabel.Content;
                    if (RoomResults[RoomsGrid.SelectedIndex].ChipPolicy != 0)
                    {
                        LoggedUser.Chips = chip - RoomResults[RoomsGrid.SelectedIndex].ChipPolicy;
                    }
                    else
                    {
                        LoggedUser.Chips = 0;
                    }
                    MessageBox.Show("Joined room successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                    var gameWindow = new GameWindow(LoggedUser, JoinNameTxt.Text, roomState, false, this);
                    Application.Current.MainWindow = gameWindow;
                    gameWindow.Show();
                }
                else
                {
                    MessageBox.Show(roomState.Messege, "Error in join", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LogoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            var controller = "User?username=" + LoggedUser.Username + "&mode=logout";
            var ans = RestClient.MakeGetRequest(controller);
            ans = ans.Normalize();
            if (ans != "\"\"")
            {
                MessageBox.Show(ans, "Error in logout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show("Logged out successfully!", "Success", MessageBoxButton.OK,
                    MessageBoxImage.Information);
                var login = new LoginWindow();
                Application.Current.MainWindow = login;
                _loggedIn = false;
                Close();
                login.Show();
                
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_loggedIn)
            {
                e.Cancel = true;
                MessageBox.Show("Cannot exit before logging out!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
    }
}
