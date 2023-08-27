using AutoMapper;
using Notion.Application.Models.Request;
using Notion.Application.Models.Response;
using Notion.Domain.Entities;

namespace Notion.Application.Mapper;

public class ToDoListProfile : Profile
{
    public ToDoListProfile()
    {
        CreateMap<CreateToDoList, ToDoList>(MemberList.Source);


        CreateMap<ToDoList, GetAllToDoListResponse>(MemberList.Destination);
        
        CreateMap<ToDoList, GetToDoListResponse>()
            .ForMember(dest => dest.Items, opt =>
                opt.MapFrom(src => src.Items));
    }
}