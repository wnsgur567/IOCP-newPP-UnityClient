// GENERATED AUTOMATICALLY FROM 'Assets/PlayerInput.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerInput : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""MoveActions"",
            ""id"": ""1f789fa4-fd38-46d0-84be-70c153480e3a"",
            ""actions"": [
                {
                    ""name"": ""New action"",
                    ""type"": ""Button"",
                    ""id"": ""8edba45f-d61d-4d00-a47c-9e17e7d89ea3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""df506cf9-2946-4424-b760-bb36a76a1244"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0eee115d-cd33-4bd7-acb4-96a4d43f45f3"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a3cbeb49-ec72-449a-97d4-1ea1ed1aa2d5"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""5def82c4-0dc4-43c3-8182-b6ef7c359048"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cfc45a8f-2357-4c51-8dcb-21302a5d3432"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""New action"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // MoveActions
        m_MoveActions = asset.FindActionMap("MoveActions", throwIfNotFound: true);
        m_MoveActions_Newaction = m_MoveActions.FindAction("New action", throwIfNotFound: true);
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

    // MoveActions
    private readonly InputActionMap m_MoveActions;
    private IMoveActionsActions m_MoveActionsActionsCallbackInterface;
    private readonly InputAction m_MoveActions_Newaction;
    public struct MoveActionsActions
    {
        private @PlayerInput m_Wrapper;
        public MoveActionsActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Newaction => m_Wrapper.m_MoveActions_Newaction;
        public InputActionMap Get() { return m_Wrapper.m_MoveActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MoveActionsActions set) { return set.Get(); }
        public void SetCallbacks(IMoveActionsActions instance)
        {
            if (m_Wrapper.m_MoveActionsActionsCallbackInterface != null)
            {
                @Newaction.started -= m_Wrapper.m_MoveActionsActionsCallbackInterface.OnNewaction;
                @Newaction.performed -= m_Wrapper.m_MoveActionsActionsCallbackInterface.OnNewaction;
                @Newaction.canceled -= m_Wrapper.m_MoveActionsActionsCallbackInterface.OnNewaction;
            }
            m_Wrapper.m_MoveActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Newaction.started += instance.OnNewaction;
                @Newaction.performed += instance.OnNewaction;
                @Newaction.canceled += instance.OnNewaction;
            }
        }
    }
    public MoveActionsActions @MoveActions => new MoveActionsActions(this);
    public interface IMoveActionsActions
    {
        void OnNewaction(InputAction.CallbackContext context);
    }
}
