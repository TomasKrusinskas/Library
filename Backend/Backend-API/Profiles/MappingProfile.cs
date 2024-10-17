using AutoMapper;
using Backend_API.Models;

namespace Backend_API.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ReservationDto, Reservation>();
            CreateMap<Reservation, ReservationDto>()
                .ForMember(dest => dest.BookName, opt => opt.MapFrom(src => src.Book.Name))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.TotalPrice));
        }
    }
}
