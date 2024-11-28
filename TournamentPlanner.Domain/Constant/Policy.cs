namespace TournamentPlanner.Domain.Constant;

public static class Policy
{
    public const string Read = "Read";
    public const string Create = "Create";
    public const string Edit = "Edit";
    public const string AddScore = "AddScore";
    public const string Delete = "Delete";
    public static List<string> GetAllPolicy() => new List<string> { Read, Create, Edit, AddScore, Delete };
}
