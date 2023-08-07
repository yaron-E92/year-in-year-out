namespace YaronEfrat.Yiyo.Domain.Reflection.Models.Entities;

/// <summary>
/// A world event that has happened
/// during the passing <see cref="YearIn"/>
/// </summary>
public class WorldEvent : ReflectionEvent
{
    public IList<Source> Sources { get; set; }

    public override void Validate()
    {
        base.Validate();

        Sources ??= new List<Source>();

        if (Sources.Any(IsSourceUrlInvalid))
        {
            throw new EntityException("One or more of the source URLs are not valid web addresses", GetType());
        }
    }

    private static bool IsSourceUrlInvalid(Source source)
    {
        return !source.Url.IsAbsoluteUri ||
               !(source.Url.Scheme.Equals(Uri.UriSchemeHttp) || source.Url.Scheme.Equals(Uri.UriSchemeHttps));
    }
}
