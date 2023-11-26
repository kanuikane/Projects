using System;
using System.Windows.Forms;

namespace C969
{
    partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            try
            {
                if(Session.Login(usernameTextBox.Text, passwordTextBox.Text))
                {
                    this.Close();
                }
                else
                {
                    MessageBox.Show(Languages.LanguageFill("$internalerror $cannotset $username"));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.InnerException + "\n(" + Languages.LanguageFill("$usetesttest")+")");

            }
        }
    }
}
