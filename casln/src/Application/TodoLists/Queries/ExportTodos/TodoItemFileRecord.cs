using casln.Application.Common.Mappings;
using casln.Domain.Entities;

namespace casln.Application.TodoLists.Queries.ExportTodos
{
    public class TodoItemRecord : IMapFrom<TodoItem>
    {
        public string Title { get; set; }

        public bool Done { get; set; }
    }
}
