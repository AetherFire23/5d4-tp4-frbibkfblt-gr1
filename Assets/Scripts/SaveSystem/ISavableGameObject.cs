using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

namespace SaveSystem
{
    public interface ISavableGameObject
    {
        public ObjectData GetObjectData();
        public void BuildFromObjectData(ObjectData objectData);
        GameObject GetGameObject();
    }
}