using System;
using ICities;

namespace AutoEmptyEx
{
    public class AdditionalFunc
    {
        // �ً}���ԃ��[�h
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
            // �ً}���[�hON�t���O�F���Ґ��������\���� 5% ������
            // �ً}���[�hOFF�t���O�F���Ґ��������\�����������

            _statisticsManager = ColossalFramework.Singleton<StatisticsManager>.instance;
            
            return true;
        }

        private static Boolean IsDeathEmergency()
        {
            var buffer = _buildingManager.m_buildings.m_buffer;

            _statisticsManager = ColossalFramework.Singleton<StatisticsManager>.instance;

            _buildingManager = ColossalFramework.Singleton<BuildingManager>.instance;

            // �S�~�֘A�{�݂̃��X�g
            garbageBuildings = _buildingManager.GetServiceBuildings(ItemClass.Service.Garbage);
            // ���N�֘A�{�݂̃��X�g
            healthBuildings = _buildingManager.GetServiceBuildings(ItemClass.Service.HealthCare);

            // ���ʃJ�E���g
            FastList<ushort> listLandFill = new FastList<ushort>();
            FastList<ushort> listCemetery = new FastList<ushort>();
            foreach (ushort serviceGarbage in garbageBuildings) {
                var _buildingAi = buffer[serviceGarbage].Info.m_buildingAI;

                // �S�~�W�Ϗ�̏ꍇ�̂�
                if (_buildingAi is LandfillSiteAI) {
                    listLandFill.Add(serviceGarbage);
                }
            }
            foreach (ushort deathCare in healthBuildings) {
                var _buildingAi = buffer[deathCare].Info.m_buildingAI;
                // ��n�̏ꍇ�̂�
                if (_buildingAi is CemeteryAI) {
                    listCemetery.Add(deathCare);
                }
            }
            
            // ���Ґ�
            int _deadAmount = _statisticsManager.Get(StatisticType.DeadAmount).GetTotalInt32();
            // �����X�y�[�X
            int _deadCapacity = _statisticsManager.Get(StatisticType.DeadCapacity).GetTotalInt32();
            // ���S��
            int _deathRate = _statisticsManager.Get(StatisticType.DeathRate).GetTotalInt32();
            // �S�~�ċp�\��
            int _incinerationCapacity = _statisticsManager.Get(StatisticType.IncinerationCapacity).GetTotalInt32();
            // �S�~�W�Ϗ���

            // ���T�C�N���Z���^�[��

            return false;
        }

        private static Boolean IsGarbageEmergency()
        {

            return false;
        }
	}
}