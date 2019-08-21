using UnityEngine;
using System.Collections;

class UiSceneCharacterLevelSale : GuiUiSceneBase
{
    public override int uiSceneId { get { return (int)UiSceneUICamera.UISceneId.Id_UICharacterLevelSale; } }
    public GuiPlaneAnimationText payMoney;
    protected override void OnInitializationUI()
    {
        GuiExtendDialog dlg = GetComponent<GuiExtendDialog>();
        if (dlg != null)
        {
            dlg.callbackFuntion += OnDialogReback;
            dlg.buttonSelectStatus = ( GuiExtendDialog.DialogFlag ) IGamerProfile.gameBaseDefine.platformChargeIntensityData.closeLevel_CharacterLevelSale_BtnIndex;
        }

        int currentSelectCharacterIndex = (int)m_UpgradeCharacterDt.id;
        int money = 0;
        switch (m_UpgradeCharacterDt.att)
        {
            case UiSceneSelectGameCharacter.CharacterAttribute.AttributeA:
                {
                    money = IGamerProfile.gameCharacter.characterDataList[currentSelectCharacterIndex].LevelAToMoney.GetValue(
                                                                IGamerProfile.Instance.playerdata.characterData[currentSelectCharacterIndex].levelA);
                    break;
                }
            case UiSceneSelectGameCharacter.CharacterAttribute.AttributeB:
                {
                    money = IGamerProfile.gameCharacter.characterDataList[currentSelectCharacterIndex].LevelBToMoney.GetValue(
                                                                IGamerProfile.Instance.playerdata.characterData[currentSelectCharacterIndex].levelB);
                    break;
                }
        }
        payMoney.Text = money.ToString();
    }

    UiSceneSelectGameMap.UpgradeCharacterData m_UpgradeCharacterDt;
    public void Init(UiSceneSelectGameMap.UpgradeCharacterData dt)
    {
        m_UpgradeCharacterDt = dt;
    }

    private void OnDialogReback(int dialogid, GuiExtendDialog.DialogFlag ret)
    {
        switch (ret)
        {
            case GuiExtendDialog.DialogFlag.Flag_Cancel:
                {
                    SoundEffectPlayer.Play("buttonok.wav");
                    UnityEngine.Object.DestroyObject(this.gameObject);
                }
                break;
            case GuiExtendDialog.DialogFlag.Flag_Ok:
                {
                    SoundEffectPlayer.Play("buttonok.wav");
                    int currentSelectCharacterIndex = (int)m_UpgradeCharacterDt.id;
                    if (m_UpgradeCharacterDt.att == UiSceneSelectGameCharacter.CharacterAttribute.AttributeA)
                    {
                        IGamerProfile.Instance.PayMoney(new IGamerProfile.PayMoneyData(IGamerProfile.PayMoneyItem.PayMoneyItem_LevelCharacter,
                                               IGamerProfile.gameCharacter.characterDataList[currentSelectCharacterIndex].LevelAToMoney.GetValue(
                                                        IGamerProfile.Instance.playerdata.characterData[currentSelectCharacterIndex].levelA),
                                               0,
                                               PayMoneyCallback), this);
                    }
                    else if (m_UpgradeCharacterDt.att == UiSceneSelectGameCharacter.CharacterAttribute.AttributeB)
                    {
                        IGamerProfile.Instance.PayMoney(new IGamerProfile.PayMoneyData(IGamerProfile.PayMoneyItem.PayMoneyItem_LevelCharacter,
                                               IGamerProfile.gameCharacter.characterDataList[currentSelectCharacterIndex].LevelBToMoney.GetValue(
                                                        IGamerProfile.Instance.playerdata.characterData[currentSelectCharacterIndex].levelB),
                                               0,
                                               PayMoneyCallback), this);
                    }
                }
                break;
        }

    }
    private void PayMoneyCallback(IGamerProfile.PayMoneyData paydata, bool isSucceed)
    {
        switch (paydata.item)
        {
            case IGamerProfile.PayMoneyItem.PayMoneyItem_LevelCharacter:
                {
                    if (!isSucceed)
                    {
                        UnityEngine.Object.DestroyObject(this.gameObject);
                        return;
                    }
                    //处理档案
                    //增加角色级别
                    int currentSelectCharacterIndex = (int)m_UpgradeCharacterDt.id;
                    if (m_UpgradeCharacterDt.att == UiSceneSelectGameCharacter.CharacterAttribute.AttributeA)
                    {
                        if (IGamerProfile.Instance.playerdata.characterData[currentSelectCharacterIndex].levelA < IGamerProfile.gameCharacter.characterDataList[currentSelectCharacterIndex].maxlevelA)
                        {
                            IGamerProfile.Instance.playerdata.characterData[currentSelectCharacterIndex].levelA += 1;
                        }
                        if (IGamerProfile.Instance.playerdata.characterData[currentSelectCharacterIndex].levelA > IGamerProfile.gameCharacter.characterDataList[currentSelectCharacterIndex].maxlevelA)
                        {
                            IGamerProfile.Instance.playerdata.characterData[currentSelectCharacterIndex].levelA = IGamerProfile.gameCharacter.characterDataList[currentSelectCharacterIndex].maxlevelA;
                        }
                    }
                    else if (m_UpgradeCharacterDt.att == UiSceneSelectGameCharacter.CharacterAttribute.AttributeB)
                    {
                        if (IGamerProfile.Instance.playerdata.characterData[currentSelectCharacterIndex].levelB < IGamerProfile.gameCharacter.characterDataList[currentSelectCharacterIndex].maxlevelB)
                        {
                            IGamerProfile.Instance.playerdata.characterData[currentSelectCharacterIndex].levelB += 1;
                        }
                        if (IGamerProfile.Instance.playerdata.characterData[currentSelectCharacterIndex].levelB > IGamerProfile.gameCharacter.characterDataList[currentSelectCharacterIndex].maxlevelB)
                        {
                            IGamerProfile.Instance.playerdata.characterData[currentSelectCharacterIndex].levelB = IGamerProfile.gameCharacter.characterDataList[currentSelectCharacterIndex].maxlevelB;
                        }
                    }
                    //存储档案
                    IGamerProfile.Instance.SaveGamerProfileToServer();
                    UnityEngine.Object.DestroyObject(this.gameObject);
                }
                break;

        }
    }
}
