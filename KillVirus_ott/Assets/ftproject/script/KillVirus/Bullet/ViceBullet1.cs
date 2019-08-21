using DG.Tweening;
using UnityEngine;

namespace ViceBullet
{
    public class ViceBullet1 : MonoBehaviour
    {

        [SerializeField] private float _moveSpeed;

        private bool _isEmit;
        private bool _isReady;
        private bool _isReverse;
        private float _damageValue;
        private int _num;

        private Vector3 _origin;
        private Vector3 _end;
        private float _ammoScale = 0.55f;


        public void Initi(float damageValue)
        {
            _damageValue = damageValue;
            InitAmmoScale();
            InitAmmoSpeed();
        }

        void InitAmmoScale()
        {
            if (IGamerProfile.Instance != null)
            {
                //max min maxLv minLv
                //k = (max - min) / (maxLv - minLV) = (cur - min) / (curLv - minLv)
                //cur = k * (curLv - minLv) + min;
                int characterIndex = (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon01;
                float max = IGamerProfile.gameCharacter.characterDataList[characterIndex].LevelARange.m_h0;
                float min = IGamerProfile.gameCharacter.characterDataList[characterIndex].LevelARange.m_h1;
                float curLv = IGamerProfile.Instance.playerdata.characterData[characterIndex].levelA;
                float maxLv = IGamerProfile.gameCharacter.characterDataList[characterIndex].maxlevelA;
                float minLv = 0f;
                float k = (max - min) / (maxLv - minLv);
                //curLv = maxLv; //test
                float val = k * (curLv - minLv) + min;
                _ammoScale = Mathf.Clamp(val, min, max);
            }
        }

        void InitAmmoSpeed()
        {
            if (IGamerProfile.Instance != null)
            {
                //max min maxLv minLv
                //k = (max - min) / (maxLv - minLV) = (cur - min) / (curLv - minLv)
                //cur = k * (curLv - minLv) + min;
                int characterIndex = (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon01;
                float max = IGamerProfile.gameCharacter.characterDataList[characterIndex].LevelBRange.m_h0;
                float min = IGamerProfile.gameCharacter.characterDataList[characterIndex].LevelBRange.m_h1;
                float curLv = IGamerProfile.Instance.playerdata.characterData[characterIndex].levelB;
                float maxLv = IGamerProfile.gameCharacter.characterDataList[characterIndex].maxlevelB;
                float minLv = 0f;
                float k = (max - min) / (maxLv - minLv);
                //curLv = maxLv; //test
                float val = k * (curLv - minLv) + min;
                _moveSpeed = Mathf.Clamp(val, min, max);
            }
        }

        public void Initi()
        {
            _isEmit = false;
            transform.localScale = new Vector3(0.05f, 0.05f, 1f);
            transform.DOScale(new Vector3(0.25f, 0.25f, 1f), 2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                _isReady = true;
            });
            _isReverse = false;
            _origin = new Vector3(0.25f, 0.25f, 1f);
            _end = new Vector3(0.28f, 0.28f, 1);
        }

        public void Emit()
        {
            _isEmit = true;
            _isReady = false;
            transform.parent = null;
        }


        private void Update()
        {
            if (_isReady)
            {
                _num++;
                if (_num >= 5)
                {
                    _num = 0;
                    _isReverse = !_isReverse;
                    if (_isReverse)
                    {
                        _origin = new Vector3(0.28f, 0.28f, 1);
                        _end = new Vector3(0.25f, 0.25f, 1f);
                    }
                    else
                    {
                        _origin = new Vector3(0.25f, 0.25f, 1f);
                        _end = new Vector3(0.28f, 0.28f, 1);
                    }
                }
                transform.localScale = Vector3.LerpUnclamped(_origin, _end, _num * 1.0f / 5f);
            }
            if (_isEmit)
            {
                float delta = Time.deltaTime * 1;
                if (transform.localScale.x + delta > _ammoScale)
                {
                    //生物炮最终大小
                    transform.localScale = new Vector3(_ammoScale, _ammoScale, 1);
                }
                else
                    transform.localScale += new Vector3(delta, delta, 1);

                //生物炮运动控制
                transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
                if (transform.position.y > 12f)
                {
                    BulletPools.Instance.DeSpawn(gameObject);
                }
            }

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Virus") && _isEmit)
            {
                var basevirus = collision.GetComponent<BaseVirus>();
                if (basevirus.VirusHealth.Value >= _damageValue)
                {
                    if (!basevirus.IsDeath)
                    {
                        basevirus.Injured(_damageValue, false);
                    }
                    BulletPools.Instance.DeSpawn(gameObject);
                }
                else
                {
                    _damageValue -= basevirus.VirusHealth.Value;
                    if (!basevirus.IsDeath)
                    {
                        basevirus.Injured(basevirus.VirusHealth.Value, false);
                    }
                }
                VirusSoundMrg.Instance.PlaySound(VirusSoundType.ViceBullet1Explosion);
            }
        }

       

    }
}
