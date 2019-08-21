using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPropItem : MonoBehaviour
    {
        [SerializeField]
        private List<Image> linesList;
        [SerializeField]
        private Image propImage;
        int m_LinesIndex = 0;
        public void Initi(VirusPropEnum propEnum)
        {
            m_LinesIndex = 0;
            for (int i = 0; i < linesList.Count; i++)
            {
                var line = linesList[i];
                line.fillAmount = 1;
            }
            propImage.sprite = VirusSpritesMrg.Instance.GetVirusPropSprite(propEnum);
        }

        public void Reiniti()
        {
            m_LinesIndex = 0;
            for (int i = 0; i < linesList.Count; i++)
            {
                var line = linesList[i];
                line.fillAmount = 1;
            }
        }

        void UpdateLines(float fillAmount)
        {
            if (m_LinesIndex < linesList.Count)
            {
                if (fillAmount <= (float)(linesList.Count - m_LinesIndex) / linesList.Count
                    && fillAmount > (float)(linesList.Count - m_LinesIndex - 1) / linesList.Count)
                {
                    var v1 = (fillAmount - (float)(linesList.Count - m_LinesIndex - 1) / linesList.Count) / (1.0f / linesList.Count);
                    if (v1 < 0.01f)
                    {
                        v1 = 0;
                    }
                    linesList[m_LinesIndex].fillAmount = v1;

                    if (v1 <= 0f)
                    {
                        m_LinesIndex++;
                    }
                }
            }
        }

        public void OnUpdate(float fillAmount)
        {
            UpdateLines(fillAmount);
        }
    }
}
