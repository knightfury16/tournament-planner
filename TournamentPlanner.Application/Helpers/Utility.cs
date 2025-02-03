namespace TournamentPlanner.Application.Helpers;

public static class Utility
{
    public static string ByePlayerName => "ByePlayer";
    public static string ByePlayerEmail => "byeplayer@gmail.com";
    public static readonly string Knockout = "Knockout";
    public static readonly string QuaterFinal = "Quater Final";
    public static readonly string SemiFinal = "Semi Final";
    public static readonly string PlayOff = "Playoff 3/4";
    public static readonly string Final = "Final";
    public const int PlayOffPlayerNumber = -1;
    public const int QuaterFinalPlayerNumber = 8;
    public const int SemiFinalPlayerNumber = 4;
    public const int FinalPlayerNumber = 2;

    public static string NavigationPrpertyCreator(params string [] propertyNames){
        string navigationString = string.Empty;
        for (int i = 0; i < propertyNames.Length; i++){
            navigationString += propertyNames[i];
            if(i != propertyNames.Length-1){
                navigationString += ".";
            }
        }
        return navigationString;
    }
    
}
