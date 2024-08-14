namespace TodoListClient.Models
{
    public record TodoModel(int Id, string Title, string Owner)
    {
        public static TodoModel New (Todo t) => new(t.Id, t.Title, t.Owner);

        public Todo ToDomain() => new(Id, Title, Owner);
    }
}
