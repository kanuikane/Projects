using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C969
{
    public partial class LoginScreen : Form
    {
        public LoginScreen()
        {
            InitializeComponent();
        }

        private bool isUpcoming()
        {
            string connString = "server=127.0.0.1;uid=sqlUser;pwd=Passw0rd!;database=client_schedule";
            string username = txtLoginUsername.Text;
            bool result = false;
            try
            {
                var conn = new MySqlConnection();
                conn.ConnectionString = connString;
                conn.Open();

                string upcomingAppointmentQuery =
                    $"SELECT * FROM appointment WHERE start BETWEEN NOW() AND NOW() + INTERVAL 15 MINUTE AND userId=(SELECT userId FROM user WHERE userName='{username}')";
                var upcomingAppointmentCMD = new MySqlCommand(upcomingAppointmentQuery, conn);
                int upcomingAppointmentIndex = Convert.ToInt32(upcomingAppointmentCMD.ExecuteScalar());

                if (upcomingAppointmentIndex == 0)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }

            }
            catch (MySqlException)
            {
                MessageBox.Show("Nope Error!");
            }
            return result;
        }

        private void btnLoginCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string connString = "server=127.0.0.1;uid=sqlUser;pwd=Passw0rd!;database=client_schedule";

            try
            {
                string path = @"log.txt";

                var conn = new MySqlConnection();
                conn.ConnectionString = connString;
                conn.Open();
                string username = txtLoginUsername.Text;
                string password = txtLoginPassword.Text;
                string query = $"SELECT * FROM user WHERE userName='{username}' AND password='{password}'";
                var cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    Main mainForm = new Main();
                    if (isUpcoming() != true)
                    {
                        MessageBox.Show("Successful Login!");
                        mainForm.Show();
                        this.Hide();

                        //Successful login attempts filed
                        try
                        {
                            if (File.Exists(path) == false)
                            {
                                File.Create(path).Dispose();

                                using (TextWriter tw = new StreamWriter(path))
                                {
                                    tw.WriteLine($"SUCCESSFUL LOGIN ATTEMPT FOR USER {username} AT {DateTime.Now} {Environment.NewLine}");
                                }
                            }
                            else if (File.Exists(path) == true)
                            {
                                File.AppendAllText(path, $"SUCCESSFUL LOGIN ATTEMPT FOR USER {username} AT {DateTime.Now} {Environment.NewLine}");
                            }

                        }
                        catch (IOException error)
                        {
                            MessageBox.Show(error.ToString());
                        }

                    }
                    else
                    {
                        MessageBox.Show("Your upcoming appointment is in 15 minutes.");
                        mainForm.Show();
                        this.Hide();
                    }

                }
                else
                {
                    //Check for English
                    if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "en")
                    {
                        MessageBox.Show("Username or Password is incorrect. Please try again.");
                    }
                    //Check for Spanish
                    else if (CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "es")
                    {
                        MessageBox.Show("El nombre de usuario o la contraseña son incorrectos. Por favor, inténtelo de nuevo.");
                    }

                    try
                    {
                        if (File.Exists(path) == false)
                        {
                            File.Create(path).Dispose();

                            using (TextWriter tw = new StreamWriter(path))
                            {
                                tw.WriteLine($"UNSUCCESSFUL LOGIN ATTEMPT FOR USER {username} AT {DateTime.Now} {Environment.NewLine}");
                            }
                        }
                        else if (File.Exists(path) == true)
                        {
                            File.AppendAllText(path, $"UNSUCCESSFUL LOGIN ATTEMPT FOR USER {username} AT {DateTime.Now} {Environment.NewLine}");
                        }

                    }
                    catch (IOException error)
                    {
                        MessageBox.Show(error.ToString());
                    }
                }
            }
            catch (MySqlException)
            {
                MessageBox.Show("Unable to connect!");
            }
            finally
            {

            }



        }
    }
}
