using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Stenography.Core.Image
{
    public sealed class ImagePatchworkTransformService : IDisposable
    {
        private readonly PatchworkTransformService _patchworkTransformService;
        private readonly Bitmap _bitmap;
        private bool _disposed;

        public ImagePatchworkTransformService(PatchworkTransformService patchworkTransformService, string path)
        {
            this._bitmap = new Bitmap(path);
            this._patchworkTransformService = patchworkTransformService;
        }

        public void ApplyTransform()
        {
            var rect = new Rectangle(0, 0, this._bitmap.Width, this._bitmap.Height);
            BitmapData bitmapData = this._bitmap.LockBits(rect, ImageLockMode.ReadWrite, this._bitmap.PixelFormat);
            IntPtr startPtr = bitmapData.Scan0;

            int imageSize = Math.Abs(bitmapData.Stride) * this._bitmap.Height;
            var rgbBytes = new byte[imageSize];
            Marshal.Copy(startPtr, rgbBytes, 0, imageSize);

            this._patchworkTransformService.ApplyTransform(rgbBytes);

            Marshal.Copy(rgbBytes, 0, startPtr, imageSize);
            this._bitmap.UnlockBits(bitmapData);
        }

        public int Analyze()
        {
            var rect = new Rectangle(0, 0, this._bitmap.Width, this._bitmap.Height);
            BitmapData bitmapData = this._bitmap.LockBits(rect, ImageLockMode.ReadWrite, this._bitmap.PixelFormat);
            IntPtr startPtr = bitmapData.Scan0;

            int imageSize = Math.Abs(bitmapData.Stride) * this._bitmap.Height;
            var rgbBytes = new byte[imageSize];
            Marshal.Copy(startPtr, rgbBytes, 0, imageSize);

            var result = this._patchworkTransformService.Analyze(rgbBytes);

            Marshal.Copy(rgbBytes, 0, startPtr, imageSize);
            this._bitmap.UnlockBits(bitmapData);

            return result;
        }

        public void Save(string filePath)
        {
            this._bitmap.Save(filePath);
        }

        public void Dispose()
        {
         this.Dispose(true);   
        }

        private void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._bitmap.Dispose();
                    this._disposed = true;
                }
            }
        }
    }
}
