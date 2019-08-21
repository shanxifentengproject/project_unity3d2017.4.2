using DG.Tweening;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Tool;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UISettlePanel : BasePanel
    {
        //[SerializeField] private List<Sprite> _coinSheets;
        [SerializeField] private GameObject _resultBg;
        //[SerializeField] private Image coinImage;
        //[SerializeField] private Text _coinText;
        [SerializeField] private Text _tipText;
        //[SerializeField] private Button getAwardBtn;

        [System.Serializable]
        public class CoinData
        {
            public Text CoinText;
            public Text TotalCoinText;

            public Text JiaChengText;
            public Text JiaChengCoinText;
        }
        public CoinData _coinData;
        public Transform m_MapGroupParent;

        private List<string> _tipStrList;

        //private int _index;
        //private int _count;
        //private bool _isActive;
        private void Awake()
        {
            _tipStrList = new List<string>();
            TextAsset str = Resources.Load<TextAsset>("Tip");
            var list = str.text.Split('/');
            for (int i = 0; i < list.Length; i++)
            {
                string tipstr = Regex.Replace(list[i], @"[\n\r]", "");
                _tipStrList.Add(tipstr);
            }
            _tipStrList.RemoveAt(list.Length - 1);

            //getAwardBtn.onClick.AddListener(() =>
            //{
            //    EventManager.TriggerEvent(new FirstByteClick5DownEvent());
            //});
        }

        void InitCoinData()
        {
            ShowCoinData();
        }

        void ShowCoinData()
        {
            int coin = VirusGameDataAdapter.GetCurLevelCoin();
            int jiaCheng = UnityEngine.Random.Range(10, 100);
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
        }

        //private void Update()
        //{
        //    if (_isActive)
        //    {
        //        _count++;
        //        if (_count > 4)
        //        {
        //            _count = 0;
        //            _index++;
        //            if (_index >= _coinSheets.Count)
        //                _index = 0;

        //            coinImage.sprite = _coinSheets[_index];
        //            coinImage.SetNativeSize();
        //        }
        //    }
        //}


        //public void SetCoinText(int coin)
        //{
            //_coinText.text = VirusTool.GetStrByIntger(coin);
        //}

        public override void Active()
        {
            _resultBg.transform.localScale = Vector3.zero;
            _resultBg.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce);
            gameObject.SetActive(true);
            //_index = 0;
            //_count = 0;
            //_isActive = true;
            //coinImage.sprite = _coinSheets[_index];
            int i = UnityEngine.Random.Range(0, _tipStrList.Count);
            _tipText.text = _tipStrList[i];
            //_coinText.text = VirusGameDataAdapter.GetCurLevelCoin().ToString();
            InitCoinData();
            if (VirusGameMrg.Instance.m_UIMrg.m_MapManage != null)
            {
                VirusGameMrg.Instance.m_UIMrg.m_MapManage.ChangeMapGroupParent(m_MapGroupParent);
            }
        }

        public override void UnActive()
        {
            //_isActive = false;
            _resultBg.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBounce).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }
    }
}
