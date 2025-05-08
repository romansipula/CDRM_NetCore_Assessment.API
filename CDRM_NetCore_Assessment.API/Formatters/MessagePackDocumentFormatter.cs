using System.Threading.Tasks;
using CDRM_NetCore_Assessment.API.Models;
using MessagePack;
using MessagePack.Resolvers;

namespace CDRM_NetCore_Assessment.API.Formatters
{
    public class MessagePackDocumentFormatter : IDocumentFormatter
    {
        public string ContentType => "application/x-msgpack";

        private static readonly MessagePackSerializerOptions Options =
            MessagePackSerializerOptions.Standard.WithResolver(ContractlessStandardResolver.Instance);

        public Task<byte[]> SerializeAsync(Document document)
        {
            var bytes = MessagePackSerializer.Serialize(document, Options);
            return Task.FromResult(bytes);
        }

        public Task<Document> DeserializeAsync(byte[] data)
        {
            var doc = MessagePackSerializer.Deserialize<Document>(data, Options);
            return Task.FromResult(doc);
        }
    }
}
