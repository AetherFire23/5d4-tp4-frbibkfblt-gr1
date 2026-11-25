using System;
using Knight.Pure;
using SaveSystem;
using UnityEngine;

namespace Knight
{
    [Serializable]
    public class KnightRegionSave : ObjectData
    {
    
        public KnightState KnightState { get; set; }

        
        public KnightRegionSave()
        {
        }

        public KnightRegionSave(Transform transform, string objectId, KnightState knightState) : base(transform.position, transform.rotation.eulerAngles, transform.localScale, objectId)
        {
            KnightState = knightState;
        }
    }
}