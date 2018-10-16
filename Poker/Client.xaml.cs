using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Poker
{
    /// <summary>
    /// Interaction logic for Client.xaml
    /// </summary>
    public partial class Client : Page
    {
        private string user;     
        public Client(string user)
        {
            InitializeComponent();
            this.user = user;
        }

        NetComm.Client client; //The client object used for the communication

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            client = new NetComm.Client(); //Initialize the client object

            //Adding event handling methods for the client
            client.Connected += new NetComm.Client.ConnectedEventHandler(client_Connected);
            client.Disconnected += new NetComm.Client.DisconnectedEventHandler(client_Disconnected);
            client.DataReceived += new NetComm.Client.DataReceivedEventHandler(client_DataReceived);

            //Connecting to the host
            client.Connect("localhost", 2020, user); //Connecting to the host (on the same machine) with port 2020 and ID "Jack"
        }

        private void Grid_Unloaded(object sender, RoutedEventArgs e)
        {
            if (client.isConnected) client.Disconnect(); //Disconnects if the client is connected, closing the communication thread
        }

        void client_DataReceived(byte[] Data, string ID)
        {
            Log.Text += ID + ": " + ConvertBytesToString(Data) + Environment.NewLine; //Updates the log with the current connection state
            SendBtn.Click -= SendBtn_Click;
        }

        void client_Disconnected()
        {
            Log.Text += "Disconnected from host!" + Environment.NewLine; //Updates the log with the current connection state
        }

        void client_Connected()
        {
            Log.Text += "Connected succesfully!" + Environment.NewLine; //Updates the log with the current connection state
        }
        private void SendBtn_Click(object sender, RoutedEventArgs e)
        {
            client.SendData(ConvertStringToBytes(ChatMessage.Text)); //Sends the message to the host
            ChatMessage.Clear(); //Clears the chatmessage textbox text
        }

        string ConvertBytesToString(byte[] bytes)
        {
            return ASCIIEncoding.ASCII.GetString(bytes);
        }

        byte[] ConvertStringToBytes(string str)
        {
            return ASCIIEncoding.ASCII.GetBytes(str);
        }

    }
}
