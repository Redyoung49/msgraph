using System.Windows;
using MyHeartClient.Views;
using Prism.Ioc;

namespace MyHeartClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<GraphService>();
            containerRegistry.RegisterForNavigation<EmailView>(AppContracts.Nav_EMmail);
            containerRegistry.RegisterForNavigation<TodoView>(AppContracts.Nav_TODO);
        }
    }
}
