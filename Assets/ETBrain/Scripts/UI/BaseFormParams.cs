using GameFramework;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public class BaseFormParams : IReference
    {
        public UIFormLogic UIForm;

        public virtual void Clear()
        {
            UIForm = null;
        }
    }
}

