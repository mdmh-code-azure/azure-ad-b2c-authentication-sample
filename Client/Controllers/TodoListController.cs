using Microsoft.AspNetCore.Mvc;
using TodoListClient.Services;

namespace TodoListClient.Controllers
{
    public class TodoListController(ITodoListService todoListService) : Controller
    {
        // GET: TodoList
        [AuthorizeForScopes(ScopeKeySection = "TodoList:TodoListScope")]
        public async Task<ActionResult> Index()
        {
            return View((await todoListService.GetAsync()).Select(TodoModel.New));
        }

        // GET: TodoList/Details/5
        public async Task<ActionResult> Details(int id)
        {
            return View(TodoModel.New(await todoListService.GetAsync(id)));
        }

        // GET: TodoList/Create
        public ActionResult Create()
        {
            var todo = new TodoModel(0, string.Empty, HttpContext.User.Identity.Name);
            return View(todo);
        }

        // POST: TodoList/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Title,Owner")] TodoModel model)
        {
            await todoListService.AddAsync(model.ToDomain());
            return RedirectToAction("Index");
        }

        // GET: TodoList/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var todo = await todoListService.GetAsync(id);
            return todo == null ? NotFound() : View(TodoModel.New(todo));
        }

        // POST: TodoList/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Title,Owner")] TodoModel model)
        {
            await todoListService.EditAsync(model.ToDomain());
            return RedirectToAction("Index");
        }

        // GET: TodoList/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var todo = await todoListService.GetAsync(id);
            return todo == null ? NotFound() : View(TodoModel.New(todo));
        }

        // POST: TodoList/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, [Bind("Id,Title,Owner")] TodoModel todoModel)
        {
            await todoListService.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}