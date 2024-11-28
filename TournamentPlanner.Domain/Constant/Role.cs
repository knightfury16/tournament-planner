namespace TournamentPlanner.Domain.Constant;

public static class Role
{
    public const string Admin = "Admin";
    public const string Moderator = "Moderator";
    public const string Player = "Player";
    public const string User = "User";

    public static List<string> GetAllRole() => new List<string> { Admin, Moderator, Player, User };
}
