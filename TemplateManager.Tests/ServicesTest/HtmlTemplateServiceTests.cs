using Moq;
using Microsoft.EntityFrameworkCore;
using TemplateManager.Infrastructure.DAL;
using TemplateManager.Domain.Entities;
using TemplateManager.Application.DTO;
using TemplateManager.Infrastructure.Services;

public class HtmlTemplateServiceTests
{
    private readonly Mock<DbSet<HtmlTemplate>> _mockHtmlTemplateDbSet;
    private readonly Mock<ApplicationDbContext> _mockDbContext;
    private readonly HtmlTemplateService _htmlTemplateService;

    public HtmlTemplateServiceTests()
    {
        var htmlTemplates = new List<HtmlTemplate>();
        _mockHtmlTemplateDbSet = CreateMockDbSet(htmlTemplates);

        _mockDbContext = new Mock<ApplicationDbContext>();
        _mockDbContext.Setup(db => db.HtmlTemplates).Returns(_mockHtmlTemplateDbSet.Object);
        _mockDbContext.Setup(db => db.SaveChangesAsync(default)).ReturnsAsync(1);

        _htmlTemplateService = new HtmlTemplateService(_mockDbContext.Object);
    }

    [Fact]
    public void GetHtmlTemplateByIdAsync_ThrowsArgumentException_WhenIdIsEmpty()
    {
        // Arrange

        Guid emptyId = Guid.Empty;

        // Act

        var exception = Assert.ThrowsAsync<ArgumentException>(
            () => _htmlTemplateService.GetHtmlTemplateByIdAsync(emptyId))
            .GetAwaiter().GetResult();

        // Assert

        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("The template ID cannot be empty.", exception.Message);
    }

    [Fact]
    public void UpdateHtmlTemplateAsync_ThrowsArgumentException_WhenIdIsEmpty()
    {
        // Arrange

        var updateDto = new UpdateHtmlTemplateDto { Id = Guid.Empty, Name = "Test", Content = "Content" };

        // Act

        var exception = Assert.ThrowsAsync<ArgumentException>(
            () => _htmlTemplateService.UpdateHtmlTemplateAsync(updateDto))
            .GetAwaiter().GetResult();

        // Assert

        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("The template ID cannot be empty.", exception.Message);
    }

    [Fact]
    public void DeleteHtmlTemplateAsync_ThrowsArgumentException_WhenIdIsEmpty()
    {
        // Arrange

        Guid emptyId = Guid.Empty;

        // Act

        var exception = Assert.ThrowsAsync<ArgumentException>(
            () => _htmlTemplateService.DeleteHtmlTemplateAsync(emptyId))
            .GetAwaiter().GetResult();

        // Assert

        Assert.IsType<ArgumentException>(exception);
        Assert.Equal("The template ID cannot be empty.", exception.Message);
    }

    private Mock<DbSet<T>> CreateMockDbSet<T>(IList<T> sourceList) where T : class
    {
        var queryable = sourceList.AsQueryable();
        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());

        mockSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>(sourceList.Add);
        mockSet.Setup(d => d.Remove(It.IsAny<T>())).Callback<T>(t => sourceList.Remove(t));

        return mockSet;
    }
}
