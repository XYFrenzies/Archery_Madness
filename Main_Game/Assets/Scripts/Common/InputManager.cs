using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class InputManager : Singleton<InputManager>
{
    public XRBindingProfile LeftController
    {
        get
        {
            if (!m_LeftController.IsSet)
            {
                List<InputDevice> devices = new List<InputDevice>();
                InputDevices.GetDevicesWithCharacteristics(
                    InputDeviceCharacteristics.HeldInHand | 
                    InputDeviceCharacteristics.TrackedDevice | 
                    InputDeviceCharacteristics.Controller | 
                    InputDeviceCharacteristics.Left,
                    devices);

                if ( devices.Count == 0 )
                {
                    return null;
                }

                m_LeftController.SetInputDevice(devices[0]);
            }
            return m_LeftController;
        }
    }
    public XRBindingProfile RightController
    {
        get
        {
            if (!m_RightController.IsSet)
            {
                List<InputDevice> devices = new List<InputDevice>();
                InputDevices.GetDevicesWithCharacteristics(
                    InputDeviceCharacteristics.HeldInHand | 
                    InputDeviceCharacteristics.TrackedDevice | 
                    InputDeviceCharacteristics.Controller | 
                    InputDeviceCharacteristics.Right,
                    devices);

                if ( devices.Count == 0 )
                {
                    return null;
                }

                m_RightController.SetInputDevice(devices[0]);
            }
            return m_RightController;
        }
    }

    private void Awake()
    {
        m_LeftController.SetupBindings();
        m_RightController.SetupBindings();
    }

    private void Update()
    {
        m_LeftController.Update();
        m_RightController.Update();
    }

    #pragma warning disable 0649
    [SerializeField] private XRBindingProfile m_LeftController;
    [SerializeField] private XRBindingProfile m_RightController;
    #pragma warning restore 0649
}

[Serializable]
public class XRBindingProfile
{
    public bool IsSet
    {
        get
        {
            return m_IsSet;
        }
    }
    public void SetupBindings()
    {
        m_BindingLookup = new Dictionary<uint, XRBinding>();
        foreach (XRBinding binding in m_Bindings)
        {
            m_BindingLookup.Add((uint)binding.Button | (uint)binding.PressType, binding);
        }
    }
    public void SetInputDevice(InputDevice a_InputDevice)
    {
        m_InputDevice = a_InputDevice;
        m_IsSet = true;
    }
    public void Update()
    {
        if (m_Bindings == null || m_Bindings.Length == 0)
        {
            return;
        }

        foreach (XRBinding binding in m_Bindings)
        {
            binding.Update(ref m_InputDevice);
        }
    }
    public bool TryGetBinding(XRButton a_Button, PressType a_PressType, out XRBinding o_Binding)
    {
        return m_BindingLookup.TryGetValue((uint)a_Button | (uint)a_PressType, out o_Binding);
    }

    private InputDevice m_InputDevice;
    #pragma warning disable 0649
    [SerializeField] private XRBinding[] m_Bindings;
    private Dictionary<uint, XRBinding> m_BindingLookup;
    private bool m_IsSet;
    #pragma warning restore 0649
}

[Serializable]
public class XRBinding
{
    public bool Active
    {
        get
        {
            return m_Active;
        }
    }

    public XRButton Button
    {
        get
        {
            return m_Button;
        }
    }

    public PressType PressType
    {
        get
        {
            return m_PressType;
        }
    }

    public void Update(ref InputDevice a_Device)
    {
        a_Device.TryGetFeatureValue(XRStatics.GetFeature(m_Button), out m_IsPressed);
        m_Active = false;

        switch (m_PressType)
        {
            case PressType.Continuous: m_Active = m_IsPressed; break;
            case PressType.Begin: m_Active = m_IsPressed && !m_WasPressed; break;
            case PressType.End: m_Active = !m_IsPressed && m_WasPressed; break;
        }

        if (m_Active) m_OnActive.Invoke();
        m_WasPressed = m_IsPressed;
    }

    #pragma warning disable 0649
    [SerializeField] private XRButton m_Button;
    [SerializeField] private PressType m_PressType;
    [SerializeField] private UnityEvent m_OnActive;
    #pragma warning restore 0649

    private bool m_IsPressed;
    private bool m_WasPressed;
    private bool m_Active;
}

public enum XRButton
{
    Trigger            = 1 << 0,
    Grip               = 1 << 1,
    Primary            = 1 << 2,
    PrimaryTouch       = 1 << 3,
    Secondary          = 1 << 4,
    SecondaryTouch     = 1 << 5,
    Primary2DAxisClick = 1 << 6,
    Primary2DAxisTouch = 1 << 7
}

public enum PressType
{
    Begin      = 1 << 8,
    End        = 1 << 9,
    Continuous = 1 << 10
}

public static class XRStatics
{
    public static InputFeatureUsage<bool> GetFeature(XRButton a_Button)
    {
        switch (a_Button)
        {
            case XRButton.Trigger: return CommonUsages.triggerButton;
            case XRButton.Grip: return CommonUsages.gripButton;
            case XRButton.Primary: return CommonUsages.primaryButton;
            case XRButton.PrimaryTouch: return CommonUsages.primaryTouch;
            case XRButton.Secondary: return CommonUsages.secondaryButton;
            case XRButton.SecondaryTouch: return CommonUsages.secondaryTouch;
            case XRButton.Primary2DAxisClick: return CommonUsages.primary2DAxisClick;
            case XRButton.Primary2DAxisTouch: return CommonUsages.primary2DAxisTouch;
            default: Debug.LogError("button " + a_Button + " not found"); return CommonUsages.triggerButton;
        }
    }
}
