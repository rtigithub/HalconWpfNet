using System;
using System.Windows;
using HalconDotNet;
using Microsoft.Win32;

namespace HalconWpfNet
{
     /// <summary>
     /// Interaction logic for MainWindow.xaml
     /// </summary>
     public partial class MainWindow : Window
     {
          #region Public Constructors

          public MainWindow()
          {
               InitializeComponent();
          }

          #endregion Public Constructors

          #region Private Methods

          private void ButtonReadImage_Click(object sender, RoutedEventArgs e)
          {
               var openFileDialog = new OpenFileDialog()
               {
                    InitialDirectory = Environment.GetEnvironmentVariable(@"HALCONEXAMPLES") + @"\images",
                    Filter = "Image Files | *.png;*.jpg;*.jp2;*.tif;*.bmp;*.hobj;*.ima; |All Files | *.*"
               };
               if (openFileDialog.ShowDialog() == true)
               {
                    try
                    {
                         var image = new HImage(openFileDialog.FileName);
                         HalconSmartWindow.HalconWindow.DispImage(image.CopyObj(1, -1));
                         HalconSmartWindow.SetFullImagePart(null);
                    }
                    catch (HalconException halconException)
                    {
                         StatusMessage.Text = halconException.Message;
                    }
                    HelpMessage.Text = "Right click and drag image to move.\r\nMouse wheel zooms.\r\nDouble click image to recenter and fit.";
               }
          }

          private void HalconSmartWindow_Initialized(object sender, EventArgs e)
          {
               //(sender as HSmartWindowControlWPF).HKeepAspectRatio = true;
          }

          #endregion Private Methods
     }
}