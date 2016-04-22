using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using System.IO;
using System.Net.Http.Headers;
using Mpp2XmlApi.Content;

namespace Mpp2XmlApi.Controllers
{
    public class Mpp2XmlController : ApiController
    {
        public async Task<HttpResponseMessage> PostAsync()
        {
            if (Request.Content.IsMimeMultipartContent())
            {
                string uploadPath = HttpContext.Current.Server.MapPath("~/uploads");

                MyStreamProvider streamProvider = new MyStreamProvider(uploadPath);

                await Request.Content.ReadAsMultipartAsync(streamProvider);
                
                MppXml xml = null;
                foreach (var file in streamProvider.FileData)
                {
                    FileInfo fi = new FileInfo(file.LocalFileName);
                    xml = new MppXml(file.LocalFileName);
                    break;
                }
                if (xml != null && xml.Xml != null)
                {
                    return new HttpResponseMessage()
                    {
                        RequestMessage = Request,
                        StatusCode = HttpStatusCode.OK,
                        //Content = new XmlContent(xml.Xml)
                        Content = new StringContent(xml.Xml.InnerXml)
                    };
                }
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Request!"));
            }
            else
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid Request!"));
            }
        }
    }


    public class MyStreamProvider : MultipartFormDataStreamProvider
    {
        public MyStreamProvider(string uploadPath)
            : base(uploadPath)
        {

        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            string fileName = headers.ContentDisposition.FileName;
            if(string.IsNullOrWhiteSpace(fileName))
            {
                fileName = Guid.NewGuid().ToString() + ".data";
            }
            return fileName.Replace("\"", string.Empty);
        }
    }
}