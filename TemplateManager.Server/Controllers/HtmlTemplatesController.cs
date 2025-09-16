using Microsoft.AspNetCore.Mvc;
using TemplateManager.Application.DTO;
using TemplateManager.Application.Interfaces;

namespace TemplateManager.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HtmlTemplatesController : Controller
    {
        private readonly IHtmlTemplateService _htmlTemplateService;
        private readonly IPdfProcessor _pdfProcessor;

        public HtmlTemplatesController(IHtmlTemplateService htmlTemplateService, IPdfProcessor pdfProcessor)
        {
            _htmlTemplateService = htmlTemplateService;
            _pdfProcessor = pdfProcessor;
        }

        [HttpGet("list")]
        public async Task<ActionResult<List<HtmlTemplateDto>>> GetListAsync()
        {
            var htmlTemplates = await _htmlTemplateService.GetHtmlTemplatesAsync();

            return Ok(htmlTemplates);
        }

        [HttpGet("{htmlTemplateId}")]
        public async Task<ActionResult<HtmlTemplateDto>> GetByIdAsync(Guid htmlTemplateId)
        {
            if (htmlTemplateId == Guid.Empty)
            {
                return BadRequest("Invalid template ID.");
            }

            var htmlTemplate = await _htmlTemplateService.GetHtmlTemplateByIdAsync(htmlTemplateId);

            if (htmlTemplate == null)
            {
                return NotFound();
            }

            return Ok(htmlTemplate);
        }

        [HttpPost]
        public async Task<ActionResult<HtmlTemplateDto>> CreateAsync([FromBody] CreateHtmlTemplateDto createHtmlTemplateDto)
        {
            if (createHtmlTemplateDto == null)
            {
                return BadRequest("Request body is null.");
            }

            var htmlTemplate = await _htmlTemplateService.CreateHtmlTemplateAsync(createHtmlTemplateDto);

            return Ok(htmlTemplate);
        }

        [HttpPut("{htmlTemplateId}")]
        public async Task<ActionResult<HtmlTemplateDto>> UpdateAsync([FromRoute] Guid htmlTemplateId, [FromBody] UpdateHtmlTemplateDto updateHtmlTemplateDto)
        {
            if (updateHtmlTemplateDto == null)
            {
                return BadRequest("Request body is null.");
            }

            var updatedTemplate = await _htmlTemplateService.UpdateHtmlTemplateAsync(updateHtmlTemplateDto);

            return Ok(updatedTemplate);
        }


        [HttpDelete("{htmlTemplateId}")]
        public async Task<IActionResult> DeleteAsync(Guid htmlTemplateId)
        {
            if (htmlTemplateId == Guid.Empty)
            {
                return BadRequest("Invalid template ID.");
            }

            await _htmlTemplateService.DeleteHtmlTemplateAsync(htmlTemplateId);

            return NoContent();
        }

        [HttpPost("{htmlTemplateId}/generate-pdf")]
        public async Task<IActionResult> GeneratePdfAsync(Guid htmlTemplateId, [FromBody] Dictionary<string, string> placeholders)
        {
            if (htmlTemplateId == Guid.Empty)
            {
                return BadRequest("Invalid template ID.");
            }

            var template = await _htmlTemplateService.GetHtmlTemplateByIdAsync(htmlTemplateId);

            if (template == null)
            {
                return NotFound();
            }

            try
            {
                var pdfBytes = _pdfProcessor.GeneratePdf(template.Content, placeholders);

                return File(pdfBytes, "application/pdf", $"{template.Name}.pdf");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred.");
            }
        }
    }
}
