using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;

namespace MyHeartClient
{
    public class GraphService
    {
        private static string[] _scopes = new[] { "User.Read" ,"mail.read","mail.send", "Tasks.ReadWrite" };
        private const string TenantId = "2fe49359-e0af-48e4-bc22-100d956fa833";
        private const string ClientId = "807a7bdb-c4cc-49ae-94a1-f7718ec708c1";
        private static GraphServiceClient _client;
        static GraphService()
        {
            Initialize();
        }

        private static void Initialize()
        {
            var options = new InteractiveBrowserCredentialOptions
            {
                TenantId = TenantId,
                ClientId = ClientId,
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud,
                RedirectUri = new Uri("http://localhost:60739"),
            };

            InteractiveBrowserCredential interactiveCredential = new(options);
            _client = new GraphServiceClient(interactiveCredential, _scopes);
        }
        public async Task<User> GetMyDetailsAsync()
        {
            try
            {
                return await _client.Me.GetAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading user details: {ex}");
                return null;
            }
        }
        public async Task<MessageCollectionResponse> GetInboxAsync()
        {
            // Ensure client isn't null
            _ = _client ??
                throw new System.NullReferenceException("Graph has not been initialized for user auth");

            var response = await _client.Me
                // Only messages from Inbox folder
                .MailFolders["Inbox"]
                .Messages
                .GetAsync(config =>
                {
                    config.QueryParameters.Top = 10;
                    config.QueryParameters.Count = true;
                    config.QueryParameters.Orderby = new string[] {
                        "ReceivedDateTime DESC"
                    };
                });
            return response;
        }
        public async Task<TodoTaskListCollectionResponse> GetTodoAsync()
        {
            // Ensure client isn't null
            _ = _client ??
                throw new System.NullReferenceException("Graph has not been initialized for user auth");
            var todoTaskList = await _client.Me.Todo.Lists.GetAsync(config => {
                config.QueryParameters.Top = 10;
                config.QueryParameters.Count = true;
            });
            return todoTaskList;
        }
        public async Task<TodoTaskCollectionResponse> GetTodoListTasksAsync(string listId)
        {
            // Ensure client isn't null
            _ = _client ??
                throw new System.NullReferenceException("Graph has not been initialized for user auth");
            var todoTask = await _client.Me.Todo.Lists[listId].Tasks.GetAsync(config => {
            });
            return todoTask;
        }
        public async Task AddTodoTaskToList(string listId, TodoTask todoTask)
        {
            await _client.Me.Todo.Lists[listId].Tasks.PostAsync(todoTask);
        }
        public async Task RemoveTodoTask(string listId, string taskId)
        {
            await _client.Me.Todo.Lists[listId].Tasks[taskId].DeleteAsync();
        }
    }
}
