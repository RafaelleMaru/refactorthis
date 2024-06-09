using RefactorThis.Application.Invoices;
using RefactorThis.Domain;
using RefactorThis.Domain.Enums;
using Constants = RefactorThis.Domain.Constants;

namespace RefactorThis.Service
{
    public class InvoiceService : ServiceBase
    {
        public Invoice Invoice { get; set; }
        public async Task<string> ProcessPayment(Payment payment)
        {
            // Fetch the invoice using the mediator
            var inv = await Mediator.Send(new Details.Query { Id = payment.InvoiceId });

            // Check if the invoice exists
            if (inv == null)
            {
                throw new InvalidOperationException(Constants.NoInvoiceMatchingThisPayment);
            }

            string responseMessage;

            // Process the invoice based on its amount and payments
            if (inv.Amount == 0)
            {
                responseMessage = ProcessZeroAmountInvoice(inv);
            }
            else
            {
                responseMessage = await ProcessNonZeroAmountInvoice(inv, payment);
            }

            // Save the updated invoice
            await Mediator.Send(new Edit.Command { Invoice = inv });

            return responseMessage;
        }

        private string ProcessZeroAmountInvoice(Invoice inv)
        {
            if (inv.Payments == null || !inv.Payments.Any())
            {
                return Constants.NoPaymentNeeded;
            }
            else
            {
                throw new InvalidOperationException(Constants.InvalidInvoiceZeroAmountWithPayments);
            }
        }

        private async Task<string> ProcessNonZeroAmountInvoice(Invoice inv, Payment payment)
        {
            string responseMessage;

            if (inv.Payments != null && inv.Payments.Any())
            {
                var totalPaid = inv.Payments.Sum(x => x.Amount);

                if (totalPaid != 0 && inv.Amount == totalPaid)
                {
                    return Constants.InvoiceWasAlreadyFullyPaid;
                }
                else if (totalPaid != 0 && payment.Amount > (inv.Amount - inv.AmountPaid))
                {
                    return Constants.ThePaymentIsGreaterThanPartialAmountRemaining;
                }
                else
                {
                    responseMessage = ProcessPartialOrFullPayment(inv, payment);
                }
            }
            else
            {
                responseMessage = ProcessInitialPayment(inv, payment);
            }

            return responseMessage;
        }

        private string ProcessPartialOrFullPayment(Invoice inv, Payment payment)
        {
            if ((inv.Amount - inv.AmountPaid) == payment.Amount)
            {
                inv.AmountPaid += payment.Amount;
                inv.Payments.Add(payment);
                return GetFinalPaymentResponseMessage(inv);
            }
            else
            {
                inv.AmountPaid += payment.Amount;
                inv.Payments.Add(payment);
                return GetPartialPaymentResponseMessage(inv);
            }
        }

        private string ProcessInitialPayment(Invoice inv, Payment payment)
        {
            if (payment.Amount > inv.Amount)
            {
                return Constants.PaymentIsGreaterThanInvoiceAmount;
            }
            else if (inv.Amount == payment.Amount)
            {
                inv.AmountPaid = payment.Amount;
                inv.Payments.Add(payment);
                return GetFinalPaymentResponseMessage(inv);
            }
            else
            {
                inv.AmountPaid = payment.Amount;
                inv.Payments.Add(payment);
                return GetPartialPaymentResponseMessage(inv);
            }
        }

        private string GetFinalPaymentResponseMessage(Invoice inv)
        {
            if (inv.Type == InvoiceType.Commercial)
            {
                inv.TaxAmount += inv.AmountPaid * 0.14m;
            }
            return Constants.InvoiceIsNowFullyPaid;
        }

        private string GetPartialPaymentResponseMessage(Invoice inv)
        {
            if (inv.Type == InvoiceType.Commercial)
            {
                inv.TaxAmount += inv.AmountPaid * 0.14m;
            }
            return Constants.InvoicePartiallyPaid;
        }
    }
}