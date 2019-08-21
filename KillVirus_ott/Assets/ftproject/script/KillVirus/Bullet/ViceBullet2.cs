using UnityEngine;

namespace ViceBullet
{
    public class ViceBullet2 : MonoBehaviour
    {

        [SerializeField] private float _damageRadius;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _attackLength;


        private int _damageValue;
        private float _curMoveLength;
        private Vector3 _direction;
        public void Initi(int damageValue)
        {
            _damageValue = damageValue;
            InitAmmoSpeed();
            InitAttackLength();
            InitDamageRadius();
        }

        void InitAmmoSpeed()
        {
            if (IGamerProfile.Instance != null)
            {
                //max min maxLv minLv
                //k = (max - min) / (maxLv - minLV) = (cur - min) / (curLv - minLv)
                //cur = k * (curLv - minLv) + min;
                int characterIndex = (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon02;
                float max = IGamerProfile.gameCharacter.characterDataList[characterIndex].LevelCRange.m_h0;
                float min = IGamerProfile.gameCharacter.characterDataList[characterIndex].LevelCRange.m_h1;
                float curLv = IGamerProfile.Instance.playerdata.characterData[characterIndex].levelA;
                float maxLv = IGamerProfile.gameCharacter.characterDataList[characterIndex].maxlevelA;
                float minLv = 0f;
                float k = (max - min) / (maxLv - minLv);
                //curLv = maxLv; //test
                float val = k * (curLv - minLv) + min;
                _moveSpeed = Mathf.Clamp(val, min, max);
            }
        }

        void InitAttackLength()
        {
            if (IGamerProfile.Instance != null)
            {
                //max min maxLv minLv
                //k = (max - min) / (maxLv - minLV) = (cur - min) / (curLv - minLv)
                //cur = k * (curLv - minLv) + min;
                int characterIndex = (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon02;
                float max = IGamerProfile.gameCharacter.characterDataList[characterIndex].LevelBRange.m_h0;
                float min = IGamerProfile.gameCharacter.characterDataList[characterIndex].LevelBRange.m_h1;
                float curLv = IGamerProfile.Instance.playerdata.characterData[characterIndex].levelB;
                float maxLv = IGamerProfile.gameCharacter.characterDataList[characterIndex].maxlevelB;
                float minLv = 0f;
                float k = (max - min) / (maxLv - minLv);
                //curLv = maxLv; //test
                float val = k * (curLv - minLv) + min;
                _attackLength = Mathf.Clamp(val, min, max);
            }
        }
        
        void InitDamageRadius()
        {
            if (IGamerProfile.Instance != null)
            {
                //max min maxLv minLv
                //k = (max - min) / (maxLv - minLV) = (cur - min) / (curLv - minLv)
                //cur = k * (curLv - minLv) + min;
                int characterIndex = (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon02;
                float max = IGamerProfile.gameCharacter.characterDataList[characterIndex].LevelDRange.m_h0;
                float min = IGamerProfile.gameCharacter.characterDataList[characterIndex].LevelDRange.m_h1;
                float curLv = IGamerProfile.Instance.playerdata.characterData[characterIndex].levelB;
                float maxLv = IGamerProfile.gameCharacter.characterDataList[characterIndex].maxlevelB;
                float minLv = 0f;
                float k = (max - min) / (maxLv - minLv);
                //curLv = maxLv; //test
                float val = k * (curLv - minLv) + min;
                _damageRadius = Mathf.Clamp(val, min, max);
            }
        }

        public void Emit(Vector3 direction)
        {
            _direction = direction;
            _curMoveLength = 0;

            transform.localScale = new Vector3(0.5f, 0.5f, 1);
        }


        private void Update()
        {
            float scaleDelta = Time.deltaTime*1;
            if (transform.localScale.x + scaleDelta > 1)
                transform.localScale = Vector3.one;
            else
                transform.localScale += new Vector3(scaleDelta, scaleDelta, 1);

            float delta = _moveSpeed * Time.deltaTime;
            if (_curMoveLength + delta >= _attackLength)
            {
                CalculateDamage();
                BulletPools.Instance.DeSpawn(gameObject);
                SpawnExplosion(transform.position);
            }
            else
            {
                transform.position += _direction * delta;
                _curMoveLength += delta;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Virus"))
            {
                VirusSoundMrg.Instance.PlaySound(VirusSoundType.ViceBullet2Explosion);
                SpawnExplosion(transform.position);
                CalculateDamage();
                BulletPools.Instance.DeSpawn(gameObject);
            }
        }



        private void SpawnExplosion(Vector3 pos)
        {
            var explosion = EffectPools.Instance.Spawn("ViceBullet2Explosion");
            explosion.transform.position = pos;
        }


        private void CalculateDamage()
        {
            var lay = 1 << LayerMask.NameToLayer("Virus");
            var coliders = Physics2D.OverlapCircleAll(transform.position, _damageRadius, lay);
            foreach (var t in coliders)
            {
                if (t != null)
                {
                    var virus = t.GetComponent<BaseVirus>();
                    if (!virus.IsDeath)
                    {
                        virus.Injured(_damageValue, false);
                    }
                }
            }
        }



    }
}
