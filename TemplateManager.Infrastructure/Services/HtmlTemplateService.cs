using TemplateManager.Application.DTO;
using TemplateManager.Application.Interfaces;
using TemplateManager.Domain.Entities;
using TemplateManager.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace TemplateManager.Infrastructure.Services
{
    public class HtmlTemplateService : IHtmlTemplateService
    {
        private readonly ApplicationDbContext _dbContext;

        public HtmlTemplateService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<HtmlTemplateDto>> GetHtmlTemplatesAsync()
        {
            var htmlTemplates = await _dbContext.HtmlTemplates.ToListAsync();

            var htmlTemplateDtos = htmlTemplates
                .Select(t => new HtmlTemplateDto
                {
                    Id = t.Id,
                    Name = t.Name,
                    Content = t.Content
                })
                .ToList();

            return htmlTemplateDtos;
        }

        public async Task<HtmlTemplateDto> GetHtmlTemplateByIdAsync(Guid htmlTemplateId)
        {
            if (htmlTemplateId == Guid.Empty)
            {
                throw new ArgumentException("The template ID cannot be empty.");
            }

            var htmlTemplate = await _dbContext.HtmlTemplates.FirstOrDefaultAsync(t => t.Id == htmlTemplateId);

            if (htmlTemplate == null)
            {
                throw new ArgumentException("Template with specified ID does not exist.");
            }

            var htmlTemplateDto = new HtmlTemplateDto
            {
                Id = htmlTemplate.Id,
                Name = htmlTemplate.Name,
                Content = htmlTemplate.Content
            };

            return htmlTemplateDto;
        }

        public async Task<HtmlTemplateDto> CreateHtmlTemplateAsync(CreateHtmlTemplateDto createHtmlTemplateDto)
        {
            var existingHtmlTemplate = await _dbContext.HtmlTemplates
                .FirstOrDefaultAsync(t => t.Name == createHtmlTemplateDto.Name && t.Content == createHtmlTemplateDto.Content);

            if (existingHtmlTemplate != null)
            {
                var htmlTemplateDto = new HtmlTemplateDto
                {
                    Id = existingHtmlTemplate.Id,
                    Name = existingHtmlTemplate.Name,
                    Content = existingHtmlTemplate.Content
                };

                return htmlTemplateDto;
            }

            var id = Guid.NewGuid();

            var htmlTemplate = new HtmlTemplate(id, createHtmlTemplateDto.Name, createHtmlTemplateDto.Content);

            await _dbContext.HtmlTemplates.AddAsync(htmlTemplate);

            await _dbContext.SaveChangesAsync();

            var newHtmlTemplate = new HtmlTemplateDto
            {
                Id = htmlTemplate.Id,
                Name = htmlTemplate.Name,
                Content = htmlTemplate.Content
            };

            return newHtmlTemplate;
        }

        public async Task<HtmlTemplateDto> UpdateHtmlTemplateAsync(UpdateHtmlTemplateDto updateHtmlTemplateDto)
        {
            if (updateHtmlTemplateDto.Id == Guid.Empty)
            {
                throw new ArgumentException("The template ID cannot be empty.");
            }

            var existingHtmlTemplate = await _dbContext.HtmlTemplates.FirstOrDefaultAsync(t => t.Id == updateHtmlTemplateDto.Id);

            if (existingHtmlTemplate == null)
            {
                throw new InvalidOperationException("Template with specified ID does not exist.");
            }

            existingHtmlTemplate.Name = updateHtmlTemplateDto.Name;
            existingHtmlTemplate.Content = updateHtmlTemplateDto.Content;

            await _dbContext.SaveChangesAsync();

            var htmlTemplateDto = new HtmlTemplateDto
            {
                Id = existingHtmlTemplate.Id,
                Name = existingHtmlTemplate.Name,
                Content = existingHtmlTemplate.Content
            };

            return htmlTemplateDto;
        }

        public async Task DeleteHtmlTemplateAsync(Guid htmlTemplateId)
        {
            if (htmlTemplateId == Guid.Empty)
            {
                throw new ArgumentException("The template ID cannot be empty.");
            }

            var existingHtmlTemplate = await _dbContext.HtmlTemplates.FirstOrDefaultAsync(t => t.Id == htmlTemplateId);

            if (existingHtmlTemplate == null)
            {
                throw new InvalidOperationException("Template with specified ID does not exist.");
            }

            _dbContext.HtmlTemplates.Remove(existingHtmlTemplate);

            await _dbContext.SaveChangesAsync();
        }
    }
}
