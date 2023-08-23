using AutoMapper;
using Notion.Application.Models.Request;
using Notion.Domain.Entities;

namespace Notion.Application.Mapper;

public class ToDoItemProfile : Profile
{
    public ToDoItemProfile()
    {
        CreateMap<CreateToDoItem, ToDoItem>(MemberList.Source);
    }
}