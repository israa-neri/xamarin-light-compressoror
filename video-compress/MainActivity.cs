using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.Snackbar;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Java.Lang;
using Android;
using Java.Net;
using Org.Apache.Commons.Logging;
using Android.Content;
using Java.IO;
using Xamarin.Essentials;
using Iceteck.SiliCompressorrLib;

namespace video_compress
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        public Button btSelect;
        public VideoView videoView1, videoView2;
        public TextView textView1, textView2, textView3;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            btSelect = FindViewById<Button>(Resource.Id.bt_select);
            videoView1 = FindViewById<VideoView>(Resource.Id.video_view1);
            videoView2 = FindViewById<VideoView>(Resource.Id.Video_view2);
            textView1 = FindViewById<TextView>(Resource.Id.TextView1);
            textView2 = FindViewById<TextView>(Resource.Id.text_view2);
            textView3 = FindViewById<TextView>(Resource.Id.TextView3);

            btSelect.Click += (sender, args) =>
            {
                // check condition
                if (ContextCompat.CheckSelfPermission(
                        this,
                        Manifest.Permission
                            .WriteExternalStorage)
                    == Android.Content.PM.Permission.Granted)
                {
                    // When permission is granted
                    // Create method
                    selectVideo();
                }
                else
                {
                    // When permission is not granted
                    // request permission
                    ActivityCompat.RequestPermissions(
                        this,
                        new string[] {
                                Manifest.Permission
                                    .WriteExternalStorage },
                        1);
                }
            };
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
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void selectVideo()
        {
            // Initialize intent
            Intent intent = new Intent(Intent.Action);
            // Set type
            intent.SetType("video/*");
            // set action
            intent.SetAction(Intent.ActionGetContent);
            // Start activity result
            StartActivityForResult(
                Intent.CreateChooser(intent, "Select Video"),
                100);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            base.OnActivityResult(requestCode, resultCode,
                                   data);
            // Check condition
            if (requestCode == 100 && resultCode == Result.Ok
                && data != null)
            {
                // When result is ok
                // Initialize Uri
                var uri = data.Data;
                // Set video uri
                videoView1.SetVideoURI(uri);
                // Initialize file
                File file = new File(
                    Android.OS.Environment.ExternalStorageDirectory.AbsolutePath);
                // Create compress video method
                new CompressVideo(this).Execute(
                    "false", uri.ToString(), file.Path);
            }
        }

        private class CompressVideo: AsyncTask<string, string, string> {

            private MainActivity _activity;
            // Initialize dialog
            Dialog dialog;

            public CompressVideo(MainActivity activity)
            {
                _activity = activity;
            }

            protected override void OnPreExecute()
            {
                base.OnPreExecute();
                // Display dialog
                dialog = ProgressDialog.Show(
                    (Platform.CurrentActivity as AppCompatActivity), "", "Compressing...");
            }

            protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] native_parms)
            {
                // Initialize video path
                string videoPath = null;

                try
                {
                    // Initialize uri
                    Android.Net.Uri uri = Android.Net.Uri.Parse((string)native_parms[1]);
                    // Compress video
                    videoPath
                        = SiliCompressor.With(_activity)
                              .CompressVideo(uri, (string)native_parms[2]);
                }
                catch (URISyntaxException e)
                {
                    e.PrintStackTrace();
                }
                // Return Video path
                return videoPath;
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                base.OnPostExecute(result);
                // Dismiss dialog
                dialog.Dismiss();

                // Visible all views
                _activity.videoView1.Visibility = ViewStates.Visible;
                _activity.textView1.Visibility = ViewStates.Visible;
                _activity.videoView2.Visibility = ViewStates.Visible;
                _activity.textView2.Visibility = ViewStates.Visible;
                _activity.textView3.Visibility = ViewStates.Visible;

                // Initialize file
                File file = new File((string)result);
                // Initialize uri
                Android.Net.Uri uri = Android.Net.Uri.FromFile(file);
                // set video uri
                _activity.videoView2.SetVideoURI(uri);

                // start both video
                _activity.videoView1.Start();
                _activity.videoView2.Start();

                // Compress video size
                float size = file.Length() / 1024f;
                // Set size on text view
                _activity.textView3.Text = string.Format("Size : %.2f KB", size);
            }

            protected override string RunInBackground(params string[] @params)
            {
                throw new NotImplementedException();
            }
        }
}
}
