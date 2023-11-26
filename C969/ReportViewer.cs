using System.Windows.Forms;

namespace C969
{
    public partial class ReportViewer : Form
    {
        public ReportViewer(string htmlCode, string reportName)
        {
            InitializeComponent();
            webBrowser1.DocumentText = htmlCode;
            webBrowser1.AccessibleDescription = htmlCode;
            this.Text = reportName;
        }
    }
}
