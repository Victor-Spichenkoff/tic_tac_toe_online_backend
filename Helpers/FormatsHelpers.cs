using System.Text.Json;
using asp_rest_model.Models;

namespace asp_rest_model.Helpers;

public class FormatsHelpers
{
    public static ReceiveInfosObject? ParseReceiveFromString(string inGameAsString)
    {
        try
        {
            var receiveObject = JsonSerializer.Deserialize<ReceiveInfosObject>(inGameAsString);
            return receiveObject;
        }
        catch
        {
            return null;
        }
    }
}