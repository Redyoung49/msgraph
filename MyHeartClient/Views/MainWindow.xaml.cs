using System;
using System.Windows;
using Microsoft.Graph.Models;

namespace MyHeartClient.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private GraphService graphService;
        private User user;
        private async void GetUserInfoBtn_Clicked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (graphService == null)
                {
                    graphService = new GraphService();
                }
                user = await graphService.GetMyDetailsAsync();
                if (user == null)
                {
                    MessageBox.Show("登录失败！");
                    return;
                }
                MessageBox.Show(this, $"Hello, {user.DisplayName}!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

    }
}
