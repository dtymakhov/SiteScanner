namespace SiteScanner.ViewModels;

public class ResultPageViewModel
{
    public string Url { get; set; }
    public int ResponseTime { get; set; }
    public int MinResponseTime { get; set; }
    public int MaxResponseTime { get; set; }
}