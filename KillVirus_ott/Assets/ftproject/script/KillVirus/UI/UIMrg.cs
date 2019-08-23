using UnityEngine;

namespace UI
{
    public class UIMrg : MonoBehaviour
    {
        [SerializeField] private UIMainPanel mainPanel;
        [SerializeField] private UITittlePanel titlePanel;
        [SerializeField] private UISettlePanel settlePanel;
        [SerializeField] private UICoinPanel coinPanel;
        [SerializeField] private UIRestartPanel restartPanel;
        [SerializeField] private UIFailedPanel failedPanel;
        [SerializeField] private UIExitPanel exitPanel;
        //[SerializeField] private UIBuyCoinsPanel buyCoinsPanel;

        public UIMainPanel MainPanel { get { return mainPanel; } }

        public UITittlePanel TitlePanel { get { return titlePanel; } }

        public UISettlePanel SettlePanel { get { return settlePanel; } }

        public UICoinPanel CoinPanel { get { return coinPanel; } }

        public UIRestartPanel RestartPanel { get { return restartPanel; } }

        public UIFailedPanel FailedPanel { get { return failedPanel; } }

        public UIExitPanel ExitPanel { get { return exitPanel; } }

        //public UIBuyCoinsPanel BuyCoinsPanel { get { return buyCoinsPanel; } }

        internal KVGameMapManage m_MapManage;

        public void FadeIn()
        {
            titlePanel.Active();
        }

        public void FadeOut()
        {
            titlePanel.UnActive();
        }
    }
}
