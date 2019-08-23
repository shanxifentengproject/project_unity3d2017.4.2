/// <summary>
/// 游戏里面购买金币控制组件
/// </summary>
class UiGameBuyCoins : GuiUiSceneBase
{
    public delegate void EventHandel(bool isSucceed);
    public void AddEvent(EventHandel ev)
    {
        CallBackEvent += ev;
    }

    public void RemoveEvent(EventHandel ev)
    {
        CallBackEvent -= ev;
    }

    event EventHandel CallBackEvent;
    void OnCallBackEvent(bool isSucceed)
    {
        if (CallBackEvent != null)
        {
            CallBackEvent(isSucceed);
        }
    }

    public void PlayerBuyCoins(int payMoney)
    {
        IGamerProfile.Instance.PayMoney(new IGamerProfile.PayMoneyData(IGamerProfile.PayMoneyItem.PayMoneyItem_LevelCharacter,
                                    payMoney, 0, PayMoneyCallback), this);
        UiSceneUICamera.Instance.transform.position = UnityEngine.Vector3.left * 30f;
    }

    private void PayMoneyCallback(IGamerProfile.PayMoneyData paydata, bool isSucceed)
    {
        OnCallBackEvent(isSucceed);
        Destroy(gameObject);
        UiSceneUICamera.Instance.transform.position = UnityEngine.Vector3.zero;
    }
}
