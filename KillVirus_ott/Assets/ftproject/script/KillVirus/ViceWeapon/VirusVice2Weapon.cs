using Assets.Tool.Pools;
using UnityEngine;
using ViceBullet;

namespace ViceWeapon
{
    public class VirusVice2Weapon : BaseVirusViceWeapon
    {

        [SerializeField] private Transform _leftShootPos;
        [SerializeField] private Transform _rightShootPos;
        [SerializeField] private float _shootDuration;

        private float _totalTime;
        private bool _isLeft;

        public override void Initi()
        {
            _totalTime = 0;
            _isLeft = true;
            InitShootDuration();
        }

        void InitShootDuration()
        {
            if (IGamerProfile.Instance != null)
            {
                //max min maxLv minLv
                //k = (max - min) / (maxLv - minLV) = (cur - min) / (curLv - minLv)
                //cur = k * (curLv - minLv) + min;
                int characterIndex = (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon02;
                float max = IGamerProfile.gameCharacter.characterDataList[characterIndex].LevelARange.m_h0;
                float min = IGamerProfile.gameCharacter.characterDataList[characterIndex].LevelARange.m_h1;
                float curLv = IGamerProfile.Instance.playerdata.characterData[characterIndex].levelA;
                float maxLv = IGamerProfile.gameCharacter.characterDataList[characterIndex].maxlevelA;
                float minLv = 0f;
                float k = (max - min) / (maxLv - minLv);
                //curLv = maxLv; //test
                _shootDuration = k * (curLv - minLv) + min;
                _shootDuration = Mathf.Clamp(_shootDuration, max, min);
            }
        }

        public override void OnUpdate()
        {
            if (!IsUpdate)
                return;
            _totalTime += Time.deltaTime;
            if (_totalTime > _shootDuration)
            {
                _totalTime -= _shootDuration;
                _isLeft = !_isLeft;
                SpawnBullet(_isLeft);
            }
        }

        public override void ReIniti()
        {

        }


        private void SpawnBullet(bool isLeft)
        {
            var obj = BulletPools.Instance.Spawn("ViceBullet_2");
            obj.transform.localPosition = isLeft ? _leftShootPos.position : _rightShootPos.position;

            var bullet = obj.GetComponent<ViceBullet2>();
            bullet.Emit(Quaternion.Euler(0, 0, isLeft ? 5 : -5) * Vector3.up);
            bullet.Initi(Random.Range(150, 300));
        }
    }
}
