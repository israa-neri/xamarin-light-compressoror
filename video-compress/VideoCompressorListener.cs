﻿using System;
using System.Collections.Generic;
using Com.Abedelazizshe.Lightcompressorlibrary;
using Java.Lang;
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
            //_mainActivity.CompressedSize.Text = $"{(size / 1000000).ToString()} Mb";
            _mainActivity.CompressedSize.Text = $"{(size).ToString()}";
        }

        public string GetFileSize(Long size)
        {
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

