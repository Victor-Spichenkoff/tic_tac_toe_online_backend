using System.Text.Json;
using asp_rest_model.Models;

namespace asp_rest_model.Helpers;

public class FormatsHelpers
{
    public static InGameState? ParseInGameFromString(string inGameAsString)
    {
        var toError = "{\"nhe\":\"nhe1\"}";
        try
        {
            var inGameObject = JsonSerializer.Deserialize<InGameState>(toError);
            // var inGameObject = JsonSerializer.Deserialize<InGameState>(inGameAsString);
            return InGameManager.GiveInitialInGameState("1");
        }
        catch
        {
            return null;
        }
    }
}