using Microsoft.AspNetCore.Mvc;

namespace CMDB.API.Services
{
    public class LogService : ControllerBase
    {  
        private readonly IUnitOfWork _uow;
        private string LogText = "";
        public LogService(IUnitOfWork uof)
        {
            _uow = uof;
        }
        public async Task LogPdfFile(string table, int Id, string pdfFile)
        {
            pdfFile = pdfFile[36..];
            pdfFile = pdfFile.Replace('\\', '/');
            pdfFile = "../.." + pdfFile;
            LogText = $"Please find the PDFFile <a href='{pdfFile}' target='_blank'>here</a>";
            //await DoLog(table, Id);
        }
        public async Task LogPdfFile(string table, string AssetTag, string pdfFile)
        {
            LogText = $"Please find the PDFFile <a href='{pdfFile}'>here</a>";
            //await DoLog(table, AssetTag);
        }
    }
}
