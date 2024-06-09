using MediatR;
using RefactorThis.Domain;
using RefactorThis.Persistence;

namespace RefactorThis.Application.Invoices
{
    public class Create
    {
        public class Command : IRequest
        {
            public Invoice Invoice { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            public DataContext context;

            public Handler(DataContext context)
            {
                this.context = context;
            }
            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                context.Invoices.Add(request.Invoice);

                await context.SaveChangesAsync();
            }

            

        }
    }
}
