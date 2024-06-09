using NUnit.Framework.Internal;
using RefactorThis.Application.Invoices;
using RefactorThis.Domain;
using RefactorThis.Service;

namespace RefactorThis.Tests
{
    [TestFixture]
    public class InvoicePaymentProcessorTests
    {

        // private readonly Mock<Create> /////

        // Unit tests will not work. I need to check the mocking of mediatR handler in NUnit.
        // I can discuss this in the interview

        [Test]
        public void ProcessPayment_Should_ThrowException_When_NoInvoiceFoundForPaymentReference()
        {
            var handler = new Create();

            var invService = new InvoiceService();
            

            var repo = new InvoiceRepository();

            Invoice invoice = null;
            var paymentProcessor = new InvoiceService(repo);

            var payment = new Payment();
            var failureMessage = "";

            try
            {
                var result = paymentProcessor.ProcessPayment(payment);
            }
            catch (InvalidOperationException e)
            {
                failureMessage = e.Message;
            }

            Assert.That(Constants.NoInvoiceMatchingThisPayment, Is.EqualTo(failureMessage));
        }

        [Test]
        public void ProcessPayment_Should_ReturnFailureMessage_When_NoPaymentNeeded()
        {
            var repo = new InvoiceRepository();

            var invoice = new Invoice(repo)
            {
                Amount = 0,
                AmountPaid = 0,
                Payments = null
            };

            repo.Add(invoice);

            var paymentProcessor = new InvoiceService(repo);

            var payment = new Payment();

            var result = paymentProcessor.ProcessPayment(payment);

            Assert.That(result, Is.EqualTo(Constants.NoPaymentNeeded));
        }

        [Test]
        public void ProcessPayment_Should_ReturnFailureMessage_When_InvoiceAlreadyFullyPaid()
        {
            var repo = new InvoiceRepository();

            var invoice = new Invoice(repo)
            {
                Amount = 10,
                AmountPaid = 10,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = 10
                    }
                }
            };
            repo.Add(invoice);

            var paymentProcessor = new InvoiceService(repo);

            var payment = new Payment();

            var result = paymentProcessor.ProcessPayment(payment);

            Assert.That(result, Is.EqualTo(Constants.InvoiceWasAlreadyFullyPaid));
        }

        [Test]
        public void ProcessPayment_Should_ReturnFailureMessage_When_PartialPaymentExistsAndAmountPaidExceedsAmountDue()
        {
            var repo = new InvoiceRepository();
            var invoice = new Invoice(repo)
            {
                Amount = 10,
                AmountPaid = 5,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = 5
                    }
                }
            };
            repo.Add(invoice);

            var paymentProcessor = new InvoiceService(repo);

            var payment = new Payment()
            {
                Amount = 6
            };

            var result = paymentProcessor.ProcessPayment(payment);

            Assert.That(result, Is.EqualTo(Constants.ThePaymentIsGreaterThanPartialAmountRemaining));
        }

        [Test]
        public void ProcessPayment_Should_ReturnFailureMessage_When_NoPartialPaymentExistsAndAmountPaidExceedsInvoiceAmount()
        {
            var repo = new InvoiceRepository();
            var invoice = new Invoice(repo)
            {
                Amount = 5,
                AmountPaid = 0,
                Payments = new List<Payment>()
            };
            repo.Add(invoice);

            var paymentProcessor = new InvoiceService(repo);

            var payment = new Payment()
            {
                Amount = 6
            };

            var result = paymentProcessor.ProcessPayment(payment);

            Assert.That(result, Is.EqualTo(Constants.PaymentIsGreaterThanInvoiceAmount));
        }

        [Test]
        public void ProcessPayment_Should_ReturnFullyPaidMessage_When_PartialPaymentExistsAndAmountPaidEqualsAmountDue()
        {
            var repo = new InvoiceRepository();
            var invoice = new Invoice(repo)
            {
                Amount = 10,
                AmountPaid = 5,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = 5
                    }
                }
            };
            repo.Add(invoice);

            var paymentProcessor = new InvoiceService(repo);

            var payment = new Payment()
            {
                Amount = 5
            };

            var result = paymentProcessor.ProcessPayment(payment);

            Assert.That(result, Is.EqualTo(Constants.FinalPartialPaymentReceivedFullyPaidInvoice));
        }

        [Test]
        public void ProcessPayment_Should_ReturnFullyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidEqualsInvoiceAmount()
        {
            var repo = new InvoiceRepository();
            var invoice = new Invoice(repo)
            {
                Amount = 10,
                AmountPaid = 0,
                Payments = new List<Payment>() { new Payment() { Amount = 10 } }
            };
            repo.Add(invoice);

            var paymentProcessor = new InvoiceService(repo);

            var payment = new Payment()
            {
                Amount = 10
            };

            var result = paymentProcessor.ProcessPayment(payment);

            Assert.That(result, Is.EqualTo(Constants.InvoiceWasAlreadyFullyPaid));
        }

        [Test]
        public void ProcessPayment_Should_ReturnPartiallyPaidMessage_When_PartialPaymentExistsAndAmountPaidIsLessThanAmountDue()
        {
            var repo = new InvoiceRepository();
            var invoice = new Invoice(repo)
            {
                Amount = 10,
                AmountPaid = 5,
                Payments = new List<Payment>
                {
                    new Payment
                    {
                        Amount = 5
                    }
                }
            };
            repo.Add(invoice);

            var paymentProcessor = new InvoiceService(repo);

            var payment = new Payment()
            {
                Amount = 1
            };

            var result = paymentProcessor.ProcessPayment(payment);

            Assert.That(result, Is.EqualTo(Constants.PartialPaymentReceivedNotFullyPaid));
        }

        [Test]
        public void ProcessPayment_Should_ReturnPartiallyPaidMessage_When_NoPartialPaymentExistsAndAmountPaidIsLessThanInvoiceAmount()
        {
            var repo = new InvoiceRepository();
            var invoice = new Invoice(repo)
            {
                Amount = 10,
                AmountPaid = 0,
                Payments = new List<Payment>()
            };
            repo.Add(invoice);

            var paymentProcessor = new InvoiceService(repo);

            var payment = new Payment()
            {
                Amount = 1
            };

            var result = paymentProcessor.ProcessPayment(payment);

            Assert.That(result, Is.EqualTo(Constants.InvoicePartiallyPaid));
        }
    }
}