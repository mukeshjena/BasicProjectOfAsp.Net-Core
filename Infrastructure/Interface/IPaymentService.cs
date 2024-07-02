namespace PracticeForRevision.Infrastructure.Interface
{
    public interface IPaymentService
    {
        Task<string> CreatePayPalPaymentAsync(string amount);
        Task StorePaymentDetailsAsync(string paymentId, string token, string payerId);
    }
}
