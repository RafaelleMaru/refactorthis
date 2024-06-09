using RefactorThis.Domain.Enums;

namespace RefactorThis.Domain
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public decimal TaxAmount { get; set; }
        public List<Payment> Payments { get; set; }

        public InvoiceType Type { get; set; }
    }
}