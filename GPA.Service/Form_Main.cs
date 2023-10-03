using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Win32;
using System.Configuration;
using GPA.Service.Utilities;
using GPA.Service.Services;

namespace GPA.Service
{
    public partial class Form_Main : Form
    {
        int TimerInterval = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["TimerInterval"]);
        int ScriptExecutionHour = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["ScriptExecutionHour"]);
        int CheckKilometrageAgainHour = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["KilometrageRecheckHour"]);

        public int sendAlertesEmails = 0;
        public int kilometrageChecked = 0;

        public Form_Main()
        {

            InitializeComponent();

            RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (rkApp.GetValue("BeliveGPA") == null)
            {
                rkApp.SetValue("BeliveGPA", Application.ExecutablePath);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
            Application.Exit();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void Form_Main_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.BalloonTipTitle = "Gestion Parc Auto";
                this.notifyIcon1.BalloonTipText = "L'application tourne en arrière plan";
                this.notifyIcon1.ShowBalloonTip(1000);
            }
        }

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form_Main());
            }
            catch (Exception exp)
            {
                Logger.LogException(exp.Message);
            }
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.BalloonTipTitle = "Gestion Parc Auto";
                this.notifyIcon1.BalloonTipText = "L'application tourne en arrière plan";
                this.notifyIcon1.ShowBalloonTip(1000);
            }

            //// Send emails alertes 
            Thread AlertesEmailsThread = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    AlertesEmails();
                    Thread.Sleep(TimerInterval);
                }

            }));
            AlertesEmailsThread.Start();

        }

        private void aProposToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;

        }

        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            this.notifyIcon1.BalloonTipTitle = "Gestion Parc Auto";
            this.notifyIcon1.BalloonTipText = "L'application tourne en arrière plan";
            this.notifyIcon1.ShowBalloonTip(1000);
            e.Cancel = true;
        }

        public void AlertesEmails()
        {
            try
            {
                if (DateTime.Now.Hour == ScriptExecutionHour)
                {
                    sendAlertesEmails++;                  
                }
                else if (DateTime.Now.Hour == CheckKilometrageAgainHour)
                {
                    sendAlertesEmails = 0;

                    if (kilometrageChecked == 1)
                    {
                        KilometrageService.KilometrageUpdate();
                        kilometrageChecked = 0;
                    }

                }
                else
                {
                    sendAlertesEmails = 0;
                }

                if (sendAlertesEmails == 1)
                {
                    bool reCheck = KilometrageService.KilometrageUpdate();
                    if (reCheck)
                    {
                        kilometrageChecked++;
                    }
                    
                    AlerteService alerteService = new AlerteService();
                    alerteService.sendEntretienAlertes();
                    alerteService.SendAssuranceAlertes();
                    alerteService.SendTaxeAlertes();
                    alerteService.SendVisiteTechniqueAlertes();
                }

                
            }
            catch (Exception exp)
            {
                Logger.LogException(exp);
            }
        }       
    }
}
