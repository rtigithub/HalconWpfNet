// ***********************************************************************
// Assembly         : HalconWpfNet
// Author           : Resolution Technology, Inc.
// Created          : 07-13-2017
// Last Modified On : 07-20-2017
// ***********************************************************************
// <copyright file="MainWindow.xaml.cs" company="Resolution Technology, Inc.">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Windows;
using HalconDotNet;
using Microsoft.Win32;

namespace HalconWpfNet
{
     /// <summary>
     /// Interaction logic for MainWindow.xaml
     /// </summary>
     /// <seealso cref="System.Windows.Window" />
     /// <seealso cref="System.Windows.Markup.IComponentConnector" />
     public partial class MainWindow : Window
     {
          #region Private Fields

          /// <summary>
          /// The halcon image
          /// </summary>
          private HImage halconImage = new HImage();
          /// <summary>
          /// The image height
          /// </summary>
          private HTuple imageHeight = 0;
          /// <summary>
          /// The image width
          /// </summary>
          private HTuple imageWidth = 0;

          #endregion Private Fields

          #region Public Constructors

          /// <summary>
          /// Initializes a new instance of the <see cref="MainWindow" /> class.
          /// </summary>
          public MainWindow()
          {
               InitializeComponent();
          }

          #endregion Public Constructors

          #region Public Properties

          /// <summary>
          /// Gets or sets the halcon image.
          /// </summary>
          /// <value>The halcon image.</value>
          public HImage HalconImage
          {
               get => this.halconImage;
               set
               {
                    this.halconImage = value;
                    this.halconImage.GetImageSize(out imageWidth, out imageHeight);
               }
          }

          #endregion Public Properties

          #region Private Methods

          /// <summary>
          /// Handles the Click event of the ButtonReadImage control.
          /// </summary>
          /// <param name="sender">The source of the event.</param>
          /// <param name="e">The <see cref="RoutedEventArgs" /> instance containing the event data.</param>
          private void ButtonReadImage_Click(object sender, RoutedEventArgs e)
          {
               var openFileDialog = new OpenFileDialog()
               {
                    InitialDirectory = Environment.GetEnvironmentVariable("HALCONEXAMPLES") + @"\images",
                    Filter = "Image Files | *.png;*.jpg;*.jp2;*.tif;*.bmp;*.hobj;*.ima; |All Files | *.*"
               };
               if (openFileDialog.ShowDialog() == true)
               {
                    try
                    {
                         HalconImage = new HImage(openFileDialog.FileName);
                         HalconSmartWindow.HalconWindow.DispImage(HalconImage.CopyObj(1, -1));
                         HalconSmartWindow.SetFullImagePart(null);
                    }
                    catch (HalconException halconException)
                    {
                         StatusMessage.Text = halconException.Message;
                    }
                    HelpMessage.Text = "Right click and drag image to move.\r\nMouse wheel zooms.\r\nDouble click image to recenter and fit.";
               }
          }

          /// <summary>
          /// Handles the Initialized event of the HalconSmartWindow control.
          /// </summary>
          /// <param name="sender">The source of the event.</param>
          /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
          private void HalconSmartWindow_Initialized(object sender, EventArgs e)
          {
               //(sender as HSmartWindowControlWPF).HKeepAspectRatio = true;
          }

          /// <summary>
          /// Handles the MouseMove event of the HalconSmartWindow control.
          /// </summary>
          /// <param name="sender">The source of the event.</param>
          /// <param name="e">The <see cref="System.Windows.Input.MouseEventArgs"/> instance containing the event data.</param>
          private void HalconSmartWindow_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
          {
               Point mousePosition = e.GetPosition(sender as IInputElement);
               (sender as HSmartWindowControlWPF).HalconWindow.ConvertCoordinatesWindowToImage(
                   mousePosition.Y,
                   mousePosition.X,
                   out double rowInImage,
                   out double columnInImage);
               if (HalconImage?.IsInitialized() == true)
               {
                    rowInImage = rowInImage < 0 ? 0 : Math.Min(rowInImage, imageHeight - 1);
                    columnInImage = columnInImage < 0 ? 0 : Math.Min(columnInImage, imageWidth - 1);
                    HTuple pixelValue = HalconImage.GetGrayval((int)rowInImage, (int)columnInImage);
                    StatusMessage.Text = String.Format("X {0}, Y {1}, Pixel Value {2}",
                         mousePosition.X.ToString("F1"),
                         mousePosition.Y.ToString("F1"),
                         pixelValue);
               }
          }

          #endregion Private Methods
     }
}