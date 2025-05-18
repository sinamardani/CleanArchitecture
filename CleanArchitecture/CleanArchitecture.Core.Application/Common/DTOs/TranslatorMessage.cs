namespace CleanArchitecture.Core.Application.Common.DTOs;

public struct TranslatorMessage(string text, object[] args)
{
    public string Text { get; set; } = text;
    public object?[] Args { get; set; } = args;
}