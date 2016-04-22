using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using net.sf.mpxj;
using net.sf.mpxj.reader;
using net.sf.mpxj.writer;
using System.IO;

namespace Mpp2XmlApi.Content
{
    public class MppXml
    {
        private XmlDocument xmlDocument = null;

        public XmlDocument Xml
        {
            get
            {
                return xmlDocument;
            }
        }

        public string InnerXml
        {
            get
            {
                return xmlDocument.InnerXml;
            }
        }

        public MppXml(string localFileName)
        {
            try
            {
                string xmlFileName = localFileName + ".xml";
                ProjectReader reader = ProjectReaderUtility.getProjectReader(localFileName);
                ProjectFile projectFile = reader.read(localFileName);
                ProjectWriter writer = ProjectWriterUtility.getProjectWriter(xmlFileName);
                writer.write(projectFile, xmlFileName);
                XmlDocument tmpDoc = new XmlDocument();
                tmpDoc.Load(xmlFileName);
                xmlDocument = tmpDoc;
                File.Delete(localFileName);
                File.Delete(xmlFileName);
            }
            catch (Exception)
            {
            }
        }
    }
}