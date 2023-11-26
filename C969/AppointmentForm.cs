using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace C969
{
    partial class AppointmentForm : Form
    {
        Appointment _appointment;

        CustomerInfo _customer;
        User _user;
        int _duration;
        int Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                this.durationTextBox.Text = _duration.ToString() + Languages.LanguageFill(" $minutes");
                UpdateEndTimePicker();
            }
        }
        DateTime Start
        {
            get { return startDatePicker.Value.Date.AddHours(startTimePicker.Value.Hour).AddMinutes(startTimePicker.Value.Minute); }
            set
            {
                startDatePicker.Value = value.Date;
                startTimePicker.Value = value;
                ValidateForm(out string errorMessage);
            }
        }
        DateTime End
        {
            get { return Start.Date + endTimePicker.Value.TimeOfDay; }
        }
        void UpdateEndTimePicker()
        {
            this.endTimePicker.Value = this.startTimePicker.Value.AddMinutes(Duration);
            ValidateForm(out string errorMessage);
        }
        CustomerInfo Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                this.customerSelectionButton.Text = _customer.Name + ", " + _customer.DisplayAddress();
                ValidateForm(out string errorMessage);
            }
        }
        User User
        {
            get { return _user; }
            set
            {
                _user = value;
                this.userSelectionButton.Text = _user.DisplayName();
            }
        }
        internal AppointmentForm(Appointment appointment)
        {
            ComboInitalize();
            this.Text = "$editappointment";
            _appointment = appointment;
            Duration = (int)appointment.Duration.TotalMinutes;
            Start = appointment.Start;
            User = appointment.User;
            Customer = appointment.Customer;
            titleTextBox.Text = appointment.Title;
            descriptionTextBox.Text = appointment.Description;
            locationTextBox.Text = appointment.Location;
            contactTextBox.Text = appointment.Contact;
            typeTextBox.Text = appointment.Type;
            urlTextBox.Text = appointment.URL;
        }
        internal AppointmentForm(DateTime suggestedStart)
        {
            ComboInitalize();
            this.Text = "$addappointment";
            Duration = 15;

            if (startTimePicker.Value.Minute % 15 != 0)
                startTimePicker.Value = startTimePicker.Value.AddMinutes(15 - (startTimePicker.Value.Minute % 15));
        }
        internal bool IsBusinessHours(DateTime time)
        {
            if (time.Hour < 6 || time.Hour >= 18)
                return false;
            return true;
        }
        internal bool CheckAppointmentTime(DateTime start, int durationMinutes)
        {
            DateTime end = start.AddMinutes(durationMinutes);
            if (!IsBusinessHours(start))
            {
                return false;
            }
            if (!IsBusinessHours(end))
            {
                return false;
            }
            return Database.ValidateAppointmentTime(start, end, User.ID, _appointment == default(Appointment) ? null : (int?)_appointment.ID);
        }
        void ComboInitalize()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
            OrganizeTabIndex(this.Controls);
            this.User = new User(Session.GetVariable("username"));
            this.userSelectionButton.FlatStyle = FlatStyle.Flat;

            foreach (Control control in this.Controls)
            {
                if (control is TextBox)
                {
                    ((TextBox)control).TextChanged += textBox_TextChanged;
                }
            }
            this.customerSelectionButton.Click += CustomerSelectionButton_Click;
            this.startTimePicker.ValueChanged += StartTimePicker_ValueChanged;
            this.startDatePicker.ValueChanged += StartDatePicker_ValueChanged;
            UpdateEndTimePicker();
        }

        private void StartDatePicker_ValueChanged(object sender, EventArgs e) => ValidateForm(out string errorMessage);

        private void textBox_TextChanged(object sender, EventArgs e) => ValidateForm(out string errorMessage);

        private void StartTimePicker_ValueChanged(object sender, EventArgs e) => UpdateEndTimePicker();

        private void CustomerSelectionButton_Click(object sender, EventArgs e)
        {
            CustomerListView customerList = new CustomerListView();
            this.Controls.Add(customerList);
            customerList.DoubleClick += CustomerListViewDoubleClick;
            customerList.Dock = DockStyle.Fill;
            customerList.BringToFront();
            customerList.RefreshCustomers();
        }

        private void CustomerListViewDoubleClick(object sender, EventArgs e)
        {
            CustomerListView customerList = (CustomerListView)sender;
            this.Customer = customerList.SelectedCustomer();
            this.Controls.Remove((Control)sender);
            ((ListView)sender).Dispose();
            ValidateForm(out string errorMessage);
        }

        void OrganizeTabIndex(Control.ControlCollection controls)
        {
            List<Control> controlList = controls.Cast<Control>().ToList();
            //lambda expression is used here for ordering controls by x, then y.
            //LINQ require the use of lambda expressions, so this lambda expression makes the program more efficient by enabling the use of LINQ order functions.
            controlList = controlList.OrderBy(c => c.Location.Y).ThenBy(c => c.Location.X).ToList();
            {//code block to define scope of tabIndex.
                int tabIndex = 0;
                foreach (Control control in controlList)
                {
                    control.TabIndex = tabIndex;
                    tabIndex++;
                    if (control.HasChildren)
                    {
                        OrganizeTabIndex(control.Controls);
                    }
                }
            }
        }
        bool ValidateTime(out string errorMessage)
        {
            try
            {
                if (!IsBusinessHours(startTimePicker.Value)
                    || !IsBusinessHours(endTimePicker.Value))
                {
                    errorMessage = "-" + Languages.LanguageFill("$appointmentoutsidebusinesshours \n");
                    return false;
                }
                if (!Database.ValidateAppointmentTime(Start, End, User.ID, _appointment == default(Appointment) ? null : (int?)_appointment.ID))
                {
                    errorMessage = "-" + Languages.LanguageFill("$appointmentconflict \n");
                    return false;
                }
                errorMessage = "";
                return true;
            }
            catch (Exception e)
            {
                errorMessage = "-" + Languages.LanguageFill(e.Message + "\n" + e.StackTrace + "\n\n");
            }
            return false;
        }
        bool ValidateForm(out string errorMessage)
        {
            bool validates = true;
            if (ValidateTime(out errorMessage))
            {
                startLabel.BackColor = endLabel.BackColor = durationLabel.BackColor = DefaultBackColor;
            }
            else
            {
                startLabel.BackColor = endLabel.BackColor = durationLabel.BackColor = Color.PaleVioletRed;
                validates = false;
            }
            if (_customer == default(CustomerInfo))
            {
                errorMessage += "-" + Languages.LanguageFill("$selectacustomer \n");
                customerSelectionButton.BackColor = Color.PaleVioletRed;
                validates = false;
            }
            else
            {
                customerSelectionButton.BackColor = Color.White;
            }
            if (_user == default(User))
            {
                errorMessage += "-" + Languages.LanguageFill("No consultant exists: $selectaconsultant \n");
                userSelectionButton.BackColor = Color.PaleVioletRed;
                validates = false;
            }
            else
            {
                userSelectionButton.BackColor = Color.White;
            }
            return validates;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm(out string errorMessage))
            {
                try
                {
                    if (_appointment == default(Appointment))
                    {
                        //new appointment
                        _appointment = new Appointment(Customer, User, titleTextBox.Text, descriptionTextBox.Text, locationTextBox.Text, contactTextBox.Text, typeTextBox.Text, urlTextBox.Text, Start, End);
                    }
                    else
                    {
                        _appointment.Update(Customer, User, titleTextBox.Text, descriptionTextBox.Text, locationTextBox.Text, contactTextBox.Text, typeTextBox.Text, urlTextBox.Text, Start, End);
                    }
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("-" + Languages.LanguageFill(ex.Message + "\n" + ex.StackTrace));
                }
            }
            else
            {
                MessageBox.Show(errorMessage);
            }
        }
    }
}
