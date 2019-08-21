using UnityEngine;
using ViceBullet;

namespace ViceWeapon
{
    public class VirusVice3Weapon : BaseVirusViceWeapon
    {

        [SerializeField] private GameObject _lefMiniWing;
        [SerializeField] private GameObject _rightMiniWing;
        [SerializeField] private ViceBullet3[] _viceBullet3;
        [SerializeField] private float _shootDuration;

        private float _totalTime;

        private int _num;
        private bool _isActive;

        public override void Initi()
        {
            _isActive = true;
            _lefMiniWing.SetActive(true);
            _rightMiniWing.SetActive(true);

            _totalTime = 0;
            for (int i = 0; i < _viceBullet3.Length; i++)
            {
                _viceBullet3[i].Initi(Random.Range(1600, 2000));
            }
            InitShootDuration();
        }
        
        void InitShootDuration()
        {
            if (IGamerProfile.Instance != null)
            {
                //max min maxLv minLv
                //k = (max - min) / (maxLv - minLV) = (cur - min) / (curLv - minLv)
                //cur = k * (curLv - minLv) + min;
                int characterIndex = (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon03;
                float max = IGamerProfile.gameCharacter.characterDataList[characterIndex].LevelBRange.m_h0;
                float min = IGamerProfile.gameCharacter.characterDataList[characterIndex].LevelBRange.m_h1;
                float curLv = IGamerProfile.Instance.playerdata.characterData[characterIndex].levelB;
                float maxLv = IGamerProfile.gameCharacter.characterDataList[characterIndex].maxlevelB;
                float minLv = 0f;
                float k = (max - min) / (maxLv - minLv);
                //curLv = maxLv; //test
                float val = k * (curLv - minLv) + min;
                _shootDuration = Mathf.Clamp(val, max, min);
            }
        }

        public override void OnUpdate()
        {
            if (!IsUpdate)
                return;
            _num++;
            if (_num > 5)
            {
                _num = 0;
                _isActive = !_isActive;
                _lefMiniWing.SetActive(_isActive);
                _rightMiniWing.SetActive(_isActive);
            }

            _totalTime += Time.deltaTime;
            if (_totalTime > _shootDuration)
            {
                _totalTime -= _shootDuration;
                for (int i = 0; i < _viceBullet3.Length; i++)
                {
                    _viceBullet3[i].Show();
                }
            }
        }


        public override void ReIniti()
        {

        }

    }
}
