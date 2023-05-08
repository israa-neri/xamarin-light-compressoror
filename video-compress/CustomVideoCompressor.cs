using System;
using System.Collections.Generic;
using Com.Abedelazizshe.Lightcompressorlibrary;
using Com.Abedelazizshe.Lightcompressorlibrary.Config;
using Java.Lang;
using Xamarin.Essentials;

namespace video_compress
{
	public class CustomVideoCompressor : IVideoCompressor
    {
		IVideoCompressorListener _listener;
		List<Android.Net.Uri> _uris;
        private MainActivity _mainActivity;

        public CustomVideoCompressor(
            IVideoCompressorListener listener,
            List<Android.Net.Uri> uris,
            MainActivity mainActivity
            )
		{
			_listener = listener;
			_uris = uris;
        }

		public void StartCompression() {

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

            var vcl = new VideoCompressorListener(_listener);

            VideoCompressor.Start(_mainActivity,
                                      _uris,
                                      $"{Android.OS.Environment.DirectoryMovies}/compressedVideos",
                                      vcl,
                                      configuration);
        }

        public class VideoCompressorListener : Java.Lang.Object, ICompressionListener
        {
            IVideoCompressorListener _listener;

            public VideoCompressorListener(IVideoCompressorListener listener)
            {
                _listener = listener;
            }

            public void OnCancelled(int index)
            {
                _listener.OnCompressCancelled(index);
            }

            public void OnFailure(int index, string failureMessage)
            {
                _listener.OnCompressFailure(index, failureMessage);
            }

            public void OnProgress(int index, float percent)
            {
                _listener.OnCompressProgress(index, percent);
            }

            public void OnStart(int index)
            {
                _listener.OnCompressStart(index);
            }

            public void OnSuccess(int index, long size, string path)
            {
                _listener.OnCompressSuccess(index, size, path);
            }
        }
    }
}

