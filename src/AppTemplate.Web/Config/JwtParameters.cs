using System.Text;

namespace AppTemplate.Web.Config;

public class JwtParameters
{
    public const string SectionName = "Jwt";

    public string Issuer { get; set; } = String.Empty;
    public string Audience { get; set; } = String.Empty;
    public string Key { get; set; } = String.Empty;

    public SecurityKey IssuerSigningKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
}