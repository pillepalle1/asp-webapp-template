namespace AppTemplate.Web.Config;

public class AppSettings
{
    public const string SectionName = "AppSettings";
    
    public bool RunApi { init; get; }
    public bool RunApp { init; get; }
}