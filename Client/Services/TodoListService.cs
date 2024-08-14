using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TodoListClient.Services
{
    public class TodoListService(
        ITokenAcquisition tokenAcquisition,
        HttpClient httpClient,
        IConfiguration configuration)
        : ITodoListService
    {
        private readonly string _todoListScope = configuration["TodoList:TodoListScope"];
        private readonly string _todoListBaseAddress = configuration["TodoList:TodoListBaseAddress"];

        public async Task<Todo> AddAsync(Todo todo)
        {
            await PrepareAuthenticatedClient();

            var jsonRequest = JsonConvert.SerializeObject(todo);
            var jsonContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync($"{_todoListBaseAddress}/api/todolist", jsonContent);

            CheckStatusCodes(response);

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Todo>(content);
        }

        public async Task DeleteAsync(int id)
        {
            await PrepareAuthenticatedClient();
            var response = await httpClient.DeleteAsync($"{_todoListBaseAddress}/api/todolist/{id}");
            CheckStatusCodes(response);
        }

        public async Task<Todo> EditAsync(Todo todo)
        {
            await PrepareAuthenticatedClient();

            var jsonRequest = JsonConvert.SerializeObject(todo);
            var jsonContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

            var response =
                await httpClient.PatchAsync($"{_todoListBaseAddress}/api/todolist/{todo.Id}", jsonContent);

            CheckStatusCodes(response);

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Todo>(content);
        }

        public async Task<IEnumerable<Todo>> GetAsync()
        {
            await PrepareAuthenticatedClient();

            var response = await httpClient.GetAsync($"{_todoListBaseAddress}/api/todolist");
            CheckStatusCodes(response);

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Todo>>(content);
        }

        public async Task<Todo> GetAsync(int id)
        {
            await PrepareAuthenticatedClient();

            var response = await httpClient.GetAsync($"{_todoListBaseAddress}/api/todolist/{id}");
            CheckStatusCodes(response);

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Todo>(content);
        }

        private static void CheckStatusCodes(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.OK)
                return;
            
            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");
        }

        private async Task PrepareAuthenticatedClient()
        {
            var accessToken = await tokenAcquisition.GetAccessTokenForUserAsync(new[] { _todoListScope });
            Debug.WriteLine($"access token-{accessToken}");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}