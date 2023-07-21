namespace ExchangeService.BankProvider.Models
{
    public class JsonExchangeRate
    {
        public string Currency { get; set; }

        public decimal SaleRate { get; set; }

        public decimal PurchaseRate { get; set; }
    }
}