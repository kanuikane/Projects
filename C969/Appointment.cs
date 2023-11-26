using System;
using System.Linq;
using System.Windows.Forms;

namespace C969
{
    class Appointment
    {
        //variables for appointment
        int appID;
        CustomerInfo cust;
        int custId;
        User user1;
        int userId1;
        string title1;
        string appDescription;
        string appLocation;
        string contactInfo; 
        string appType;
        string url1;
        DateTime startTime;
        DateTime endTime;

        // create a new corresponding object for both user and customers using the corresponding ID.
        public CustomerInfo Customer
        {
            get
            {
                if (cust == default(CustomerInfo))
                {
                    cust = new CustomerInfo(custId);
                }
                return cust;
            }
        }
        public User User
        {
            get
            {
                if (user1 == default(User))
                {
                    user1 = new User(userId1);
                }
                return user1;
            }
        }
        public Appointment(int appointmentId)
        {
            //read from Database
            Database.AppointmentRecordGet(appointmentId, out custId, out userId1, out title1, out appDescription, out appLocation, out contactInfo, out appType, out url1, out startTime, out endTime);
            appID = appointmentId;
        }
        public Appointment(CustomerInfo customer, User user, string title, string description, string location, string contact, string type, string url, DateTime start, DateTime end)
        {
            //new appID after inserted into Database
            appID = Database.AppointmentRecordSet(null, customer.ID, user.ID, title, description, location, contact, type, url, start, end);
            cust = customer;
            custId = customer.ID;
            user1 = user;
            userId1 = user.ID;
            title1 = title;
            appDescription = description;
            appLocation = location;
            contactInfo = contact;
            appType = type;
            url1 = url;
            startTime = start;
            endTime = end;
        }
        public Appointment(int appointmentId, CustomerInfo customer, User user, string title, string description, string location, string contact, string type, string url, DateTime start, DateTime end)
        {
            appID = appointmentId;
            cust = customer;
            custId = customer.ID;
            user1 = user;
            userId1 = user.ID;
            title1 = title;
            appDescription = description;
            appLocation = location;
            contactInfo = contact;
            appType = type;
            url1 = url;
            startTime = start;
            endTime = end;
        }
        public Appointment(int appointmentId, int customerId, int userId, string description, string location, string contact, string type, DateTime start, DateTime end)
        {
            //leave cust and user1 blank, they can be pulled later when needed.
            appID = appointmentId;
            custId = customerId;
            userId1 = userId;
            appDescription = description;
            appLocation = location;
            contactInfo = contact;
            appType = type;
            startTime = start;
            endTime = end;
        }
        public void Update(CustomerInfo customer, User user, string title, string description, string location, string contact, string type, string url, DateTime start, DateTime end)
        {
            //new appID after inserted into Database
            Database.AppointmentRecordSet(appID, customer.ID, user.ID, title, description, location, contact, type, url, start, end);
            cust = customer;
            custId = customer.ID;
            user1 = user;
            userId1 = user.ID;
            title1 = title;
            appDescription = description;
            appLocation = location;
            contactInfo = contact;
            appType = type;
            url1 = url;
            startTime = start;
            endTime = end;
        }
        public int ID
        {
            get { return this.appID; }
        }

        public string Description
        {
            get { return this.appDescription; }
        }

        public string Title
        {
            get { return this.title1; }
        }
        public string Location
        {
            get { return this.appLocation; }
        }
        public string Contact
        {
            get { return this.contactInfo; }
        }
        public string Type
        {
            get { return this.appType; }
        }
        public string URL
        {
            get { return this.url1; }
        }

        public DateTime Start
        {
            get { return this.startTime; }
        }
        public DateTime End
        {
            get { return this.startTime; }
        }
        public TimeSpan Duration
        {
            get { return this.endTime - this.startTime; }
        }

        public ListViewItem ToListViewItem(ListView list)
        {
            ListViewItem item = new ListViewItem();
            bool firstPass = true;
            foreach (ColumnHeader header in list.Columns)
            {
                string columnValue = "";
                if (header.Name == "$customer")
                {
                    columnValue = cust.Name;
                }
                else if (header.Name == "$consultant")
                {
                    columnValue = user1.DisplayName();
                }
                else if (header.Name == "$type")
                {
                    columnValue = appType;
                }
                else if (header.Name == "$time")
                {
                    columnValue = startTime.ToString("MM/dd/yy hh:mm - ") + ((int)Duration.TotalMinutes).ToString() + "min";
                }
                else
                {
                    throw new Exception(Languages.LanguageFill("(Invalid header found: " + header.Text + ")"));
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
            item.Tag = appID;
            return item;
        }
    }

    class AppointmentListView : ListView
    {
        public AppointmentListView()
        {
            this.View = View.Details;
            foreach (string columnText in (new string[] { "$customer", "$consultant", "$type", "$time" }))
            {
                this.Columns.Add(columnText, Languages.LanguageFill(columnText));
            }
            this.AutoResizeColumns(ColumnHeaderAutoResizeStyle.None);
            this.FullRowSelect = true;
            this.Resize += new System.EventHandler(UpdateColumns);
            this.MultiSelect = false;
        }

        public Appointment SelectedAppointment()
        {
            return new Appointment((int)this.SelectedItems[0].Tag);
        }
        void UpdateColumns(object sender, EventArgs e)
        {
            foreach (ColumnHeader header in this.Columns)
            {
                header.Width = (this.Width - 1) / this.Columns.Count - 1;
            }
        }
        public void RefreshAppointments(int? userId, DateTime startDate, DateTime endDate)
        {
            this.Items.Clear();

            foreach (Appointment appointment in Database.AppointmentList(userId, startDate, endDate).OrderBy(x => x.Start).ThenBy(x => x.User.DisplayName()))
            {
                this.Items.Add(appointment.ToListViewItem(this));
            }
        }
    }
}
