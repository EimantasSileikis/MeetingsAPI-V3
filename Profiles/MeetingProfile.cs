using AutoMapper;
using MeetingsAPI_V3.Entities;
using MeetingsAPI_V3.Models;

namespace MeetingsAPI_V2.Profiles
{
    public class MeetingProfile: Profile
    {
        public MeetingProfile()
        {
            CreateMap<Meeting, MeetingDto>();
            CreateMap<MeetingDto, Meeting>();
            CreateMap<Meeting, MeetingGetDto>();
            CreateMap<MeetingGetDto, Meeting>();
            CreateMap<UserDto, User>();
        }
    }
}
