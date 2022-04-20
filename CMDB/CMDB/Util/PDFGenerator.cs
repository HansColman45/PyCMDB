using Aspose.Html;
using CMDB.Domain.Entities;
using IronPdf;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;

namespace CMDB.Util
{
    public class PDFGenerator
    {
        private string HTML;
        private string _type;
        private readonly List<Device> devices = new();
        private readonly List<IdenAccount> accounts = new();
        public string Type
        {
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
        public string Language { get; set; }
        public string Receiver { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserID { get; set; }
        public string Singer { get; set; }
        public string ITEmployee { get; set; }

        public PDFGenerator()
        {
            HTML = "<HTML>";
            HTML += "<head>";
            HTML += "<link rel=\"stylesheet\" href=\"https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css\" integrity=\"sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO\" crossorigin=\"anonymous\" />";
            HTML += "<script src=\"https://code.jquery.com/jquery-3.3.1.min.js\"></script>";
            HTML += "</head>";
            HTML += "<body>";
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
                path = _env.WebRootPath + @"\PDF-Files\release_" + this.UserID + "_" + date.ToString("dd-MM-yyyy-HH-mm-ss") + ".pdf";
                switch (this.Language)
                {
                    case "NL":
                        HTML += "<p>Beste " + this.Receiver + " Gelieve te teken voor het terug geven van het volgende materiaal:</p>";
                        break;
                    case "EN":
                        HTML += "<p>Dear " + this.Receiver + " please sing for the recievment of the following material:</p>";
                        break;
                    case "FR":
                        HTML += "<p>Dear " + this.Receiver + " please sing for the recievment of the following material:</p>";
                        break;
                }
            }
            else
            {
                path = _env.WebRootPath + @"\PDF-Files\Assign_" + this.UserID + "_" + date.ToString("dd-MM-yyyy-HH-mm-ss") + ".pdf";
            }
            switch (Language)
            {
                case "NL":
                    HTML += "<h3>Gegevens van de employee</h3>";
                    break;
                case "EN":
                    HTML += "<h3>Info from the employee</h3>";
                    break;
                case "FR":
                    HTML += "<h3>Info from the employee</h3>";
                    break;
            }
            HTML += "<table class=\"table table-striped table-bordered\">";
            HTML += "<thead>";
            HTML += "<tr>";
            HTML += "<th>FirstName</th>";
            HTML += "<th>LastName</th>";
            HTML += "<th>UserId</th>";
            HTML += "</tr>";
            HTML += "</thead>";
            HTML += "<tbody>";
            HTML += "<tr>";
            HTML += $"<td>{FirstName}</td>";
            HTML += $"<td>{LastName}</td>";
            HTML += $"<td>{UserID}</td>";
            HTML += "</tr>";
            HTML += "</tbody>";
            HTML += "</table>";
            if (accounts.Count > 0)
            {
                switch (Language)
                {
                    case "NL":
                        HTML += "<h3>Gegevens van de account</h3>";
                        break;
                    case "EN":
                        HTML += "<h3>Info of the account</h3>";
                        break;
                    case "FR":
                        HTML += "<h3>Info of the account</h3>";
                        break;
                }
                HTML += "<table class=\"table table-striped table-bordered\">";
                HTML += "<thead>";
                HTML += "<tr>";
                HTML += "<th>UserID</th>";
                HTML += "<th>Application</th>";
                HTML += "<th>From</th>";
                HTML += "<th>Until</th>";
                HTML += "</tr>";
                HTML += "</thead>";
                HTML += "<tbody>";
                foreach (IdenAccount a in accounts)
                {
                    HTML += "<tr>";
                    HTML += "<td>" + a.Account.UserID + "</td>";
                    HTML += "<td>" + a.Account.Application.Name + "</td>";
                    HTML += "<td>" + a.ValidFrom.ToString("dd/MM/yyyy") + "</td>";
                    HTML += "<td>" + a.ValidUntil.ToString("dd/MM/yyyy") + "</td>";
                    HTML += "</tr>";
                }
                HTML += "</tbody>";
                HTML += "</table>";
            }
            if (devices.Count > 0)
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
                foreach (Device d in devices)
                {
                    this.HTML += "<tr>";
                    this.HTML += "<td>" + d.Category.Category + "</td>";
                    this.HTML += "<td>" + d.Type.Vendor + " " + d.Type.Type + "</td>";
                    this.HTML += "<td>" + d.AssetTag + "</td>";
                    this.HTML += "<td>" + d.SerialNumber + "</td>";
                    this.HTML += "</tr>";
                }
                this.HTML += "</tbody>";
                this.HTML += "</table>";
            }
            if (!String.IsNullOrEmpty(this.Singer) && !String.IsNullOrEmpty(this.ITEmployee))
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
                this.HTML += "<td>" + this.Singer + "</td>";
                this.HTML += "<td>" + this.ITEmployee + "</td>";
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
            var converter = new ChromePdfRenderer();
            var PDF = converter.RenderHTMLFileAsPdf(HTML);
            PDF.SaveAs(path);
        }
    }
}
