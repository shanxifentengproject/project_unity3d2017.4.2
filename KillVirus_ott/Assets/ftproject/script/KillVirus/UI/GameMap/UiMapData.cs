using UnityEngine;
using UnityEngine.UI;

class UiMapData : MonoBehaviourIgnoreGui
{
    public GuiPlaneAnimationText levelNum;
    public Text levelText;
    public GameObject locker;
    public GameObject boss;

    public void SetActiveLocker(bool isActive)
    {
        QyFun.SetActive(locker, isActive);
    }

    public void SetActiveBoss(bool isActive)
    {
        QyFun.SetActive(boss, isActive);
    }
}
