// GENERATED AUTOMATICALLY FROM 'Assets/New Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @NewControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @NewControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""New Controls"",
    ""maps"": [
        {
            ""name"": ""activate_toggle"",
            ""id"": ""f2c5d088-96b8-45a3-af0d-4fb469e01ac5"",
            ""actions"": [
                {
                    ""name"": ""activate_toggle"",
                    ""type"": ""Button"",
                    ""id"": ""ed3e237a-3c2a-4267-b7ca-d107a9e17ac7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8b4832ef-9755-44f3-8eef-cd7d4e7136fc"",
                    ""path"": ""<Keyboard>/#(A)"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""ConsoleActivation"",
                    ""action"": ""activate_toggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""ConsoleActivation"",
            ""bindingGroup"": ""ConsoleActivation"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // activate_toggle
        m_activate_toggle = asset.FindActionMap("activate_toggle", throwIfNotFound: true);
        m_activate_toggle_activate_toggle = m_activate_toggle.FindAction("activate_toggle", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // activate_toggle
    private readonly InputActionMap m_activate_toggle;
    private IActivate_toggleActions m_Activate_toggleActionsCallbackInterface;
    private readonly InputAction m_activate_toggle_activate_toggle;
    public struct Activate_toggleActions
    {
        private @NewControls m_Wrapper;
        public Activate_toggleActions(@NewControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @activate_toggle => m_Wrapper.m_activate_toggle_activate_toggle;
        public InputActionMap Get() { return m_Wrapper.m_activate_toggle; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Activate_toggleActions set) { return set.Get(); }
        public void SetCallbacks(IActivate_toggleActions instance)
        {
            if (m_Wrapper.m_Activate_toggleActionsCallbackInterface != null)
            {
                @activate_toggle.started -= m_Wrapper.m_Activate_toggleActionsCallbackInterface.OnActivate_toggle;
                @activate_toggle.performed -= m_Wrapper.m_Activate_toggleActionsCallbackInterface.OnActivate_toggle;
                @activate_toggle.canceled -= m_Wrapper.m_Activate_toggleActionsCallbackInterface.OnActivate_toggle;
            }
            m_Wrapper.m_Activate_toggleActionsCallbackInterface = instance;
            if (instance != null)
            {
                @activate_toggle.started += instance.OnActivate_toggle;
                @activate_toggle.performed += instance.OnActivate_toggle;
                @activate_toggle.canceled += instance.OnActivate_toggle;
            }
        }
    }
    public Activate_toggleActions @activate_toggle => new Activate_toggleActions(this);
    private int m_ConsoleActivationSchemeIndex = -1;
    public InputControlScheme ConsoleActivationScheme
    {
        get
        {
            if (m_ConsoleActivationSchemeIndex == -1) m_ConsoleActivationSchemeIndex = asset.FindControlSchemeIndex("ConsoleActivation");
            return asset.controlSchemes[m_ConsoleActivationSchemeIndex];
        }
    }
    public interface IActivate_toggleActions
    {
        void OnActivate_toggle(InputAction.CallbackContext context);
    }
}
