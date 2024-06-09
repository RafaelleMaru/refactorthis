using AutoMapper;
using RefactorThis.Domain;

namespace RefactorThis.Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<Invoice, Invoice>();
        }
    }
}
