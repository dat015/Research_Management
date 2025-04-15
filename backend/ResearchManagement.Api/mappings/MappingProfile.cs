using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ResearchManagement.Api.dtos;
using ResearchManagement.Api.models;

namespace ResearchManagement.Api.mappings
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<ResearchTopic, ResearchProgressDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TopicId.ToString()))
            .ForMember(dest => dest.Status, opt => opt.Ignore()); // Status sẽ được tính toán ở frontend

            CreateMap<Milestone, MilestoneDto>();
            CreateMap<Issue, IssueDto>();

            // Ánh xạ ngược khi cập nhật
            CreateMap<UpdateProgressDto, ResearchTopic>()
                .ForMember(dest => dest.Milestones, opt => opt.Ignore())
                .ForMember(dest => dest.Issues, opt => opt.Ignore());

            CreateMap<MilestoneDto, Milestone>();
            CreateMap<IssueDto, Issue>();
            CreateMap<MilestoneDto_1, Milestone>().ReverseMap();
            CreateMap<IssueDto_1, Issue>().ReverseMap();


        }
    }

}