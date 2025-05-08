using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;
using CDRM_NetCore_Assessment.API.Controllers;
using CDRM_NetCore_Assessment.API.Models;
using CDRM_NetCore_Assessment.API.Storage;
using CDRM_NetCore_Assessment.API.Formatters;

namespace CDRM_NetCore_Assessment.Tests
{
    public class DocumentsControllerTests
    {
        private Document GetSampleDocument(string id = "doc-1") => new Document
        {
            Id = id,
            Tags = new List<string> { "tagA", "tagB" },
            Data = new Dictionary<string, object> { { "foo", "bar" }, { "num", 42 } }
        };

        [Fact]
        public async Task Post_ReturnsCreated_WhenValid()
        {
            var storage = new Mock<IDocumentStorage>();
            storage.Setup(s => s.ExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            var formatters = new List<IDocumentFormatter>();
            var controller = new DocumentsController(storage.Object, formatters);
            var doc = GetSampleDocument();
            var result = await controller.Post(doc);
            Assert.IsType<CreatedAtActionResult>(result);
        }

        [Fact]
        public async Task Post_ReturnsConflict_WhenDuplicateId()
        {
            var storage = new Mock<IDocumentStorage>();
            storage.Setup(s => s.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            var formatters = new List<IDocumentFormatter>();
            var controller = new DocumentsController(storage.Object, formatters);
            var doc = GetSampleDocument();
            var result = await controller.Post(doc);
            var conflict = Assert.IsType<ConflictObjectResult>(result);
            Assert.Contains("already exists", conflict.Value.ToString());
        }

        [Fact]
        public async Task Put_ReturnsNoContent_WhenValid()
        {
            var storage = new Mock<IDocumentStorage>();
            storage.Setup(s => s.ExistsAsync(It.IsAny<string>())).ReturnsAsync(true);
            var formatters = new List<IDocumentFormatter>();
            var controller = new DocumentsController(storage.Object, formatters);
            var doc = GetSampleDocument();
            var result = await controller.Put(doc.Id, doc);
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Put_ReturnsNotFound_WhenNotExists()
        {
            var storage = new Mock<IDocumentStorage>();
            storage.Setup(s => s.ExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            var formatters = new List<IDocumentFormatter>();
            var controller = new DocumentsController(storage.Object, formatters);
            var doc = GetSampleDocument();
            var result = await controller.Put(doc.Id, doc);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Get_ReturnsNotFound_WhenNotExists()
        {
            var storage = new Mock<IDocumentStorage>();
            storage.Setup(s => s.GetAsync(It.IsAny<string>())).ReturnsAsync((Document)null);
            var formatters = new List<IDocumentFormatter> { new JsonDocumentFormatter() };
            var controller = new DocumentsController(storage.Object, formatters);
            var result = await controller.Get("notfound");
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
