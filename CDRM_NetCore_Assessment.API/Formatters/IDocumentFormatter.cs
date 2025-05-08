using System.Threading.Tasks;
using CDRM_NetCore_Assessment.API.Models;

namespace CDRM_NetCore_Assessment.API.Formatters
{
    public interface IDocumentFormatter
    {
        string ContentType { get; }
        Task<byte[]> SerializeAsync(Document document);
        Task<Document> DeserializeAsync(byte[] data);
    }
}
