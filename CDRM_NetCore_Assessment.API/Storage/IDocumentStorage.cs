namespace CDRM_NetCore_Assessment.API.Storage
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using CDRM_NetCore_Assessment.API.Models;

    public interface IDocumentStorage
    {
        Task<Document> GetAsync(string id);
        Task AddAsync(Document document);
        Task UpdateAsync(Document document);
        Task<bool> ExistsAsync(string id);
        Task<IEnumerable<Document>> GetAllAsync();
    }
}
