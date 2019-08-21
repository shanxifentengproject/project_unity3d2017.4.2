using UnityEngine;

public class QyFun
{
    public static void SetActive(GameObject obj, bool isActive)
    {
        if (obj != null && obj.activeSelf != isActive)
        {
            obj.SetActive(isActive);
        }
    }
}