using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class GameState
{
    //TODO add tout ce qu<on veut preserver dans le gamestate
}

public static class SaveSystem
{
    static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");

    public static void SaveGame()
    {
        //TODO instancier ce qui track notre progres, objet transporter, position, etc. tous les flags qu<on veut preserver
        var state = new GameState{};
        File.WriteAllText(SavePath, JsonConvert.SerializeObject(state));
    }

    public static bool SaveExists()
    {
        return File.Exists(SavePath);
    }
    

    public static GameState LoadStateFromSave()
    {
        return JsonConvert.DeserializeObject<GameState>(File.ReadAllText(SavePath));
    }
}