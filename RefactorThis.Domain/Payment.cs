

namespace RefactorThis.Domain
{
    public class Payment
    {
        public Guid InvoiceId { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
    }
}
