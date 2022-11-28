using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskManagerCourse.Client.ViewModels;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.Views.AddWindows
{
    /// <summary>
    /// Логика взаимодействия для AddUsersToProjectWindow.xaml
    /// </summary>
    public partial class AddUsersToProjectWindow : Window
    {
        public AddUsersToProjectWindow()
        {
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = (PorjectsPageViewModel)DataContext;
            foreach(UserModel user in e.RemovedItems) 
            {
                viewModel.SelectedUsersForProject.Remove(user);
            }
            foreach (UserModel user in e.RemovedItems)
            {
                viewModel.SelectedUsersForProject.Add(user);
            }
        }
    }
}
