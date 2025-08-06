namespace LogCap.Model;
public record WebInfo(
    long ID,
    string SiteDesc,
    string IpAddress,
    string? RefUrl = null,
    string? Info = null,
    DateTime Created = default
)
{
    public WebInfo(string siteDesc, string ipAddress, string? refUrl = null, string? info = null)
        : this(0, siteDesc, ipAddress, refUrl, info, DateTime.Now)
    {
    }
}
