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
                .ForMember(evm => evm.DealPriceCurrencyId, conf => conf.MapFrom(e => e.DealPriceCurrencyId))
                .ForMember(evm => evm.PriceCurrencyId, conf => conf.MapFrom(e => e.PriceCurrencyId));
                // .ForMember(x => x.CoverPhoto, opt =>
                //     opt.MapFrom((src, dest, destMember, context) =>
                //                     {
                //                         destMember = src.Photos.FirstOrDefault(p => p.IsHiddenAfterSold == true);
                //                         if (destMember == null)
                //                         {
                //                             destMember = src.Photos.LastOrDefault();
                //                         }
                //                         if (destMember == null)
                //                         {
                //                             destMember = new Photo()
                //                             {
                //                                 SavePath = "\\images\\Equipment\\default.jpg",
                //                                 RequestPath = "/images/Equipment/5af6c17b-a58f-4134-80ca-7b7134f697e5.jpg",
                //                                 FileName = "default.jpg"
                //                             };
                //                         }
                //                         return destMember;
                //                     }
                //                 ));
            CreateMap<EquipmentViewModel, Equipment>()
                .ForMember(e => e.DealPriceCurrencyId, conf => conf.MapFrom(evm => evm.DealPriceCurrencyId))
                .ForMember(e => e.PriceCurrencyId, conf => conf.MapFrom(evm => evm.PriceCurrencyId));;

            CreateMap<Photo, PhotoViewModel>()
                .ForMember(x => x.Equipment, opt => opt.Ignore())
                .ForMember(x => x.BoomEquipment, opt => opt.Ignore())
                .ForMember(x => x.CabEquipment, opt => opt.Ignore())
                .ForMember(x => x.CoverEquipment, opt => opt.Ignore())
                .ForMember(x => x.EngineEquipment, opt => opt.Ignore())
                .ForMember(x => x.ExteriorEquipment, opt => opt.Ignore())
                .ForMember(x => x.TrackedChassisEquipment, opt => opt.Ignore());
            CreateMap<PhotoViewModel, Photo>()
                .ForMember(x => x.Equipment, opt => opt.Ignore())
                .ForMember(x => x.BoomEquipment, opt => opt.Ignore())
                .ForMember(x => x.CabEquipment, opt => opt.Ignore())
                .ForMember(x => x.CoverEquipment, opt => opt.Ignore())
                .ForMember(x => x.EngineEquipment, opt => opt.Ignore())
                .ForMember(x => x.ExteriorEquipment, opt => opt.Ignore())
                .ForMember(x => x.TrackedChassisEquipment, opt => opt.Ignore());

            CreateMap<Photo, EquipmentPhotoViewModel>()
                .ForMember(x => x.Equipment, opt => opt.Ignore())
                .ForMember(x => x.BoomEquipment, opt => opt.Ignore())
                .ForMember(x => x.CabEquipment, opt => opt.Ignore())
                .ForMember(x => x.CoverEquipment, opt => opt.Ignore())
                .ForMember(x => x.EngineEquipment, opt => opt.Ignore())
                .ForMember(x => x.ExteriorEquipment, opt => opt.Ignore())
                .ForMember(x => x.TrackedChassisEquipment, opt => opt.Ignore());
            CreateMap<EquipmentPhotoViewModel, Photo>()
                .ForMember(x => x.Equipment, opt => opt.Ignore())
                .ForMember(x => x.BoomEquipment, opt => opt.Ignore())
                .ForMember(x => x.CabEquipment, opt => opt.Ignore())
                .ForMember(x => x.CoverEquipment, opt => opt.Ignore())
                .ForMember(x => x.EngineEquipment, opt => opt.Ignore())
                .ForMember(x => x.ExteriorEquipment, opt => opt.Ignore())
                .ForMember(x => x.TrackedChassisEquipment, opt => opt.Ignore());


            CreateMap<ApplicationUser, ForgotPasswordViewModel>();
            CreateMap<ForgotPasswordViewModel, ApplicationUser>();


            CreateMap<ApplicationUser, Models.ManageViewModels.IndexViewModel>();
            CreateMap<Models.ManageViewModels.IndexViewModel, ApplicationUser>();


            CreateMap<ApplicationUser, ApplicationUserViewModel>()
                .ForMember(x => x.UserRoles, opt => opt.MapFrom(x => x.UserRoles))
                .ForMember(x => x.Roles, opt =>
                {
                    opt.MapFrom((src, dest, destMember, context) =>
                    {
                        if(src.UserRoles != null && src.UserRoles.Count > 0){
                            return src.UserRoles.Select(ur => ur.Role).ToList();
                        }
                        return null;
                    });
                });
            CreateMap<ApplicationUserViewModel, ApplicationUser>()
                .ForMember(x => x.UserRoles, opt => opt.MapFrom(x => x.UserRoles));

            CreateMap<ApplicationUserRole, ApplicationUserRoleViewModel>();
            CreateMap<ApplicationUserRoleViewModel, ApplicationUserRole>();

            
            CreateMap<ApplicationRole, ApplicationRoleViewModel>();
            CreateMap<ApplicationRoleViewModel, ApplicationRole>();
        }
    }
}