using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Com.Abedelazizshe.Lightcompressorlibrary;
using System.Collections.Generic;
using Com.Abedelazizshe.Lightcompressorlibrary.Config;
using Android.Content;
using Android.Util;
using Java.IO;
using Android.Graphics;
using Android.Provider;
using Android.Widget;
using static Android.Hardware.Camera;
using Java.Lang;
using static Android.InputMethodServices.Keyboard;

namespace video_compress
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private int _selectVideo = 1;

        private string _selectedVideoPath;

        public TextView OriginalName, OriginalSize, CompressedName, CompressedSize, CompressingProgress;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            AndroidX.AppCompat.Widget.Toolbar toolbar = FindViewById<AndroidX.AppCompat.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            OriginalName = FindViewById<TextView>(Resource.Id.OriginalVideoName);
            OriginalSize = FindViewById<TextView>(Resource.Id.OriginalVideoSize);

            CompressingProgress = FindViewById<TextView>(Resource.Id.CompressingProgress);

            CompressedName = FindViewById<TextView>(Resource.Id.CompressedVideoName);
            CompressedSize = FindViewById<TextView>(Resource.Id.CompressedVideoSize);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (View.IOnClickListener)null).Show();

            Intent i = new Intent(Intent.ActionPick, Android.Provider.MediaStore.Video.Media.ExternalContentUri);
            StartActivityForResult(i, _selectVideo);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                if (requestCode == _selectVideo)
                {
                    // _selectedVideoPath = getPath(data.Data);

                    OriginalName.Text = data.Data.Path;
                    OriginalSize.Text = $"{GetSize(data.Data)}";

                    var vcl = new VideoCompressorListener(this);

                    //var configuration = new Configuration(
                    //    VideoQuality.VeryLow,
                    //    Java.Lang.Integer.ValueOf(60),
                    //    false,
                    //    Java.Lang.Integer.ValueOf(50000), // Bitrate (bit/s)
                    //    false,
                    //    false,
                    //    Java.Lang.Double.ValueOf(720),
                    //    Java.Lang.Double.ValueOf(500)
                    //  );

                    // Bigger
                    //var configuration = new Configuration(
                    //    VideoQuality.VeryHigh,
                    //    Java.Lang.Integer.ValueOf(24),
                    //    false,
                    //    Java.Lang.Integer.ValueOf(50000), //Bitrate (bit/s)
                    //    false,
                    //    false,
                    //    Java.Lang.Double.ValueOf(360),
                    //    Java.Lang.Double.ValueOf(480)
                    //  );

                    // Smaller
                    //var configuration = new Configuration(
                    //    VideoQuality.VeryLow,
                    //    Java.Lang.Integer.ValueOf(90),
                    //    false,
                    //    Java.Lang.Integer.ValueOf(50000), //Bitrate (bit/s)
                    //    false,
                    //    false,
                    //    Java.Lang.Double.ValueOf(360),
                    //    Java.Lang.Double.ValueOf(480)
                    //  );

                    // WORK
                    //var configuration = new Configuration(
                    //    VideoQuality.VeryLow,
                    //    Java.Lang.Integer.ValueOf(120),
                    //    false,
                    //    Java.Lang.Integer.ValueOf(1500), //Bitrate (bit/s)
                    //    false,
                    //    true,
                    //    null,
                    //    null
                    //  );

                    var configuration = new Configuration(
                        VideoQuality.VeryLow,
                        Java.Lang.Integer.ValueOf(120),
                        false,
                        Java.Lang.Integer.ValueOf(1500), //Bitrate (bit/s)
                        false,
                        true,
                        null,
                        null
                      );

                    var uris = new List<Android.Net.Uri>
                      {
                          data.Data
                      };

                    VideoCompressor.Start(this,
                                      uris,
                                      $"{Android.OS.Environment.DirectoryMovies}/compressedVideos",
                                      vcl,
                                      configuration);
                    try
                    {
                        if (_selectedVideoPath == null)
                        {
                            //Log.e("selected video path = null!");
                            //finish();
                        }
                        else
                        {
                            /**
                             * try to do something there
                             * selectedVideoPath is path to the selected video
                             */
                        }
                    }
                    catch (IOException e)
                    {
                        //#debug
                        e.PrintStackTrace();
                    }
                }
            }
        }

        //public string GetPath(Android.Net.Uri uri)
        //{
        //    string[] projection = { MediaStore.Images.Media };
        //    Cursor cursor = ManagedQuery(uri, projection, null, null, null);
        //    if (cursor != null)
        //    {
        //        int column_index = cursor.GetColumnIndexOrThrow(MediaStore.Video.Media.DATA);
        //        cursor.MoveToFirst();
        //        return cursor.GetString(column_index);
        //    }
        //    else return null;
        //}

        public string GetSize(Android.Net.Uri uri)
        {
            using (var fd = ContentResolver.OpenFileDescriptor(uri, "r"))
                //return $"{(fd.StatSize / 1000000).ToString()} Mb";
                return fd.StatSize.ToString();
        }

        public string GetFileSize(Long size) {
            if (size.IntValue() <= 0)
                return "0";

            var units = new List<string> { "B", "KB", "MB", "GB", "TB" };
            var digitGroups = ((int)(System.Math.Log10(size.DoubleValue()) / System.Math.Log10(1024.0)));

            return $"{System.Math.Pow(1024.0, (double)digitGroups).ToString("#,##0.#")} {units[digitGroups]}";

            //DecimalFormat("#,##0.#").format(
            //    size / .Pow(digitGroups.toDouble())
            //) + " " + units[digitGroups];
        }
}
}

