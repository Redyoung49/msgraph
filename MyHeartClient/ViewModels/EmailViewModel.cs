using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Graph.Models;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace MyHeartClient.ViewModels
{
    public class EmailViewModel : BindableBase,INavigationAware
    {
        private List<Message> _messages;
        public List<Message> Messages
        {
            get { return _messages; }
            set { SetProperty(ref _messages, value); }
        }
        private IContainerExtension _containerExtension;
        private GraphService graphService;
        public EmailViewModel(IContainerExtension containerExtension)
        {
            _containerExtension = containerExtension;
            graphService = _containerExtension.Resolve<GraphService>();

        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            try
            {
                var response = await graphService.GetInboxAsync();
                Messages = response.Value;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
    }
}
