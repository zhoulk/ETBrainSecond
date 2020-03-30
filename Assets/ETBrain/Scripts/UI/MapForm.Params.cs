
using GameFramework;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public partial class MapForm : UGuiForm
    {
        public sealed class MapFormParams : BaseFormParams
        {
            public Game.Stage Stage
            {
                get;
                private set;
            }

            public static MapFormParams Create(UIFormLogic uIForm, Game.Stage stage, ProcedureBase procedure = null)
            {
                MapFormParams param = ReferencePool.Acquire<MapFormParams>();
                param.UIForm = uIForm;
                param.Stage = stage;
                return param;
            }

            public override void Clear()
            {
                base.Clear();
                Stage = Game.Stage.Unknown;
            }
        }
    }
}