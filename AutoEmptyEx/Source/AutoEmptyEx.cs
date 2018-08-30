using ICities;
using ColossalFramework.Plugins;

namespace AutoEmptyEx
{
    public class Monitor : ThreadingExtensionBase
    {
        private BuildingManager _buildingManager;
        //private CemeteryAI cemeteryAI;
        FastList<ushort> garbageBuildings, healthBuildings;

        //シミュレーション完了後タイミングのイベント呼出し
        public override void OnAfterSimulationTick()
        {
            // シミュレーションTick 1024 回に 1 回だけ動作する
            if (threadingManager.simulationTick % 1024 == 0 && !threadingManager.simulationPaused)
            {
                _buildingManager = ColossalFramework.Singleton<BuildingManager>.instance;

                // ゴミ関連施設のリスト
                garbageBuildings = _buildingManager.GetServiceBuildings(ItemClass.Service.Garbage);
                // 健康関連施設のリスト
                healthBuildings = _buildingManager.GetServiceBuildings(ItemClass.Service.HealthCare);

                var buffer = _buildingManager.m_buildings.m_buffer;
                int amount = 0;

                foreach (ushort serviceGarbage in garbageBuildings)
                {
                    var _buildingAi = buffer[serviceGarbage].Info.m_buildingAI;

                    // ゴミ集積場の場合のみ、以下処理
                    if (_buildingAi is LandfillSiteAI)
                    {
                        amount = _buildingAi.GetGarbageAmount(serviceGarbage, ref buffer[serviceGarbage]);
                        // TODO 放出開始閾値 7600000 は妥当なのか？
                        // MAX 8000000
                        if (amount > 7600000)
                        {
                            // 放出開始
                            _buildingAi.SetEmptying(serviceGarbage, ref buffer[serviceGarbage], true);
                        }
                        // TODO 回収開始閾値 400000 は妥当なのか？
                        else if (amount < 400000)
                        {
                            // 回収開始
                            _buildingAi.SetEmptying(serviceGarbage, ref buffer[serviceGarbage], false);
                        }
                        // 上記条件に当たらない場合、何もしない
                    }
                }

                foreach (ushort deathCare in healthBuildings)
                {
                    var _buildingAi = buffer[deathCare].Info.m_buildingAI;

                    // 墓地の場合のみ、以下処理
                    if (_buildingAi is CemeteryAI)
                    {
                        //cemeteryAI = (CemeteryAI)_buildingAi;

                        // TODO: 数値で閾値設定ができないのか？
                        if (_buildingAi.IsFull(deathCare, ref buffer[deathCare]))
                        {
                            // 放出開始
                            _buildingAi.SetEmptying(deathCare, ref buffer[deathCare], true);
                        }
                        // CanBeRelocated(移動可能) で空かどうかを判定
                        // TODO: 数値で閾値設定できないのか？
                        else if (_buildingAi.CanBeRelocated(deathCare, ref buffer[deathCare]))
                        {
                            // 回収開始
                            _buildingAi.SetEmptying(deathCare, ref buffer[deathCare], false);
                        }
                        // 上記条件に当たらない場合、何もしない
                    }
                }
            }

            base.OnAfterSimulationTick();
        }
    }

    public class OutputLogger
    {
        const string prefix = "[Auto Empty Ex] ";

        public static void PrintMessage(string message)
        {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, prefix + message);
        }

        public static void PrintError(string message)
        {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Error, prefix + message);
        }

        public static void PrintWarning(string message)
        {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Warning, prefix + message);
        }
    }

}