using AutoMapper;
using Notion.Application.Models.Request;
using Notion.Application.Models.Response;
using Notion.Domain.Entities;

namespace Notion.Application.Mapper;

public class ToDoListProfile : Profile
{
    public ToDoListProfile()
    {
        CreateMap<CreateToDoList, ToDoList>()
            .AfterMap((src, dest) =>
            {
                // Map properties directly
                dest.Title = src.Title;

                // Convert IEnumerable<string> to ICollection<string>
                dest.Contributors = src.Contributors?.ToList();

                // Map ToDoItems collection using Select
                dest.Items = src.ToDoItems?.Select(x => new ToDoItem()
                {
                    Title = x.Title,
                    Description = x.Description,
                    Completed = x.Completed
                }).ToList(); // Convert IEnumerable<ToDoItem> to ICollection<ToDoItem>
            });

        CreateMap<ToDoList, GetAllToDoListResponse>(MemberList.Destination);
    }
}