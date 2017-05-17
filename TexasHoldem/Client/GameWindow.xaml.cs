using System.Windows;
using Client.Data;

namespace Client
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow
    {
        private RoomState _state;

        public GameWindow(RoomState state)
        {
            InitializeComponent();
            _state = state;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CurrentBet_Label != null)
            {
                CurrentBet_Label.Content = (int)e.NewValue;
            }
        }
    }
}
