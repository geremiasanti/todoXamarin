using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using todo.Models;
using Newtonsoft.Json;

namespace todo.Services
{
    class TodoService
    {
        public async Task<String> Login(String email, String password) {
            ApiService apiService = new ApiService();
            string url = "http://10.0.2.2:8000/api/login";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("email", email);
            parameters.Add("password", password);
            string response = await apiService.CallPostApiNoAuth(url, parameters);
            return response;
        }

        private class ActivityFromBoredApi 
        {
            public string Type;
            public string Activity;
        }
        public async Task<TodoItem> GetRandomTodoItem()
        {
            ApiService apiService = new ApiService();
            String url = "https://www.boredapi.com/api/activity";
            String jsonResponse = await apiService.CallGetApiNoAuth(url);

            ActivityFromBoredApi activity = JsonConvert.DeserializeObject<ActivityFromBoredApi>(jsonResponse);

            TodoItem generated = new TodoItem();
            generated.Title = activity.Type;
            generated.Description = activity.Activity;
            return generated;
        }
    }
}
