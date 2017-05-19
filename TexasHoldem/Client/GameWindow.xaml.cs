using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using Client.Data;
using Newtonsoft.Json.Linq;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Media;
using Image = System.Windows.Controls.Image;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow
    {
        public string RoomName;
        public string SelfPlayerName;
        public Dictionary<string,int> PlayerMap;
        public int CountPlayers;
        public List<string> ChatComboBoxContent;
        public Label[] NameLabels;
        public Label[] ChipLabels;
        public Label[] BetLabels;
        public Image[] CommunityCards;
        public Rectangle[] TurnSymbol;
        public bool Creator;

        public GameWindow(string self, RoomState state, bool creator)
        {
            InitializeComponent();
            SelfPlayerName = self;
            CountPlayers = 1;
            RoomName = state.RoomName;
            Creator = creator;
            RoomNameLbl.Content = RoomName;
            NameLabels = new[]{P1Lbl, P2Lbl, P3Lbl, P4Lbl, P5Lbl, P6Lbl, P7Lbl, P8Lbl, P9Lbl};
            ChipLabels = new[]{C1Lbl, C2Lbl, C3Lbl, C4Lbl, C5Lbl, C6Lbl, C7Lbl, C8Lbl, C9Lbl};
            BetLabels = new[] { Bet1, Bet2, Bet3, Bet4, Bet5, Bet6, Bet7, Bet8, Bet9 };
            CommunityCards = new[] { Com1, Com2, Com3, Com4, Com5 };
            TurnSymbol = new[] { RecP1, RecP2, RecP3, RecP4, RecP5, RecP6, RecP7, RecP8, RecP9 };
            PlayerMap = new Dictionary<string, int>();
            ChatComboBoxContent = new List<string> {"ALL"};
            PlayerMap.Add(SelfPlayerName, CountPlayers);
            UpdateRoom(state);
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CurrentBet_Label != null)
            {
                CurrentBet_Label.Content = (int)e.NewValue;
            }
        }

        private void UpdateRoom(RoomState state)
        {
            if (state.IsOn == false && Creator)
            {
                Start.Dispatcher.Invoke(() => Start.Visibility = Visibility.Visible);
            }
            else
            {
                Start.Dispatcher.Invoke(() => Start.Visibility = Visibility.Hidden);
            }
            foreach (var p  in state.AllPlayers)
            {
                if (!PlayerMap.ContainsKey(p.PlayerName))
                {
                    CountPlayers++;
                    PlayerMap.Add(p.PlayerName, CountPlayers);
                    ChatComboBoxContent.Add(p.PlayerName);
                }
                PlayerMap.TryGetValue(p.PlayerName, out int playerVal);
                if (p.PlayerName == state.CurrentPlayer && state.IsOn)
                {
                    foreach (Rectangle r in TurnSymbol)
                    {
                        r.Dispatcher.Invoke(() => r.Fill = System.Windows.Media.Brushes.White);
                    }
                    TurnSymbol[playerVal - 1].Dispatcher.Invoke(()=>TurnSymbol[playerVal - 1].Fill = System.Windows.Media.Brushes.Red);
                }
                if (playerVal != -1)
                {
                    UpdatePlayer(playerVal, p);
                }
            }
            UpdateCommunityCards(state.CommunityCards);
            ChatComboBox.Dispatcher.Invoke(()=> ChatComboBox.ItemsSource = ChatComboBoxContent);
            ChatComboBox.Dispatcher.Invoke(() => ChatComboBox.Items.Refresh());

            if (SelfPlayerName == state.CurrentPlayer && state.IsOn)
            {
                BetSlide.Dispatcher.Invoke(() => BetSlide.IsEnabled = true);
                BetSlide.Dispatcher.Invoke(() => BetSlide.Value = 1);
                CurrentBet_Label.Dispatcher.Invoke(() => CurrentBet_Label.Content = 1);
                CurrentBet_Label.Dispatcher.Invoke(() => CurrentBet_Label.Visibility = Visibility.Visible);
                Bet.Dispatcher.Invoke(() => Bet.IsEnabled = true);
                Call.Dispatcher.Invoke(() => Call.IsEnabled = true);
                Fold.Dispatcher.Invoke(() => Fold.IsEnabled = true);
            }
            else
            {
                BetSlide.Dispatcher.Invoke(() => BetSlide.IsEnabled = false);
                CurrentBet_Label.Dispatcher.Invoke(() => CurrentBet_Label.Visibility = Visibility.Hidden);
                Bet.Dispatcher.Invoke(() => Bet.IsEnabled = false);
                Call.Dispatcher.Invoke(() => Call.IsEnabled = false);
                Fold.Dispatcher.Invoke(() => Fold.IsEnabled = false);
            }

            if ((state.IsOn == false && !Creator) || (state.IsOn && state.CurrentPlayer != SelfPlayerName))
            {
                System.Threading.Timer timer = null;
            timer = new System.Threading.Timer((obj) =>
                {
                    StatusRequest();
                    timer.Dispose();
                },
                null, 2000, System.Threading.Timeout.Infinite);
            }
        }

        private void StatusRequest()
        {
            var controller = "Room?gameName=" + RoomName + "&playerName=" + SelfPlayerName;
            var ans = RestClient.MakePutRequest(controller, "");
            var json = JObject.Parse(ans);
            var roomState = json.ToObject<RoomState>();
            if (roomState.Messege == null)
            {
                UpdateRoom(roomState);
            }
            else
            {
                MessageBox.Show(roomState.Messege, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdatePlayer(int i, Player p)
        {
            NameLabels[i - 1].Dispatcher.Invoke(()=> NameLabels[i - 1].Content = p.PlayerName);
            ChipLabels[i - 1].Dispatcher.Invoke(()=> ChipLabels[i - 1].Content = p.ChipsAmount);
            BetLabels[i - 1].Dispatcher.Invoke(()=> BetLabels[i - 1].Content = p.CurrentBet);
            if (p.PlayerName == SelfPlayerName)
            {
                UpdateSelfCards(p.PlayerHand);
                BetSlide.Dispatcher.Invoke(()=> BetSlide.Maximum = p.ChipsAmount);
            }
        }

        private void UpdateSelfCards(string[] hand)
        {
            if (hand?[0] != null && hand[1]!=null)
            {
                P1Card1.Dispatcher.Invoke(()=> P1Card1.Source = new BitmapImage(new Uri(@"Resources/_" + hand[0] + ".png", UriKind.Relative)));
                P1Card2.Dispatcher.Invoke(()=> P1Card2.Source = new BitmapImage(new Uri(@"Resources/_" + hand[1] + ".png", UriKind.Relative)));
            }
        }

        private void UpdateCommunityCards(string[] cards)
        {
            if (cards == null || cards.Length == 0)
                return;
            for(int i=0; i<cards.Length;i++)
            {
                if (cards[i] != null)
                    CommunityCards[i].Dispatcher.Invoke(()=>  CommunityCards[i].Source = new BitmapImage(new Uri(@"Resources/_" + cards[i] + ".png", UriKind.Relative)));
            }
        }

        private void Bet_Click(object sender, RoutedEventArgs e)
        {
            var controller = "Room?gameName=" + RoomName + "&playerName=" + SelfPlayerName +
                                "&bet=" + (int)BetSlide.Value;
            var ans = RestClient.MakeGetRequest(controller);
            var json = JObject.Parse(ans);
            var roomState = json.ToObject<RoomState>();
            if (roomState.Messege == null)
            {
                UpdateRoom(roomState);
            }
            else
            {
                MessageBox.Show(roomState.Messege, "Error in bet", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Call_Click(object sender, RoutedEventArgs e)
        {
            var controller = "Room?gameName=" + RoomName + "&playerName=" + SelfPlayerName + "&bet=call";
            var ans = RestClient.MakeGetRequest(controller);
            var json = JObject.Parse(ans);
            var roomState = json.ToObject<RoomState>();
            if (roomState.Messege == null)
            {
                UpdateRoom(roomState);
            }
            else
            {
                MessageBox.Show(roomState.Messege, "Error in call", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Fold_Click(object sender, RoutedEventArgs e)
        {
            var controller = "Room?gameName=" + RoomName + "&playerName=" + SelfPlayerName + "&bet=fold";
            var ans = RestClient.MakeGetRequest(controller);
            var json = JObject.Parse(ans);
            var roomState = json.ToObject<RoomState>();
            if (roomState.Messege == null)
            {
                UpdateRoom(roomState);
            }
            else
            {
                MessageBox.Show(roomState.Messege, "Error in fold", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var controller = "Room?gameName=" + RoomName + "&playerName=" + SelfPlayerName;
            var ans = RestClient.MakeGetRequest(controller);
            var json = JObject.Parse(ans);
            var roomState = json.ToObject<RoomState>();
            if (roomState.Messege == null)
            {
                Start.Dispatcher.Invoke(() => Start.Visibility = Visibility.Hidden);
                UpdateRoom(roomState);
            }
            else
            {
                MessageBox.Show(roomState.Messege, "Cannot start game", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
