using AutoMapper;
using MediatR;
using RefactorThis.Domain;
using RefactorThis.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorThis.Application.Invoices
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Invoice Invoice { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private DataContext context;

            private readonly IMapper mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            public async Task Handle(Command request, CancellationToken cancellationToken)
            {
                var invoice = await context.Invoices.FindAsync(request.Invoice.Id);

                mapper.Map(request.Invoice, invoice);

                _ = await context.SaveChangesAsync();

            }
        }
    }
}
