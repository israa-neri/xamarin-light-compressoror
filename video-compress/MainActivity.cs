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

namespace video_compress
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private int _selectVideo = 1;

        private String _selectedVideoPath;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Toolbar toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
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
                    var vcl = new VideoCompressorListener();

                    var configuration = new Configuration(
                        VideoQuality.Medium,
                        null,
                        false,
                        Java.Lang.Integer.ValueOf(1500000), // Bitrate (bit/s)
                        false,
                        false,
                        Java.Lang.Double.ValueOf(1280),
                        Java.Lang.Double.ValueOf(720)
                      );

                    var uris = new List<Android.Net.Uri>
                      {
                          data.Data
                      };

                    VideoCompressor.Start(this,
                                      uris,
                                      Android.OS.Environment.DirectoryMovies,
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
    }
}

