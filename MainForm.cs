using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ReportFromXenu
{
    public partial class MainForm : Form
    {
        public static List<ReportData> ListReport;
        public string FileName { get; set; }
        private static ReportStatusCodes _filterStatusCode = ReportStatusCodes.AllUrls;
       
        public MainForm()
        {
            InitializeComponent();
        }

        private ReportStatusCodes GetFilterReportCode(string statusName)
        {
            ReportStatusCodes code;
            if (!Enum.TryParse(statusName, out code))
            {
                MessageBox.Show(this, "Wrong status name!", "Error");
            }

            return code;
        }

        private void CreateReport(Func<ReportData, bool> filter)
        {
            FileName = Path.ChangeExtension(tbFilename.Text, ".xml");
            var root = new XElement("root");
            root.Add(new XElement("Site", "www.frederikssund.dk", new XAttribute("AllUrls", ListReport.Count())));
            var filteredData = ListReport.Where(filter);

            foreach (var reportData in filteredData)
            {
                root.Add(new XElement("ItemUrl",
                 new XAttribute("Address", reportData.Address),
                 new XAttribute("StatusCode", reportData.StatusCode),
                 new XAttribute("StatusText", reportData.StatusText),
                 new XAttribute("Error", reportData.Error),
                 new XAttribute("Size", reportData.Size),
                 new XAttribute("Title", reportData.Title),
                 new XAttribute("Date", reportData.Date),
                 new XAttribute("Level", reportData.Level),
                 new XAttribute("LinksOut", reportData.LinksOut),
                 new XAttribute("LinksIn", reportData.LinksIn),
                 new XAttribute("Server", reportData.Server),
                 new XAttribute("Duration", reportData.Duration),
                 new XAttribute("Charset", reportData.Charset)
                 ));
            }

            root.Add(new XElement("TotalUrls", ListReport.Count(filter), new XAttribute("StatusCode", _filterStatusCode)));

            root.Save(FileName);
        }
        private void btnCreate_Click(object sender, EventArgs e)
        {
            switch (_filterStatusCode)
            {
                case ReportStatusCodes.Ok:
                     CreateReport(i => i.StatusCode == "200");
                    break;
                case ReportStatusCodes.NoInfoToReturn:
                    CreateReport(i => i.StatusCode == "204");
                    break;
                case ReportStatusCodes.NoObjectData:
                    CreateReport(i => i.StatusCode == "400");
                    break;
                case ReportStatusCodes.AuthRequired:
                    CreateReport(i => i.StatusCode == "401");
                    break;
                case ReportStatusCodes.ForbiddenRequest:
                    CreateReport(i => i.StatusCode == "403");
                    break;
                case ReportStatusCodes.NotFound:
                    CreateReport(i => i.StatusCode == "404");
                    break;
                case ReportStatusCodes.NotFoundInternalUrls:
                    CreateReport(i => i.StatusCode == "404" && i.Address.Contains("http://www.frederikssund.dk/"));
                    break;
                case ReportStatusCodes.NoLongerAvailable:
                    CreateReport(i => i.StatusCode == "410");
                    break;
                case ReportStatusCodes.ServerError:
                    CreateReport(i => i.StatusCode == "500");
                    break;
                case ReportStatusCodes.Timeout:
                    CreateReport(i => i.StatusCode == "12002");
                    break;
                case ReportStatusCodes.NoSuchHost:
                    CreateReport(i => i.StatusCode == "12007");
                    break;
                case ReportStatusCodes.Cancelled:
                    CreateReport(i => i.StatusCode == "12017");
                    break;
                case ReportStatusCodes.NoConnection:
                    CreateReport(i => i.StatusCode == "12029");
                    break;
                case ReportStatusCodes.SkipType:
                    CreateReport(i => i.StatusCode == "-3");
                    break;
                case ReportStatusCodes.ExternalLinks:
                    CreateReport(i => i.StatusCode == "-2");
                    break;
                case ReportStatusCodes.Mailto:
                    CreateReport(i => i.StatusCode == "-6");
                    break;
                case ReportStatusCodes.AllUrls:
                    CreateReport(i => true);
                    break;
                case ReportStatusCodes.AllUrlsButOk:
                    CreateReport(i => i.StatusCode != "200");
                    break;
                case ReportStatusCodes.WithoutExternalUrlsAndOk:
                    CreateReport(i => i.StatusCode != "-2" && i.StatusCode != "200");
                    break;
                case ReportStatusCodes.OnlyMediaUrls:
                    CreateReport(i => i.StatusCode != "-2" && i.StatusCode != "200" && i.Address.Contains("/media("));
                    break;
                case ReportStatusCodes.OnlyOldurls:
                    CreateReport(i => i.StatusCode != "-2" && i.StatusCode != "200" && i.Address.Contains("http://www.frederikssund.dk/content/dk/"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void cbFilterStatusCode_SelectedValueChanged(object sender, EventArgs e)
        {
            _filterStatusCode = GetFilterReportCode(cbFilterStatusCode.Text);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cbFilterStatusCode.Items.AddRange(Enum.GetNames(typeof(ReportStatusCodes)));
            cbFilterStatusCode.SelectedItem = cbFilterStatusCode.Items[0];
            var path = Path.Combine(Environment.CurrentDirectory, "Data", "export.txt");
            if (!File.Exists(path))
            {
                throw new Exception("Please, make sure the export.txt exists in 'Data' directory!");
            }

            ListReport = ReportData.CreateReports(path).ToList();
        }
    }
}
