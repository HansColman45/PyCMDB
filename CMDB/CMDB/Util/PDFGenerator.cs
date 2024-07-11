using CMDB.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;

namespace CMDB.Util
{
    public class PDFGenerator : IDocument
    {
        private readonly List<Device> devices = new();
        private readonly List<Mobile> mobiles = new();
        private readonly List<IdenAccount> accounts = new();
        private readonly List<Subscription> subscriptions = new();
        private readonly TextStyle h3Style = TextStyle.Default.FontFamily("Arial").FontSize(16).SemiBold().FontColor(Colors.Black);
        private readonly TextStyle titleStyle = TextStyle.Default.FontFamily("Arial").FontSize(20).SemiBold().FontColor(Colors.Black);
        private readonly TextStyle defaultStyle = TextStyle.Default.FontFamily("Arial").FontSize(9);
        public string Type { get; set; }
        public string Language { get; set; }
        public string Receiver { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserID { get; set; }
        public string Singer { get; set; }
        public string ITEmployee { get; set; }
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;
        public DocumentSettings GetSettings() => DocumentSettings.Default;
        public PDFGenerator()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }
        public void SetAssetInfo(Device device)
        {
            devices.Add(device);
        }
        public void SetMobileInfo(Mobile mobile)
        {
            mobiles.Add(mobile);
        }
        public void SetAccontInfo(IdenAccount idenaccount)
        {
            accounts.Add(idenaccount);
        }
        public void SetSubscriptionInfo(Subscription subscription)
        {
            subscriptions.Add(subscription);
        }
        public string GeneratePath(IWebHostEnvironment _env)
        {
            string path;
            DateTime date = DateTime.Now;
            if (!String.IsNullOrEmpty(Type))
                path = _env.WebRootPath + @$"\PDF-Files\release_{UserID}_{date:dd-MM-yyyy-HH-mm-ss}.pdf";
            else
                path = _env.WebRootPath + @$"\PDF-Files\Assign_{UserID}_{date:dd-MM-yyyy-HH-mm-ss}.pdf";
            return path;
        }
        /// <summary>
        /// This is the general method to generate the PDF
        /// </summary>
        /// <param name="container"></param>
        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(50);
                //Generate Header
                page.Header()
                    .Height(50)
                    .Background(Colors.Grey.Lighten1)
                    .Element(ComposeHeader);
                //Generate Body
                page.Content()
                    .Element(ComposeBody);
                //Generate Footer
                page.Footer()
                    .Height(50)
                    .Background(Colors.Grey.Lighten1)
                    .AlignCenter()
                    .Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
            });
        }
        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(colum =>
                {
                    if (string.IsNullOrEmpty(Type))
                    {
                        colum.Item().Text("Relase Form").AlignCenter().Style(titleStyle);
                        colum.Item().AlignLeft().Text(" ").Style(defaultStyle);
                        switch (Language)
                        {
                            case "NL":
                                colum.Item()
                                    .AlignLeft()
                                    .Text($"Beste {Receiver} gelieve te teken voor het terug geven van het volgene materiaal")
                                    .Style(defaultStyle).Bold();
                                break;
                            case "EN":
                                colum.Item()
                                    .AlignLeft()
                                    .Text($"Dear {Receiver} please sign for the receivment of the following material")
                                    .Bold()
                                    .Style(defaultStyle);
                                break;
                            case "FR":
                                colum.Item()
                                    .AlignLeft()
                                    .Text($"Dear {Receiver} please sign for the receivment of the following material")
                                    .Style(defaultStyle)
                                    .Bold();
                                break;
                        }
                    }
                    else
                    {
                        colum.Item().Text("Asign Form").AlignCenter().Style(titleStyle);
                        colum.Item().AlignLeft().Text(" ").Style(defaultStyle);
                        switch (Language)
                        {
                            case "NL":
                                colum.Item().AlignLeft().Text($"Beste {Receiver} gelieve te teken voor het ontvangen van het volgene materiaal").Style(defaultStyle).Bold();
                                break;
                            case "EN":
                                colum.Item().AlignLeft().Text($"Dear {Receiver} please sign for the recievement of the following material").Bold().Style(defaultStyle);
                                break;
                            case "FR":
                                colum.Item().AlignLeft().Text($"Dear {Receiver} please sign for the receivment of the following material").Bold().Style(defaultStyle);
                                break;
                        }
                    }
                });
            });
        }
        private void ComposeEmployeTitle(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(colum =>
                {
                    switch (Language)
                    {
                        case "NL":
                            colum.Item().Text("Gegevens van de employee").Style(h3Style);
                            break;
                        case "EN":
                            colum.Item().Text("Info from the employee").Style(h3Style);
                            break;
                        case "FR":
                            colum.Item().Text("Info from the employee").Style(h3Style);
                            break;
                    }
                });
            });
        }
        private void ComposeBody(IContainer container)
        {
            container
                .PaddingVertical(10)
                .Background(Colors.Grey.Lighten3)
                .AlignLeft()
                .Column(column =>
                    {
                        column.Spacing(5);
                        column.Item().Element(ComposeEmployeTitle);
                        column.Item().Element(ComposeEmployeeTable);
                        if (accounts.Count > 0)
                        {
                            column.Item().Element(ComposeAccountTitle);
                            column.Item().Element(ComposeAccountTable);
                        }
                        if (devices.Count > 0)
                        {
                            column.Item().Element(ComposeDeviceTitle);
                            column.Item().Element(ComposeDeviceTable);
                        }
                        if (mobiles.Count > 0)
                        {
                            column.Item().Element(ComposeMobileTitle);
                            column.Item().Element(ComposeMobileTable);
                        }
                        if(subscriptions.Count > 0)
                        {
                            column.Item().Element(ComposeSubscriptionTitle);
                            column.Item().Element(ComposeSubscriptionTable);
                        }
                        if (!String.IsNullOrEmpty(Singer) && !String.IsNullOrEmpty(ITEmployee))
                        {
                            column.Item().Element(ComposeSinger);
                            column.Item().Element(ComposeSingerTable);
                        }
                    });
        }
        private void ComposeDeviceTitle(IContainer container)
        {
            if (string.IsNullOrEmpty(Type))
            {
                container.Row(row =>
                {
                    row.RelativeItem().Column(colum =>
                    {
                        switch (Language)
                        {
                            case "NL":
                                colum.Item().Text("Gegevens van het terug gebracht matteriaal").Style(h3Style);
                                break;
                            case "EN":
                                colum.Item().Text("Gegevens van het terug gebracht matteriaal").Style(h3Style);
                                break;
                            case "FR":
                                colum.Item().Text("Gegevens van het terug gebracht matteriaal").Style(h3Style);
                                break;
                        }
                    });
                });
            }
            else
            {
                container.Row(row =>
                {
                    row.RelativeItem().Column(colum =>
                    {
                        switch (Language)
                        {
                            case "NL":
                                colum.Item().Text("Gegevens van het ontvangen matteriaal").Style(h3Style);
                                break;
                            case "EN":
                                colum.Item().Text("Gegevens van het ontvangen matteriaal").Style(h3Style);
                                break;
                            case "FR":
                                colum.Item().Text("Gegevens van het ontvangen matteriaal").Style(h3Style);
                                break;
                        }
                    });
                });
            }
        }
        private void ComposeDeviceTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });
                table.Header(header =>
                {
                    header.Cell().Element(Style).Text("Category").Style(defaultStyle).ExtraBold();
                    header.Cell().Element(Style).Text("Asset Type").Style(defaultStyle).ExtraBold();
                    header.Cell().Element(Style).Text("AssetTag").Style(defaultStyle).ExtraBold();
                    header.Cell().Element(Style).Text("SerialNumber").Style(defaultStyle).ExtraBold();
                    IContainer Style(IContainer container)
                    {
                        return container
                            .BorderTop(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderLeft(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderRight(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .Padding(5);
                    }
                });
                foreach (var device in devices) 
                { 
                    table.Cell().Element(Style).Text(device.Category.Category).Style(defaultStyle);
                    table.Cell().Element(Style).Text($"{device.Type}").Style(defaultStyle);
                    table.Cell().Element(Style).Text(device.AssetTag).Style(defaultStyle);
                    table.Cell().Element(Style).Text(device.SerialNumber).Style(defaultStyle);
                    IContainer Style(IContainer container)
                    {
                        return container
                            .BorderLeft(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderRight(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .Padding(5);
                    }
                }
            });
        }
        private void ComposeSinger(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(colum =>
                {
                    switch (Language)
                    {
                        case "NL":
                            colum.Item().Text("Info van wie er tekent").Style(h3Style);
                            colum.Item().Text("Gelieve hier te tekenen:").Style(defaultStyle);
                            break;
                        case "EN":
                            colum.Item().Text("Info about who will sign").Style(h3Style);
                            colum.Item().Text("Please sing here:").Style(defaultStyle);
                            break;
                        case "FR":
                            colum.Item().Text("Info van wie er tekent").Style(h3Style);
                            colum.Item().Text("Please sing here:").Style(defaultStyle);
                            break;
                    }
                });
            });
        }
        private void ComposeSingerTable(IContainer container)
        {
            container.Table(table =>
            {
                //First set the definition of the columns
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });
                //Then set the Header
                table.Header(header =>
                {
                    header.Cell().Element(Style).Text("Employee").Style(defaultStyle).ExtraBold();
                    header.Cell().Element(Style).Text("IT Employee").Style(defaultStyle).ExtraBold();
                    IContainer Style(IContainer container)
                    {
                        return container
                            .BorderTop(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderLeft(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderRight(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .Padding(5);
                    }
                });
                //Now set the details
                table.Cell().Element(Style).Text(Singer).Style(defaultStyle); 
                table.Cell().Element(Style).Text(ITEmployee).Style(defaultStyle);
                table.Cell().Element(Style).Text("Signature:").Style(defaultStyle);
                table.Cell().Element(Style).Text("Signature:").Style(defaultStyle);
                table.Cell().Element(SignStyle).MinHeight(50).Text("");
                table.Cell().Element(SignStyle).MinHeight(50).Text("");
                IContainer Style(IContainer container)
                {
                    return container
                        .BorderLeft(1)
                        .BorderColor(Colors.Grey.Darken2)
                        .BorderBottom(1)
                        .BorderColor(Colors.Grey.Darken2)
                        .BorderRight(1)
                        .BorderColor(Colors.Grey.Darken2)
                        .Padding(5);
                }
                IContainer SignStyle(IContainer container)
                {
                    return container
                        .DefaultTextStyle(x => x.ExtraLight())
                        .BorderTop(1)
                        .BorderColor(Colors.Red.Accent4)
                        .BorderLeft(1)
                        .BorderColor(Colors.Red.Accent4)
                        .BorderBottom(1)
                        .BorderColor(Colors.Red.Accent4)
                        .BorderRight(1)
                        .BorderColor(Colors.Red.Accent4)
                        .Padding(5);
                }
            });
        }
        private void ComposeEmployeeTable(IContainer container)
        {
            container.Table(table =>
            {
                //First set the definition of the columns
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });
                //Then set the Header
                table.Header(header =>
                {
                    header.Cell().Element(Style).Text("FirstName").Style(defaultStyle);
                    header.Cell().Element(Style).Text("LastName").Style(defaultStyle);
                    header.Cell().Element(Style).Text("UserID").Style(defaultStyle);
                    IContainer Style(IContainer container)
                    {
                        return container
                            .BorderTop(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderLeft(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderRight(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .Padding(5);
                    }
                });
                //Now set the body
                table.Cell().Element(Style).Text(FirstName).Style(defaultStyle);
                table.Cell().Element(Style).Text(LastName).Style(defaultStyle);
                table.Cell().Element(Style).Text(UserID).Style(defaultStyle);
                IContainer Style(IContainer container)
                {
                    return container
                        .BorderLeft(1)
                        .BorderColor(Colors.Grey.Darken2)
                        .BorderBottom(1)
                        .BorderColor(Colors.Grey.Darken2)
                        .BorderRight(1)
                        .BorderColor(Colors.Grey.Darken2)
                        .Padding(5);
                }
            });
        }
        private void ComposeAccountTitle(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(colum =>
                {
                    switch (Language)
                    {
                        case "NL":
                            colum.Item().Text("Gegevens van de account").Style(h3Style);
                            break;
                        case "EN":
                            colum.Item().Text("Info from the account").Style(h3Style);
                            break;
                        case "FR":
                            colum.Item().Text("Info from the account").Style(h3Style);
                            break;
                    }
                });
            });
        }
        private void ComposeAccountTable(IContainer container)
        {
            container.Table(table =>
            {
                //First set the definition of the columns
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });
                //Then set the Header
                table.Header(header =>
                {
                    header.Cell().Element(Style).Text("UserID").Style(defaultStyle);
                    header.Cell().Element(Style).Text("Application").Style(defaultStyle);
                    header.Cell().Element(Style).Text("From").Style(defaultStyle);
                    header.Cell().Element(Style).Text("Until").Style(defaultStyle);
                    IContainer Style(IContainer container)
                    {
                        return container
                            .BorderTop(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderLeft(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderRight(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .Padding(5);
                    }
                });
                //Now set the details
                foreach (IdenAccount a in accounts) { 
                    table.Cell().Element(Style).Text(a.Account.UserID).Style(defaultStyle);
                    table.Cell().Element(Style).Text(a.Account.Application.Name).Style(defaultStyle);
                    table.Cell().Element(Style).Text(a.ValidFrom.ToString("dd/MM/yyyy")).Style(defaultStyle);
                    table.Cell().Element(Style).Text(a.ValidUntil.ToString("dd/MM/yyyy")).Style(defaultStyle);
                    IContainer Style(IContainer container)
                    {
                        return container
                            .BorderLeft(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderRight(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .Padding(5);
                    }
                }
            });
        }
        private void ComposeMobileTitle(IContainer container)
        {
            if (string.IsNullOrEmpty(Type))
            {
                container.Row(row =>
                {
                    row.RelativeItem().Column(colum =>
                    {
                        switch (Language)
                        {
                            case "NL":
                                colum.Item().Text("Gegevens van de teruggebrachte GSM").Style(h3Style);
                                break;
                            case "EN":
                                colum.Item().Text("Gegevens van de teruggebrachte GSM").Style(h3Style);
                                break;
                            case "FR":
                                colum.Item().Text("Gegevens van de teruggebrachte GSM").Style(h3Style);
                                break;
                        }
                    });
                });
            }
            else
            {
                container.Row(row =>
                {
                    row.RelativeItem().Column(colum =>
                    {
                        switch (Language)
                        {
                            case "NL":
                                colum.Item().Text("Gegevens van de ontvangen GSM").Style(h3Style);
                                break;
                            case "EN":
                                colum.Item().Text("Info of the recieved Mobile").Style(h3Style);
                                break;
                            case "FR":
                                colum.Item().Text("Gegevens van de ontvangen GSM").Style(h3Style);
                                break;
                        }
                    });
                });
            }
        }
        private void ComposeMobileTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });
                table.Header(header =>
                {
                    header.Cell().Element(Style).Text("Category").Style(defaultStyle).ExtraBold();
                    header.Cell().Element(Style).Text("Asset Type").Style(defaultStyle).ExtraBold();
                    header.Cell().Element(Style).Text("IMEI").Style(defaultStyle).ExtraBold();
                    IContainer Style(IContainer container)
                    {
                        return container
                            .BorderTop(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderLeft(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderRight(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .Padding(5);
                    }
                });
                foreach(var mobile in mobiles)
                {
                    table.Cell().Element(Style).Text(mobile.Category.Category).Style(defaultStyle);
                    table.Cell().Element(Style).Text($"{mobile.MobileType}").Style(defaultStyle);
                    table.Cell().Element(Style).Text($"{mobile.IMEI}").Style(defaultStyle);
                    IContainer Style(IContainer container)
                    {
                        return container
                            .BorderLeft(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderRight(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .Padding(5);
                    }
                }
            });
        }
        private void ComposeSubscriptionTitle(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(colum =>
                {
                    switch (Language)
                    {
                        case "NL":
                            colum.Item().Text("Gegevens van het abbonement").Style(h3Style);
                            break;
                        case "EN":
                            colum.Item().Text("Gegevens van het abbonement").Style(h3Style);
                            break;
                        case "FR":
                            colum.Item().Text("Gegevens van het abbonement").Style(h3Style);
                            break;
                    }
                });
            });
        }
        private void ComposeSubscriptionTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(col =>
                {
                    col.RelativeColumn();
                    col.RelativeColumn();
                    col.RelativeColumn();
                });
                table.Header(header =>
                {
                    header.Cell().Element(Style).Text("Category").Style(defaultStyle).ExtraBold();
                    header.Cell().Element(Style).Text("Subscription Type").Style(defaultStyle).ExtraBold();
                    header.Cell().Element(Style).Text("Phone Number").Style(defaultStyle).ExtraBold();
                    IContainer Style(IContainer container)
                    {
                        return container
                            .BorderTop(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderLeft(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderRight(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .Padding(5);
                    }
                });
                foreach(var subcription in subscriptions)
                {
                    table.Cell().Element(Style).Text(subcription.Category.Category).Style(defaultStyle);
                    table.Cell().Element(Style).Text($"{subcription.SubscriptionType}").Style(defaultStyle);
                    table.Cell().Element(Style).Text(subcription.PhoneNumber).Style(defaultStyle);
                    IContainer Style(IContainer container)
                    {
                        return container
                            .BorderLeft(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderBottom(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .BorderRight(1)
                            .BorderColor(Colors.Grey.Darken2)
                            .Padding(5);
                    }
                }
            });
        }
    }
}
