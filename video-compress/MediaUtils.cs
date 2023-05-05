using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace video_compress
{
	public class MediaUtils
	{
		public MediaUtils()
		{
		}

        public static async Task<FileResult> TakeVideoAsync()
        {
            try
            {
                var path = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
                //var photo = await MediaPicker.CapturePhotoAsync();
                var video = await MediaPicker.CaptureVideoAsync();
                //var photoResult = await LoadPhotoAsync(video);
                //Console.WriteLine($"CapturePhotoAsync COMPLETED: {path}");
                return video;
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature is not supported on the device
                return null;
            }
            catch (PermissionException pEx)
            {
                // Permissions not granted
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
                return null;
            }
        }

        public static async Task<string> LoadMediaAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                return "";
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            return newFile;
        }
    }
}

