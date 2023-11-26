using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace C969
{
    class Report
    {
        string name;
        string sqlCode1;
        string subSqlCode;

        public Report(string name, string sqlCode)
        {
            Name = name;
            SqlCode = sqlCode;
        }
        public Report(string name, string sqlCode, string subSqlCode)
        {
            Name = name;
            SqlCode = sqlCode;
            SubSqlCode = subSqlCode;
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string SqlCode
        {
            get { return sqlCode1; }
            set { sqlCode1 = value; }
        }
        public string SubSqlCode
        {
            get { return subSqlCode; }
            set { subSqlCode = value; }
        }
        public string Execute()
        {
            if (SubSqlCode == default(string))
            {
                return Database.AdHocToHTML(sqlCode1);
            }
            else
            {
                string ret = "";
                foreach (string s in Database.AdHocToStringList(sqlCode1))
                {
                    try
                    {
                        ret += "<p>" + s + "</p>";
                        ret += Database.AdHocToHTML(subSqlCode, s);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + " s=" + s + "; subcode: " + subSqlCode);
                    }
                }
                return ret;
            }
        }
    }
    static class Reports
    {
        static List<Report> _reports = new List<Report>();
        static Reports()
        {
            _reports.Clear();
            _reports.Add(new Report(Languages.LanguageFill("$#appointmenttypebymonth")
                , "SELECT"
                    + " CONCAT(MONTHNAME(start), ' ', YEAR(start)) "
                    + " AS `" + Languages.LanguageFill("$month") + "`"
                    + " , type "
                    + " as `" + Languages.LanguageFill("$appointmenttype") + "`"
                    + " , COUNT(*) "
                    + " AS `" + Languages.LanguageFill("$numberofappointments") + "` "
                    + " FROM appointment "
                    + " group by concat(monthname(start), ' ', year(start)), month(start), year(start), type "
                    + " order by year(start), month(start), type "));
            _reports.Add(new Report(Languages.LanguageFill("$scheduleforeachconsultant")
                , "select u.username from user u inner join appointment a on a.userId = u.userId group by u.username"
                , "select "
                    + " c.customerName "
                    + " as `" + Languages.LanguageFill("#customer") + "`"
                    + " , a.title "
                    + " as `" + Languages.LanguageFill("#title") + "`"
                    + " , a.type "
                    + " as `" + Languages.LanguageFill("#appointmenttype") + "`"
                    + " , a.start "
                    + " as `" + Languages.LanguageFill("#start") + "`"
                    + " , timestampdiff(MINUTE, a.start,a.end) "
                    + " as `" + Languages.LanguageFill("#duration") + "`"
                    + " from appointment a "
                    + " inner join user u on a.userId = u.userId "
                    + " inner join customer c on c.customerId = a.customerId "
                    + " where u.username = @p0 and a.start > utc_timestamp();"));
            _reports.Add(new Report(Languages.LanguageFill("$myscheduledhourspermonth")
                , " select "
                    + " concat(monthname(start), ' ', year(start)) "
                    + " AS `" + Languages.LanguageFill("$month") + "`"
                    + " , sum(timestampdiff(MINUTE, a.start, a.end)+1) / 60 "
                    + " as `" + Languages.LanguageFill("$hours") + "`"
                    + " , COUNT(*) "
                    + " AS `" + Languages.LanguageFill("$numberofappointments") + "` "
                    + " FROM appointment a "
                    + " inner join user u on a.userId = u.userId "
                    + " where u.userName = @username "
                    + " group by concat(monthname(start), ' ', year(start)), month(start), year(start) "
                    + " order by year(start), month(start) "));
        }
        public static List<Report> AllReports
        {
            get { return _reports; }
        }
    }
    class ReportsListView : ListView
    {
        public ReportsListView()
        {
            this.Clear();
            this.View = View.Details;
            foreach (string columnText in (new string[] { "$reportname" }))
            {
                this.Columns.Add(columnText, Languages.LanguageFill(columnText));
            }
            this.AutoResizeColumns(ColumnHeaderAutoResizeStyle.None);
            this.FullRowSelect = true;
            this.Resize += new System.EventHandler(UpdateColumns);
            this.MultiSelect = false;
            this.DoubleClick += ReportsListView_DoubleClick;
            RefreshReports();
        }

        private void ReportsListView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string reportName = this.SelectedItems[0].Text;
                string htmlCode = Reports.AllReports.Where(a => a.Name == reportName).First().Execute();
                Form form = new ReportViewer(htmlCode, reportName);
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(Languages.LanguageFill("$errorrunningreport " + ex.Message));
            }
        }

        void UpdateColumns(object sender, EventArgs e)
        {
            foreach (ColumnHeader header in this.Columns)
            {
                header.Width = (this.Width - 1) / this.Columns.Count - 1;
            }
        }
        public void RefreshReports()
        {
            this.Items.Clear();
            foreach (Report report in Reports.AllReports)
            {
                this.Items.Add(report.Name);
            }
        }
    }
}
