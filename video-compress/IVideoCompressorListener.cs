using System;
namespace video_compress
{
	public interface IVideoCompressorListener
	{
        public void OnCompressCancelled(int index);
        public void OnCompressFailure(int index, string failureMessage);
        public void OnCompressProgress(int index, float percent);
        public void OnCompressStart(int index);
        public void OnCompressSuccess(int index, long size, string path);
    }
}

