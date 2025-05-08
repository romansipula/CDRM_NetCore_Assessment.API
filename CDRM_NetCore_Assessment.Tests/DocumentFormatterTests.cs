using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using CDRM_NetCore_Assessment.API.Models;
using CDRM_NetCore_Assessment.API.Formatters;

namespace CDRM_NetCore_Assessment.Tests
{
    public class DocumentFormatterTests
    {
        private Document GetSampleDocument() => new Document
        {
            Id = "doc-1",
            Tags = new List<string> { "tagA", "tagB" },
            Data = new Dictionary<string, object> { { "foo", "bar" }, { "num", 42 } }
        };

        [Theory]
        [InlineData("json")]
        [InlineData("xml")]
        [InlineData("msgpack")]
        public async Task Formatter_SerializeDeserialize_Works(string type)
        {
            IDocumentFormatter formatter = type switch
            {
                "json" => new JsonDocumentFormatter(),
                "xml" => new XmlDocumentFormatter(),
                "msgpack" => new MessagePackDocumentFormatter(),
                _ => null
            };
            var doc = GetSampleDocument();
            var bytes = await formatter.SerializeAsync(doc);
            var deserialized = await formatter.DeserializeAsync(bytes);
            Assert.NotNull(deserialized);
            Assert.Equal(doc.Id, deserialized.Id);
            Assert.Equal(doc.Tags, deserialized.Tags);
            Assert.Equal(doc.Data["foo"].ToString(), deserialized.Data["foo"].ToString());
            Assert.Equal(doc.Data["num"].ToString(), deserialized.Data["num"].ToString());
        }
    }
}
