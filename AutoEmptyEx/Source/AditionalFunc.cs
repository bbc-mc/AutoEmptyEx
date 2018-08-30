using System;
using ICities;

namespace AutoEmptyEx
{
    public class AdditionalFunc
    {
        // 緊急事態モード
        private static Boolean flgEmergency = false;

        //
        private static BuildingManager _buildingManager;

        //
        private static StatisticsManager _statisticsManager;

        //
        private static FastList<ushort> garbageBuildings, healthBuildings;

        public static Boolean IsEmergency() {
            //int deathPeopleNum;
            //int deathCareNum;
            // 緊急モードONフラグ：死者数が処理可能数を 5% 超えた
            // 緊急モードOFFフラグ：死者数が処理可能数を下回った

            _statisticsManager = ColossalFramework.Singleton<StatisticsManager>.instance;
            
            return true;
        }

        private static Boolean IsDeathEmergency()
        {
            var buffer = _buildingManager.m_buildings.m_buffer;

            _statisticsManager = ColossalFramework.Singleton<StatisticsManager>.instance;

            _buildingManager = ColossalFramework.Singleton<BuildingManager>.instance;

            // ゴミ関連施設のリスト
            garbageBuildings = _buildingManager.GetServiceBuildings(ItemClass.Service.Garbage);
            // 健康関連施設のリスト
            healthBuildings = _buildingManager.GetServiceBuildings(ItemClass.Service.HealthCare);

            // 数量カウント
            FastList<ushort> listLandFill = new FastList<ushort>();
            FastList<ushort> listCemetery = new FastList<ushort>();
            foreach (ushort serviceGarbage in garbageBuildings) {
                var _buildingAi = buffer[serviceGarbage].Info.m_buildingAI;

                // ゴミ集積場の場合のみ
                if (_buildingAi is LandfillSiteAI) {
                    listLandFill.Add(serviceGarbage);
                }
            }
            foreach (ushort deathCare in healthBuildings) {
                var _buildingAi = buffer[deathCare].Info.m_buildingAI;
                // 墓地の場合のみ
                if (_buildingAi is CemeteryAI) {
                    listCemetery.Add(deathCare);
                }
            }
            
            // 死者数
            int _deadAmount = _statisticsManager.Get(StatisticType.DeadAmount).GetTotalInt32();
            // 埋葬スペース
            int _deadCapacity = _statisticsManager.Get(StatisticType.DeadCapacity).GetTotalInt32();
            // 死亡率
            int _deathRate = _statisticsManager.Get(StatisticType.DeathRate).GetTotalInt32();
            // ゴミ焼却可能数
            int _incinerationCapacity = _statisticsManager.Get(StatisticType.IncinerationCapacity).GetTotalInt32();
            // ゴミ集積所数

            // リサイクルセンター数

            return false;
        }

        private static Boolean IsGarbageEmergency()
        {

            return false;
        }
	}
}