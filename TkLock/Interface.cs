using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

using gma.System.Windows;



namespace TkLock
{

    

    public partial class Interface : Form
    {

        public string Password;
        public int CurrentCharPos = 0;
        public bool BruteForceBreak = false;
        public int WrongTries = 0;
        public int WrongTriesLimit = 20;
        public String XmlFileName = "config.xml";

        XmlDocument XmlFile;

        public void CreateConfigFileIfNotExists()
        {
            XmlFile = new XmlDocument();
            if (!File.Exists(XmlFileName))
            {
                XmlNode XmlTmpRoot, XmlTmpNode;
                XmlTmpRoot = XmlFile.CreateElement("TkLock");


                XmlTmpNode = XmlFile.CreateElement("Password");
                XmlTmpNode.InnerText = "TkLock";
                XmlTmpRoot.AppendChild(XmlTmpNode);
                XmlTmpNode = XmlFile.CreateElement("Text1");
                XmlTmpNode.InnerText = "Hör auf zu Probieren! Kleine Pause.";
                XmlTmpRoot.AppendChild(XmlTmpNode);
                XmlTmpNode = XmlFile.CreateElement("Text2");
                XmlTmpNode.InnerText = "Richtig! Du darfst rein!";
                XmlTmpRoot.AppendChild(XmlTmpNode);
                XmlTmpNode = XmlFile.CreateElement("Text3");
                XmlTmpNode.InnerText = "Schlecht...";
                XmlTmpRoot.AppendChild(XmlTmpNode);
                XmlTmpNode = XmlFile.CreateElement("Text4");
                XmlTmpNode.InnerText = "Ja, weiter!";
                XmlTmpRoot.AppendChild(XmlTmpNode);
                XmlTmpNode = XmlFile.CreateElement("Text5");
                XmlTmpNode.InnerText = "Ey! So einfach kommst du hier nicht rein!";
                XmlTmpRoot.AppendChild(XmlTmpNode);

                XmlFile.AppendChild(XmlTmpRoot);

                XmlFile.Save(XmlFileName);




            }
            else
            {
                XmlFile.Load(XmlFileName);
            }
        }

        public Interface()
        {

            InitializeComponent();
            CreateConfigFileIfNotExists();
            SetProgramSize();
            SetElementPositions();


            


            label1.Text = XmlFile.SelectSingleNode("/TkLock/Text5").InnerText;
            Password = XmlFile.SelectSingleNode("/TkLock/Password").InnerText;
            progressBar1.Maximum = Password.Length;
        }



        public void CheckEntry(string Entry)
        {
            timer1.Enabled = false;
            if (WrongTries == WrongTriesLimit)
            {
                timer2.Enabled = true;
                CurrentCharPos = 0;
                progressBar1.Value = 0;
                WrongTries = 0;
                SetText(XmlFile.SelectSingleNode("/TkLock/Text1").InnerText);
                BruteForceBreak = true;
            }
            if (!BruteForceBreak)
            {
                if (CurrentCharPos < Password.Length && Password.Substring(CurrentCharPos, 1) == Entry)
                {
                    CurrentCharPos++;
                    progressBar1.Value = CurrentCharPos;
                    SetText(XmlFile.SelectSingleNode("/TkLock/Text4").InnerText);
                }
                else
                {
                    timer1.Enabled = true;
                    CurrentCharPos = 0;
                    progressBar1.Value = 0;
                    WrongTries++;
                    SetText(XmlFile.SelectSingleNode("/TkLock/Text3").InnerText);
                }
                if (CurrentCharPos == Password.Length)
                {
                    SetText(XmlFile.SelectSingleNode("/TkLock/Text2").InnerText);
                    timer3.Enabled = true;
                }
            }
        }


        public void SetElementPositions()
        {
            pictureBox1.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - pictureBox1.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - pictureBox1.Height - progressBar1.Height);
            progressBar1.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 - progressBar1.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - progressBar1.Height + 50);
            pictureBox2.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 + pictureBox1.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2 - pictureBox1.Height - pictureBox2.Height - progressBar1.Height);
            label1.Location = new Point(Screen.PrimaryScreen.Bounds.Width / 2 + pictureBox1.Width / 2 + 85, Screen.PrimaryScreen.Bounds.Height / 2 - pictureBox1.Height - pictureBox2.Height - progressBar1.Height + 15);
        }
        public void SetProgramSize()
        {
            Size = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
        
        }
        public void SetText(string Text)
        {
            label1.Text = Text;
        }




        private void timer1_Tick(object sender, EventArgs e)
        {
            SetText(XmlFile.SelectSingleNode("/TkLock/Text5").InnerText);
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            BruteForceBreak = false;
            SetText(XmlFile.SelectSingleNode("/TkLock/Text5").InnerText);
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            this.Close();
        }


        UserActivityHook actHook;

        private void Interface_Load(object sender, EventArgs e)
        {
            
            actHook = new UserActivityHook();
            actHook.OnMouseActivity += new MouseEventHandler(MouseMoved);
            actHook.KeyDown += new KeyEventHandler(MyKeyDown);
            actHook.KeyPress += new KeyPressEventHandler(MyKeyPress);
            actHook.KeyUp += new KeyEventHandler(MyKeyUp);
        }
        public void MyKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData.ToString() != "LShiftKey")
            {
                e.Handled = true;
            }
        }
        public void MyKeyPress(object sender, KeyPressEventArgs e)
        {
            
            CheckEntry(e.KeyChar.ToString());
            e.Handled = true;

        }
        public void MyKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData.ToString() != "LShiftKey")
            {
                  e.Handled = true;
            }
        }
        public void MouseMoved(object sender, MouseEventArgs e)
        {

        }

    }


}
