using ICities;

namespace AutoEmptyEx
{
    class UtilGarbageBuildings : ThreadingExtensionBase
    {
        private static BuildingManager _buildingManager;
        private static FastList<ushort> garbageBuildings;

        // LandFill の Building番号リスト
        private static FastList<ushort> listLandFill = new FastList<ushort>();

        //シミュレーション完了後タイミングのイベント呼出し
        public override void OnAfterSimulationTick()
        {

            // シミュレーションTick 1024 回に 1 回だけ動作する
            if (threadingManager.simulationTick % 1024 == 0 && !threadingManager.simulationPaused)
            {


            }
            base.OnAfterSimulationTick();
        }

        private static void UpdateGarbageBuildingsInformations()
        {
            _buildingManager = ColossalFramework.Singleton<BuildingManager>.instance;
            var buffer = _buildingManager.m_buildings.m_buffer;

            // ゴミ関連施設のリスト
            garbageBuildings = _buildingManager.GetServiceBuildings(ItemClass.Service.Garbage);

            foreach (ushort serviceGarbage in garbageBuildings) {
                var _buildingAi = buffer[serviceGarbage].Info.m_buildingAI;

                // ゴミ集積場の場合のみ
                if (_buildingAi is LandfillSiteAI)
                {
                    listLandFill.Add(serviceGarbage);
                }

            }
        }
    }
}
