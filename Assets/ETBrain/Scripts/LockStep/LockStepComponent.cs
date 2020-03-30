
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Network")]
    public class LockStepComponent : GameFrameworkComponent
    {
        BattleLogic battleLogic = new BattleLogic();

        public void Init()
        {
            battleLogic.Init();
        }

        void Update()
        {
            battleLogic.updateLogic();
        }
    }
}
    
