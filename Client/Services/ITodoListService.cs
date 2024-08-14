namespace TodoListClient.Services
{
    public interface ITodoListService
    {
        Task<IEnumerable<Todo>> GetAsync();

        Task<Todo> GetAsync(int id);

        Task DeleteAsync(int id);

        Task<Todo> AddAsync(Todo todoModel);

        Task<Todo> EditAsync(Todo todo);
    }
}
