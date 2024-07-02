namespace PracticeForRevision.Infrastructure.Interface
{
    public interface IExportService
    {
        Task<byte[]> ExportDataToExcelAsync();
    }
}
