using System.Windows;
using System.Windows.Media;

namespace Poker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void clientBtn_Click(object sender, RoutedEventArgs e)
        {
            if(idTbx.Text != "")
            {
                Main.Content = new Client(idTbx.Text);
            }
        }

        private void hostBtn_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Host();
        }
    }
}
