﻿using System;
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

namespace TaskManagerCourse.Client.Views.AddWindows
{
    /// <summary>
    /// Логика взаимодействия для CreateOrUpdateTaskWindow.xaml
    /// </summary>
    public partial class CreateOrUpdateTaskWindow : Window
    {
        public CreateOrUpdateTaskWindow()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
