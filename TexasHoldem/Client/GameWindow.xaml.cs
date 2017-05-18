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
using Client.Data;
using Newtonsoft.Json.Linq;
using System.Timers;

namespace Client
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public string RoomName;
        public string SelfPlayerName;
        public Dictionary<string,int> PlayerMap;
        public int CountPlayers;
        public List<string> ChatComboBoxContent;
        public Label[] NameLabels;
        public Label[] ChipLabels;
        public Label[] BetLabels;
        public bool Creator;

        public GameWindow(string self, RoomState state, bool creator)
        {
            InitializeComponent();
            this.SelfPlayerName = self;
            this.CountPlayers = 1;
            this.RoomName = state.RoomName;
            this.Creator = creator;
            RoomNameLbl.Content = this.RoomName;
            NameLabels = new Label[9]{P1Lbl, P2Lbl, P3Lbl, P4Lbl, P5Lbl, P6Lbl, P7Lbl, P8Lbl, P9Lbl};
            ChipLabels = new Label[9]{C1Lbl, C2Lbl, C3Lbl, C4Lbl, C5Lbl, C6Lbl, C7Lbl, C8Lbl, C9Lbl};
            BetLabels = new Label[9] { Bet1, Bet2, Bet3, Bet4, Bet5, Bet6, Bet7, Bet8, Bet9 };
            PlayerMap = new Dictionary<string, int>();
            ChatComboBoxContent = new List<string>();
            ChatComboBoxContent.Add("ALL");
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
            foreach (Player p  in state.AllPlayers)
            {
                if (state.IsOn == false && this.Creator)
                {
                    Start.Visibility = Visibility.Visible;
                }
                else
                {
                    Start.Visibility = Visibility.Hidden;
                }
                if (!PlayerMap.ContainsKey(p.PlayerName))
                {
                    CountPlayers++;
                    PlayerMap.Add(p.PlayerName, CountPlayers);
                    ChatComboBoxContent.Add(p.PlayerName);
                }
                int playerVal = - 1;
                PlayerMap.TryGetValue(p.PlayerName, out playerVal);
                if (playerVal != -1)
                {
                    UpdatePlayer(playerVal, p);
                }
            }
            UpdateCommunityCards(state.CommunityCards);
            ChatComboBox.ItemsSource = ChatComboBoxContent;
            ChatComboBox.Items.Refresh();

            System.Threading.Timer timer = null;
            timer = new System.Threading.Timer((obj) =>
                {
                    StatusRequest();
                    timer.Dispose();
                },
                null, 1000, System.Threading.Timeout.Infinite);
        }

        private void StatusRequest()
        {
            string controller = "Room?gameName=" + this.RoomName + "&playerName=" + this.SelfPlayerName;
            string ans = RestClient.MakePutRequest(controller);
            MessageBox.Show(ans, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            JObject json = JObject.Parse(ans);
            RoomState roomState = json.ToObject<RoomState>();
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
            NameLabels[i - 1].Content = p.PlayerName;
            ChipLabels[i - 1].Content = p.ChipsAmount;
            BetLabels[i - 1].Content = p.CurrentBet;
            if (p.PlayerName == SelfPlayerName)
            {
                UpdateSelfCards(p.PlayerHand);
                BetSlide.Maximum = p.ChipsAmount;
            }
        }

        private void UpdateSelfCards(string[] hand)
        {
            if (hand != null && hand[0]!=null && hand[1]!=null)
            {
                P1Card1.Source = new BitmapImage(new Uri(@"Resources/_" + hand[0] + ".png"));
                P1Card2.Source = new BitmapImage(new Uri(@"Resources/_" + hand[1] + ".png"));
            }
        }

        private void UpdateCommunityCards(string[] cards)
        {
            if (cards == null || cards.Length == 0)
                return;
            foreach (string s in cards)
            {
                if (s != null)
                    Com1.Source = new BitmapImage(new Uri(@"Resources/_" + s + ".png", UriKind.Relative));
            }
        }

        private void Bet_Click(object sender, RoutedEventArgs e)
        {
            string controller = "Room?gameName=" + this.RoomName + "&player_name=" + this.SelfPlayerName +
                                "&bet=" + (int)CurrentBet_Label.Content;
            string ans = RestClient.MakeGetRequest(controller);
            JObject json = JObject.Parse(ans);
            RoomState roomState = json.ToObject<RoomState>();
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
            string controller = "Room?gameName=" + this.RoomName + "&playerName=" + this.SelfPlayerName +
                                "&bet=call";
            string ans = RestClient.MakeGetRequest(controller);
            JObject json = JObject.Parse(ans);
            RoomState roomState = json.ToObject<RoomState>();
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
            string controller = "Room?gameName=" + this.RoomName + "&playerName=" + this.SelfPlayerName +
                                "&bet=fold";
            string ans = RestClient.MakeGetRequest(controller);
            JObject json = JObject.Parse(ans);
            RoomState roomState = json.ToObject<RoomState>();
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
            string controller = "Room?gameName=" + this.RoomName + "&playerName=" + this.SelfPlayerName;
            string ans = RestClient.MakeGetRequest(controller);
            JObject json = JObject.Parse(ans);
            RoomState roomState = json.ToObject<RoomState>();
            if (roomState.Messege == null)
            {
                Start.Visibility = Visibility.Hidden;
                UpdateRoom(roomState);
            }
            else
            {
                MessageBox.Show(roomState.Messege, "Cannot start game", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
