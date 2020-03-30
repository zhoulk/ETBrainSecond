
using UnityEngine;

namespace ETBrain
{
    public partial class GameEntry : MonoBehaviour
    {
        private void Start()
        {
            InitBuiltinComponents();
            InitCustomComponents();
            InitCustomDataNode();
        }
    }
}

