using TodoListDomain;

namespace TodoListRepositories;

public class TodoInMemoryRepository : ITodoRepository
{
    private static readonly List <Todo> Db = [];

    public IEnumerable<Todo> GetByOwner(string owner) => Db.Where(x => x.Owner == owner);
    public Todo? GetById(int id) => Db.Find(t => t.Id == id);

    public int GetCurrentId() => Db.MaxBy(x => x.Id)?.Id ?? 0;
    
    public bool Any() => Db.Count != 0;
    
    public void Add(Todo toDo) => Db.Add(toDo);

    public void Delete(int id)
    {
        Db.Remove(GetById(id) ?? throw new InvalidOperationException($"Todo with Id {id} does not exists"));
    }
}