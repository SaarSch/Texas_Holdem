using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Client.Annotations;
using Client.Data;
using Newtonsoft.Json.Linq;

namespace Client
{
    /// <summary>
    ///     Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : INotifyPropertyChanged
    {
        private BindingList<string> _chatComboBoxContent;
        private bool _first_play;
        private bool _got_win_msg;
        private bool _me;
        private string _msg;
        private readonly List<RoomState> _replayStates;
        public Image[] Avatars;
        public Label[] BetLabels;
        public Image[] ChipImages;
        public Label[] ChipLabels;
        public Image[] CommunityCards;
        public int CountPlayers;
        public Image[][] HandCards;
        public MainWindow Main;
        public Label[] NameLabels;
        public Dictionary<string, int> PlayerMap;
        public bool Playing;
        public string RoomName;
        public string SelfPlayerName;
        public Rectangle[] TurnSymbol;
        public UserData User;
        private int CountCrashes;


        public GameWindow(UserData user, string self, RoomState state, MainWindow main, List<RoomState> replayStates)
        {
            InitializeComponent();
            Main = main;
            SelfPlayerName = self;
            CountCrashes = 0;
            User = user;
            RoomName = state.RoomName;
            RoomNameLbl.Content = RoomName;
            PlayerMap = new Dictionary<string, int>();
            ChatComboBoxContent = new BindingList<string>();
            Playing = true;
            _got_win_msg = false;
            _first_play = true;
            _me = false;
            _replayStates = replayStates;
            InitGuiArrays();
            if (SelfPlayerName == null)
            {
                P1Lbl.Foreground = Brushes.Black;
                C1Lbl.Foreground = Brushes.Black;
            }
            DataContext = this;

            if (_replayStates == null)
            {
                UpdateRoom(state);
            }
            else
            {
                Chat.Visibility = Visibility.Hidden;
                ChatScroll.Visibility = Visibility.Hidden;
                ChatComboBox.Visibility = Visibility.Hidden;
                Message.Visibility = Visibility.Hidden;
                Send.Visibility = Visibility.Hidden;
                Bet.Visibility = Visibility.Hidden;
                Call.Visibility = Visibility.Hidden;
                Fold.Visibility = Visibility.Hidden;
                BetSlide.Visibility = Visibility.Hidden;
                CurrentBet_Label.Visibility = Visibility.Hidden;
                ReplayMsg.Visibility = Visibility.Visible;
                ReplayLbl.Visibility = Visibility.Visible;
                Replay();
            }
        }

        public BindingList<string> ChatComboBoxContent
        {
            get => _chatComboBoxContent;
            set
            {
                _chatComboBoxContent = value;
                OnPropertyChanged();
            }
        }

        public string Msg
        {
            get => _msg;
            set
            {
                _msg = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Replay()
        {
            CountPlayers = 1;
            PlayerMap.Add(SelfPlayerName, CountPlayers);
            UpdateAllPlayers(_replayStates[0]);
            UpdateCommunityCards(_replayStates[0].CommunityCards, _replayStates[0].IsOn,
                _replayStates[0].CurrentWinners != null && _replayStates[0].CurrentWinners != "");
            ResetPlayers(CountPlayers);
        }

        private void DrawReplayStates()
        {
            for (var i = 0; i < _replayStates.Count; i++)
            {
                UpdateAllPlayers(_replayStates[i]);
                UpdateCommunityCards(_replayStates[i].CommunityCards, _replayStates[i].IsOn,
                    _replayStates[i].CurrentWinners != null && _replayStates[i].CurrentWinners != "");
                ResetPlayers(CountPlayers);
                Thread.Sleep(2000);
                if (!string.IsNullOrEmpty(_replayStates[i].CurrentWinners) && Playing)
                {
                    MessageBox.Show(_replayStates[i].CurrentWinners, "Game Over!", MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    Start.Dispatcher.Invoke(() => Start.Visibility = Visibility.Visible);
                    break;
                }
            }
        }


        private void InitGuiArrays()
        {
            NameLabels = new[] {P1Lbl, P2Lbl, P3Lbl, P4Lbl, P5Lbl, P6Lbl, P7Lbl, P8Lbl, P9Lbl};
            ChipLabels = new[] {C1Lbl, C2Lbl, C3Lbl, C4Lbl, C5Lbl, C6Lbl, C7Lbl, C8Lbl, C9Lbl};
            ChipImages = new[]
                {ChipImg1, ChipImg2, ChipImg3, ChipImg4, ChipImg5, ChipImg6, ChipImg7, ChipImg8, ChipImg9};
            BetLabels = new[] {Bet1, Bet2, Bet3, Bet4, Bet5, Bet6, Bet7, Bet8, Bet9};
            CommunityCards = new[] {Com1, Com2, Com3, Com4, Com5};
            Avatars = new[] {Avatar1, Avatar2, Avatar3, Avatar4, Avatar5, Avatar6, Avatar7, Avatar8, Avatar9};
            TurnSymbol = new[] {RecP1, RecP2, RecP3, RecP4, RecP5, RecP6, RecP7, RecP8, RecP9};
            HandCards = new[]
            {
                new[] {P1Card1, P1Card2}, new[] {P2Card1, P2Card2}, new[] {P3Card1, P3Card2},
                new[] {P4Card1, P4Card2}, new[] {P5Card1, P5Card2}, new[] {P6Card1, P6Card2},
                new[] {P7Card1, P7Card2}, new[] {P8Card1, P8Card2}, new[] {P9Card1, P9Card2}
            };
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CurrentBet_Label != null)
                CurrentBet_Label.Content = (int) e.NewValue;
        }

        private void EndOfGameUpdate(RoomState state)
        {
            if (state.IsOn == false && !string.IsNullOrEmpty(state.CurrentWinners) && !_got_win_msg && !_first_play &&
                Playing)
            {
                _got_win_msg = true;
                MessageBox.Show(state.CurrentWinners, "Game Over!", MessageBoxButton.OK, MessageBoxImage.Information);
                foreach (var r in TurnSymbol)
                    r.Dispatcher.Invoke(() => r.Fill = Brushes.White);
            }
        }

        private void StartOfGameUpdate(RoomState state)
        {
            if (state.IsOn == false)
            {
                if (SelfPlayerName != null)
                    Start.Dispatcher.Invoke(() => Start.Visibility = Visibility.Visible);
                else
                    Start.Dispatcher.Invoke(() => Start.Visibility = Visibility.Hidden);
            }
            else
            {
                Start.Dispatcher.Invoke(() => Start.Visibility = Visibility.Hidden);
                _got_win_msg = false;
                _first_play = false;
            }
        }

        private void UpdateAllPlayers(RoomState state)
        {
            var tmpComboboxList = new BindingList<string>();

            foreach (var r in TurnSymbol)
                r.Dispatcher.Invoke(() => r.Fill = Brushes.White);

            if (!state.IsOn && _replayStates == null) // add self to player-related lists
            {
                PlayerMap.Clear();
                CountPlayers = 0;
                tmpComboboxList.Add("ALL");
                if (SelfPlayerName != null)
                {
                    CountPlayers++;
                    PlayerMap.Add(SelfPlayerName, CountPlayers);
                }
            }

            foreach (var p in state.AllPlayers) // modify all participating players
            {
                if (!PlayerMap.ContainsKey(p.PlayerName))
                {
                    CountPlayers++;
                    PlayerMap.Add(p.PlayerName, CountPlayers);
                    if (SelfPlayerName != null && _replayStates == null)
                        tmpComboboxList.Add(p.PlayerName);
                }
                PlayerMap.TryGetValue(p.PlayerName, out int playerVal);
                if (p.PlayerName == state.CurrentPlayer && state.IsOn)
                    TurnSymbol[playerVal - 1]
                        .Dispatcher.Invoke(() => TurnSymbol[playerVal - 1].Fill = Brushes.Red);
                if (p.PlayerName == SelfPlayerName && state.IsOn == false &&
                    !string.IsNullOrEmpty(state.CurrentWinners) && _replayStates == null)
                    User.Chips = p.ChipsAmount;
                if (playerVal != -1)
                    UpdatePlayer(playerVal, p);
            }

            if (!state.IsOn && _replayStates == null) // add spectators to chat combobox
            {
                foreach (var s in state.Spectators)
                    if (!tmpComboboxList.Contains(s.Username) && s.Username != User.Username)
                        tmpComboboxList.Add(s.Username);
                ChatComboBoxContent = tmpComboboxList;
            }
        }

        private void UpdateRoom(RoomState state)
        {
            StartOfGameUpdate(state);
            UpdateAllPlayers(state);
            UpdateCommunityCards(state.CommunityCards, state.IsOn,
                state.CurrentWinners != null && state.CurrentWinners != "");
            ResetPlayers(CountPlayers);
            UpdateBetGui(state);
            UpdateChat(state);
            EndOfGameUpdate(state);

            if (Playing)
            {
                Timer timer = null;
                timer = new Timer(obj =>
                    {
                        StatusRequest();
                        timer.Dispose();
                    },
                    null, 2000, Timeout.Infinite);
            }
        }

        private void StatusRequest()
        {
            var nameToSend = SelfPlayerName;
            if (SelfPlayerName == null)
                nameToSend = User.Username;
            var controller = "Room?gameName=" + RoomName + "&playerName=" + nameToSend + "&token=" + User.token;
            try
            {
                var ans = RestClient.MakePutRequest(controller, "");
                var json = JObject.Parse(ans);
                var roomState = json.ToObject<RoomState>();
                CountCrashes = 0;
                if (roomState.Messege == null && roomState.RoomName == RoomName)
                {
                    UpdateRoom(roomState);
                }
                else
                {
                    if (Playing)
                        if (roomState.Messege.Contains("exist"))
                        {
                            MessageBox.Show(roomState.Messege + "\nThis room is closed.", "Room is closed",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                            Application.Current.Dispatcher.Invoke(() => Close());
                        }
                        else
                        {
                            MessageBox.Show(roomState.Messege, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                }
            }
            catch(Exception e)
            {
                if(CountCrashes == 3)
                {
                    MessageBox.Show(e.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    Application.Current.Dispatcher.Invoke(() => Close());
                }
                else
                {
                    CountCrashes++;
                    StatusRequest();
                }
                
            }
        }

        private void UpdatePlayer(int i, Player p)
        {
            NameLabels[i - 1].Dispatcher.Invoke(() => NameLabels[i - 1].Visibility = Visibility.Visible);
            ChipLabels[i - 1].Dispatcher.Invoke(() => ChipLabels[i - 1].Visibility = Visibility.Visible);
            ChipImages[i - 1].Dispatcher.Invoke(() => ChipImages[i - 1].Visibility = Visibility.Visible);
            BetLabels[i - 1].Dispatcher.Invoke(() => BetLabels[i - 1].Visibility = Visibility.Visible);
            Avatars[i - 1].Dispatcher.Invoke(() => Avatars[i - 1].Visibility = Visibility.Visible);
            TurnSymbol[i - 1]
                .Dispatcher.Invoke(() => TurnSymbol[i - 1].Visibility = Visibility.Visible);
            NameLabels[i - 1].Dispatcher.Invoke(() => NameLabels[i - 1].Content = p.PlayerName);
            ChipLabels[i - 1].Dispatcher.Invoke(() => ChipLabels[i - 1].Content = p.ChipsAmount);
            BetLabels[i - 1].Dispatcher.Invoke(() => BetLabels[i - 1].Content = p.CurrentBet);
            Avatars[i - 1]
                .Dispatcher.Invoke(() => Avatars[i - 1].Source = new BitmapImage(new Uri(p.Avatar, UriKind.Relative)));
            if (p.PlayerName == SelfPlayerName)
                BetSlide.Dispatcher.Invoke(() => BetSlide.Maximum = p.ChipsAmount);
            UpdateCards(i, p.PlayerHand, true);
        }

        private void ResetPlayers(int startIndex)
        {
            for (var i = startIndex; i < NameLabels.Length; i++)
            {
                NameLabels[i].Dispatcher.Invoke(() => NameLabels[i].Visibility = Visibility.Hidden);
                ChipLabels[i].Dispatcher.Invoke(() => ChipLabels[i].Visibility = Visibility.Hidden);
                ChipImages[i].Dispatcher.Invoke(() => ChipImages[i].Visibility = Visibility.Hidden);
                BetLabels[i].Dispatcher.Invoke(() => BetLabels[i].Visibility = Visibility.Hidden);
                Avatars[i].Dispatcher.Invoke(() => Avatars[i].Visibility = Visibility.Hidden);
                TurnSymbol[i]
                    .Dispatcher.Invoke(() => TurnSymbol[i].Visibility = Visibility.Hidden);
                NameLabels[i].Dispatcher.Invoke(() => NameLabels[i].Content = "PlayerName");
                ChipLabels[i].Dispatcher.Invoke(() => ChipLabels[i].Content = 0);
                BetLabels[i].Dispatcher.Invoke(() => BetLabels[i].Content = "");
                Avatars[i]
                    .Dispatcher.Invoke(() => Avatars[i].Source =
                        new BitmapImage(new Uri(@"Resources/profilePicture.png", UriKind.Relative)));
                UpdateCards(i + 1, null, false);
            }
        }

        private void UpdateBetGui(RoomState state)
        {
            if (SelfPlayerName == state.CurrentPlayer && state.IsOn)
            {
                if (!_me)
                {
                    _me = true;
                    BetSlide.Dispatcher.Invoke(() => BetSlide.IsEnabled = true);
                    BetSlide.Dispatcher.Invoke(() => BetSlide.Value = 1);
                    CurrentBet_Label.Dispatcher.Invoke(() => CurrentBet_Label.Content = 1);
                    CurrentBet_Label.Dispatcher.Invoke(() => CurrentBet_Label.Visibility = Visibility.Visible);
                    Bet.Dispatcher.Invoke(() => Bet.IsEnabled = true);
                    Call.Dispatcher.Invoke(() => Call.IsEnabled = true);
                    Fold.Dispatcher.Invoke(() => Fold.IsEnabled = true);
                }
            }
            else
            {
                _me = false;
                BetSlide.Dispatcher.Invoke(() => BetSlide.IsEnabled = false);
                BetSlide.Dispatcher.Invoke(() => BetSlide.Value = 1);
                CurrentBet_Label.Dispatcher.Invoke(() => CurrentBet_Label.Visibility = Visibility.Hidden);
                Bet.Dispatcher.Invoke(() => Bet.IsEnabled = false);
                Call.Dispatcher.Invoke(() => Call.IsEnabled = false);
                Fold.Dispatcher.Invoke(() => Fold.IsEnabled = false);
            }
        }

        private void UpdateCards(int i, string[] hand, bool display)
        {
            if (display)
            {
                HandCards[i - 1][0]
                    .Dispatcher.Invoke(() => HandCards[i - 1][0].Visibility = Visibility.Visible);
                HandCards[i - 1][1]
                    .Dispatcher.Invoke(() => HandCards[i - 1][1].Visibility = Visibility.Visible);
                if (hand != null && hand[0] != null && hand[1] != null)
                {
                    HandCards[i - 1][0]
                        .Dispatcher.Invoke(() => HandCards[i - 1][0].Source =
                            new BitmapImage(new Uri(@"Resources/_" + hand[0] + ".png", UriKind.Relative)));
                    HandCards[i - 1][1]
                        .Dispatcher.Invoke(() => HandCards[i - 1][1].Source =
                            new BitmapImage(new Uri(@"Resources/_" + hand[1] + ".png", UriKind.Relative)));
                }
                else
                {
                    HandCards[i - 1][0]
                        .Dispatcher.Invoke(() => HandCards[i - 1][0].Source =
                            new BitmapImage(new Uri(@"Resources/new_back.png", UriKind.Relative)));
                    HandCards[i - 1][1]
                        .Dispatcher.Invoke(() => HandCards[i - 1][1].Source =
                            new BitmapImage(new Uri(@"Resources/new_back.png", UriKind.Relative)));
                }
            }
            else
            {
                HandCards[i - 1][0]
                    .Dispatcher.Invoke(() => HandCards[i - 1][0].Visibility = Visibility.Hidden);
                HandCards[i - 1][1]
                    .Dispatcher.Invoke(() => HandCards[i - 1][1].Visibility = Visibility.Hidden);
            }
        }


        private void UpdateCommunityCards(string[] cards, bool isOn, bool gameOver)
        {
            if (!isOn && !gameOver)
            {
                for (var i = 0; i < 5; i++)
                    CommunityCards[i]
                        .Dispatcher.Invoke(() => CommunityCards[i].Visibility = Visibility.Hidden);
                return;
            }
            if (cards == null || cards.Length == 0)
            {
                for (var i = 0; i < 5; i++)
                {
                    CommunityCards[i]
                        .Dispatcher.Invoke(() => CommunityCards[i].Visibility = Visibility.Visible);
                    CommunityCards[i]
                        .Dispatcher.Invoke(() => CommunityCards[i].Source =
                            new BitmapImage(new Uri(@"Resources/new_back.png", UriKind.Relative)));
                }
                return;
            }
            for (var i = 0; i < cards.Length; i++)
            {
                CommunityCards[i]
                    .Dispatcher.Invoke(() => CommunityCards[i].Visibility = Visibility.Visible);
                if (cards[i] != null)
                    CommunityCards[i]
                        .Dispatcher.Invoke(() => CommunityCards[i].Source =
                            new BitmapImage(new Uri(@"Resources/_" + cards[i] + ".png", UriKind.Relative)));
                else
                    CommunityCards[i]
                        .Dispatcher.Invoke(() => CommunityCards[i].Source =
                            new BitmapImage(new Uri(@"Resources/new_back.png", UriKind.Relative)));
            }
        }

        private void Bet_Click(object sender, RoutedEventArgs e)
        {
            var controller = "Room?gameName=" + RoomName + "&playerName=" + SelfPlayerName +
                             "&bet=" + (int) BetSlide.Value + "&token=" + User.token;
            try
            {
                var ans = RestClient.MakeGetRequest(controller);
                var json = JObject.Parse(ans);
                var roomState = json.ToObject<RoomState>();
                if (roomState.Messege == null)
                {
                    //           UpdateRoom(roomState);
                }
                else
                {
                    MessageBox.Show(roomState.Messege, "Error in bet", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                Application.Current.Dispatcher.Invoke(() => Close());
            }
        }

        private void Call_Click(object sender, RoutedEventArgs e)
        {
            var controller = "Room?gameName=" + RoomName + "&playerName=" + SelfPlayerName + "&option=call&token=" +
                             User.token;
            try
            {
                var ans = RestClient.MakeGetRequest(controller);
                var json = JObject.Parse(ans);
                var roomState = json.ToObject<RoomState>();
                if (roomState.Messege == null)
                {
                    //          UpdateRoom(roomState);
                }
                else
                {
                    MessageBox.Show(roomState.Messege, "Error in call", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                Application.Current.Dispatcher.Invoke(() => Close());
            }
        }

        private void Fold_Click(object sender, RoutedEventArgs e)
        {
            var controller = "Room?gameName=" + RoomName + "&playerName=" + SelfPlayerName + "&option=fold&token=" +
                             User.token;
            try
            {
                var ans = RestClient.MakeGetRequest(controller);
                var json = JObject.Parse(ans);
                var roomState = json.ToObject<RoomState>();
                if (roomState.Messege == null)
                {
                    //           UpdateRoom(roomState);
                }
                else
                {
                    MessageBox.Show(roomState.Messege, "Error in fold", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch
            {
                Application.Current.Dispatcher.Invoke(() => Close());
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (_replayStates != null)
            {
                var drawStates = new Thread(DrawReplayStates);
                Start.Visibility = Visibility.Hidden;
                drawStates.Start();
                return;
            }

            var controller = "Room?gameName=" + RoomName + "&playerName=" + SelfPlayerName + "&token=" + User.token;
            try
            {
                var ans = RestClient.MakeGetRequest(controller);
                var json = JObject.Parse(ans);
                var roomState = json.ToObject<RoomState>();
                if (roomState.Messege != null)
                    MessageBox.Show(roomState.Messege, "Cannot start game", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch
            {
                Application.Current.Dispatcher.Invoke(() => Close());
            }

        }

        private void Leave_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (Playing)
            {
                if (_replayStates != null)
                {
                    Playing = false;
                    Main.OpenWindows.Remove(this);
                    Application.Current.MainWindow = Main;
                    Main.Show();
                    return;
                }

                var option = "leave";
                if (SelfPlayerName == null)
                    option = "leaveSpectator";
                var controller = "Room?userName=" + Crypto.Encrypt(User.Username) + "&gameName=" + RoomName +
                                 "&playerName=" + SelfPlayerName + "&option=" + option + "&token=" + User.token;
                try
                {
                    var ans = RestClient.MakeGetRequest(controller);
                    var json = JObject.Parse(ans);
                    var roomState = json.ToObject<RoomState>();
                    if (roomState.Messege == null)
                    {
                        Playing = false;
                        UpdateUserDataOnExit();
                        Main.OpenWindows.Remove(this);
                        Application.Current.MainWindow = Main;
                        Main.Show();
                    }
                    else
                    {
                        MessageBox.Show(roomState.Messege, "Cannot leave game", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch
                {
                    Playing = false;
                    Main.HandleCrashing();
                }
            }
        }

        private void UpdateUserDataOnExit()
        {
            var controller = "User?userName=" + Crypto.Encrypt(User.Username);
            try
            {
                var ans = RestClient.MakePutRequest(controller, "");
                var json = JObject.Parse(ans);
                var data = json.ToObject<UserData>();
                if (data.Message == null)
                {
                    User.Chips = data.Chips;
                    User.Rank = data.Rank;
                }
            }
            catch
            {
                Application.Current.Dispatcher.Invoke(() => Close());
            }
        }

        private void UpdateChat(RoomState state)
        {
            var tmpMsg = "";
            if (SelfPlayerName != null)
            {
                foreach (var p in state.AllPlayers)
                    if (p.PlayerName == SelfPlayerName)
                        foreach (var message in p.Messages)
                            tmpMsg = tmpMsg + message + "\n";
            }
            else
            {
                foreach (var u in state.Spectators)
                    if (u.Username == User.Username)
                        foreach (var message in u.Messages)
                            tmpMsg = tmpMsg + message + "\n";
            }
            Msg = tmpMsg;
            ChatScroll.Dispatcher.Invoke(() => ChatScroll.ScrollToBottom());
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            var msg = Message.Text;
            var nameToSend = SelfPlayerName;
            if (SelfPlayerName == null)
                nameToSend = User.Username;
            if (string.IsNullOrEmpty(msg))
            {
                MessageBox.Show("An empty message cannot be sent.", "Cannot send message", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                string controller;
                var status = "player";
                if (SelfPlayerName == null)
                    status = "spectator";
                if (ChatComboBox.SelectedIndex <= 0 || ChatComboBox.SelectedIndex >= ChatComboBoxContent.Count)
                    controller = "Message?room=" + RoomName + "&sender=" + nameToSend +
                                 "&message=" + msg + "&status=" + status + "&token=" + User.token;
                else
                    controller = "Message?room=" + RoomName + "&sender=" + nameToSend +
                                 "&reciver=" + ChatComboBoxContent[ChatComboBox.SelectedIndex] + "&message=" + msg +
                                 "&status=" + status + "&token=" + User.token;
                Message.Text = "";
                try
                {
                    RestClient.MakeGetRequest(controller);
                }
                catch
                {
                    Application.Current.Dispatcher.Invoke(() => Close());
                }
            }
        }

        private void Card_MouseEnter(object sender, MouseEventArgs e)
        {
            var card = sender as Image;
            ZoomCard.Dispatcher.Invoke(() => ZoomCard.Source = card.Source);
        }

        private void Card_MouseLeave(object sender, MouseEventArgs e)
        {
            ZoomCard.Dispatcher.Invoke(() => ZoomCard.Source =
                new BitmapImage(new Uri(@"Resources/new_back.png", UriKind.Relative)));
        }
    }
}