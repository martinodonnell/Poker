using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Poker
{
    /// <summary>
    /// Interaction logic for Host.xaml
    /// </summary>
    public partial class Host : Page
    {

        int minNumPeople = 2;
        const int waitingForPeopleCon = 0;
        const int everyoneConnected = 1;
        const int sendingQuestion = 2;
        


        private NetComm.Host Server; //Creates the host variable object

        Dictionary<string, byte[]> userAnswers = new Dictionary<string, byte[]>();


        public Host()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, EventArgs e)
        {

            
            Log.Text = "Server set up. Waiting for everyone to connect" + Environment.NewLine;
            Server = new NetComm.Host(2020); //Initialize the Server object, connection will use the 2020 port number
            Server.StartConnection(); //Starts listening for incoming clients

            //Adding event handling methods, to handle the server messages
            Server.onConnection += new NetComm.Host.onConnectionEventHandler(Server_onConnection);
            Server.lostConnection += new NetComm.Host.lostConnectionEventHandler(Server_lostConnection);
            Server.DataReceived += new NetComm.Host.DataReceivedEventHandler(Server_DataReceived);
        }

        private void Grid_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Server.CloseConnection(); //Closes all of the opened connections and stops listening
        }

        void Server_DataReceived(string ID, byte[] Data)
        {
            Log.Text += (ID + ": " + ConvertBytesToString(Data) + Environment.NewLine); //Updates the log when a new message arrived, converting the Data bytes to a string
            userAnswers.Add(ID, Data);
            Server.SendData(ID, ConvertStringToBytes("Answer Recieved")); 	//Jack is the ID of the client 
        }

        void Server_lostConnection(string id)
        {
            //if (Log.IsDisposed) return; //Fixes the invoke error
            Log.Text += (id + " disconnected" + Environment.NewLine); //Updates the log textbox when user leaves the room
        }
        void Server_onConnection(string id)
        {
            Log.Text += (id + " connected!" + Environment.NewLine); //Updates the log textbox when new user joined
            int numPeopleCon = Server.Users.Count;
            if (minNumPeople == Server.Users.Count)
            {
                string message = everyoneConnected + ":";
                Log.Text += (message + Environment.NewLine);
                Server.Brodcast(ConvertStringToBytes(message));

                message = sendingQuestion + ":What number is my name";
                Log.Text += (message + Environment.NewLine); 
                Server.Brodcast(ConvertStringToBytes(message));
            }
            else
            {
                string message = waitingForPeopleCon + ":Waiting for " + (minNumPeople-numPeopleCon) +" to join";
                Log.Text += (message + Environment.NewLine);
                Server.Brodcast(ConvertStringToBytes(message));
            }
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
