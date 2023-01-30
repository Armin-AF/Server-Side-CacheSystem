namespace GitHub.Server.Models;

public class GitHubUser
{
    public string Login { get; set; }
    public int Id { get; set; }
    public string AvatarUrl { get; set; }
    public string Url { get; set; }
    public string HtmlUrl { get; set; }
    
    public override bool Equals(object obj)
    {
        var other = obj as GitHubUser;
        return other != null && Id == other.Id && Login == other.Login;
    }
}