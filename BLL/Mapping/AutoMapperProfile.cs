using AutoMapper;
using BLL.DTOs;
using BLL.Extensions;
using DAL.Entities;

namespace BLL.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl, 
                opt => opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalcuateAge()))
            .ForMember(dest => dest.ThisUserFriendIds, opt => opt.MapFrom(src => src.ThisUserFriends))
            .ForMember(dest => dest.UserIsFriendIds, opt => opt.MapFrom(src => src.UserIsFriend))
            .ForMember(dest => dest.Specialization, o => o.MapFrom(src => src.Specialization.Name));
        CreateMap<Photo, PhotoDto>();
        CreateMap<Photo, PhotoForApprovalDto>();
        CreateMap<Specialization, SpecializationDto>();
        CreateMap<MemberUpdateDto, AppUser>();
        CreateMap<RegisterDto, AppUser>();
        CreateMap<Message, MessageDto>()
            .ForMember(d => d.SenderPhotoUrl, o => o.MapFrom(s => s.Sender.Photos
                .FirstOrDefault(x => x.IsMain).Url))
            .ForMember(d => d.RecipientPhotoUrl, o => o.MapFrom(s => s.Recipient.Photos
                .FirstOrDefault(x => x.IsMain).Url));
        CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue ? 
            DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
    }
}