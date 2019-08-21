using UnityEngine;

class QyDebug : MonoBehaviourIgnoreGui
{
    static string m_DebugHeadInfo = "UnityQy: ";
    public static void Log(string info)
    {
        Debug.Log(m_DebugHeadInfo + info);
    }

    public static void LogWarning(string info)
    {
        Debug.LogWarning(m_DebugHeadInfo + info);
    }

    public static void LogError(string info)
    {
        Debug.LogError(m_DebugHeadInfo + info);
    }
}
