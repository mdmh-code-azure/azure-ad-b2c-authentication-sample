using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;
using TodoListRepositories;
using TodoListService.Models;

namespace TodoListService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [RequiredScope(ScopeRequiredByApi)]
    public class TodoListController(ITodoRepository repository) : Controller
    {
        private const string ScopeRequiredByApi = "task.read";
        private string IdentityName
        {
            get
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity; 
                return identity?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value ?? string.Empty;
            }
        }

        [HttpGet]
        public IEnumerable<TodoModel> Get() => repository.GetByOwner(IdentityName).Select(TodoModel.New);

        [HttpGet("{id:int}", Name = "Get")]
        public TodoModel Get(int id) => TodoModel.New(repository.GetById(id));

        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            repository.Delete(id);
        }

        [HttpPost]
        public IActionResult Post([FromBody] TodoModel model)
        {
            var id = repository.GetCurrentId() + 1;
            var todo = model.ToDomain() with { Id = id, Owner = IdentityName};
            repository.Add(todo);
            return Ok(model);
        }

        [HttpPatch("{id:int}")]
        public IActionResult Patch(int id, [FromBody] TodoModel todoModel)
        {
            if (id != todoModel.Id)
            {
                return NotFound();
            }

            if (repository.GetById(id) == null)
            {
                return NotFound();
            }

            repository.Delete(id);
            repository.Add(todoModel.ToDomain());

            return Ok(todoModel);
        }
    }
}