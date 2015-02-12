using System.Collections.Generic;
using System.IO;
using FileZipper.Models;
using Ionic.Zip;

namespace FileZipper
{
    public class ZipClient
    {
        private readonly ZipFile _zf;

        public ZipClient()
        {
            _zf = new ZipFile();
        }

        /// <summary>
        /// Zips a collection of documents into one 
        /// </summary>
        /// <param name="documents">A collection of documents to be saved</param>
        /// <returns>A byte array of the finished zip.</returns>
        public byte[] ZipFiles(IEnumerable<Document> documents)
        {
            var stream = new MemoryStream();

            using(_zf)
            {
                foreach(var document in documents)
                {
                    _zf.AddEntry(document.Filename, document.Bytes);
                }
                _zf.Save(stream);
            }
            stream.Position = 0;
            return stream.ToArray();
        }
    }
}
