using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Hardware;
using Android.Views.Animations;

namespace itsbeta.achievements
{
    [Activity(Theme = "@android:style/Theme.NoTitleBar.Fullscreen",
                ScreenOrientation = ScreenOrientation.Portrait)]
    public class QRReaderActivity : Activity , ISurfaceHolderCallback
    {
        Camera camera;
        ISurfaceHolder surfaceHolder;
        Animation buttonClickAnimation;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.QRReaderActivityLayout);
            buttonClickAnimation = AnimationUtils.LoadAnimation(this, global::Android.Resource.Animation.FadeIn);

            SurfaceView surfaceView = FindViewById<SurfaceView>(Resource.Id.qrreader_camerasurfaceView);
            Button readyImageButton = FindViewById<Button>(Resource.Id.qrcodereader_readyButton);
            Button cancelImageButton = FindViewById<Button>(Resource.Id.qrcodereaderscr_cancelButton);

            surfaceHolder = surfaceView.Holder;
            surfaceHolder.AddCallback(this);
            surfaceHolder.SetType(SurfaceType.PushBuffers);

            readyImageButton.Click += delegate { readyImageButton.StartAnimation(buttonClickAnimation); }; //здесь запустить процесс распознавания;
            cancelImageButton.Click += delegate { cancelImageButton.StartAnimation(buttonClickAnimation); Finish(); };
        }



        public void SurfaceChanged(ISurfaceHolder holder, Android.Graphics.Format format, int width, int height)
        {
            // Now that the size is known, set up the camera parameters and begin
            // the preview.
            Camera.Parameters parameters = camera.GetParameters();

            IList<Camera.Size> sizes = parameters.SupportedPreviewSizes;
            Camera.Size optimalSize = GetOptimalPreviewSize(sizes, width, height);

            parameters.SetPreviewSize(optimalSize.Width, optimalSize.Height);

            camera.SetDisplayOrientation(90);

            camera.SetParameters(parameters);
            camera.StartPreview();
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            // The Surface has been created, acquire the camera and tell it where
            // to draw.
            camera = Camera.Open();

            try
            {
                camera.SetPreviewDisplay(holder);
            }
            catch (Exception)
            {
                camera.Release();
                camera = null;
                // TODO: add more exception handling logic here
            }
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            // Surface will be destroyed when we return, so stop the preview.
            // Because the CameraDevice object is not a shared resource, it's very
            // important to release it when the activity is paused.
            camera.StopPreview();
            camera.Release();
            camera = null;
        }

        private Camera.Size GetOptimalPreviewSize(IList<Camera.Size> sizes, int w, int h)
        {
            const double ASPECT_TOLERANCE = 0.05;
            double targetRatio = (double)1;

            if (sizes == null)
                return null;

            Camera.Size optimalSize = null;
            double minDiff = Double.MaxValue;

            int targetHeight = h;

            // Try to find an size match aspect ratio and size
            for (int i = 0; i < sizes.Count; i++)
            {
                Camera.Size size = sizes[i];
                double ratio = (double)size.Width / size.Height;

                if (Math.Abs(ratio - targetRatio) > ASPECT_TOLERANCE)
                    continue;

                if (Math.Abs(size.Height - targetHeight) < minDiff)
                {
                    optimalSize = size;
                    minDiff = Math.Abs(size.Height - targetHeight);
                }
            }

            // Cannot find the one match the aspect ratio, ignore the requirement
            if (optimalSize == null)
            {
                minDiff = Double.MaxValue;
                for (int i = 0; i < sizes.Count; i++)
                {
                    Camera.Size size = sizes[i];

                    if (Math.Abs(size.Height - targetHeight) < minDiff)
                    {
                        optimalSize = size;
                        minDiff = Math.Abs(size.Height - targetHeight);
                    }
                }
            }

            return optimalSize;
        }

    }

}