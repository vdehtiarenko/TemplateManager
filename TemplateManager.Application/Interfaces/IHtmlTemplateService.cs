using TemplateManager.Application.DTO;

namespace TemplateManager.Application.Interfaces
{
    public interface IHtmlTemplateService
    {
        Task<List<HtmlTemplateDto>> GetHtmlTemplatesAsync();

        Task<HtmlTemplateDto> GetHtmlTemplateByIdAsync(Guid htmlTemplateId);

        Task<HtmlTemplateDto> CreateHtmlTemplateAsync(CreateHtmlTemplateDto createHtmlTemplateDto);

        Task<HtmlTemplateDto> UpdateHtmlTemplateAsync(UpdateHtmlTemplateDto updateHtmlTemplateDto);

        Task DeleteHtmlTemplateAsync(Guid htmlTemplateId);
    }
}
