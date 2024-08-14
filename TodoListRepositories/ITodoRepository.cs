using TodoListDomain;

namespace TodoListRepositories;

public interface ITodoRepository
{
    bool Any();
    IEnumerable<Todo> GetByOwner(string owner);
    Todo? GetById(int id);
    void Delete(int id);
    void Add(Todo toDo);
    int GetCurrentId();
}