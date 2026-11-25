namespace SaveSystem
{
    public interface IUniqueObject: ISavableGameObject
    {
        public void BuildFromObjectData(ObjectData objectData);
    }
}