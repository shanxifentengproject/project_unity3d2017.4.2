using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputKillVirus : MonoBehaviour
{
    static InputKillVirus _Instance;
    public static InputKillVirus Instance
    {
        get
        {
            if (_Instance == null)
            {
                GameObject obj = new GameObject("InputKillVirus");
                _Instance = obj.AddComponent<InputKillVirus>();
            }
            return _Instance;
        }
    }
    class KeyCodeTV
    {
        /// <summary>
        /// 遥控器确定键的键值.
        /// </summary>
        public static KeyCode PadEnter01 = (KeyCode)10;
        public static KeyCode PadEnter02 = (KeyCode)66;
    }
    public enum ButtonState
    {
        DOWN = 0,
        UP = 1,
    }
    public enum EventState
    {
        Null = -1,
        Enter = 0,
        Esc = 1,
        Up = 2,
        Down = 3,
        Left = 4,
        Right = 5,
    }
    /// <summary>
    /// 按键响应事件.
    /// </summary>
    public delegate void EventHandel(ButtonState val);
    public void AddEvent(EventHandel ev, EventState type)
    {
        switch (type)
        {
            case EventState.Enter:
                {
                    ClickEnterBtEvent += ev;
                    break;
                }
            case EventState.Esc:
                {
                    ClickEscBtEvent += ev;
                    break;
                }
            case EventState.Up:
                {
                    ClickUpBtEvent += ev;
                    break;
                }
            case EventState.Down:
                {
                    ClickDownBtEvent += ev;
                    break;
                }
            case EventState.Left:
                {
                    ClickLeftBtEvent += ev;
                    break;
                }
            case EventState.Right:
                {
                    ClickRightBtEvent += ev;
                    break;
                }
        }
    }

    public void RemoveEvent(EventHandel ev, EventState type)
    {
        switch (type)
        {
            case EventState.Enter:
                {
                    ClickEnterBtEvent -= ev;
                    break;
                }
            case EventState.Esc:
                {
                    ClickEscBtEvent -= ev;
                    break;
                }
            case EventState.Up:
                {
                    ClickUpBtEvent -= ev;
                    break;
                }
            case EventState.Down:
                {
                    ClickDownBtEvent -= ev;
                    break;
                }
            case EventState.Left:
                {
                    ClickLeftBtEvent -= ev;
                    break;
                }
            case EventState.Right:
                {
                    ClickRightBtEvent -= ev;
                    break;
                }
        }
    }

    event EventHandel ClickEnterBtEvent;
    void ClickEnterBt(ButtonState val)
    {
        if (ClickEnterBtEvent != null)
        {
            //Debug.Log("ClickEnterBt -> val == " + val);
            ClickEnterBtEvent(val);
        }
    }
    event EventHandel ClickEscBtEvent;
    void ClickEscBt(ButtonState val)
    {
        if (ClickEscBtEvent != null)
        {
            //Debug.Log("ClickEscBt -> val == " + val);
            ClickEscBtEvent(val);
        }
    }
    event EventHandel ClickUpBtEvent;
    public void ClickUpBt(ButtonState val)
    {
        if (ClickUpBtEvent != null)
        {
            //Debug.Log("ClickUpBt -> val == " + val);
            ClickUpBtEvent(val);
        }
    }
    event EventHandel ClickDownBtEvent;
    void ClickDownBt(ButtonState val)
    {
        if (ClickDownBtEvent != null)
        {
            //Debug.Log("ClickDownBt -> val == " + val);
            ClickDownBtEvent(val);
        }
    }
    event EventHandel ClickLeftBtEvent;
    void ClickLeftBt(ButtonState val)
    {
        if (ClickLeftBtEvent != null)
        {
            //Debug.Log("ClickLeftBt -> val == " + val);
            ClickLeftBtEvent(val);
        }
    }
    event EventHandel ClickRightBtEvent;
    void ClickRightBt(ButtonState val)
    {
        if (ClickRightBtEvent != null)
        {
            //Debug.Log("ClickRightBt -> val == " + val);
            ClickRightBtEvent(val);
        }
    }

    void Update()
    {
        if (IGamerProfile.Instance != null)
        {
            if (InputDevice.ButtonOkDown)
            {
                ClickEnterBt(ButtonState.DOWN);
            }
            else if (InputDevice.ButtonOkUp)
            {
                ClickEnterBt(ButtonState.UP);
            }

            if (InputDevice.ButtonBackDown)
            {
                ClickEscBt(ButtonState.DOWN);
            }
            else if (InputDevice.ButtonBackUp)
            {
                ClickEscBt(ButtonState.UP);
            }

            if (InputDevice.ButtonLeftDown)
            {
                ClickLeftBt(ButtonState.DOWN);
            }
            else if (InputDevice.ButtonLeftUp)
            {
                ClickLeftBt(ButtonState.UP);
            }

            if (InputDevice.ButtonRightDown)
            {
                ClickRightBt(ButtonState.DOWN);
            }
            else if (InputDevice.ButtonRightUp)
            {
                ClickRightBt(ButtonState.UP);
            }

            if (InputDevice.ButtonUpDown)
            {
                ClickUpBt(ButtonState.DOWN);
            }
            else if (InputDevice.ButtonUpUp)
            {
                ClickUpBt(ButtonState.UP);
            }

            if (InputDevice.ButtonDownDown)
            {
                ClickDownBt(ButtonState.DOWN);
            }
            else if (InputDevice.ButtonDownUp)
            {
                ClickDownBt(ButtonState.UP);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.KeypadEnter)
                || Input.GetKeyDown(KeyCode.Return)
                || Input.GetKeyDown(KeyCodeTV.PadEnter01)
                || Input.GetKeyDown(KeyCodeTV.PadEnter02)
                || Input.GetKeyDown(KeyCode.JoystickButton0))
            {
                ClickEnterBt(ButtonState.DOWN);
            }
            else if (Input.GetKeyUp(KeyCode.KeypadEnter)
                || Input.GetKeyUp(KeyCode.Return)
                || Input.GetKeyUp(KeyCodeTV.PadEnter01)
                || Input.GetKeyUp(KeyCodeTV.PadEnter02)
                || Input.GetKeyUp(KeyCode.JoystickButton0))
            {
                ClickEnterBt(ButtonState.UP);
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClickEscBt(ButtonState.DOWN);
            }
            else if (Input.GetKeyUp(KeyCode.Escape))
            {
                ClickEscBt(ButtonState.UP);
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ClickLeftBt(ButtonState.DOWN);
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                ClickLeftBt(ButtonState.UP);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ClickRightBt(ButtonState.DOWN);
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                ClickRightBt(ButtonState.UP);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ClickUpBt(ButtonState.DOWN);
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                ClickUpBt(ButtonState.UP);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ClickDownBt(ButtonState.DOWN);
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                ClickDownBt(ButtonState.UP);
            }
        }
    }
}
