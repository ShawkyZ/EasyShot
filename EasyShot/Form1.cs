using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace EasyShot
{
    public partial class Form1 : Form
    {
        private RegisterHotKeyClass _RegisKey = new RegisterHotKeyClass();
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();
        public Form1()
        {
            InitializeComponent();
            cbExtension.SelectedIndex = 0;
        }
        private void checkIfKeyPressed()
        {
            Bitmap image = Screenshot();
            switch (cbExtension.SelectedIndex)
            {
                case 0:
                    image.Save(label1.Text+"\\ScreenShot "+DateTime.Now.ToShortTimeString().Replace(":",".")+".png", System.Drawing.Imaging.ImageFormat.Png);
                    break;
                case 1:
                    image.Save(label1.Text + "\\ScreenShot " + DateTime.Now.ToShortTimeString().Replace(":", ".") + ".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    break;
                case 2:
                    image.Save(label1.Text + "\\ScreenShot " + DateTime.Now.ToShortTimeString().Replace(":", ".") + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp);
                    break;
                case 3:
                    image.Save(label1.Text + "\\ScreenShot " + DateTime.Now.ToShortTimeString().Replace(":", ".") + ".gif", System.Drawing.Imaging.ImageFormat.Gif);
                    break;
                case 4:
                    image.Save(label1.Text + "\\ScreenShot " + DateTime.Now.ToShortTimeString().Replace(":",".") + ".ico", System.Drawing.Imaging.ImageFormat.Icon);
                    break;
                default:
                    MessageBox.Show("Invalid Image Extension");
                    break;
            }
        }
        private static Bitmap Screenshot()
        {
            var screenWidth = Screen.PrimaryScreen.Bounds.Width;
            var screenHeight = Screen.PrimaryScreen.Bounds.Height;

            var screenBmp = new Bitmap(screenWidth, screenHeight);
            var g = Graphics.FromImage(screenBmp);

            var dc1 = GetDC(GetDesktopWindow());
            var dc2 = g.GetHdc();

            BitBlt(dc2, 0, 0, screenWidth, screenHeight, dc1, 0, 0, 13369376);

            ReleaseDC(GetDesktopWindow(), dc1);
            g.ReleaseHdc(dc2);
            g.Dispose();

            return screenBmp;
        }
        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (label1.Text != "Choose Save Location:")
            {
                try
                {
                    this.Hide();
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon1.BalloonTipText = "I'm Running Here";
                    notifyIcon1.BalloonTipTitle = "Hey!";
                    notifyIcon1.ShowBalloonTip(3000);
                    notifyIcon1.ContextMenuStrip = contextMenuStrip1;
                    _RegisKey.Keys = Keys.PrintScreen;
                    _RegisKey.ModKey = 0;
                    _RegisKey.WindowHandle = this.Handle;
                    _RegisKey.HotKey += new RegisterHotKeyClass.HotKeyPass(checkIfKeyPressed);
                    _RegisKey.StarHotKey();
                }
                catch { }
            }
            else
            {
                MessageBox.Show("Choose Saving Path");
            }
        }

        private void sourceCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ShawkyZ/EasyShot");
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            if(folderBrowserDialog1.SelectedPath!="")
            label1.Text = folderBrowserDialog1.SelectedPath;
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Show();
        }
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
