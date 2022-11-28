﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskManagerCourse.Client.Views.AddWindows;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Client.Services
{
    public class CommonViewService
    {
        private string _imageDialogFilterPattern =  "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
        public Window CurrentOpenedWindow { get; private set; }
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }
        public void ShowActionResult(System.Net.HttpStatusCode code, string message) 
        {
            if (code == System.Net.HttpStatusCode.OK)
            {
                ShowMessage(code.ToString() + $"\n{message}");
            }
            else
            {
                ShowMessage(code.ToString() + $"ЧТО-ТО ПОЛОМАЛОСЬ!");
            }
        }
        public void OpenWindow(Window wnd, BindableBase viewModel) 
        {
            CurrentOpenedWindow = wnd;
            wnd.DataContext = viewModel;
            wnd.ShowDialog();
        }

        public string GetFileFromDialog(string Filter) 
        {
            string filePath = String.Empty;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = Filter;
            bool? result = dlg.ShowDialog();
            if(result == true) 
            {
                filePath = dlg.FileName;
            }
            return filePath;
        }
        public void SetPhotoForObject(CommonModel model) 
        {
            string photoPath = GetFileFromDialog(_imageDialogFilterPattern);
            if(string.IsNullOrEmpty(photoPath) == false) 
            {
                var photoBytes = File.ReadAllBytes(photoPath);
                model.Photo = photoBytes;
            }
        }
    }
}
