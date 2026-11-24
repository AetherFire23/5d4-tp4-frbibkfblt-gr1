using System;
using JetBrains.Annotations;
using UnityEngine;

namespace SaveSystem
{
    /// <summary>
    /// This class represents the data to serialize in the json file and the data to be loaded.
    /// IF you need custom serializable data you can inherit this class, just make sure all the fields are serializable   
    /// </summary>
    [Serializable]
    public class ObjectData
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Scale;
        [CanBeNull] public string Id;


        public ObjectData(Vector3 position, Vector3 rotation, Vector3 scale, string objectId)
        {
            Position = position;
            Rotation = rotation;
            Scale = scale;
            Id = objectId;
        }

        public ObjectData() : this(Vector3.zero, Vector3.zero, Vector3.zero, null)
        {
            
        }
    }
}