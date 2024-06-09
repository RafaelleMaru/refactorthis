using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RefactorThis.Service
{
    public class ServiceBase
    {
        private IMediator mediator { get; set; }
        protected IMediator Mediator => mediator;
    }
}
