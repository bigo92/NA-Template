using casln.Application.TodoLists.Queries.ExportTodos;
using CsvHelper.Configuration;

namespace casln.Infrastructure.Files.Maps
{
    public class TodoItemRecordMap : ClassMap<TodoItemRecord>
    {
        public TodoItemRecordMap()
        {
            AutoMap();
            Map(m => m.Done).ConvertUsing(c => c.Done ? "Yes" : "No");
        }
    }
}
