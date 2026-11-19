using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveSystem
{
    public class UniqueObjectFactory: MonoBehaviour
    {
        public static void GenerateAll(List<IUniqueObject> objects, Dictionary<Type, ObjectData> objectDatas)
        {
            foreach (var go in objects)
            {
                ObjectData dataOfObject = objectDatas[go.GetType()];
                go.BuildFromObjectData(dataOfObject);
            }
        }
    }
}