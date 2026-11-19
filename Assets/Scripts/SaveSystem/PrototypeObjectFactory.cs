using UnityEngine;

namespace SaveSystem
{
    public abstract class AbstractPrototypeFactory: MonoBehaviour
    {
        private string protoId;
        void Awake()
        {
            Persistance.RegisterPrototypeFactory(this);
        }
        
        public abstract void Create(ObjectData objectData);

        public string GetProtoId()
        {
            return protoId;
        }
    }
}