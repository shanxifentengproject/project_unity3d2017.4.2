using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIRestartPanel : BasePanel
    {
        //[SerializeField] private Text _leftTimeText;
        [SerializeField] private GameObject _adBtn;
        [SerializeField] private CanvasGroup canvasGroup;

        public enum ButtonState
        {
            Null = -1,
            Restart = 0,
            Back = 1,
        }
        ButtonState _buttonState = ButtonState.Null;

        [System.Serializable]
        public class ButtonData
        {
            public Transform RestartBt;
            public Transform BackBt;
            public Transform SelectBt;
            internal void ChangeSelectButton(ButtonState type)
            {
                switch (type)
                {
                    case ButtonState.Restart:
                        {
                            SelectBt.SetParent(RestartBt);
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

        [System.Serializable]
        public class CoinData
        {
            public Text CoinText;
            public Text TotalCoinText;

            public Text JiaChengText;
            public Text JiaChengCoinText;
        }
        public CoinData _coinData;

        void InitCoinData()
        {
            ShowCoinData();
        }

        void ShowCoinData()
        {
            int coin = VirusGameDataAdapter.GetCurLevelCoin();
            int jiaCheng = Random.Range(10, 100);
            if (IGamerProfile.Instance != null)
            {
                UiSceneSelectGameCharacter.CharacterId id = UiSceneSelectGameCharacter.CharacterId.ShouYi;
                IGamerProfile.PlayerData.PlayerChacterData dt = IGamerProfile.Instance.playerdata.characterData[(int)id];
                GameCharacter.CharacterData characterDt = IGamerProfile.gameCharacter.characterDataList[(int)id];
                jiaCheng = characterDt.LevelBToVal.GetValue(dt.levelB); //关卡加成
            }

            int jiaChengCoin = Mathf.CeilToInt(jiaCheng * coin / 100f);
            int totalCoin = coin + jiaChengCoin;
            _coinData.CoinText.text = VirusTool.GetStrByIntger(coin);
            _coinData.TotalCoinText.text = VirusTool.GetStrByIntger(totalCoin);
            _coinData.JiaChengText.text = jiaCheng.ToString() + "%:";
            _coinData.JiaChengCoinText.text = VirusTool.GetStrByIntger(jiaChengCoin);
            VirusGameDataAdapter.AddTotalCoin(totalCoin);
        }

        void InitButtonData()
        {
            _timeLast = Time.time;
            _buttonState = ButtonState.Restart;
            ChangeSelectButton();
        }

        void ChangeSelectButton()
        {
            _buttonData.ChangeSelectButton(_buttonState);
        }

        //public void SetLeftTime(int value)
        //{
        //    _leftTimeText.text = value.ToString();
        //}


        public override void Active()
        {
            gameObject.SetActive(true);
            _adBtn.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBounce);
            canvasGroup.DOFade(1, 0.5f);
            InitCoinData();
            InitButtonData();
            InputKillVirus.Instance.AddEvent(ClickEnterBtEvent, InputKillVirus.EventState.Enter);
            InputKillVirus.Instance.AddEvent(ClickEscBtEvent, InputKillVirus.EventState.Esc);
            InputKillVirus.Instance.AddEvent(ClickLeftBtEvent, InputKillVirus.EventState.Left);
            InputKillVirus.Instance.AddEvent(ClickRightBtEvent, InputKillVirus.EventState.Right);
        }
        
        public override void UnActive()
        {
            gameObject.SetActive(false);
            _adBtn.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBounce);
            canvasGroup.DOFade(0, 0.5f);
            InputKillVirus.Instance.RemoveEvent(ClickEnterBtEvent, InputKillVirus.EventState.Enter);
            InputKillVirus.Instance.RemoveEvent(ClickEscBtEvent, InputKillVirus.EventState.Esc);
            InputKillVirus.Instance.RemoveEvent(ClickLeftBtEvent, InputKillVirus.EventState.Left);
            InputKillVirus.Instance.RemoveEvent(ClickRightBtEvent, InputKillVirus.EventState.Right);
        }

        private void ClickEnterBtEvent(InputKillVirus.ButtonState val)
        {
            if (val == InputKillVirus.ButtonState.DOWN)
            {
                return;
            }

            switch (_buttonState)
            {
                case ButtonState.Restart:
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene(VirusGameData.GameScene.game1.ToString());
                        break;
                    }
                case ButtonState.Back:
                    {
                        if (IGamerProfile.Instance == null)
                        {
                            UnityEngine.SceneManagement.SceneManager.LoadScene(VirusGameData.GameScene.game1.ToString());
                        }
                        else
                        {
                            //退回游戏主界面
                            UnityEngine.SceneManagement.SceneManager.LoadScene(VirusGameData.GameScene.UI.ToString());
                        }
                        break;
                    }
            }
        }

        private void ClickEscBtEvent(InputKillVirus.ButtonState val)
        {
            if (val == InputKillVirus.ButtonState.DOWN)
            {
                return;
            }

            if (IGamerProfile.Instance == null)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(VirusGameData.GameScene.game1.ToString());
            }
            else
            {
                //退回游戏主界面
                UnityEngine.SceneManagement.SceneManager.LoadScene(VirusGameData.GameScene.UI.ToString());
            }
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
                case ButtonState.Restart:
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
                        _buttonState = ButtonState.Restart;
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
