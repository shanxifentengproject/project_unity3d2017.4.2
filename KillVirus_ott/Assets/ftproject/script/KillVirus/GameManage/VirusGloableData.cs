/// <summary>
/// 消灭病毒游戏全局数据管理
/// </summary>
public class VirusGloableData
{
    public class VirusData
    {
        /// <summary>
        /// 是否加载过rootpackage场景
        /// </summary>
        public bool IsLoadingRootScene = false;
    }
    public VirusData m_VirusData = new VirusData();

    static VirusGloableData _Instance;
    public static VirusGloableData Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new VirusGloableData();
            }
            return _Instance;
        }
    }
}
