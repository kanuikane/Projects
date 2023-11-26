using System;
using System.Windows.Forms;

namespace C969
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool isExiting = false;
            while (!Session.IsAuthenticated() && !isExiting)
            {
                Form login = new LoginForm();
                Languages.LanguageFill(ref login);
                login.ShowDialog();
                if (Session.IsAuthenticated())
                {
                    Form mainForm = new MainForm();
                    Languages.LanguageFill(ref mainForm);
                    Application.Run(mainForm);
                }
                else
                {
                    isExiting = true;
                }
            }
        }
    }
}
