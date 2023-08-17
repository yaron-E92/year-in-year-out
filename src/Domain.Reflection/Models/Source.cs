namespace YaronEfrat.Yiyo.Domain.Reflection.Models;

public class Source : ValueObject
{
    public Uri Url { get; }

    public Source(Uri url)
    {
        Url = url;
    }

    public Source(string url)
    {
        Url = new Uri(url);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Url;
    }
}
