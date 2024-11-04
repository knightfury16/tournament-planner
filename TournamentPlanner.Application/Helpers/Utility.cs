namespace TournamentPlanner.Application.Helpers;

public static class Utility
{
    public static string ByePlayerName => "ByePlayer";
    public static string ByePlayerEmail => "byeplayer@gmail.com";

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
