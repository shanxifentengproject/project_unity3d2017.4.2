using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIRestartPanel : BasePanel
    {
        [SerializeField] private Text _needCoinText;
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

        void SetNeedCoin()
        {
            _needCoinText.text = m_NeedCoin.ToString();
        }
        
        public enum ButtonState
        {
            Null = -1,
            Enter = 0,
            Back = 1,
        }
        ButtonState _buttonState = ButtonState.Null;

        [System.Serializable]
        public class ButtonData
        {
            public Transform EnterBt;
            public Transform BackBt;
            public Transform SelectBt;
            internal void ChangeSelectButton(ButtonState type)
            {
                switch (type)
                {
                    case ButtonState.Enter:
                        {
                            SelectBt.SetParent(EnterBt);
                            break;
                        }
                    case ButtonState.Back:
                        {
                            SelectBt.SetParent(BackBt);
                            break;
                        }
                }
                SelectBt.localPosition = Vector3.zero;
            }
        }
        public ButtonData _buttonData;
        float _timeLast = 0f;
        float _timeMax = 0.5f;

        void InitButtonData()
        {
            _timeLast = Time.time;
            if (IGamerProfile.Instance != null)
            {
                _buttonState = (ButtonState)IGamerProfile.gameBaseDefine.platformChargeIntensityData.closeLevel_Revive_BtnIndex;
            }
            else
            {
                _buttonState = ButtonState.Enter;
            }
            ChangeSelectButton();
        }

        void ChangeSelectButton()
        {
            _buttonData.ChangeSelectButton(_buttonState);
        }

        public override void Active()
        {
            VirusGameMrg.Instance.VirusPlayer.Invincible = true;
            SetNeedCoin();
            gameObject.SetActive(true);
            _adBtn.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBounce);
            canvasGroup.DOFade(1, 0.5f);
            InitButtonData();
            AddEvent();
        }

        public override void UnActive()
        {
            HiddenPanel();
            _adBtn.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBounce);
            canvasGroup.DOFade(0, 0.5f);
            RemoveEvent();
        }

        bool _IsAddEvent = false;
        void AddEvent()
        {
            if (_IsAddEvent == true)
            {
                return;
            }
            _IsAddEvent = true;
            InputKillVirus.Instance.AddEvent(ClickEnterBtEvent, InputKillVirus.EventState.Enter);
            InputKillVirus.Instance.AddEvent(ClickEscBtEvent, InputKillVirus.EventState.Esc);
            InputKillVirus.Instance.AddEvent(ClickLeftBtEvent, InputKillVirus.EventState.Left);
            InputKillVirus.Instance.AddEvent(ClickRightBtEvent, InputKillVirus.EventState.Right);
        }

        void RemoveEvent()
        {
            if (_IsAddEvent == false)
            {
                return;
            }
            _IsAddEvent = false;
            InputKillVirus.Instance.RemoveEvent(ClickEnterBtEvent, InputKillVirus.EventState.Enter);
            InputKillVirus.Instance.RemoveEvent(ClickEscBtEvent, InputKillVirus.EventState.Esc);
            InputKillVirus.Instance.RemoveEvent(ClickLeftBtEvent, InputKillVirus.EventState.Left);
            InputKillVirus.Instance.RemoveEvent(ClickRightBtEvent, InputKillVirus.EventState.Right);
        }

        internal void SubCoin()
        {
            //扣除玩家金币
            VirusGameDataAdapter.MinusTotalCoin(m_NeedCoin);
        }

        private void ClickEnterBtEvent(InputKillVirus.ButtonState val)
        {
            if (val == InputKillVirus.ButtonState.DOWN)
            {
                return;
            }

            switch (_buttonState)
            {
                case ButtonState.Enter:
                    {
                        int playerCoin = VirusGameDataAdapter.GetTotalCoin();
                        if (playerCoin >= m_NeedCoin)
                        {
                            //确认复活
                            RevivePlayer();
                            SubCoin();
                        }
                        else
                        {
                            //需要充值
                            //VirusGameMrg.Instance.m_UIMrg.BuyCoinsPanel.Active();
                            if (IGamerProfile.Instance != null)
                            {
                                HiddenPanel();
                                RemoveEvent();
                                UiGameBuyCoins com = (UiGameBuyCoins)UiSceneUICamera.Instance.CreateAloneScene(UiSceneUICamera.UISceneId.Id_UIGameBuyCoins);
                                com.PlayerBuyCoins(m_NeedCoin);
                                com.AddEvent(CallBackEvent);
                                return;
                            }
                            else
                            {
                                RevivePlayer();
                            }
                        }
                        UnActive();
                        break;
                    }
                case ButtonState.Back:
                    {
                        ClosePanel();
                        break;
                    }
            }
        }

        private void CallBackEvent(bool isSucceed)
        {
            if (true == isSucceed)
            {
                //付费成功
                //复活玩家
                RevivePlayer();
                VirusGameDataAdapter.UpdateTotalCoin(IGamerProfile.Instance.playerdata.playerMoney);
                UnActive();
            }
            else
            {
                //付费失败
                ClosePanel();
            }
        }

        void HiddenPanel()
        {
            QyFun.SetActive(gameObject, false);
        }

        /// <summary>
        /// 复活玩家
        /// </summary>
        void RevivePlayer()
        {
            VirusGameMrg.Instance.VirusPlayer.Revive();
            VirusGameMrg.Instance.VirusPlayer.gameObject.SetActive(true);
            VirusGameMrg.Instance.VirusPlayer.transform.position = new Vector3(0f, -5f, 0f);
            VirusGameMrg.Instance.VirusPlayer.SetPlayerState(true, true);
            VirusGameMrg.Instance.m_fsm.ChangeState(VirusGameState.None);
        }

        private void ClickEscBtEvent(InputKillVirus.ButtonState val)
        {
            if (val == InputKillVirus.ButtonState.DOWN)
            {
                return;
            }
            ClosePanel();
        }

        void ClosePanel()
        {
            //关闭界面
            UnActive();
            VirusGameMrg.Instance.m_UIMrg.FailedPanel.Active();
            VirusGameMrg.Instance.m_fsm.ChangeState(VirusGameState.None);
        }

        private void ClickLeftBtEvent(InputKillVirus.ButtonState val)
        {
            if (val == InputKillVirus.ButtonState.DOWN)
            {
                return;
            }

            if (Time.time - _timeLast < _timeMax)
            {
                return;
            }

            switch (_buttonState)
            {
                case ButtonState.Enter:
                    {
                        _buttonState = ButtonState.Back;
                        ChangeSelectButton();
                        break;
                    }
            }

            if (IGamerProfile.Instance != null)
            {
                SoundEffectPlayer.Play("scroll.wav");
            }
        }

        private void ClickRightBtEvent(InputKillVirus.ButtonState val)
        {
            if (val == InputKillVirus.ButtonState.DOWN)
            {
                return;
            }

            if (Time.time - _timeLast < _timeMax)
            {
                return;
            }

            switch (_buttonState)
            {
                case ButtonState.Back:
                    {
                        _buttonState = ButtonState.Enter;
                        ChangeSelectButton();
                        break;
                    }
            }

            if (IGamerProfile.Instance != null)
            {
                SoundEffectPlayer.Play("scroll.wav");
            }
        }
    }
}
