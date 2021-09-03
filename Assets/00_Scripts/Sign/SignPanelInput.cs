// GENERATED AUTOMATICALLY FROM 'Assets/00_Scripts/Sign/SignPanelInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @SignPanelInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @SignPanelInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""SignPanelInput"",
    ""maps"": [
        {
            ""name"": ""SignKeys"",
            ""id"": ""819e96a6-7bd1-4093-8f78-59388bf52679"",
            ""actions"": [
                {
                    ""name"": ""Tab"",
                    ""type"": ""Button"",
                    ""id"": ""eaf807d7-2ade-447d-8132-7e1e7f8f17ca"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d8685d8f-9e1c-4336-8c08-4747410f9876"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // SignKeys
        m_SignKeys = asset.FindActionMap("SignKeys", throwIfNotFound: true);
        m_SignKeys_Tab = m_SignKeys.FindAction("Tab", throwIfNotFound: true);
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

    // SignKeys
    private readonly InputActionMap m_SignKeys;
    private ISignKeysActions m_SignKeysActionsCallbackInterface;
    private readonly InputAction m_SignKeys_Tab;
    public struct SignKeysActions
    {
        private @SignPanelInput m_Wrapper;
        public SignKeysActions(@SignPanelInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Tab => m_Wrapper.m_SignKeys_Tab;
        public InputActionMap Get() { return m_Wrapper.m_SignKeys; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SignKeysActions set) { return set.Get(); }
        public void SetCallbacks(ISignKeysActions instance)
        {
            if (m_Wrapper.m_SignKeysActionsCallbackInterface != null)
            {
                @Tab.started -= m_Wrapper.m_SignKeysActionsCallbackInterface.OnTab;
                @Tab.performed -= m_Wrapper.m_SignKeysActionsCallbackInterface.OnTab;
                @Tab.canceled -= m_Wrapper.m_SignKeysActionsCallbackInterface.OnTab;
            }
            m_Wrapper.m_SignKeysActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Tab.started += instance.OnTab;
                @Tab.performed += instance.OnTab;
                @Tab.canceled += instance.OnTab;
            }
        }
    }
    public SignKeysActions @SignKeys => new SignKeysActions(this);
    public interface ISignKeysActions
    {
        void OnTab(InputAction.CallbackContext context);
    }
}
