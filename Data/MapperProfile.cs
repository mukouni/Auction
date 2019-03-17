using Auction;
using Auction.Identity.Entities;
using Auction.Models;
using Auction.Models.AccountViewModels;
using Auction.Models.ManageViewModels;
using Auction.Models.EquipmentViewModels;
using Microsoft.AspNetCore.Identity;
using System.Data;
using AutoMapper;
using Auction.Entities;
using System.Linq;

namespace Auction.Data.AutoMapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Equipment, EquipmentViewModel>()
                .ForMember(x => x.CoverPhoto, opt => 
                    opt.MapFrom((src, dest, destMember, context) => 
                                    destMember = src.Photos.FirstOrDefault(p => p.IsCover == true)));
            CreateMap<EquipmentViewModel, Equipment>();

            CreateMap<Photo, PhotoViewModel>()
                .ForMember(x => x.Equipment, opt => opt.Ignore());
            CreateMap<PhotoViewModel, Photo>()
                .ForMember(x => x.Equipment, opt => opt.Ignore());

            CreateMap<Photo, EquipmentPhotoViewModel>()
            .ForMember(x => x.Equipment, opt => opt.Ignore());
            CreateMap<EquipmentPhotoViewModel, Photo>()
                .ForMember(x => x.Equipment, opt => opt.Ignore());
        }
    }
}