using System.Globalization;
using System.Resources;
using CleanArchitecture.Core.Application.Common.DTOs;
using CleanArchitecture.Core.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Resources.ProjectResources;

namespace CleanArchitecture.Infrastructure.Resources.Service;

public class TranslatorService : ITranslator
{
    private readonly ResourceManager _resourceMessage = new(typeof(ResourceMessage).FullName!, typeof(ResourceMessage).Assembly);
    private readonly ResourceManager _resourceGeneral = new(typeof(ResourceGeneral).FullName!, typeof(ResourceGeneral).Assembly);
    public string this[string name] => _resourceGeneral.GetString(name, CultureInfo.CurrentCulture) ?? name;

    string ITranslator.GetString(string name)
    {
        return _resourceMessage.GetString(name, CultureInfo.CurrentCulture) ?? name;
    }

    public string GetString(TranslatorMessage message)
    {
        var value = _resourceMessage.GetString(message.Text, CultureInfo.CurrentCulture) ?? message.Text.Replace("_", " ");
        return string.Format(value, message.Args);
    }
}