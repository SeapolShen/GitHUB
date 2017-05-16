using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Management;
using System.Management.Instrumentation;
using System.Threading;

namespace CyBLE_MTK_Application
{
    public partial class SerialPortSettingsDialog : Form
    {
        private const string _connectString = "Co&nnect";
        private const string _disconnectString = "Disco&nnect";
        private CyBLEMTKSerialPort _mTKSerialPort;
        private List<COMPortInfo> _ComPortInfoList;

        private bool _closeOnConnect;
        public bool CloseOnConnect
        {
            get { return _closeOnConnect; }
            set { _closeOnConnect = value; }
        }
        //private LogManager Log;
        //private string CommandResult, PrevDUTConnStatus, PrevHostConnStatus, CurHostConnStatus, CurDUTConnStatus;
        //private bool ContinuePoll;
        //private int PollInterval;
        //private Thread DevicePollThread;
        //private bool ConnectionTime;

        //public event ConnectionEventHandler OnDUTConnectionStatusChange;
        //public event ConnectionEventHandler OnHostConnectionStatusChange;

        //public delegate void ConnectionEventHandler(string ConnectionStatus);

        //public PortType SerialPortType;
        //public bool CheckDUTPresence, AutoVerifyON;

        //public SerialPort DeviceSerialPort
        //{
        //    get { return _DeviceSerialPort; }
        //    set 
        //    { 
        //        _DeviceSerialPort = value;
        //        SerialPortCombo.Enabled = true;
        //        RefreshButton.Enabled = true;
        //        ConnectButton.Text = "Co&nnect";
        //    }
        //}

        public SerialPortSettingsDialog()
        {
            InitializeComponent();
            _mTKSerialPort = new CyBLEMTKSerialPort();
            CloseOnConnect = false;
            //Log = new LogManager();
            //SerialPortType = PortType.NoType;
            //CheckDUTPresence = false;
            //AddPorts();
            //CurHostConnStatus = "";
            //CurDUTConnStatus = "";
            //PrevHostConnStatus = "";
            //PrevDUTConnStatus = "";
            //ContinuePoll = false;
            //PollInterval = 1000;
            //AutoVerifyON = false;
            //ConnectionTime = false;
        }

        public SerialPortSettingsDialog(CyBLEMTKSerialPort SerialPort)
            : this()
        {
            _mTKSerialPort = SerialPort;
        }

        private void AddPorts()
        {
            _ComPortInfoList = _mTKSerialPort.ComPortList;
            Graphics ComboGraphics = SerialPortCombo.CreateGraphics();
            Font ComboFont = SerialPortCombo.Font;
            int MaxWidth = 0;
            foreach (COMPortInfo ComPort in _ComPortInfoList)
            {
                string s = ComPort.Name + " - " + ComPort.Description;
                SerialPortCombo.Items.Add(s);
                int VertScrollBarWidth = (SerialPortCombo.Items.Count > SerialPortCombo.MaxDropDownItems) ? SystemInformation.VerticalScrollBarWidth : 0;
                int DropDownWidth = (int)ComboGraphics.MeasureString(s, ComboFont).Width + VertScrollBarWidth;
                if (MaxWidth < DropDownWidth)
                {
                    SerialPortCombo.DropDownWidth = DropDownWidth;
                    MaxWidth = DropDownWidth;
                }
            }
            if (SerialPortCombo.Items.Count > 0)
            {
                SerialPortCombo.SelectedIndex = 0;
            }
        }

        private void RefreshPortList()
        {
            SerialPortCombo.Items.Clear();
            AddPorts();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            RefreshPortList();
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            ConnectButton.Enabled = false;
            if (ConnectButton.Text == _connectString)
            {
                if (SerialPortCombo.SelectedIndex >= 0)
                {
                    try
                    {
                        if (_mTKSerialPort.OpenPort(_ComPortInfoList[SerialPortCombo.SelectedIndex]) != OpenPortStatus.Open)
                        {
                            MessageBox.Show("Unable to open " + _ComPortInfoList[SerialPortCombo.SelectedIndex].Name, "Error", MessageBoxButtons.OK);
                        }
                        else
                        {
                            ConnectButton.Text = _disconnectString;
                            SerialPortCombo.Enabled = false;
                            if (_closeOnConnect)
                            {
                                this.Close();
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                try
                {
                    _mTKSerialPort.ClosePort();
                }
                catch
                {
                }
                ConnectButton.Text = _connectString;
                SerialPortCombo.Enabled = true;
            }
            ConnectButton.Enabled = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (_mTKSerialPort.DeviceSerialPort.IsOpen == true)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }
            base.OnClosing(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            AddPorts();

            if (_mTKSerialPort.DeviceSerialPort.IsOpen)
            {
                bool isPresent = false;

                for (int i = 0; i < _ComPortInfoList.Count; i++)
                {
                    if (_ComPortInfoList[i].Name == _mTKSerialPort.DeviceSerialPort.PortName)
                    {
                        SerialPortCombo.SelectedIndex = i;
                        isPresent = true;
                        break;
                    }
                }

                if (isPresent)
                {
                    ConnectButton.Text = _disconnectString;
                    SerialPortCombo.Enabled = false;
                }
                else
                {
                    try
                    {
                        _mTKSerialPort.ClosePort();
                    }
                    catch
                    {
                    }
                }
            }

            base.OnLoad(e);
        }
    }
}
