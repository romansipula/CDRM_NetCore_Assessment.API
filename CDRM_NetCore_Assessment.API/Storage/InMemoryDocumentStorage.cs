using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CDRM_NetCore_Assessment.API.Models;

namespace CDRM_NetCore_Assessment.API.Storage
{
    public class InMemoryDocumentStorage : IDocumentStorage
    {
        private readonly ConcurrentDictionary<string, Document> _documents = new();

        public Task<Document> GetAsync(string id)
        {
            _documents.TryGetValue(id, out var doc);
            return Task.FromResult(doc ?? new Document { Id = id });
        }

        public Task AddAsync(Document document)
        {
            _documents[document.Id] = document;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Document document)
        {
            _documents[document.Id] = document;
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(string id)
        {
            return Task.FromResult(_documents.ContainsKey(id));
        }

        public Task<IEnumerable<Document>> GetAllAsync()
        {
            return Task.FromResult(_documents.Values.AsEnumerable());
        }
    }
}
