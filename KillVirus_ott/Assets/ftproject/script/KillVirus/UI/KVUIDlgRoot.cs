using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class KvUIDlgRoot : BasePanel
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        public enum ButtonState
        {
            Null = -1,
            Btn01 = 0,
            Btn02 = 1,
        }
        ButtonState _buttonState = ButtonState.Null;

        [System.Serializable]
        public class ButtonData
        {
            public Transform[] BtArray;
            public Transform SelectTr;
            internal void ChangeSelectButton(ButtonState type)
            {
                int index = (int)type;
                if (SelectTr != null)
                {
                    if (BtArray.Length > index && BtArray[index] != null)
                    {
                        SelectTr.SetParent(BtArray[index]);
                    }
                    SelectTr.localPosition = Vector3.zero;
                }
            }
        }
        public ButtonData _buttonData;
        float _timeLast = 0f;
        float _timeMax = 0.5f;

        internal void InitButtonData(ButtonState state)
        {
            _timeLast = Time.time;
            _buttonState = state;
            ChangeSelectButton();
        }

        void ChangeSelectButton()
        {
            _buttonData.ChangeSelectButton(_buttonState);
        }

        public override void Active()
        {
            gameObject.SetActive(true);
            _canvasGroup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutBounce);
            _canvasGroup.DOFade(1, 0.5f);
            InputKillVirus.Instance.AddEvent(ClickEnterBtEvent, InputKillVirus.EventState.Enter);
            InputKillVirus.Instance.AddEvent(ClickEscBtEvent, InputKillVirus.EventState.Esc);
            InputKillVirus.Instance.AddEvent(ClickLeftBtEvent, InputKillVirus.EventState.Left);
            InputKillVirus.Instance.AddEvent(ClickRightBtEvent, InputKillVirus.EventState.Right);
        }

        public override void UnActive()
        {
            gameObject.SetActive(false);
            _canvasGroup.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InOutBounce);
            _canvasGroup.DOFade(0, 0.5f);
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
            OnClickEnterBt(_buttonState);

            if (IGamerProfile.Instance != null)
            {
                SoundEffectPlayer.Play("buttonok.wav");
            }
        }

        public virtual void OnClickEnterBt(ButtonState state)
        {
        }

        private void ClickEscBtEvent(InputKillVirus.ButtonState val)
        {
            if (val == InputKillVirus.ButtonState.DOWN)
            {
                return;
            }

            if (IGamerProfile.Instance != null)
            {
                SoundEffectPlayer.Play("buttonok.wav");
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
                case ButtonState.Btn01:
                    {
                        _buttonState = ButtonState.Btn02;
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
                case ButtonState.Btn02:
                    {
                        _buttonState = ButtonState.Btn01;
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
