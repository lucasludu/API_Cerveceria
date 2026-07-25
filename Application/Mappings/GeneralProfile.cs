using Application.DTOs.Request._beer;
using Application.DTOs.Request._wholesaler;
using Application.DTOs.Response._beer;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {

            //Beer
            CreateMap<Beer, BeerRequest>().ReverseMap();
            CreateMap<UpdateBeerRequest, Beer>();
            CreateMap<Beer, BeerResponse>();

            //Wholesaler
            CreateMap<AddBeerSaleRequest, WholesaleInventory>()
                .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.Quantity));

        }
    }
}
