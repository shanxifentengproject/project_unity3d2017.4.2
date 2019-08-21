using UnityEngine;

public class ViceBullet5 : MonoBehaviour
{

    [SerializeField] private float _moveSpeed;

    private float _damageValue;
    public void Initi(float value)
    {
        //_damageValue = value;
        InitDamageValue(value);
    }

    void InitDamageValue(float value)
    {
        if (IGamerProfile.Instance != null)
        {
            //max min maxLv minLv
            //k = (max - min) / (maxLv - minLV) = (cur - min) / (curLv - minLv)
            //cur = k * (curLv - minLv) + min;
            int characterIndex = (int)UiSceneSelectGameCharacter.CharacterId.ChildWeapon05;
            float max = IGamerProfile.gameCharacter.characterDataList[characterIndex].LevelBRange.m_h0;
            float min = IGamerProfile.gameCharacter.characterDataList[characterIndex].LevelBRange.m_h1;
            float curLv = IGamerProfile.Instance.playerdata.characterData[characterIndex].levelB;
            float maxLv = IGamerProfile.gameCharacter.characterDataList[characterIndex].maxlevelB;
            float minLv = 0f;
            float k = (max - min) / (maxLv - minLv);
            //curLv = maxLv; //test
            float val = k * (curLv - minLv) + min;
            _damageValue = Mathf.Clamp(val, min, max);
        }
        else
        {
            _damageValue = value;
        }
    }

    private void Update()
    {
        transform.position += _moveSpeed * transform.up * Time.deltaTime;
        BorderCheck();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Virus"))
        {
            var virus = collision.transform.GetComponent<BaseVirus>();
            if (!virus.IsDeath)
            {
                virus.Injured(_damageValue, false);
            }
            BulletPools.Instance.DeSpawn(gameObject);

            var obj = EffectPools.Instance.Spawn("HitEffect");
            obj.transform.position = transform.position;
            obj.transform.localScale = Vector3.one * 1.5f;
        }
    }


    private void BorderCheck()
    {
        bool b1 = transform.position.x > VirusTool.RightX + 2f;
        bool b2 = transform.position.x < VirusTool.LeftX - 2f;
        bool b3 = transform.position.y > VirusTool.TopY + 2f;
        bool b4 = transform.position.y < VirusTool.BottomY - 2f;
        if (b1 || b2 || b3 || b4)
        {
            BulletPools.Instance.DeSpawn(gameObject);
        }
    }



}