using System.Numerics;

namespace SaveSystem
{
    /// <summary>
    /// This class represents the data to serialize in the json file and the data to be loaded.
    /// IF you need custom serializable data you can inherit this class, just make sure all the fields are serializable   
    /// </summary>
    public class ObjectData
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
        public string Id;


        public ObjectData(Vector3 position, Quaternion rotation, Vector3 scale, string objectId)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
            Id = objectId;
        }
    }
}