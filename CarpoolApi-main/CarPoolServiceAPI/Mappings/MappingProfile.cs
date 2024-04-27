using AutoMapper;
using CarPoolService.Models.DBModels;
using DTO = CarpoolService.Contracts.DTOs;


namespace CarPoolServiceAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, DTO.User>().ReverseMap(); 
            CreateMap<CarPoolRide, DTO.Ride>().ReverseMap();
            CreateMap<Booking, DTO.Booking>().ReverseMap();
            CreateMap<City, DTO.City>().ReverseMap();
        }
    }
}
