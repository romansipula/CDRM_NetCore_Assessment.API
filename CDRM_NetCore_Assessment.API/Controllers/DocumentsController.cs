using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using CDRM_NetCore_Assessment.API.Models;
using CDRM_NetCore_Assessment.API.Storage;
using CDRM_NetCore_Assessment.API.Formatters;

namespace CDRM_NetCore_Assessment.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentStorage _storage;
        private readonly IEnumerable<IDocumentFormatter> _formatters;

        public DocumentsController(IDocumentStorage storage, IEnumerable<IDocumentFormatter> formatters)
        {
            _storage = storage;
            _formatters = formatters;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Document document)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (await _storage.ExistsAsync(document.Id))
                return Conflict("Document with this ID already exists.");
            await _storage.AddAsync(document);
            return CreatedAtAction(nameof(Get), new { id = document.Id }, document);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, [FromBody] Document document)
        {
            if (document == null || id != document.Id)
                return BadRequest("ID mismatch or invalid document.");
            if (!await _storage.ExistsAsync(id))
                return NotFound();
            await _storage.UpdateAsync(document);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var doc = await _storage.GetAsync(id);
            if (doc == null)
                return NotFound();

            var accept = Request.Headers["Accept"].ToString();
            var formatter = _formatters.FirstOrDefault(f => accept.Contains(f.ContentType, StringComparison.OrdinalIgnoreCase))
                ?? _formatters.First(f => f.ContentType == MediaTypeNames.Application.Json); // default to JSON

            var bytes = await formatter.SerializeAsync(doc);
            return File(bytes, formatter.ContentType);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            return StatusCode(405, "Delete operation is not allowed.");
        }

        [HttpPatch("{id}")]
        public IActionResult Patch(string id)
        {
            return StatusCode(405, "Patch operation is not allowed.");
        }
    }
}
