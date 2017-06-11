using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using Client.Data;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Input;
using System.Windows.Media;
using Image = System.Windows.Controls.Image;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Client.Annotations;

namespace Client
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : INotifyPropertyChanged
    {
        public string RoomName;
        public string SelfPlayerName;
        public UserData User;
        public Dictionary<string, int> PlayerMap;
        public int CountPlayers;
        private BindingList<string> _chatComboBoxContent;
        public BindingList<string> ChatComboBoxContent
        {
            get { return _chatComboBoxContent; }
            set
            {
                _chatComboBoxContent = value;
                OnPropertyChanged();
            }
        }
        public Label[] NameLabels;
        public Label[] ChipLabels;
        public Image[] ChipImages;
        public Label[] BetLabels;
        public Image[] CommunityCards;
        public Image[] Avatars;
        public Image[][] HandCards;
        public Rectangle[] TurnSymbol;
        public MainWindow Main;
        private bool _playing;
        private bool _got_win_msg;
        private bool _first_play;
        private bool _me;
        private string _msg;

        public string Msg
        {
            get { return _msg; }
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



        public GameWindow(UserData user, string self, RoomState state, MainWindow main)
        {
            InitializeComponent();
            Main = main;
            SelfPlayerName = self;
            User = user;
            RoomName = state.RoomName;
            RoomNameLbl.Content = RoomName;
            PlayerMap = new Dictionary<string, int>();
            ChatComboBoxContent = new BindingList<string>();
            _playing = true;
            _got_win_msg = false;
            _first_play = true;
            _me = false;
            InitGuiArrays();
            if (SelfPlayerName == null)
            {
                P1Lbl.Foreground = System.Windows.Media.Brushes.Black;
             //   P1Lbl.FontWeight = FontWeights.Normal;
                C1Lbl.Foreground = System.Windows.Media.Brushes.Black;
            }

            UpdateRoom(state);
            DataContext = this;
        }

        private void InitGuiArrays()
        {
            NameLabels = new[] { P1Lbl, P2Lbl, P3Lbl, P4Lbl, P5Lbl, P6Lbl, P7Lbl, P8Lbl, P9Lbl };
            ChipLabels = new[] { C1Lbl, C2Lbl, C3Lbl, C4Lbl, C5Lbl, C6Lbl, C7Lbl, C8Lbl, C9Lbl };
            ChipImages = new[] { ChipImg1, ChipImg2, ChipImg3, ChipImg4, ChipImg5, ChipImg6, ChipImg7, ChipImg8, ChipImg9 };
            BetLabels = new[] { Bet1, Bet2, Bet3, Bet4, Bet5, Bet6, Bet7, Bet8, Bet9 };
            CommunityCards = new[] { Com1, Com2, Com3, Com4, Com5 };
            Avatars = new[] { Avatar1, Avatar2, Avatar3, Avatar4, Avatar5, Avatar6, Avatar7, Avatar8, Avatar9 };
            TurnSymbol = new[] { RecP1, RecP2, RecP3, RecP4, RecP5, RecP6, RecP7, RecP8, RecP9 };
            HandCards = new[]
            {
                new[] {P1Card1, P1Card2}, new[] { P2Card1, P2Card2 }, new[] { P3Card1, P3Card2 },
                new[] { P4Card1, P4Card2 },  new[] { P5Card1, P5Card2 },  new[] { P6Card1, P6Card2 },
                new[] { P7Card1, P7Card2 },  new[] { P8Card1, P8Card2 },  new[] { P9Card1, P9Card2 }
            };
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CurrentBet_Label != null)
            {
                CurrentBet_Label.Content = (int)e.NewValue;
            }
        }

        private void EndOfGameUpdate(RoomState state)
        {
            if (state.IsOn == false && !string.IsNullOrEmpty(state.CurrentWinners) && !_got_win_msg && !_first_play)
            {
                MessageBox.Show(state.CurrentWinners, "Game Over!", MessageBoxButton.OK, MessageBoxImage.Information);
                _got_win_msg = true;
                foreach (Rectangle r in TurnSymbol)
                {
                    r.Dispatcher.Invoke(() => r.Fill = System.Windows.Media.Brushes.White);
                }
            }
        }

        private void StartOfGameUpdate(RoomState state)
        {
            if (state.IsOn == false)
            {
                if (SelfPlayerName != null)
                {
                    Start.Dispatcher.Invoke(() => Start.Visibility = Visibility.Visible);
                }
                else
                {
                    Start.Dispatcher.Invoke(() => Start.Visibility = Visibility.Hidden);
                }
            }
            else
            {
                Start.Dispatcher.Invoke(() => Start.Visibility = Visibility.Hidden);
                _got_win_msg = false;
                _first_play = false;
            }
        }

        private void UpdateRoom(RoomState state)
        {
            BindingList<string> tmpComboboxList = new BindingList<string>();

            StartOfGameUpdate(state);

            foreach (Rectangle r in TurnSymbol)
            {
                r.Dispatcher.Invoke(() => r.Fill = System.Windows.Media.Brushes.White);
            }

            if (!state.IsOn)
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


            foreach (var p in state.AllPlayers)
            {
                if (!PlayerMap.ContainsKey(p.PlayerName))
                {
                    CountPlayers++;
                    PlayerMap.Add(p.PlayerName, CountPlayers);
                    if (SelfPlayerName != null)
                    {
                        tmpComboboxList.Add(p.PlayerName);
                    }
                }
                PlayerMap.TryGetValue(p.PlayerName, out int playerVal);
                if (p.PlayerName == state.CurrentPlayer && state.IsOn)
                {
                    TurnSymbol[playerVal - 1]
                        .Dispatcher.Invoke(() => TurnSymbol[playerVal - 1].Fill = System.Windows.Media.Brushes.Red);
                }
                if (p.PlayerName == SelfPlayerName && state.IsOn == false &&
                    !string.IsNullOrEmpty(state.CurrentWinners))
                {
                    User.Chips = p.ChipsAmount;
                }
                if (playerVal != -1)
                {
                    UpdatePlayer(playerVal, p);
                }
            }

            if (!state.IsOn)
            {
                foreach (UserData s in state.Spectators)
                {
                    if (!tmpComboboxList.Contains(s.Username) && s.Username != User.Username)
                    {
                        tmpComboboxList.Add(s.Username);
                    }
                }
            }

            if (!state.IsOn)
                ChatComboBoxContent = tmpComboboxList;
            UpdateCommunityCards(state.CommunityCards, state.IsOn, (state.CurrentWinners != null && state.CurrentWinners != ""));
            /*    if (!state.IsOn)
                {
                    ChatComboBox.Dispatcher.Invoke(() => ChatComboBox.ItemsSource = ChatComboBoxContent);
                    ChatComboBox.Dispatcher.Invoke(() => ChatComboBox.Items.Refresh());
                } */

            ResetPlayers(CountPlayers);
            UpdateBetGui(state);
            UpdateChat(state);
            EndOfGameUpdate(state);



            System.Threading.Timer timer = null;
            timer = new System.Threading.Timer((obj) =>
                {
                    StatusRequest();
                    timer.Dispose();
                },
                null, 2000, System.Threading.Timeout.Infinite);

        }

        private void StatusRequest()
        {
            string nameToSend = SelfPlayerName;
            if (SelfPlayerName == null)
                nameToSend = User.Username;
            var controller = "Room?gameName=" + RoomName + "&playerName=" + nameToSend + "&token=" + User.token;
            var ans = RestClient.MakePutRequest(controller, "");
            try
            {
                var json = JObject.Parse(ans);
                var roomState = json.ToObject<RoomState>();
                if (roomState.Messege == null && roomState.RoomName == RoomName)
                {
                    UpdateRoom(roomState);
                }
                else
                {
                    if (_playing)
                    {
                        if (roomState.Messege.Contains("exist"))
                        {
                            if(roomState.Messege.Contains("player"))
                                StatusRequest();
                            else
                            {
                                MessageBox.Show(roomState+"\nThis room is closed.", "Room is closed", MessageBoxButton.OK, MessageBoxImage.Information);
                                _playing = false;
                                Application.Current.Dispatcher.Invoke(()=> Application.Current.MainWindow = Main);
                                Application.Current.Dispatcher.Invoke(() => Close());
                                Application.Current.Dispatcher.Invoke(() => Main.Show());
                            }
                            
                        }
                        else
                        MessageBox.Show(roomState.Messege, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                StatusRequest();
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
            Avatars[i - 1].Dispatcher.Invoke(() => Avatars[i - 1].Source = new BitmapImage(new Uri(@p.Avatar, UriKind.Relative)));
            if (p.PlayerName == SelfPlayerName)
            {
                BetSlide.Dispatcher.Invoke(() => BetSlide.Maximum = p.ChipsAmount);
            }
            UpdateCards(i, p.PlayerHand, true);
        }

        private void ResetPlayers(int startIndex)
        {
            for (int i = startIndex; i < NameLabels.Length; i++)
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
                for (int i = 0; i < 5; i++)
                {
                    CommunityCards[i]
                        .Dispatcher.Invoke(() => CommunityCards[i].Visibility = Visibility.Hidden);
                }
                return;
            }
            if (cards == null || cards.Length == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    CommunityCards[i]
                        .Dispatcher.Invoke(() => CommunityCards[i].Visibility = Visibility.Visible);
                    CommunityCards[i]
                        .Dispatcher.Invoke(() => CommunityCards[i].Source =
                            new BitmapImage(new Uri(@"Resources/new_back.png", UriKind.Relative)));
                }
                return;
            }
            for (int i = 0; i < cards.Length; i++)
            {
                CommunityCards[i]
                    .Dispatcher.Invoke(() => CommunityCards[i].Visibility = Visibility.Visible);
                if (cards[i] != null)
                    CommunityCards[i].Dispatcher.Invoke(() => CommunityCards[i].Source = new BitmapImage(new Uri(@"Resources/_" + cards[i] + ".png", UriKind.Relative)));
                else
                    CommunityCards[i].Dispatcher.Invoke(() => CommunityCards[i].Source = new BitmapImage(new Uri(@"Resources/new_back.png", UriKind.Relative)));
            }
        }

        private void Bet_Click(object sender, RoutedEventArgs e)
        {
            var controller = "Room?gameName=" + RoomName + "&playerName=" + SelfPlayerName +
                             "&bet=" + (int)BetSlide.Value + "&token=" + User.token;
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

        private void Call_Click(object sender, RoutedEventArgs e)
        {
            var controller = "Room?gameName=" + RoomName + "&playerName=" + SelfPlayerName + "&option=call&token=" + User.token;
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

        private void Fold_Click(object sender, RoutedEventArgs e)
        {
            var controller = "Room?gameName=" + RoomName + "&playerName=" + SelfPlayerName + "&option=fold&token=" + User.token;
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

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var controller = "Room?gameName=" + RoomName + "&playerName=" + SelfPlayerName + "&token=" + User.token;
            var ans = RestClient.MakeGetRequest(controller);
            var json = JObject.Parse(ans);
            var roomState = json.ToObject<RoomState>();
            if (roomState.Messege != null)
            {
                MessageBox.Show(roomState.Messege, "Cannot start game", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Leave_Click(object sender, RoutedEventArgs e)
        {
            var option = "leave";
            if (SelfPlayerName == null)
                option = "leaveSpectator";
            var controller = "Room?userName=" + Crypto.Encrypt(User.Username) + "&gameName=" + RoomName +
                             "&playerName=" + SelfPlayerName + "&option=" + option + "&token=" + User.token;
            var ans = RestClient.MakeGetRequest(controller);
            var json = JObject.Parse(ans);
            var roomState = json.ToObject<RoomState>();
            if (roomState.Messege == null)
            {
                _playing = false;
                Application.Current.MainWindow = Main;
                Close();
                Main.Show();
            }
            else
            {
                MessageBox.Show(roomState.Messege, "Cannot leave game", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_playing)
            {
                e.Cancel = true;
                MessageBox.Show("Cannot exit before leaving the game!", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            else
            {
                var controller = "User?userName=" + Crypto.Encrypt(User.Username);
                var ans = RestClient.MakePutRequest(controller, "");
                var json = JObject.Parse(ans);
                var data = json.ToObject<UserData>();
                if (data.Message == null)
                {
                    User.Chips = data.Chips;
                    User.Rank = data.Rank;
                }
            }
        }

        private void UpdateChat(RoomState state)
        {
            var tmpMsg = "";
            if (SelfPlayerName != null)
            {
                foreach (Player p in state.AllPlayers)
                {
                    if (p.PlayerName == SelfPlayerName)
                    {
                        foreach (string message in p.Messages)
                        {
                            tmpMsg = tmpMsg + message + "\n";
                        }
                    }
                }
            }
            else
            {
                foreach (UserData u in state.Spectators)
                {
                    if (u.Username == User.Username)
                    {
                        foreach (string message in u.Messages)
                        {
                            tmpMsg = tmpMsg + message + "\n";
                        }
                    }
                }
            }
            Msg = tmpMsg;
            ChatScroll.Dispatcher.Invoke(() => ChatScroll.ScrollToBottom());
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string msg = Message.Text;
            string nameToSend = SelfPlayerName;
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
                string status = "player";
                if (SelfPlayerName == null)
                    status = "spectator";
                if (ChatComboBox.SelectedIndex <= 0 || ChatComboBox.SelectedIndex >= ChatComboBoxContent.Count)
                {
                    controller = "Message?room=" + RoomName + "&sender=" + nameToSend +
                                 "&message=" + msg + "&status=" + status + "&token=" + User.token;
                }
                else
                {
                    controller = "Message?room=" + RoomName + "&sender=" + nameToSend +
                                 "&reciver=" + ChatComboBoxContent[ChatComboBox.SelectedIndex] + "&message=" + msg + "&status="+ status +"&token=" + User.token;
                }
                Message.Text = "";
                RestClient.MakeGetRequest(controller);
            }
        }

        private void Card_MouseEnter(object sender, MouseEventArgs e)
        {
            Image card = sender as Image;
            ZoomCard.Dispatcher.Invoke(() => ZoomCard.Source = card.Source);
        }

        private void Card_MouseLeave(object sender, MouseEventArgs e)
        {
            ZoomCard.Dispatcher.Invoke(() => ZoomCard.Source = new BitmapImage(new Uri(@"Resources/new_back.png", UriKind.Relative)));
        }
    }
}
