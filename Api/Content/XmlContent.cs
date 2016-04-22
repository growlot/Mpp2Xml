using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Mpp2XmlApi.Content
{
    public class XmlContent : HttpContent
    {
        private readonly MemoryStream _Stream = new MemoryStream();

        public XmlContent(XmlDocument document)
        {
            document.Save(_Stream);
            _Stream.Position = 0;
            Headers.ContentType = new MediaTypeHeaderValue("application/xml");
        }

        protected override Task SerializeToStreamAsync(Stream stream, System.Net.TransportContext context)
        {

            _Stream.CopyTo(stream);

            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _Stream.Length;
            return true;
        }
    }
}