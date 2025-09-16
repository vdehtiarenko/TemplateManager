namespace TemplateManager.Application.Interfaces
{
    public interface IPdfProcessor
    {
        byte[] GeneratePdf(string htmlTemplate, Dictionary<string, string> placeholders);
    }
}
