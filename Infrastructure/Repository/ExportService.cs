using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using PracticeForRevision.DAL;
using PracticeForRevision.Infrastructure.Interface;

namespace PracticeForRevision.Infrastructure.Repository
{
    public class ExportService : IExportService
    {
        private readonly ApplicationDbContext _dbContext;

        public ExportService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<byte[]> ExportDataToExcelAsync()
        {
            var paymentDetails = await _dbContext.PaymentDetails.ToListAsync();
            var products = await _dbContext.Products.ToListAsync();
            var logs = await _dbContext.LogEntries.ToListAsync(); 
            var errorLogs = await _dbContext.ErrorLogEntries.ToListAsync();

            // Create Excel package
            using (var package = new ExcelPackage())
            {
                // Add a worksheet for PaymentDetails
                var paymentSheet = package.Workbook.Worksheets.Add("PaymentDetails");
                paymentSheet.Cells.LoadFromCollection(paymentDetails, true);

                // Add a worksheet for Products
                var productSheet = package.Workbook.Worksheets.Add("Products");
                productSheet.Cells.LoadFromCollection(products, true);

                //Normal logs
                var logSheet = package.Workbook.Worksheets.Add("LogEntries");
                logSheet.Cells.LoadFromCollection(logs, true);

                //Error Logs
                var errorLogSheet = package.Workbook.Worksheets.Add("ErrorLogEntries");
                errorLogSheet.Cells.LoadFromCollection(errorLogs, true);

                // Convert package to bytes and return
                return package.GetAsByteArray();
            }
        }
    }

}
