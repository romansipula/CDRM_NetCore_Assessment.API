using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using CDRM_NetCore_Assessment.API.Models;
using CDRM_NetCore_Assessment.API.Storage;

namespace CDRM_NetCore_Assessment.Tests
{
    public class InMemoryDocumentStorageTests
    {
        [Fact]
        public async Task AddAndGetDocument_WorksCorrectly()
        {
            var storage = new InMemoryDocumentStorage();
            var doc = new Document
            {
                Id = "test-id",
                Tags = new List<string> { "tag1", "tag2" },
                Data = new Dictionary<string, object> { { "key", "value" } }
            };

            await storage.AddAsync(doc);
            var retrieved = await storage.GetAsync("test-id");

            Assert.NotNull(retrieved);
            Assert.Equal(doc.Id, retrieved.Id);
            Assert.Equal(doc.Tags, retrieved.Tags);
            Assert.Equal(doc.Data["key"], retrieved.Data["key"]);
        }
    }
}
