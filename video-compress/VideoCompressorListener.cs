using Com.Abedelazizshe.Lightcompressorlibrary;

namespace video_compress
{
    public class VideoCompressorListener : Java.Lang.Object, ICompressionListener
    {
        public VideoCompressorListener()
        {
        }

        public void OnCancelled(int index)
        {
            System.Diagnostics.Debug.WriteLine("\n\n");
            System.Diagnostics.Debug.WriteLine("OnCancelled");
            System.Diagnostics.Debug.WriteLine(index);

        }

        public void OnFailure(int index, string failureMessage)
        {
            System.Diagnostics.Debug.WriteLine("\n\n");
            System.Diagnostics.Debug.WriteLine("OnFailure");
            System.Diagnostics.Debug.WriteLine(index);
            System.Diagnostics.Debug.WriteLine(failureMessage);
        }

        public void OnProgress(int index, float percent)
        {
            System.Diagnostics.Debug.WriteLine("\n\n");
            System.Diagnostics.Debug.WriteLine("OnProgress");
            System.Diagnostics.Debug.WriteLine(index);
            System.Diagnostics.Debug.WriteLine(percent);
        }

        public void OnStart(int index)
        {
            System.Diagnostics.Debug.WriteLine("\n\n");
            System.Diagnostics.Debug.WriteLine("OnStart");
            System.Diagnostics.Debug.WriteLine(index);
        }

        public async void OnSuccess(int index, long size, string path)
        {
            System.Diagnostics.Debug.WriteLine("\n\n");
            System.Diagnostics.Debug.WriteLine("OnSuccess");
            System.Diagnostics.Debug.WriteLine(index);
            System.Diagnostics.Debug.WriteLine(size);
            System.Diagnostics.Debug.WriteLine(path);

        }
    }
}

