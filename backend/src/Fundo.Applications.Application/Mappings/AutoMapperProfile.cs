using AutoMapper;
using Fundo.Applications.Application.DTOs;
using Fundo.Applications.Domain.Entities;

namespace Fundo.Applications.Application.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Loan, LoanDto>().ReverseMap();
        }
    }
}