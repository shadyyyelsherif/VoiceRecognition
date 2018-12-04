using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Recognition;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        int WM_SYSCOMMAND = 0x112;
        int SC_MONITORPOWER = 0xF170;
        ProcessStartInfo pro;
        bool ready = false;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        Form f = new Form();
        bool turnOff = true;   //set true if you want to turn off, false if on
        SpeechRecognitionEngine recEngin = new SpeechRecognitionEngine();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Choices command = new Choices();
            command.Add(new string[] {"thanks","jack","Clean","compile" });
            GrammarBuilder gbuilder = new GrammarBuilder();
            gbuilder.Append(command);
            Grammar grammar = new Grammar(gbuilder);
            recEngin.LoadGrammarAsync(grammar);
            recEngin.SetInputToDefaultAudioDevice();
            
            recEngin.SpeechRecognized += recEngin_SpeechRecognized;
        }

        private void recEngin_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {    
            Log.Text += e.Result.Text;
            if (e.Result.Text == "jack")
            {
                ready = true;
            }
            else if (e.Result.Text == "thanks")
            {
                ready = false;
            }
            
            if(ready){
                switch (e.Result.Text)
                {

                    case "clean":
                        //    Log.Text += "compile \n";
                        Process[] pname = Process.GetProcessesByName("cmd");
                        if (pname.Length == 0)
                            cmd("builder.bat 2");
                        else
                            MessageBox.Show("run");


                        break;
                    case "compile":
                        Process[] pname1 = Process.GetProcessesByName("cmd");
                        if (pname1.Length == 0)
                        {
                            cmd("builder.bat 1 RR 15 4 10 F");
                        }
                        else
                            MessageBox.Show("run");

                        break;

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Log.Text += "Loading ...";
            recEngin.RecognizeAsync(RecognizeMode.Multiple);
            Log.Text += "Enabled\n";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Log.Text += "Loading...";
            recEngin.RecognizeAsyncStop();
            Log.Text += "Disapled\n";

        }

        private void Log_TextChanged(object sender, EventArgs e)
        {

        }
        private void cmd(string command)
        {


            pro = new ProcessStartInfo();

            //Setting the FileName to be Started like in our

            //Project we are just going to start a CMD Window.

            pro.FileName = "cmd.exe";

            //Instead of using the above two line of codes, You

            // can just use the code below:

            // ProcessStartInfo pro = new ProcessStartInfo("cmd.exe");

            //Creating an Instance of the Process Class

            // which will help to execute our Process
            pro.WorkingDirectory = @"C:\workset\sp33_after_fix\05_SwDev\05_SwDev\01_SrcCode\Builder";
            pro.Arguments = "/c "+command;

            Process proStart = new Process();


            //Setting up the Process Name here which we are
            proStart.StartInfo.RedirectStandardInput = true;
            // going to start from ProcessStartInfo

            proStart.StartInfo = pro;

            //  proStart.WaitForExit();

            //Calling the Start Method of Process class to

            // Invoke our Process viz 'cmd.exe'

            proStart.Start();
            
          
        }
    }
}
