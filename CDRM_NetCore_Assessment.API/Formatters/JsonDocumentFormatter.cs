using System.Text.Json;
using System.Threading.Tasks;
using CDRM_NetCore_Assessment.API.Models;

namespace CDRM_NetCore_Assessment.API.Formatters
{
    public class JsonDocumentFormatter : IDocumentFormatter
    {
        public string ContentType => "application/json";

        public Task<byte[]> SerializeAsync(Document document)
        {
            var json = JsonSerializer.Serialize(document);
            return Task.FromResult(System.Text.Encoding.UTF8.GetBytes(json));
        }

        public Task<Document> DeserializeAsync(byte[] data)
        {
            var json = System.Text.Encoding.UTF8.GetString(data);
            var doc = JsonSerializer.Deserialize<Document>(json) 
                      ?? throw new JsonException("Deserialization returned null.");
            return Task.FromResult(doc);
        }
    }
}
