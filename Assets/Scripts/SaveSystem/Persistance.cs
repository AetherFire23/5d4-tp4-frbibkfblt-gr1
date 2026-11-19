namespace SaveSystem
{
    public class Persistance
    {
        
        public static Persistance Instance = new(
            new GameLoader(),
            new GameSaver()
            );

        private Persistance(GameLoader gameLoader, GameSaver gameSaver)
        {
            
        }

        public static void Register(ISavableGameObject go)
        {
            
        }
    }
}