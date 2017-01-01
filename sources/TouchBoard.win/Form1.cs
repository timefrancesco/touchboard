using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsInput;
using WindowsInput.Native;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Interceptor;
using System.Threading;
using TouchBoardServerComms;

namespace TouchBoardWinServer
{
    public partial class Form1 : Form
    {

        ConnectivityServer _newServer;
        System.Windows.Forms.Timer _secondsTimer;
        int _discoverySeconds = 60;
        InputSimulator _inputemulated;
        Input _input;
        bool _useInterceptor = false;

        public Form1()
        {
            InitializeComponent();
            _newServer = new ConnectivityServer(false);
            _newServer.OnClientAskForConnection += _newServer_OnClientAskForConnection;
            _newServer.ClientConnected += _newServer_ClientConnected;
            _newServer.ClientDisconnected += _newServer_ClientDisconnected;
            StartServer();
            statusLbl.Text = "Server Started";
            _numClientsConn.Text = "0";
            _secondsTimer = new System.Windows.Forms.Timer();
            _inputemulated = new InputSimulator();

            _input = new Input();

            // Be sure to set your keyboard filter to be able to capture key presses and simulate key presses
            // KeyboardFilterMode.All captures all events; 'Down' only captures presses for non-special keys; 'Up' only captures releases for non-special keys; 'E0' and 'E1' capture presses/releases for special keys
            _input.KeyboardFilterMode = KeyboardFilterMode.All;
            // You can set a MouseFilterMode as well, but you don't need to set a MouseFilterMode to simulate mouse clicks

            // Finally, load the driver
            _input.Load();
        }

        void _newServer_ClientDisconnected()
        {

            Console.WriteLine("[FORM] Client Disconnected");
            this.Invoke((MethodInvoker)delegate
            {
                _numClientsConn.Text = _newServer.ConnectedClients().ToString();
            });
        }

        void _newServer_ClientConnected()
        {
            Console.WriteLine("[FORM] Client Connected");
            _newServer.StopBroadcast();
            _secondsTimer.Stop();

            this.Invoke((MethodInvoker)delegate
            {
                _numClientsConn.Text = _newServer.ConnectedClients().ToString();
                statusLbl.Text = "Server Started";
                DebugBtn1.Enabled = true;
            });

        }

        void _newServer_OnClientAskForConnection(xConnection conn)
        {
            Console.WriteLine("[FORM] Connection Request");

            this.Invoke((MethodInvoker)delegate
            {
                string address = conn.socket.RemoteEndPoint.ToString();
                address = address.Remove(address.IndexOf(':'));
                string msg = "Accept New Connection request from: " + address + " ?";

                DialogResult result1 = MessageBox.Show(msg, "Connection Request", MessageBoxButtons.YesNo);
                if (result1 == System.Windows.Forms.DialogResult.Yes)
                {
                    _newServer.SetAuth(true, conn);
                    ClientUtil.AddClient(new AuthClient { IPaddress = address, HostName = "temp" });
                }
                else
                    _newServer.SetAuth(false, conn);
            });
        }

        /// <summary>
        /// Discovery mode on
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DebugBtn1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("[FORM] Discovery Mode Enabled");

            DebugBtn1.Enabled = false;
            statusLbl.Text = "Discovery mode ON for 60 seconds";
            _newServer.OnDiscoveryTimeOut += _newServer_OnDiscoveryTimeOut;

            _secondsTimer.Interval = 1000;
            _discoverySeconds = 60;
            _secondsTimer.Tick += SecondsTimer_Tick;
            _secondsTimer.Start();

            new Task(() =>
            {
                _newServer.ReceiveBroadcast();
            }).Start();
        }

        //Shows the seconds going down when in discovery mode
        private void SecondsTimer_Tick(object sender, EventArgs e)
        {
            _discoverySeconds--;
            if (_discoverySeconds < 0)
                _discoverySeconds = 0;

            statusLbl.Text = "Discovery mode ON for " + _discoverySeconds.ToString() + " seconds";
        }


        void _newServer_OnDiscoveryTimeOut()
        {
            Console.WriteLine("[FORM] Discovery Timeout");

            _secondsTimer.Tick -= SecondsTimer_Tick;
            _secondsTimer.Stop();

            _newServer.OnDiscoveryTimeOut -= _newServer_OnDiscoveryTimeOut;

            this.Invoke((MethodInvoker)delegate
            {
                statusLbl.Text = "Server Started";
                DebugBtn1.Enabled = true;
            });
        }

        void StartServer()
        {
            System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.High;
            Console.WriteLine("[FORM] Starting Server");
            _newServer.OnMessageReceived += HandleOnMessageReceived;
            _newServer.StartServer();
        }

        void HandleOnMessageReceived(string message)
        {
            Console.WriteLine("[FORM] Message Received = " + message);

            var commands = JsonConvert.DeserializeObject<List<int>>(message);
            if (commands != null)
            {
                if (_useInterceptor)
                    SendKeyPresses(commands);
                else
                    SendEmulatedKeyPresses(commands);
            }
        }

        //method called by the server object when it has received the information
        //from the client
        public void ReceivedDataFromClient()
        {


        }

        private void ViewAuthClientsBtn_Click(object sender, EventArgs e)
        {
            var clients = new AuthClientsList();
            clients.ShowDialog();
        }

        private void SendEmulatedKeyPresses(List<int> commands)
        {
            List<VirtualKeyCode> keydownList = new List<VirtualKeyCode>();
            foreach (var singlecom in commands)
            {
                VirtualKeyCode newCmd = (VirtualKeyCode)singlecom;
                if (newCmd == VirtualKeyCode.LCONTROL || newCmd == VirtualKeyCode.RCONTROL || newCmd == VirtualKeyCode.RMENU || newCmd == VirtualKeyCode.LMENU || newCmd == VirtualKeyCode.RSHIFT || newCmd == VirtualKeyCode.LSHIFT)
                {
                    keydownList.Add(newCmd);
                    _inputemulated.Keyboard.KeyDown(newCmd);
                }
                else
                {
                    _inputemulated.Keyboard.KeyPress(newCmd);
                }
            }

            foreach (var cmd in keydownList)
            {
                _inputemulated.Keyboard.KeyUp(cmd);
            }
        }

        private void SendKeyPresses(List<int> commands)
        {

            List<int> newKeys = new List<int>();
            foreach (var singlecom in commands)
            {
                VirtualKeyCode newCmd = (VirtualKeyCode)singlecom;
                var x = Constants.InterceptorWrapper[newCmd];
                newKeys.Add(x);
            }


            List<Interceptor.Keys> keysDown = new List<Interceptor.Keys>(); //used if there is a ctrl, alt or shift
            foreach (var cmd in newKeys)
            {
                Interceptor.Keys newCmd = (Interceptor.Keys)cmd;
                //these are ctrl, alt and shift (left and right)
                if (newCmd == Interceptor.Keys.Control || newCmd == Interceptor.Keys.RightAlt || newCmd == Interceptor.Keys.RightAlt || newCmd == Interceptor.Keys.LeftShift || newCmd == Interceptor.Keys.RightShift)
                {
                    Console.WriteLine("[FORM] keydown = " + newCmd.ToString());
                    _input.ClickDelay = 20;
                    _input.SendKey(newCmd, KeyState.Down);

                    keysDown.Add(newCmd);
                }
                else
                {
                    Console.WriteLine("[FORM] keypress = " + newCmd.ToString());
                    _input.ClickDelay = 20;
                    _input.SendKeys(newCmd);
                }

            }

            if (keysDown.Count > 0)
            {
                foreach (var key in keysDown)
                {
                    Console.WriteLine("[FORM] Keyup = " + key.ToString());
                    _input.ClickDelay = 20;
                    _input.SendKey(key, KeyState.Up);
                }
            }
        }

        private void InterceptorHelpLabel_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/xeo-it/touchboard");
        }

        private void InterceptorDriverChkBox_CheckedChanged(object sender, EventArgs e)
        {
            _useInterceptor = !_useInterceptor;
        }
    }
}
