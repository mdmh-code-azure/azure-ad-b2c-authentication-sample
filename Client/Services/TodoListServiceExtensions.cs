using Microsoft.Extensions.DependencyInjection;

namespace TodoListClient.Services;

public static class TodoListServiceExtensions
{
    public static void AddTodoListService(this IServiceCollection services)
    {
        services.AddHttpClient<ITodoListService, TodoListService>();
    }
}