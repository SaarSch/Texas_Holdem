using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web.Script.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Client.Data;
using Newtonsoft.Json.Linq;

namespace Client
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private bool _loggedIn;
        public UserData LoggedUser;
        public List<GameWindow> OpenWindows;
        public ProfileWindow ProfileWindow;
        public List<TupleModel<string, string>> Replays;
        public List<Room> RoomResults;

        public MainWindow(UserData user)
        {
            RoomResults = new List<Room>();
            OpenWindows = new List<GameWindow>();
            Replays = new List<TupleModel<string, string>>();
            ProfileWindow = null;
            InitializeComponent();
            RoomsGrid.ItemsSource = RoomResults;
            ReplayGrid.ItemsSource = Replays;
            LoggedUser = user;
            _loggedIn = true;
            DataContext = LoggedUser;
            UpdateAvatar(LoggedUser.AvatarPath);
            ReplayRequest();
        }

        public void UpdateAvatar(string path)
        {
            ProfilePic.Dispatcher.Invoke(() => ProfilePic.Source = new BitmapImage(new Uri(path, UriKind.Relative)));
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
            var filter = new RoomFilter {User = LoggedUser.Username};
            if (PlayerCheckbox.IsChecked != null && PlayerCheckbox.IsChecked.Value)
                filter.PlayerName = PlayerNameTxt.Text;
            if (PotCheckbox.IsChecked != null && PotCheckbox.IsChecked.Value)
                try
                {
                    var pot = int.Parse(PotSizeTxt.Text);
                    filter.PotSize = pot;
                }
                catch
                {
                    MessageBox.Show("Pot size must be a number!", "Error in search", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return null;
                }
            if (LeagueCheckbox.IsChecked != null && LeagueCheckbox.IsChecked.Value)
                filter.LeagueOnly = true;
            if (GameTypeCheckbox.IsChecked != null && GameTypeCheckbox.IsChecked.Value)
                filter.GameType = GameTypeCombobox.Text;
            if (BuyinPolicyCheckbox.IsChecked != null && BuyinPolicyCheckbox.IsChecked.Value)
                try
                {
                    var buy = int.Parse(BuyinPolicyTxt.Text);
                    filter.BuyInPolicy = buy;
                }
                catch
                {
                    MessageBox.Show("Buy in policy must be a number!", "Error in search", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return null;
                }
            if (ChipPolicyCheckbox.IsChecked != null && ChipPolicyCheckbox.IsChecked.Value)
                try
                {
                    var chip = int.Parse(ChipPolicyTxt.Text);
                    filter.ChipPolicy = chip;
                }
                catch
                {
                    MessageBox.Show("Chip policy must be a number!", "Error in search", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return null;
                }
            if (MinBetCheckbox.IsChecked != null && MinBetCheckbox.IsChecked.Value)
                try
                {
                    var minB = int.Parse(MinBetTxt.Text);
                    filter.MinBet = minB;
                }
                catch
                {
                    MessageBox.Show("Minimum bet must be a number!", "Error in search", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return null;
                }
            if (MinPlayersCheckbox.IsChecked != null && MinPlayersCheckbox.IsChecked.Value)
                try
                {
                    var minP = int.Parse(MinPlayersTxt.Text);
                    filter.MinPlayers = minP;
                }
                catch
                {
                    MessageBox.Show("Minimum number of players must be a number!", "Error in search",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            if (MaxPlayersCheckbox.IsChecked != null && MaxPlayersCheckbox.IsChecked.Value)
                try
                {
                    var maxP = int.Parse(MaxPlayersTxt.Text);
                    filter.MaxPlayers = maxP;
                }
                catch
                {
                    MessageBox.Show("Maximum number of players must be a number!", "Error in search",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return null;
                }
            if (SpectatingCheckbox.IsChecked != null && SpectatingCheckbox.IsChecked.Value)
            {
                var cond = SpectatingCombobox.SelectedIndex <= 0;
                filter.SpectatingAllowed = cond;
            }
            return filter;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var filter = SetFilter();

            if (filter == null)
                return;

            var controller = "Search?token=" + LoggedUser.token;
            filter.User = Crypto.Encrypt(filter.User);
            var data = new JavaScriptSerializer().Serialize(filter);
            try
            {
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
            catch
            {
                HandleCrashing();
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
            if (ProfileWindow == null)
                ProfileWindow = new ProfileWindow(LoggedUser, this);
            Application.Current.MainWindow = ProfileWindow;
            ProfileWindow.Show();
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
                CreatorPlayerName = LoggedUser.Username,
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

            var controller = "Room?token=" + LoggedUser.token;
            room.CreatorUserName = Crypto.Encrypt(room.CreatorUserName);
            var data = new JavaScriptSerializer().Serialize(room);
            try
            {
                var ans = RestClient.MakePostRequest(controller, data);
                var json = JObject.Parse(ans);
                var roomState = json.ToObject<RoomState>();
                if (roomState.Messege == null)
                {
                    var chip = (int)chipsLabel.Content;
                    if (room.ChipPolicy != 0)
                        LoggedUser.Chips = chip - room.ChipPolicy;
                    else
                        LoggedUser.Chips = 0;
                    RoomNameTxt.Text = "";
                    var gameWindow = new GameWindow(LoggedUser, LoggedUser.Username, roomState, this, null);
                    OpenWindows.Add(gameWindow);
                    Application.Current.MainWindow = gameWindow;
                    gameWindow.Show();
                }
                else
                {
                    MessageBox.Show(roomState.Messege, "Error in creation", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                HandleCrashing();
            }
        }

        private void RoomsGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RoomsGrid.SelectedIndex >= 0)
            {
                Join.IsEnabled = true;
                Spectate.IsEnabled = true;
            }
            else
            {
                Join.IsEnabled = false;
                Spectate.IsEnabled = false;
            }
        }

        private void Join_Click(object sender, RoutedEventArgs e)
        {
            var room = RoomResults[RoomsGrid.SelectedIndex];
            var controller = "Room?userName=" + Crypto.Encrypt(LoggedUser.Username) + "&gameName=" + room.RoomName +
                             "&playerName=" + LoggedUser.Username + "&option=join&token=" + LoggedUser.token;
            try
            {
                var ans = RestClient.MakeGetRequest(controller);
                var json = JObject.Parse(ans);
                var roomState = json.ToObject<RoomState>();
                if (roomState.Messege == null)
                {
                    var chip = (int)chipsLabel.Content;
                    if (RoomResults[RoomsGrid.SelectedIndex].ChipPolicy != 0)
                        LoggedUser.Chips = chip - RoomResults[RoomsGrid.SelectedIndex].ChipPolicy;
                    else
                        LoggedUser.Chips = 0;

                    var gameWindow = new GameWindow(LoggedUser, LoggedUser.Username, roomState, this, null);
                    OpenWindows.Add(gameWindow);
                    Application.Current.MainWindow = gameWindow;
                    gameWindow.Show();
                }
                else
                {
                    MessageBox.Show(roomState.Messege, "Error in join", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                HandleCrashing();
            }
        }

        private void LogoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_loggedIn)
            {
                var controller = "User?username=" + Crypto.Encrypt(LoggedUser.Username) + "&mode=logout&token=" +
                                 LoggedUser.token;
                try
                {
                    RestClient.MakeGetRequest(controller);
                }
                catch
                { }
                finally
                {
                    foreach (var w in OpenWindows)
                    {
                        if (w != null && w.Playing)
                        {
                            w.Playing = false;
                            w.Close();
                        }
                    }
                    if (ProfileWindow != null)
                    {
                        ProfileWindow.Open = false;
                        ProfileWindow.Close();
                    }
                    var login = new LoginWindow();
                    Application.Current.MainWindow = login;
                    _loggedIn = false;
                    login.Show();
                }
            }
        }

        private void Spectate_Click(object sender, RoutedEventArgs e)
        {
            var room = RoomResults[RoomsGrid.SelectedIndex];
            var controller = "Room?userName=" + Crypto.Encrypt(LoggedUser.Username) + "&gameName=" + room.RoomName +
                             "&playerName=none&option=spectate&token=" + LoggedUser.token;
            try
            {
                var ans = RestClient.MakeGetRequest(controller);
                var json = JObject.Parse(ans);
                var roomState = json.ToObject<RoomState>();
                if (roomState.Messege == null)
                {
                    var gameWindow = new GameWindow(LoggedUser, null, roomState, this, null);
                    OpenWindows.Add(gameWindow);
                    Application.Current.MainWindow = gameWindow;
                    gameWindow.Show();
                }
                else
                {
                    MessageBox.Show(roomState.Messege, "Error in spectate", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                HandleCrashing();
            }
        }

        private void Watch_Click(object sender, RoutedEventArgs e)
        {
            var selection = Replays[ReplayGrid.SelectedIndex];
            var dateAndTime = selection.m_Item1.Split(' ');
            var controller = "Replay?user=" + Crypto.Encrypt(LoggedUser.Username) + "&roomName=" + selection.m_Item2 +
                             "&date=" + dateAndTime[0].Replace('/', '_') + ' ' + dateAndTime[1].Replace(':', '_') +
                             "&token=" + LoggedUser.token;
            try
            {
                var ans = RestClient.MakeGetRequest(controller);
                var js = new JavaScriptSerializer();
                try
                {
                    var replayStates = js.Deserialize<List<RoomState>>(ans);
                    var gameWindow = new GameWindow(LoggedUser, LoggedUser.Username, replayStates[0], this, replayStates);
                    OpenWindows.Add(gameWindow);
                    Application.Current.MainWindow = gameWindow;
                    gameWindow.Show();
                }
                catch
                {
                    MessageBox.Show("Replay is not available.", "Error in replay", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch
            {
                HandleCrashing();
            }
        }

        private void ReplayGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ReplayGrid.SelectedIndex >= 0)
                Watch.IsEnabled = true;
            else
                Watch.IsEnabled = false;
        }

        private void GameTypeCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            ReplayRequest();
        }

        private void ReplayRequest()
        {
            var controller = "Replay?user=" + Crypto.Encrypt(LoggedUser.Username) + "&&token=" + LoggedUser.token;
            try
            {
                var ans = RestClient.MakeGetRequest(controller);
                var js = new JavaScriptSerializer();
                var replayAns = js.Deserialize<List<TupleModel<string, string>>>(ans);
                var tmpList = replayAns.ToList();
                Replays.Clear();
                foreach (var pair in tmpList)
                {
                    var tmpTpl = new TupleModel<string, string>();
                    var dateAndTime = pair.m_Item1.Split(' ');
                    tmpTpl.m_Item1 = dateAndTime[0].Replace('_', '/') + ' ' + dateAndTime[1].Replace('_', ':');
                    tmpTpl.m_Item2 = pair.m_Item2;
                    Replays.Add(tmpTpl);
                }
                ReplayGrid.ItemsSource = Replays;
                ReplayGrid.Items.Refresh();
            }
            catch
            {
                HandleCrashing();
            }
        }

        public void HandleCrashing()
        {
            MessageBox.Show("Server cannot be found. You are logged out automatically.", "Server Not Found", MessageBoxButton.OK, MessageBoxImage.Error);
            Close();
        }
    }
}