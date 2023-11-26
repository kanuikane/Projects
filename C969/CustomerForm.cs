using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace C969
{
    partial class CustomerForm : Form
    {
        CustomerInfo customer;
        public CustomerForm()
        {
            ComboInitalize();
            this.Text = "$add $customerinformation";
        }
        public CustomerForm(CustomerInfo customer1)
        {
            ComboInitalize();
            this.Text = "$edit $customerinformation";
            this.nameTextBox.Text = customer1.Name;
            this.addressTextBox.Text = customer1.Address.Address1;
            this.address2TextBox.Text = customer1.Address.Address2;
            this.phoneTextBox.Text = customer1.Address.Phone;
            this.postalCodeTextBox.Text = customer1.Address.PostalCode;
            this.countryComboBox.Text = customer1.Address.City.Country.Name;
            this.cityComboBox.Text = customer1.Address.City.Name;
            customer = customer1;
        }
        void ComboInitalize()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
            PopulateCountries();
            OrganizeTabIndex();
            this.nameTextBox.KeyUp += NameTextBox_KeyUp;
            this.addressTextBox.KeyUp += Address1TextBox_KeyUp;
            this.phoneTextBox.KeyUp += PhoneTextBox_KeyUp;
            this.postalCodeTextBox.KeyUp += PostalCodeTextBox_KeyUp;
            this.cityComboBox.TextChanged += CityComboBox_TextChanged;
            this.cityComboBox.SelectedIndexChanged += CityComboBox_SelectedIndexChanged;
            this.countryComboBox.KeyUp += CountryComboBox_KeyUp;
            this.countryComboBox.SelectedValueChanged += CountryComboBox_SelectedValueChanged;
            this.saveButton.Click += SaveButton_Click;
            ValidateForm(out string errorMessage);
        }


        void OrganizeTabIndex()
        {
            OrganizeTabIndex(this.Controls);
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

        //lambda operators are used here for each event handler so that all can be directed to ValidateForm().
        private void CityComboBox_TextChanged(object sender, EventArgs e) => ValidateForm(out string errorMessage);
        private void CityComboBox_SelectedIndexChanged(object sender, EventArgs e) => ValidateForm(out string errorMessage);
        private void Address1TextBox_KeyUp(object sender, KeyEventArgs e) => ValidateForm(out string errorMessage);
        private void PostalCodeTextBox_KeyUp(object sender, KeyEventArgs e) => ValidateForm(out string errorMessage);
        private void PhoneTextBox_KeyUp(object sender, KeyEventArgs e) => ValidateForm(out string errorMessage);
        private void NameTextBox_KeyUp(object sender, KeyEventArgs e) => ValidateForm(out string errorMessage);

        private void CountryComboBox_KeyUp(object sender, KeyEventArgs e)
        {
            ValidateForm(out string errorMessage);
            PopulateCities();
        }
        private void CountryComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            ValidateForm(out string errorMessage);
            PopulateCities();
        }
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (!ValidateForm(out string errorMessage))
            {
                MessageBox.Show(errorMessage);
            }
            else
            {
                if (!CreateCustomer())
                {
                    MessageBox.Show(Languages.LanguageFill("$internalerror $creatingcustomer"));
                }
                else
                {
                    this.Close();
                }
            }
        }
        private bool CreateCustomer()
        {
            if (customer == default(CustomerInfo))
            {
                try
                {
                    this.customer = new CustomerInfo(this.nameTextBox.Text, ConstructAddress());
                }
                catch (Exception e)
                {
                    return false;
                }
                return true;
            }
            else
            {
                return customer.UpdateCustomer(this.nameTextBox.Text, ConstructAddress());
            }
        }
        private Address ConstructAddress()
        {
            Country country = new Country(this.countryComboBox.Text);
            City city = new City(this.cityComboBox.Text, country);
            Address address = new Address(this.addressTextBox.Text, this.address2TextBox.Text, city, this.postalCodeTextBox.Text, this.phoneTextBox.Text);
            return address;
        }
        bool ValidateForm(out string errorMessage)
        {
            errorMessage = "";
            if (this.nameTextBox.Text.Length < 3)
            {
                this.nameTextBox.BackColor = Color.PaleVioletRed;
                errorMessage += " $entercustomername \n";
            }
            else
            {
                this.nameTextBox.BackColor = Color.White;
            }

            if (Int64.TryParse(this.addressTextBox.Text.Split(' ')[0], out Int64 addrNum) //validate first $
                    && this.addressTextBox.Text.Contains(" ") //contains space
                    && this.addressTextBox.Text.Split(new char[] { ' ' }, 2)[1].Length > 2) //second word must be at least 3 characters
            {
                this.addressTextBox.BackColor = Color.White;
            }
            else
            {
                this.addressTextBox.BackColor = Color.PaleVioletRed;
                errorMessage += " $enteraddress \n";
            }

            if (phoneTextBox.Text.Length < 10)
            {
                this.phoneTextBox.BackColor = Color.PaleVioletRed;
                errorMessage += " $enterphone \n";
            }
            else if (!Regex.IsMatch(phoneTextBox.Text, @"^[0-9\-\(\)]+$"))
            {
                this.phoneTextBox.BackColor = Color.PaleVioletRed;
                errorMessage += " $invalidphonecharacters \n";
            }
            else
            {
                this.phoneTextBox.BackColor = Color.White;
            }

            if (countryComboBox.Text.Length > 1)
            {
                countryComboBox.BackColor = Color.White;
                cityComboBox.Enabled = true;
            }
            else
            {
                countryComboBox.BackColor = Color.PaleVioletRed;
                cityComboBox.Enabled = false;
                errorMessage += " $entercountry \n";
            }

            if (cityComboBox.Text.Length > 1)
            {
                cityComboBox.BackColor = Color.White;
            }
            else
            {
                cityComboBox.BackColor = Color.PaleVioletRed;
                errorMessage += " $entercity \n";
            }

            if (postalCodeTextBox.Text.Length < 2)
            {
                postalCodeTextBox.BackColor = Color.PaleVioletRed;
                errorMessage += " $enterzipcode \n";
            }
            else
            {
                postalCodeTextBox.BackColor = Color.White;
            }
            if (errorMessage == "")
            {
                return true;
            }
            else
            {
                errorMessage = Languages.LanguageFill(errorMessage);
                return false;
            }
        }
        void PopulateCountries()
        {
            List<string> countryList = Database.CountryList();
            countryList.Insert(0, "");

            this.countryComboBox.DataSource = countryList;
            countryComboBox.Text = "";
        }
        void PopulateCities()
        {
            string country = countryComboBox.Text;
            if (country.Length > 0)
            {
                List<string> cityList = Database.CityList(country);
                cityList.Insert(0, "");
                this.cityComboBox.DataSource = cityList;
            }
            else
            {
                cityComboBox.DataSource = null;
            }
        }
    }
}
