using Assignment.Application.Contracts.Dtos;
using Assignment.Domain.Entities;
using AutoMapper;

namespace Assignment.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Transaction, TransactionResponseDto>()
                .ForMember(d => d.Id, s => s.MapFrom(s => s.TransactionId))
                .ForMember(d => d.Payment, s => s.MapFrom(s => $"{s.Amount} {s.CurrencyCode}"))
                .ForMember(d => d.Status, s => s.MapFrom(s => GetStatus(s.Status)));
        }

        private string GetStatus(string status)
        {
            string result;
            if (status.ToLower() == "approved") result = "A";
            else if (status.ToLower() == "rejected" || status.ToLower() == "failed") result = "R";
            else if (status.ToLower() == "done" || status.ToLower() == "finished") result = "D";
            else result = "";

            return result;
        }
    }    
}
