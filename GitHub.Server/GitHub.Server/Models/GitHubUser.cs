namespace GitHub.Server.Models;

public class GitHubUser
{
    public string Login { get; set; }
    public int Id { get; set; }
    public string avatar_url { get; set; }
    public string Url { get; set; }
    public string html_url { get; set; }
    
    public int public_repos { get; set; }
    
    public string bio { get; set; }
    
    public string name { get; set; }
    
    public string location { get; set; }
    
    public DateTime created_at { get; set; }
    
    
    public bool IsFromCache { get; set; }

    public override bool Equals(object obj)
    {
        var other = obj as GitHubUser;
        return other != null && Id == other.Id && Login == other.Login;
    }
}