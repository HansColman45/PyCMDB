using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Html;
using Aspose.Html.Saving;
using CMDB.Models;
using Microsoft.AspNetCore.Hosting;

namespace CMDB.Util
{
    public class PDFGenerator
    {
        private string HTML;
        private string _type;
        private readonly List<Device> devices = new List<Device>();
        private readonly List<IdenAccount> accounts = new List<IdenAccount>();
        public string Type { 
            set 
            {
                if (!String.IsNullOrEmpty(value))
                {
                    this.HTML += "<H1>Release Form</h1>";
                    _type = value;
                }
                else
                    this.HTML += "<H1>Release Form</h1>";
            }
            get => _type;
        }
        public string Language;
        public string Receiver;
        public string UserID;
        public string Singer;
        public string ITEmployee;

        public PDFGenerator()
        {
            this.HTML = "<HTML>";
            this.HTML += "<head>";
            this.HTML += "<link rel=\"stylesheet\" href=\"https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css\" integrity=\"sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO\" crossorigin=\"anonymous\" />";
            this.HTML += "<script src=\"https://code.jquery.com/jquery-3.3.1.min.js\"></script>";
            this.HTML += "</head>";
            this.HTML += "<body>";
        }
        public void SetAssetInfo(Device device)
        {
            devices.Add(device);
        }
        public void SetAccontInfo(IdenAccount idenaccount)
        {
            accounts.Add(idenaccount);
        }
        public void GeneratePDF(IWebHostEnvironment _env)
        {
            string path;
            DateTime date = DateTime.Now;
            if (!String.IsNullOrEmpty(this.Type))
            {
                path = _env.WebRootPath +@"\PDF-Files\release_" +this.UserID+"_"+date.ToString("dd-MM-yyyy-HH-mm-ss")+".pdf";
                switch (this.Language)
                {
                    case "NL":
                        this.HTML += "<p>Beste " + this.Receiver + " Gelieve te teken voor het terug geven van het volgende materiaal:</p>";
                        break;
                    case "EN":
                        this.HTML += "<p>Dear " + this.Receiver + " please sing for the recievment of the following material:</p>";
                        break;
                    case "FR":
                        this.HTML += "<p>Dear " + this.Receiver + " please sing for the recievment of the following material:</p>";
                        break;
                }
            }
            else
            {
                path = _env.WebRootPath + @"\PDF-Files\Assign_" + this.UserID + "_" + date.ToString("dd-MM-yyyy-HH-mm-ss") + ".pdf";
            }
            if(accounts.Count > 0)
            {
                switch (this.Language)
                {
                    case "NL":
                        this.HTML += "<h3>Gegevens van de account</h3>";
                        break;
                    case "EN":
                        this.HTML += "<h3>Info of the account</h3>";
                        break;
                    case "FR":
                        this.HTML += "<h3>Info of the account</h3>";
                        break;
                }
                this.HTML += "<table class=\"table table-striped table-bordered\">";
                this.HTML += "<thead>";
                this.HTML += "<tr>";
                this.HTML += "<th>UserID</th>";
                this.HTML += "<th>Application</th>";
                this.HTML += "<th>From</th>";
                this.HTML += "<th>Until</th>";
                this.HTML += "</tr>";
                this.HTML += "</thead>";
                this.HTML += "<tbody>";
                foreach (IdenAccount a in accounts)
                {
                    this.HTML += "<tr>";
                    this.HTML += "<td>"+a.Account.UserID+"</td>";
                    this.HTML += "<td>"+ a.Account.Application.Name+"</td>";
                    this.HTML += "<td>"+a.ValidFrom.ToString("dd/MM/yyyy") +"</td>";
                    this.HTML += "<td>"+a.ValidUntil.ToString("dd/MM/yyyy") +"</td>";
                    this.HTML += "</tr>";
                }
                this.HTML += "</tbody>";
                this.HTML += "</table>";
            }
            if(devices.Count > 0)
            {
                if (!String.IsNullOrEmpty(this.Type))
                {
                    switch (this.Language)
                    {
                        case "NL":
                            this.HTML += "<h3>Gegevens van het terug gebracht matteriaal</h3>";
                            break;
                        case "EN":
                            this.HTML += "<h3>Info of the returned device</h3>";
                            break;
                        case "FR":
                            this.HTML += "<h3>Info of the returned device</h3>";
                            break;
                    }
                }
                else
                {
                    switch (this.Language)
                    {
                        case "NL":
                            this.HTML += "<h3>Gegevens van het ontvangen matteriaal</h3>";
                            break;
                        case "EN":
                            this.HTML += "<h3>Info about the received device</h3>";
                            break;
                        case "FR":
                            this.HTML += "<h3>Info about the received device</h3>";
                            break;
                    }
                }
                this.HTML += "<table class=\"table table-striped table-bordered\">";
                this.HTML += "<thead>";
                this.HTML += "<tr>";
                this.HTML += "<th>Category</th>";
                this.HTML += "<th>Asset Type</th>";
                this.HTML += "<th>AssetTag</th>";
                this.HTML += "<th>SerialNumber</th>";
                this.HTML += "</tr>";
                this.HTML += "</thead>";
                this.HTML += "<tbody>";
                foreach(Device d in devices)
                {
                    this.HTML += "<tr>";
                    this.HTML += "<td>"+d.Category.Category+"</td>";
                    this.HTML += "<td>"+d.Type.Vendor + " "+d.Type.Type +"</td>";
                    this.HTML += "<td>"+d.AssetTag+"</td>";
                    this.HTML += "<td>"+d.SerialNumber+"</td>";
                    this.HTML += "</tr>";
                }
                this.HTML += "</tbody>";
                this.HTML += "</table>";
            }
            if(!String.IsNullOrEmpty(this.Singer) && !String.IsNullOrEmpty(this.ITEmployee))
            {
                switch (this.Language)
                {
                    case "NL":
                        this.HTML += "<h3>Info van wie er tekent</h3>";
                        break;
                    case "EN":
                        this.HTML += "<h3>EN</h3>";
                        break;
                    case "FR":
                        this.HTML += "<h3>FR</h3>";
                        break;
                }
                this.HTML += "Please sing here: <br>";
                this.HTML += "<table class=\"table table-striped table-bordered\">";
                this.HTML += "<thead>";
                this.HTML += "<tr>";
                this.HTML += "<th>Employee Info</th>";
                this.HTML += "<th>IT Employee Info</th>";
                this.HTML += "</tr>";
                this.HTML += "</thead>";
                this.HTML += "<tbody>";
                this.HTML += "<tr>";
                this.HTML += "<td>"+ this.Singer + "</td>";
                this.HTML += "<td>"+ this.ITEmployee + "</td>";
                this.HTML += "</tr>";
                this.HTML += "<tr>";
                this.HTML += "<td><textarea rows=\"4\" cols=\"50\"> </textarea></td>";
                this.HTML += "<td><textarea rows=\"4\" cols=\"50\"> </textarea></td>";
                this.HTML += "</tr>";
                this.HTML += "</tbody>";
                this.HTML += "</table>";
            }
            this.HTML += "</div>";
            this.HTML += "</body>";
            this.HTML += "</html>";
            //MemoryStream stringInMemoryStream =
            //   new MemoryStream(ASCIIEncoding.Default.GetBytes(HTML));
            var document = new Aspose.Html.HTMLDocument(HTML, ".");
            Aspose.Html.Converters.Converter.ConvertHTML(document, new PdfSaveOptions(), path);
        }
    }
}
