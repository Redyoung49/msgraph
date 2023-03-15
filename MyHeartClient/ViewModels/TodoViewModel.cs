using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph.Models;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace MyHeartClient.ViewModels
{
    public class TodoViewModel : BindableBase, INavigationAware
    {
        private IContainerExtension _containerExtension;
        private GraphService graphService;
        public TodoViewModel(IContainerExtension containerExtension)
        {
            AddTodoCommand = new DelegateCommand<string>(async title => await AddTodo(title));
            RemoveTodoCommand = new DelegateCommand<TodoTask>(async todoTask=>await RemoveTodo(todoTask));
            SelectTodoTaskListCommand = new DelegateCommand<TodoTaskList>(async (todoTaskList)=>await SelectTodoTaskList(todoTaskList));
            _containerExtension = containerExtension;
            graphService = _containerExtension.Resolve<GraphService>();
        }
        private List<TodoTaskList> _todoTaskListList;
        public List<TodoTaskList> TodoTaskListList
        {
            get { return _todoTaskListList; }
            set { SetProperty(ref _todoTaskListList, value); }
        }
        private List<TodoTask> _todoTaskList;
        public List<TodoTask> TodoTaskList
        {
            get { return _todoTaskList; }
            set { SetProperty(ref _todoTaskList, value); }
        }
        private TodoTaskList _currentTodoTaskList;
        public TodoTaskList CurrentTodoTaskList
        {
            get { return _currentTodoTaskList; }
            set { SetProperty(ref _currentTodoTaskList, value); }
        }
        public DelegateCommand<string> AddTodoCommand { get; set; }
        public DelegateCommand<TodoTask> RemoveTodoCommand { get; set; }
        public DelegateCommand<TodoTaskList> SelectTodoTaskListCommand { get; set; }
        public async Task AddTodo(string name)
        {
            try
            {
                if (CurrentTodoTaskList == null)
                {
                    return;
                }
                TodoTask todoTask = new TodoTask();
                todoTask.Title = name;
                await graphService.AddTodoTaskToList(CurrentTodoTaskList.Id, todoTask);
                await RequestTodoTaskList();
            }
            catch (Exception ex)
            {

            }
            
            
        }
        public async Task RemoveTodo(TodoTask todo)
        {
            if (todo == null)
            {
                return;
            }
            if (CurrentTodoTaskList == null)
            {
                return;
            }
            await graphService.RemoveTodoTask(CurrentTodoTaskList.Id, todo.Id);
            await RequestTodoTaskList();
        }
        private async Task SelectTodoTaskList(TodoTaskList todoTaskList)
        {
            try
            {
                CurrentTodoTaskList = todoTaskList;
                await RequestTodoTaskList();
            }
            catch (Exception ex)
            {

            }
        }
        private async Task RequestTodoTaskList()
        {
            if (CurrentTodoTaskList!=null)
            {
                TodoTaskList = (await graphService.GetTodoListTasksAsync(CurrentTodoTaskList.Id)).Value;
            }
        }
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                var todoTaskListCollectionResponse = await graphService.GetTodoAsync();
                this.TodoTaskListList = todoTaskListCollectionResponse.Value;
            }
            catch (Exception ex)
            {

            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
