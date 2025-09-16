namespace TemplateManager.Domain.Entities
{
    public class HtmlTemplate
    {
        public Guid Id { get; set; } = Guid.NewGuid(); 

        public string Name { get; set; }      
        
        public string Content { get; set; }

        public HtmlTemplate(Guid id, string name, string content)
        {
            Id = id;
            Name = name;
            Content = content;
        }
    }
}
