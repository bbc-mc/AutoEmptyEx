using ICities;
using ColossalFramework.Plugins;

namespace AutoEmptyEx
{
    public class Monitor : ThreadingExtensionBase
    {
        private BuildingManager _buildingManager;
        //private CemeteryAI cemeteryAI;
        FastList<ushort> garbageBuildings, healthBuildings;

        //�V�~�����[�V����������^�C�~���O�̃C�x���g�ďo��
        public override void OnAfterSimulationTick()
        {
            // �V�~�����[�V����Tick 1024 ��� 1 �񂾂����삷��
            if (threadingManager.simulationTick % 1024 == 0 && !threadingManager.simulationPaused)
            {
                _buildingManager = ColossalFramework.Singleton<BuildingManager>.instance;

                // �S�~�֘A�{�݂̃��X�g
                garbageBuildings = _buildingManager.GetServiceBuildings(ItemClass.Service.Garbage);
                // ���N�֘A�{�݂̃��X�g
                healthBuildings = _buildingManager.GetServiceBuildings(ItemClass.Service.HealthCare);

                var buffer = _buildingManager.m_buildings.m_buffer;
                int amount = 0;

                foreach (ushort serviceGarbage in garbageBuildings)
                {
                    var _buildingAi = buffer[serviceGarbage].Info.m_buildingAI;

                    // �S�~�W�Ϗ�̏ꍇ�̂݁A�ȉ�����
                    if (_buildingAi is LandfillSiteAI)
                    {
                        amount = _buildingAi.GetGarbageAmount(serviceGarbage, ref buffer[serviceGarbage]);
                        // TODO ���o�J�n臒l 7600000 �͑Ó��Ȃ̂��H
                        // MAX 8000000
                        if (amount > 7600000)
                        {
                            // ���o�J�n
                            _buildingAi.SetEmptying(serviceGarbage, ref buffer[serviceGarbage], true);
                        }
                        // TODO ����J�n臒l 400000 �͑Ó��Ȃ̂��H
                        else if (amount < 400000)
                        {
                            // ����J�n
                            _buildingAi.SetEmptying(serviceGarbage, ref buffer[serviceGarbage], false);
                        }
                        // ��L�����ɓ�����Ȃ��ꍇ�A�������Ȃ�
                    }
                }

                foreach (ushort deathCare in healthBuildings)
                {
                    var _buildingAi = buffer[deathCare].Info.m_buildingAI;

                    // ��n�̏ꍇ�̂݁A�ȉ�����
                    if (_buildingAi is CemeteryAI)
                    {
                        //cemeteryAI = (CemeteryAI)_buildingAi;

                        // TODO: ���l��臒l�ݒ肪�ł��Ȃ��̂��H
                        if (_buildingAi.IsFull(deathCare, ref buffer[deathCare]))
                        {
                            // ���o�J�n
                            _buildingAi.SetEmptying(deathCare, ref buffer[deathCare], true);
                        }
                        // CanBeRelocated(�ړ��\) �ŋ󂩂ǂ����𔻒�
                        // TODO: ���l��臒l�ݒ�ł��Ȃ��̂��H
                        else if (_buildingAi.CanBeRelocated(deathCare, ref buffer[deathCare]))
                        {
                            // ����J�n
                            _buildingAi.SetEmptying(deathCare, ref buffer[deathCare], false);
                        }
                        // ��L�����ɓ�����Ȃ��ꍇ�A�������Ȃ�
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