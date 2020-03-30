
using GameFramework.Network;
using Msg;
using UnityGameFramework.Runtime;

namespace ETBrain
{
    public class SCHeartBeatHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return 4;
            }
        }

        public override void Handle(object sender, Packet packet)
        {
            SCData packetImpl = (SCData)packet;
            //Log.Info("Receive packet '{0}'.", packetImpl.Id.ToString());

            DataResponse dataResponse = (DataResponse)packetImpl.GetExtensionObject();
            for (int i=0; i<dataResponse.Objs.Count; i++)
            {
                ObjData objData = dataResponse.Objs[i];
                for (int j = 0; j < GameData.g_listAircreaft.Count; j++)
                {
                    Aircraft aircraft = GameData.g_listAircreaft[j];
                    if (aircraft.Id == objData.SerialId)
                    {
                        aircraft.m_fixv3RenderPosition = new FixVector3(
                            Fix64.FromRaw(objData.X),
                            Fix64.FromRaw(objData.Y),
                            Fix64.FromRaw(objData.Z));
                    }
                }
            }
        }
    }
}