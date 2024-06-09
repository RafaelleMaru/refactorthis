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
    public class Details
    {
        public class Query : IRequest<Invoice>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Invoice>
        {
            private readonly DataContext context;

            public Handler(DataContext context)
            {
                this.context = context;
            }

            public async Task<Invoice> Handle(Query request, CancellationToken cancellationToken)
            {
                return await context.Invoices.FindAsync(request.Id);
            }
        }
    }
}
