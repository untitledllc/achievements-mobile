﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using com.google.zxing.qrcode;
using Microsoft.Devices;
using System.Collections.ObjectModel;
using System.Windows.Threading;
using System.Windows.Navigation;
using com.google.zxing;
using com.google.zxing.common;
using itsbeta_wp7.ViewModel;

namespace itsbeta_wp7
{
    public class PhotoCameraLuminanceSource : LuminanceSource
    {
        public byte[] PreviewBufferY { get; private set; }

        public PhotoCameraLuminanceSource(int width, int height)
            : base(width, height)
        {
            PreviewBufferY = new byte[width * height];
        }

        public override sbyte[] Matrix
        {
            get { return (sbyte[])(Array)PreviewBufferY; }
        }

        public override sbyte[] getRow(int y, sbyte[] row)
        {
            if (row == null || row.Length < Width)
            {
                row = new sbyte[Width];
            }

            for (int i = 0; i < Height; i++)
                row[i] = (sbyte)PreviewBufferY[i * Width + y];

            return row;
        }
    }


    public partial class QRread : PhoneApplicationPage
    {
        private readonly DispatcherTimer _timer;
        private readonly ObservableCollection<string> _matches;

        private PhotoCameraLuminanceSource _luminance;
        private QRCodeReader _reader;
        private PhotoCamera _photoCamera;

        public QRread()
        {
            InitializeComponent();

            _matches = new ObservableCollection<string>();
            //_matchesList.ItemsSource = _matches;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(250);
            _timer.Tick += (o, arg) => ScanPreviewBuffer();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            try
            {
                _timer.Stop();
                _photoCamera.Dispose();
                
            }
            catch { };
            base.OnNavigatedFrom(e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _timer.Start();
            try
            {
                //if (App.ViewModel.Qr.CameraFocusSet == false)
                //{
                    _photoCamera = new PhotoCamera();
                    _photoCamera.Initialized += OnPhotoCameraInitialized;
                    _previewVideo.SetSource(_photoCamera);
                //}
            }
            catch { };

            try
            {
                //if (App.ViewModel.Qr.CameraFocusSet == false)
                //{
                    CameraButtons.ShutterKeyHalfPressed += (o, arg) => { try { _photoCamera.Focus(); } catch { }; };
                    //App.ViewModel.Qr.CameraFocusSet = true;
                //};
            }
            catch
            {
            };
            base.OnNavigatedTo(e);
        }

        private void OnPhotoCameraInitialized(object sender, CameraOperationCompletedEventArgs e)
        {
            try
            {
                int width = Convert.ToInt32(_photoCamera.PreviewResolution.Width);
                int height = Convert.ToInt32(_photoCamera.PreviewResolution.Height);

                _luminance = new PhotoCameraLuminanceSource(width, height);
                _reader = new QRCodeReader();

                Dispatcher.BeginInvoke(() =>
                {
                    _previewTransform.Rotation = _photoCamera.Orientation;
                    _timer.Start();
                });
            }
            catch
            {
            };
        }

        private void ScanPreviewBuffer()
        {
            try
            {
                _photoCamera.GetPreviewBufferY(_luminance.PreviewBufferY);
                var binarizer = new HybridBinarizer(_luminance);
                var binBitmap = new BinaryBitmap(binarizer);
                var result = _reader.decode(binBitmap);
                Dispatcher.BeginInvoke(() => DisplayResult(result.Text));
            }
            catch
            {
            }
        }

        private void DisplayResult(string text)
        {
            _timer.Stop();
            //_photoCamera.Dispose();
            try
            {
                //MessageBox.Show("Добавлен QR код c текстом: \n" + text);
                
                try
                {
                    Uri qrUrl = new Uri(text);
                    string code = qrUrl.Query.Replace("?activation_code=","");
                    this.Focus();
                    ViewModelLocator.UserStatic.ActivateAchieve(code);
                }
                catch { };
                try
                {
                    //this.NavigationService.GoBack();
                }
                catch { };
            }
            catch { };
        }


    }
}