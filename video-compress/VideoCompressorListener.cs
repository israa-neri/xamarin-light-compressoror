﻿using System;
using Com.Abedelazizshe.Lightcompressorlibrary;
using Xamarin.Essentials;

namespace video_compress
{
    public class VideoCompressorListener : Java.Lang.Object, ICompressionListener
    {
        private MainActivity _mainActivity;
        public VideoCompressorListener(MainActivity mainActivity)
        {
            _mainActivity = mainActivity;
        }

        public void OnCancelled(int index)
        {
            System.Diagnostics.Debug.WriteLine("\n\n");
            System.Diagnostics.Debug.WriteLine("OnCancelled");
            System.Diagnostics.Debug.WriteLine(index);
            _mainActivity.CompressingProgress.Text = $"0 % Compression Cancelled!";
        }

        public void OnFailure(int index, string failureMessage)
        {
            System.Diagnostics.Debug.WriteLine("\n\n");
            System.Diagnostics.Debug.WriteLine("OnFailure");
            System.Diagnostics.Debug.WriteLine(index);
            System.Diagnostics.Debug.WriteLine(failureMessage);
            _mainActivity.CompressingProgress.Text = $"0 % Compression failed!";
        }

        public void OnProgress(int index, float percent)
        {
            System.Diagnostics.Debug.WriteLine("\n\n");
            System.Diagnostics.Debug.WriteLine("OnProgress");
            System.Diagnostics.Debug.WriteLine(index);
            System.Diagnostics.Debug.WriteLine(percent);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                _mainActivity.CompressingProgress.Text = $"{percent.ToString()} %";
            });
        }

        public void OnStart(int index)
        {
            System.Diagnostics.Debug.WriteLine("\n\n");
            System.Diagnostics.Debug.WriteLine("OnStart");
            System.Diagnostics.Debug.WriteLine(index);
            _mainActivity.CompressingProgress.Text = "0 %";
        }

        public void OnSuccess(int index, long size, string path)
        {
            System.Diagnostics.Debug.WriteLine("\n\n");
            System.Diagnostics.Debug.WriteLine("OnSuccess");
            System.Diagnostics.Debug.WriteLine(index);
            System.Diagnostics.Debug.WriteLine(size);
            System.Diagnostics.Debug.WriteLine(path);

            _mainActivity.CompressingProgress.Text = "100 %";

            _mainActivity.CompressedName.Text = path;
            _mainActivity.CompressedSize.Text = size.ToString();
        }
    }
}

