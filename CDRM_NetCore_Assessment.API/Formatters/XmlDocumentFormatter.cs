using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using CDRM_NetCore_Assessment.API.Models;

namespace CDRM_NetCore_Assessment.API.Formatters
{
    public class XmlDocumentFormatter : IDocumentFormatter
    {
        public string ContentType => "application/xml";

        public Task<byte[]> SerializeAsync(Document document)
        {
            var serializer = new XmlSerializer(typeof(Document));
            using var ms = new MemoryStream();
            serializer.Serialize(ms, document);
            return Task.FromResult(ms.ToArray());
        }

        public Task<Document> DeserializeAsync(byte[] data)
        {
            var serializer = new XmlSerializer(typeof(Document));
            using var ms = new MemoryStream(data);
            var doc = (Document?)serializer.Deserialize(ms);
            if (doc == null)
            {
                throw new InvalidDataException("Deserialization resulted in a null Document.");
            }
            return Task.FromResult(doc);
        }
    }
}
