namespace Career635.Infrastructure.Common;
using Markdig;
using Microsoft.AspNetCore.Html;

public static class MarkdownRenderer
{
    private static readonly MarkdownPipeline _pipeline = new MarkdownPipelineBuilder()
        .UseAdvancedExtensions()
        .UseSoftlineBreakAsHardlineBreak()
        .Build();

    public static HtmlString Parse(string? markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown)) return new HtmlString(string.Empty);
        return new HtmlString(Markdown.ToHtml(markdown, _pipeline));
    }
}