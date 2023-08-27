using AutoMapper;
using MongoDB.Bson;
using Notion.Application.Models.Request;
using Notion.Domain.Entities;

namespace Notion.Application.Mapper;

public class ToDoItemProfile : Profile
{
    public ToDoItemProfile()
    {
        CreateMap<CreateToDoItem, ToDoItem>(MemberList.Source)
            .AfterMap((src, dest) => { dest.Id = ObjectId.GenerateNewId().ToString(); });

        CreateMap<UpdateToDoItemsRequest, ToDoItem>()
            .AfterMap((src, dest) => { dest.Id = ObjectId.GenerateNewId().ToString(); });
        ;
    }
}