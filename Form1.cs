// Developer : Jugal Nathani
// Description : It's an Tool which captures users System Active & Away Time.
// Last Modified : 03-11-2019

using Microsoft.Win32;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;


namespace Live
{
    public partial class Form1 : Form
    {
        Stopwatch stopwatch = new Stopwatch();
        Stopwatch stopwatch_away = new Stopwatch();

        private SessionSwitchEventHandler sseh;

        public Form1()
        {
            InitializeComponent();

            Rectangle workingArea = Screen.GetWorkingArea(this);
            this.Location = new Point(workingArea.Right - Size.Width,
                                      workingArea.Bottom - Size.Height);

            //Display Date
            label6.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");

            //Display Username
            label9.Text = Environment.UserName.ToString().ToUpper();

            //Display In-Time
            label10.Text = DateTime.Now.ToString("hh:mm tt");


            sseh = new SessionSwitchEventHandler(SysEventsCheck);
            SystemEvents.SessionSwitch += sseh;

            try
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                string ab = rk.GetValue(Application.ProductName).ToString();

                if (ab != null)
                {
                    toolStripMenuItem1.Checked = true;
                }
                else
                {
                    toolStripMenuItem1.Checked = true;
                }
            }
            catch
            {
            }
            stopwatch.Start();
            timer1.Start();
        }

        void SysEventsCheck(object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                case SessionSwitchReason.SessionLock:
                    stopwatch_away.Start();
                    stopwatch.Stop();
                    timer1.Stop();
                    timer2.Start();
                    break;

                case SessionSwitchReason.SessionUnlock:
                    stopwatch_away.Stop();
                    stopwatch.Start();
                    timer2.Stop();
                    timer1.Start();
                    break;
            }
        }

        //Timer1 display the active time
        private void timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = stopwatch.Elapsed;
            String abc = ts.Hours + " : " + ts.Minutes + " : " + ts.Seconds;
            label1.Text = abc;
            timer2.Stop();
        }

        //Timer2 display the away time
        private void timer2_Tick(object sender, EventArgs e)
        {
            TimeSpan ts = stopwatch_away.Elapsed;
            String abc = ts.Hours + " : " + ts.Minutes + " : " + ts.Seconds;
            label4.Text = abc;
        }

        private void toolStripMenuItem1_CheckedChanged(object sender, EventArgs e)
        {
            //settings onboot setting in registry.
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (toolStripMenuItem1.Checked == true)
            {
                rk.SetValue(Application.ProductName, Application.ExecutablePath.ToString());
                Properties.Settings.Default.setboot = true;
            }
            else
            {
                rk.DeleteValue(Application.ProductName, false);
                Properties.Settings.Default.setboot = false;
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen redPen = new Pen(Color.Black, 1);
            int X1 = 0, Y1 = 67, X2 = 200, Y2 = 67;
            e.Graphics.DrawLine(redPen, X1, Y1, X2, Y2);

            Pen redPen1 = new Pen(Color.Black, 1);
            int X3 = 90, Y3 = 68, X4 = 90, Y4 = 130;
            e.Graphics.DrawLine(redPen1, X3, Y3, X4, Y4);

            e.Dispose();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
