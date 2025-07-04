//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Res/Scripts/Control/Input/Player.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Player: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Player()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player"",
    ""maps"": [
        {
            ""name"": ""PlayerCtx"",
            ""id"": ""e5348e29-cd9d-4810-a0b6-ce7e24159d3c"",
            ""actions"": [
                {
                    ""name"": ""Pointer"",
                    ""type"": ""Value"",
                    ""id"": ""8f451b78-dc6c-4526-a66b-71dee20d359e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""b3ff33d8-c355-43c0-8453-41ebfc05b540"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""ca262fd0-0926-4280-8a76-d4e8521cdaeb"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""LeftMouse"",
                    ""type"": ""Button"",
                    ""id"": ""b22407bc-a5a0-4385-8e45-12d6d3693da0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""LeftShift"",
                    ""type"": ""Button"",
                    ""id"": ""f449bd8d-5c54-4460-a7f9-f054dfa10a06"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Space"",
                    ""type"": ""Button"",
                    ""id"": ""f7c2a87b-a6a1-4810-bd42-2a46dfcddb8d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""b9fc7491-6520-49c1-a540-582fe9559a9a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""e92fa95b-c0fc-4f92-9d55-f07226f6e47f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""c78d8eb3-4418-45ca-bfba-39b03168586f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwitchWeapon"",
                    ""type"": ""Button"",
                    ""id"": ""a0fa11f0-3b06-4cac-9b48-98e9ddaea9be"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""HeavyAttack"",
                    ""type"": ""Button"",
                    ""id"": ""9c93ebef-42e2-4b88-a75e-d36f9975f2e4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Skill"",
                    ""type"": ""Button"",
                    ""id"": ""cbc807d0-50ae-4dfb-83b7-8efbce4859c1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Search"",
                    ""type"": ""Button"",
                    ""id"": ""c2b2d9ee-6d00-4253-b2f2-73885927ff12"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f9b77d90-b6a2-4a88-9732-dbcbf9c079d5"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pointer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3e40aedd-8ff6-472e-8927-30cf2fb149cc"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""7875132b-999c-4eed-9010-b7161db8ff84"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""e031a1a6-0854-4bac-8c9c-d1caacbec6af"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""4ee8dd98-5230-451c-8079-a6fef0a7d0e0"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f8105152-74eb-452b-b374-8c8c40a5178a"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6c39f288-f365-495e-8ee7-3dead62a6b51"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""22b3311e-5955-42a1-8197-e2933a757f85"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftMouse"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c9ae651-ea33-404a-a7a2-372f74b110b3"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LeftShift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2dbd31ae-197d-49cf-81e8-a82ba33fb032"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Space"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0283bb56-7c5b-4c95-8a47-fe0ff8eb6173"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7ec7e99e-1dc0-4bcc-b303-6a08665bf8b5"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""390ea4b7-49df-4534-a57c-6e8bd3dd46be"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b83237f0-4af0-41ec-933e-00816ba82ced"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwitchWeapon"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ce1b9ca9-e13e-4d5d-b42c-c4e480c68b4f"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Hold"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""HeavyAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6c6d5951-e3c4-4ef7-8cee-c90f163a2cc6"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4f7c8c2-6fd5-43de-9ac9-92cb101e3e7c"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Search"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerCtx
        m_PlayerCtx = asset.FindActionMap("PlayerCtx", throwIfNotFound: true);
        m_PlayerCtx_Pointer = m_PlayerCtx.FindAction("Pointer", throwIfNotFound: true);
        m_PlayerCtx_Zoom = m_PlayerCtx.FindAction("Zoom", throwIfNotFound: true);
        m_PlayerCtx_Move = m_PlayerCtx.FindAction("Move", throwIfNotFound: true);
        m_PlayerCtx_LeftMouse = m_PlayerCtx.FindAction("LeftMouse", throwIfNotFound: true);
        m_PlayerCtx_LeftShift = m_PlayerCtx.FindAction("LeftShift", throwIfNotFound: true);
        m_PlayerCtx_Space = m_PlayerCtx.FindAction("Space", throwIfNotFound: true);
        m_PlayerCtx_Jump = m_PlayerCtx.FindAction("Jump", throwIfNotFound: true);
        m_PlayerCtx_Crouch = m_PlayerCtx.FindAction("Crouch", throwIfNotFound: true);
        m_PlayerCtx_Attack = m_PlayerCtx.FindAction("Attack", throwIfNotFound: true);
        m_PlayerCtx_SwitchWeapon = m_PlayerCtx.FindAction("SwitchWeapon", throwIfNotFound: true);
        m_PlayerCtx_HeavyAttack = m_PlayerCtx.FindAction("HeavyAttack", throwIfNotFound: true);
        m_PlayerCtx_Skill = m_PlayerCtx.FindAction("Skill", throwIfNotFound: true);
        m_PlayerCtx_Search = m_PlayerCtx.FindAction("Search", throwIfNotFound: true);
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

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // PlayerCtx
    private readonly InputActionMap m_PlayerCtx;
    private List<IPlayerCtxActions> m_PlayerCtxActionsCallbackInterfaces = new List<IPlayerCtxActions>();
    private readonly InputAction m_PlayerCtx_Pointer;
    private readonly InputAction m_PlayerCtx_Zoom;
    private readonly InputAction m_PlayerCtx_Move;
    private readonly InputAction m_PlayerCtx_LeftMouse;
    private readonly InputAction m_PlayerCtx_LeftShift;
    private readonly InputAction m_PlayerCtx_Space;
    private readonly InputAction m_PlayerCtx_Jump;
    private readonly InputAction m_PlayerCtx_Crouch;
    private readonly InputAction m_PlayerCtx_Attack;
    private readonly InputAction m_PlayerCtx_SwitchWeapon;
    private readonly InputAction m_PlayerCtx_HeavyAttack;
    private readonly InputAction m_PlayerCtx_Skill;
    private readonly InputAction m_PlayerCtx_Search;
    public struct PlayerCtxActions
    {
        private @Player m_Wrapper;
        public PlayerCtxActions(@Player wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pointer => m_Wrapper.m_PlayerCtx_Pointer;
        public InputAction @Zoom => m_Wrapper.m_PlayerCtx_Zoom;
        public InputAction @Move => m_Wrapper.m_PlayerCtx_Move;
        public InputAction @LeftMouse => m_Wrapper.m_PlayerCtx_LeftMouse;
        public InputAction @LeftShift => m_Wrapper.m_PlayerCtx_LeftShift;
        public InputAction @Space => m_Wrapper.m_PlayerCtx_Space;
        public InputAction @Jump => m_Wrapper.m_PlayerCtx_Jump;
        public InputAction @Crouch => m_Wrapper.m_PlayerCtx_Crouch;
        public InputAction @Attack => m_Wrapper.m_PlayerCtx_Attack;
        public InputAction @SwitchWeapon => m_Wrapper.m_PlayerCtx_SwitchWeapon;
        public InputAction @HeavyAttack => m_Wrapper.m_PlayerCtx_HeavyAttack;
        public InputAction @Skill => m_Wrapper.m_PlayerCtx_Skill;
        public InputAction @Search => m_Wrapper.m_PlayerCtx_Search;
        public InputActionMap Get() { return m_Wrapper.m_PlayerCtx; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerCtxActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerCtxActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerCtxActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerCtxActionsCallbackInterfaces.Add(instance);
            @Pointer.started += instance.OnPointer;
            @Pointer.performed += instance.OnPointer;
            @Pointer.canceled += instance.OnPointer;
            @Zoom.started += instance.OnZoom;
            @Zoom.performed += instance.OnZoom;
            @Zoom.canceled += instance.OnZoom;
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @LeftMouse.started += instance.OnLeftMouse;
            @LeftMouse.performed += instance.OnLeftMouse;
            @LeftMouse.canceled += instance.OnLeftMouse;
            @LeftShift.started += instance.OnLeftShift;
            @LeftShift.performed += instance.OnLeftShift;
            @LeftShift.canceled += instance.OnLeftShift;
            @Space.started += instance.OnSpace;
            @Space.performed += instance.OnSpace;
            @Space.canceled += instance.OnSpace;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Crouch.started += instance.OnCrouch;
            @Crouch.performed += instance.OnCrouch;
            @Crouch.canceled += instance.OnCrouch;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @SwitchWeapon.started += instance.OnSwitchWeapon;
            @SwitchWeapon.performed += instance.OnSwitchWeapon;
            @SwitchWeapon.canceled += instance.OnSwitchWeapon;
            @HeavyAttack.started += instance.OnHeavyAttack;
            @HeavyAttack.performed += instance.OnHeavyAttack;
            @HeavyAttack.canceled += instance.OnHeavyAttack;
            @Skill.started += instance.OnSkill;
            @Skill.performed += instance.OnSkill;
            @Skill.canceled += instance.OnSkill;
            @Search.started += instance.OnSearch;
            @Search.performed += instance.OnSearch;
            @Search.canceled += instance.OnSearch;
        }

        private void UnregisterCallbacks(IPlayerCtxActions instance)
        {
            @Pointer.started -= instance.OnPointer;
            @Pointer.performed -= instance.OnPointer;
            @Pointer.canceled -= instance.OnPointer;
            @Zoom.started -= instance.OnZoom;
            @Zoom.performed -= instance.OnZoom;
            @Zoom.canceled -= instance.OnZoom;
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @LeftMouse.started -= instance.OnLeftMouse;
            @LeftMouse.performed -= instance.OnLeftMouse;
            @LeftMouse.canceled -= instance.OnLeftMouse;
            @LeftShift.started -= instance.OnLeftShift;
            @LeftShift.performed -= instance.OnLeftShift;
            @LeftShift.canceled -= instance.OnLeftShift;
            @Space.started -= instance.OnSpace;
            @Space.performed -= instance.OnSpace;
            @Space.canceled -= instance.OnSpace;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Crouch.started -= instance.OnCrouch;
            @Crouch.performed -= instance.OnCrouch;
            @Crouch.canceled -= instance.OnCrouch;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @SwitchWeapon.started -= instance.OnSwitchWeapon;
            @SwitchWeapon.performed -= instance.OnSwitchWeapon;
            @SwitchWeapon.canceled -= instance.OnSwitchWeapon;
            @HeavyAttack.started -= instance.OnHeavyAttack;
            @HeavyAttack.performed -= instance.OnHeavyAttack;
            @HeavyAttack.canceled -= instance.OnHeavyAttack;
            @Skill.started -= instance.OnSkill;
            @Skill.performed -= instance.OnSkill;
            @Skill.canceled -= instance.OnSkill;
            @Search.started -= instance.OnSearch;
            @Search.performed -= instance.OnSearch;
            @Search.canceled -= instance.OnSearch;
        }

        public void RemoveCallbacks(IPlayerCtxActions instance)
        {
            if (m_Wrapper.m_PlayerCtxActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerCtxActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerCtxActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerCtxActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerCtxActions @PlayerCtx => new PlayerCtxActions(this);
    public interface IPlayerCtxActions
    {
        void OnPointer(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnLeftMouse(InputAction.CallbackContext context);
        void OnLeftShift(InputAction.CallbackContext context);
        void OnSpace(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnSwitchWeapon(InputAction.CallbackContext context);
        void OnHeavyAttack(InputAction.CallbackContext context);
        void OnSkill(InputAction.CallbackContext context);
        void OnSearch(InputAction.CallbackContext context);
    }
}
