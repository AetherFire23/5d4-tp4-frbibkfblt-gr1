using System.Collections.Generic;

namespace SaveSystem
{
    [System.Serializable]
    public class AllObjectData<T> where T : ObjectData
    {
        public List<T> objectDatas;

        public void SetObjectDatas(List<T> objectDatas)
        {
            this.objectDatas = objectDatas;
        }
    }

    public class AllObjectData : AllObjectData<ObjectData>
    {
        
    }
}