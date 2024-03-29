﻿using AutoMapper;
using TheCodeCamp.Models;

namespace TheCodeCamp.Data {
    public class CampMappingProfile : Profile {
        public CampMappingProfile() {
            CreateMap<Camp, CampModel>()
                .ForMember(model => model.Venue, opt => opt.MapFrom(camp => camp.Location.VenueName))
                .ReverseMap();

            CreateMap<Talk, TalkModel>()
                .ReverseMap()
                .ForMember(t => t.Speaker, opt => opt.Ignore())
                .ForMember(t => t.Camp, opt => opt.Ignore());

            CreateMap<Speaker, SpeakerModel>()
                .ReverseMap();
        }
    }
}
