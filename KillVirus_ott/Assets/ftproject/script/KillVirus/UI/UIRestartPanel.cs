using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIRestartPanel : BasePanel
    {
        [SerializeField] private Text _needCoinText;
        [SerializeField] private Text _leftTimeText;
        [SerializeField] private GameObject _adBtn;
        [SerializeField] private CanvasGroup canvasGroup;
        int m_NeedCoin
        {
            get
            {
                int needCoin = 200;
                if (IGamerProfile.Instance != null)
                {
                    //这里获取复活需要的金币数
                }
                return needCoin;
            }
        }

        public void SetLeftTime(int value)
        {
            _leftTimeText.text = value.ToString();
        }

        void SetNeedCoin()
        {
            _needCoinText.text = m_NeedCoin.ToString();
        }
        
        public override void Active()
        {
            VirusGameMrg.Instance.VirusPlayer.Invincible = true;
            SetNeedCoin();
            gameObject.SetActive(true);
            _adBtn.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBounce);
            canvasGroup.DOFade(1, 0.5f);
        }
        
        public override void UnActive()
        {
            gameObject.SetActive(false);
            _adBtn.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBounce);
            canvasGroup.DOFade(0, 0.5f);
        }

        internal void SubCoin()
        {
            //扣除玩家金币
            VirusGameDataAdapter.MinusTotalCoin(m_NeedCoin);
        }
    }
}
