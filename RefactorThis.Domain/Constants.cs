using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorThis.Domain
{
    public static class Constants
    {
        public const string NoInvoiceMatchingThisPayment = "There is no invoice matching this payment.";
        public const string NoPaymentNeeded = "No payment needed.";
        public const string InvalidInvoiceZeroAmountWithPayments = "The invoice is in an invalid state, it has an amount of 0 and it has payments.";
        public const string InvoiceWasAlreadyFullyPaid = "invoice was already fully paid";
        public const string ThePaymentIsGreaterThanPartialAmountRemaining = "the payment is greater than the partial amount remaining";
        public const string FinalPartialPaymentReceivedFullyPaidInvoice = "final partial payment received, invoice is now fully paid";
        public const string PartialPaymentReceivedNotFullyPaid = "another partial payment received, still not fully paid";
        public const string PaymentIsGreaterThanInvoiceAmount = "the payment is greater than the invoice amount";
        public const string InvoicePartiallyPaid = "invoice is now partially paid";
        public const string InvoiceIsNowFullyPaid = "invoice is now fully paid";
    }
}
