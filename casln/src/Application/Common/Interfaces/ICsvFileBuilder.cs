using casln.Application.TodoLists.Queries.ExportTodos;
using System.Collections.Generic;

namespace casln.Application.Common.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildTodoItemsFile(IEnumerable<TodoItemRecord> records);
    }
}
