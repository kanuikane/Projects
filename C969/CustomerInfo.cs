using System;
using System.Windows.Forms;

namespace C969
{
    class CustomerInfo
    {
        int custID;
        string custName;
        Address address1;

        public int ID
        {
            get { return custID; }
        }
        public string Name
        {
            get { return custName; }
        }
        public Address Address
        {
            get { return address1; }
        }
        public string DisplayAddress()
        {
            return address1.Address1 + (address1.Address2.Length == 0 ? " " + address1.Address2 : "") + " " + address1.City.Name + ", " + address1.City.Country.Name;
        }
        public CustomerInfo(int customerId)
        {
            //fill customer from database
            if (Database.CustomerRecordGet(customerId, out custName, out int addressId))
            {
                custID = customerId;
                address1 = new Address(addressId);
            }
            else
            {
                MessageBox.Show(Languages.LanguageFill("$cannotread $customer ID " + customerId.ToString()));
            }
        }
        public CustomerInfo(string name, Address address)
        {
            //create customer in database
            custID = Database.CustomerRecordAdd(name, address.ID);
            custName = name;
            address1 = address;
        }
        public CustomerInfo(int customerId, string name, Address address)
        {
            //full record exists, build record without database
            custID = customerId;
            custName = name;
            address1 = address;
        }
        public bool UpdateAddress(Address address)
        {
            return UpdateAddress(address.ID);
        }
        public bool UpdateAddress(int addressId)
        {
            return UpdateCustomer(custName, addressId); //use existing name
        }
        public bool UpdateName(string name)
        {
            return UpdateCustomer(name, address1.ID); //use existing address ID
        }
        public bool UpdateCustomer(string name, Address address)
        {
            return UpdateCustomer(name, address.ID);
        }
        public bool UpdateCustomer(string name, int addressId)
        {
            if (Database.CustomerRecordUpdate(custID, name, addressId))
                return true;
            else
                MessageBox.Show("xxxx");
            return false;
        }
        public ListViewItem ToListViewItem(ListView list)
        {
            ListViewItem item = new ListViewItem();
            bool firstPass = true;
            foreach (ColumnHeader header in list.Columns)
            {
                string columnValue = "";
                if (header.Name == "$name")
                {
                    columnValue = custName;
                }
                else if (header.Name == "$addressLine1")
                {
                    columnValue = this.DisplayAddress();
                }
                else if (header.Name == "$phone")
                {
                    columnValue = address1.Phone;
                }
                else
                {
                    MessageBox.Show(Languages.LanguageFill("(Invalid header found: " + header.Text + ")"));
                }
                if (firstPass)
                {
                    firstPass = false;
                    item = new ListViewItem(columnValue);
                }
                else
                {
                    item.SubItems.Add(columnValue);
                }
            }
            item.Tag = custID;
            return item;
        }
    }
    class CustomerListView : ListView
    {
        public CustomerListView()
        {
            this.View = View.Details;
            foreach (string columnText in (new string[] { "$name", "$addressLine1", "$phone" }))
            {
                this.Columns.Add(columnText, Languages.LanguageFill(columnText));
            }
            this.AutoResizeColumns(ColumnHeaderAutoResizeStyle.None);
            this.FullRowSelect = true;
            this.Resize += new System.EventHandler(UpdateColumns);
            this.MultiSelect = false;
        }

        public bool DeleteSelectedCustomer(out string errorMessage)
        {
            if (SelectedItems.Count == 1)
            {
                int selectedCustomer = (int)this.SelectedItems[0].Tag;
                if (Database.CustomerRecordDelete(selectedCustomer))
                {
                    this.Items.Remove(SelectedItems[0]);
                    errorMessage = "";
                    return true;
                }
                else
                {
                    errorMessage = Languages.LanguageFill("$cannotdelete $customer");
                    return false;
                }
            }
            else
            {
                errorMessage = Languages.LanguageFill("$mustselectrecord");
                return false;
            }
        }
        public CustomerInfo SelectedCustomer()
        {
            return new CustomerInfo((int)this.SelectedItems[0].Tag);
        }
        void UpdateColumns(object sender, EventArgs e)
        {
            foreach (ColumnHeader header in this.Columns)
            {
                header.Width = (this.Width - 1) / this.Columns.Count - 1;
            }
            //this.Columns[0].Width += this.Width % this.Columns.Count;
        }
        public void RefreshCustomers()
        {
            this.Items.Clear();

            foreach (CustomerInfo customer in Database.CustomerList())
            {
                this.Items.Add(customer.ToListViewItem(this));
            }
        }
    }
}
