using System.Collections.Generic;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;

namespace MyHeartClient.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Redyoung's Microsoft Graph";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public List<NavMenu> Menus { get; set; }
        private IContainerExtension _containerExtension;
        private IRegionManager _regionManager;
        public MainWindowViewModel(IContainerExtension containerExtension)
        {
            NavCommand = new DelegateCommand<NavMenu>(NavTo);
            Menus = new List<NavMenu>();
            Menus.Add(new NavMenu()
            {
                Name = "Email",
                Url = AppContracts.Nav_EMmail
            });
            Menus.Add(new NavMenu()
            {
                Name = "Todo",
                Url = AppContracts.Nav_TODO
            });
            _containerExtension = containerExtension;
            _regionManager = containerExtension.Resolve<IRegionManager>();
        }
        public DelegateCommand<NavMenu> NavCommand { get; set; }
        private void NavTo(NavMenu menu)
        {
            _regionManager.RequestNavigate(AppContracts.Region_MainContent, menu.Url);
        }
    }
}
