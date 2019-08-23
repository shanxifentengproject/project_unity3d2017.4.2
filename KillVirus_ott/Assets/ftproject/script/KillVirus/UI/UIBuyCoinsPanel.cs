using System;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// 购买金币界面组件(弃用)
    /// </summary>
    public class UIBuyCoinsPanel : KvUIDlgRoot
    {
        [System.Serializable]
        public class PanelData
        {
            public Text moneyText;
            public Text coinText;
            public void Init(int money, int coin)
            {
                if (moneyText != null)
                {
                    moneyText.text = money.ToString();
                }
                if (coinText != null)
                {
                    coinText.text = coin.ToString();
                }
            }
        }
        public PanelData m_PanelData;

        void Init()
        {
            int money = 20;
            int coin = 20000;
            m_PanelData.Init(money, coin);
        }

        public override void Active()
        {
            ButtonState btState = ButtonState.Btn01;
            InitButtonData(btState);
            base.Active();
            Init();
        }

        public override void UnActive()
        {
            base.UnActive();
        }

        public override void OnClickEnterBt(ButtonState state)
        {
            switch (state)
            {
                case ButtonState.Btn01:
                    {
                        //确定购买金币
                        break;
                    }
                case ButtonState.Btn02:
                    {
                        //取消
                        UnActive();
                        //展示失败界面
                        VirusGameMrg.Instance.m_UIMrg.FailedPanel.Active();
                        VirusGameMrg.Instance.m_fsm.ChangeState(VirusGameState.None);
                        break;
                    }
            }
        }
    }
}
