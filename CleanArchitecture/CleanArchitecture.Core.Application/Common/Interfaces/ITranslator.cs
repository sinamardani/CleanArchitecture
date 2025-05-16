using CleanArchitecture.Core.Application.Common.DTOs;

namespace CleanArchitecture.Core.Application.Common.Interfaces;

public interface ITranslator
{
    string this[string name]
    {
        get;
    }
    string GetString (string name);
    string GetString(TranslatorMessage message);
}