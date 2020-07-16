﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
using GW2PAO.API.Services.Interfaces;
using GW2PAO.Modules.Tasks.Models;
using GW2PAO.Modules.Tasks.ViewModels;
using GW2PAO.Views;
using NLog;

namespace GW2PAO.Modules.Tasks.Views
{
    /// <summary>
    /// Interaction logic for AddNewTaskDialog.xaml
    /// </summary>
    public partial class AddNewTaskDialog : OverlayWindow
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// The player task that this dialog is adding or editing
        /// </summary>
        [Import]
        public NewTaskDialogViewModel TaskData
        {
            get
            {
                return this.DataContext as NewTaskDialogViewModel;
            }
            set
            {
                this.DataContext = value;
            }
        }

        protected override bool SetNoFocus { get { return false; } }

        /// <summary>
        /// Constructs a new AddNewTaskDialog window
        /// </summary>
        /// <param name="taskData">The task data</param>
        public AddNewTaskDialog()
        {
            InitializeComponent();
            this.Loaded += AddNewTaskDialog_Loaded;
        }

        private void AddNewTaskDialog_Loaded(object sender, RoutedEventArgs e)
        {
            this.CenterWindowOnScreen();
        }

        /// <summary>
        /// Centers the window on the screen
        /// </summary>
        private void CenterWindowOnScreen()
        {
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            this.Left = (screenWidth / 2) - (this.ActualWidth / 2);
            this.Top = (screenHeight / 2) - (this.ActualHeight / 2);
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
            e.Handled = true;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.TaskData.ApplyCommand.Execute(null);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.IconPopup.IsOpen = true;
            e.Handled = true;
        }

        private void OnIntelliboxSuggestItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.ItemsEntryBox.ChooseCurrentItem();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            this.TaskData.ClearCommand.Execute(null);
        }

        private void IconImage_OnImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            logger.Error(e.ErrorException, "Failed to load task image");
        }
    }
}
