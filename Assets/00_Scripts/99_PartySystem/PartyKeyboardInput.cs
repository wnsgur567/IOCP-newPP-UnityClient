// GENERATED AUTOMATICALLY FROM 'Assets/00_Scripts/99_PartySystem/PartyKeyboardInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PartyKeyboardInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PartyKeyboardInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PartyKeyboardInput"",
    ""maps"": [
        {
            ""name"": ""PartySystem"",
            ""id"": ""5ef8f944-0f98-4500-9041-65459b5fc878"",
            ""actions"": [
                {
                    ""name"": ""PartySystemOnOffAction"",
                    ""type"": ""Button"",
                    ""id"": ""b29ea6df-6402-4d42-a68a-24e5e27117da"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""129940db-d41b-49b3-97f1-3014065e004c"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PartySystemOnOffAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PartySystem
        m_PartySystem = asset.FindActionMap("PartySystem", throwIfNotFound: true);
        m_PartySystem_PartySystemOnOffAction = m_PartySystem.FindAction("PartySystemOnOffAction", throwIfNotFound: true);
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

    // PartySystem
    private readonly InputActionMap m_PartySystem;
    private IPartySystemActions m_PartySystemActionsCallbackInterface;
    private readonly InputAction m_PartySystem_PartySystemOnOffAction;
    public struct PartySystemActions
    {
        private @PartyKeyboardInput m_Wrapper;
        public PartySystemActions(@PartyKeyboardInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @PartySystemOnOffAction => m_Wrapper.m_PartySystem_PartySystemOnOffAction;
        public InputActionMap Get() { return m_Wrapper.m_PartySystem; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PartySystemActions set) { return set.Get(); }
        public void SetCallbacks(IPartySystemActions instance)
        {
            if (m_Wrapper.m_PartySystemActionsCallbackInterface != null)
            {
                @PartySystemOnOffAction.started -= m_Wrapper.m_PartySystemActionsCallbackInterface.OnPartySystemOnOffAction;
                @PartySystemOnOffAction.performed -= m_Wrapper.m_PartySystemActionsCallbackInterface.OnPartySystemOnOffAction;
                @PartySystemOnOffAction.canceled -= m_Wrapper.m_PartySystemActionsCallbackInterface.OnPartySystemOnOffAction;
            }
            m_Wrapper.m_PartySystemActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PartySystemOnOffAction.started += instance.OnPartySystemOnOffAction;
                @PartySystemOnOffAction.performed += instance.OnPartySystemOnOffAction;
                @PartySystemOnOffAction.canceled += instance.OnPartySystemOnOffAction;
            }
        }
    }
    public PartySystemActions @PartySystem => new PartySystemActions(this);
    public interface IPartySystemActions
    {
        void OnPartySystemOnOffAction(InputAction.CallbackContext context);
    }
}
