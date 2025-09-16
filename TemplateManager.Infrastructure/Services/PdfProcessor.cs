using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Text.RegularExpressions;
using TemplateManager.Application.Interfaces;

namespace TemplateManager.Infrastructure.Services
{
    public class PdfProcessor : IPdfProcessor
    {
        public byte[] GeneratePdf(string htmlTemplate, Dictionary<string, string> placeholders)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            ValidatePlaceholders(htmlTemplate, placeholders);

            string processedHtml = ReplacePlaceholders(htmlTemplate, placeholders);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(50);
                    page.Content().Element(ComposeContent);

                    void ComposeContent(IContainer c)
                    {
                        c.Column(col =>
                        {
                            col.Item().Text(processedHtml).FontSize(12);
                        });
                    }
                });
            });

            return document.GeneratePdf();
        }

        private string ReplacePlaceholders(string template, Dictionary<string, string> placeholders)
        {
            if (placeholders == null || placeholders.Count == 0)
                return template;

            foreach (var pair in placeholders)
            {
                string pattern = "{{" + Regex.Escape(pair.Key) + "}}";
                template = Regex.Replace(template, pattern, pair.Value ?? string.Empty, RegexOptions.IgnoreCase);
            }

            return template;
        }

        private void ValidatePlaceholders(string template, Dictionary<string, string> placeholders)
        {
            if (placeholders == null)
            {
                throw new ArgumentException("Placeholders cannot be null.");
            }

            var matches = Regex.Matches(template, "{{(.*?)}}");

            var templateKeys = matches.Select(m => m.Groups[1].Value).ToHashSet(StringComparer.OrdinalIgnoreCase);

            var placeholderKeys = placeholders.Keys.ToHashSet(StringComparer.OrdinalIgnoreCase);

            var missingKeys = templateKeys.Except(placeholderKeys).ToList();

            if (missingKeys.Any())
            {
                throw new ArgumentException($"Missing placeholders in JSON: {string.Join(", ", missingKeys)}");
            }

            var extraKeys = placeholderKeys.Except(templateKeys).ToList();

            if (extraKeys.Any())
            {
                throw new ArgumentException($"Extra keys in JSON not used in template: {string.Join(", ", extraKeys)}");
            }
        }
    }
}
