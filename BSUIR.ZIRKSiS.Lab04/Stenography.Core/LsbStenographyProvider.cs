using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stenography.Core
{
    internal sealed class LsbStenographyProvider : IStenographyProvider, IDisposable
    {
        private bool _disposed;

        private readonly ILsbDataWriter _lsbDataWriter;

        private readonly ILsbDataAnalyzer _lsbDataAnalyzer;

        public LsbStenographyProvider(
            ILsbDataWriter dataWriter,
            ILsbDataAnalyzer dataAnalyzer)
        {
            this._lsbDataWriter = dataWriter;
            this._lsbDataAnalyzer = dataAnalyzer;
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
                    this._lsbDataWriter.Dispose();
                    this._lsbDataAnalyzer.Dispose();
                    this._disposed = true;
                }
            }
        }
    }
}
