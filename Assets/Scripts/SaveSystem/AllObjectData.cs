using System.Collections.Generic;

namespace SaveSystem
{
    [System.Serializable]
    public class AllObjectData
    {
        public List<ObjectData> objectDatas;

        public void SetObjectDatas(List<ObjectData> objectDatas)
        {
            this.objectDatas = objectDatas;
        }
    }
}