// GENERATED AUTOMATICALLY FROM 'Assets/PlayerController.inputactions'

using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerController : IInputActionCollection
{
    private InputActionAsset asset;
    public PlayerController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerController"",
    ""maps"": [
        {
            ""name"": ""CharacterController"",
            ""id"": ""f2ff80bd-9097-433e-9d94-cb4d2874b1af"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""6e73d668-e110-40df-a2ab-af646f61b4de"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""63ca5c61-aafb-4132-92f9-e0a3304ae60c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fire"",
                    ""type"": ""Value"",
                    ""id"": ""4fda8fa6-fecc-4add-a2a5-4055d1652745"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Value"",
                    ""id"": ""662652b1-82ca-46ec-8132-9ba3c9b6cd0d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Button"",
                    ""id"": ""c4bf5e0c-3b5c-45f4-b815-9e29e4b9f010"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Build"",
                    ""type"": ""Button"",
                    ""id"": ""905a57e6-85b4-4391-9583-4775a2c52e8e"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ed119aa3-c4b4-4b52-a60d-be515595f89c"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""08182bfa-44a6-4ff3-b8a9-f57edf3d13e6"",
                    ""path"": ""Dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9aa59881-6e13-4196-9be5-4400ecfeac54"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""031787dd-0cc1-4f10-96cd-ff37fb151424"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d16a5189-6634-470a-8798-5bc2462115d1"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2c9ed98a-ef6e-4e78-85d0-3526e1822e38"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""f6628be5-f940-4fa0-82b2-b69319138a91"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f38ee64e-43f6-44d6-bfd6-6f16bafbd61d"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bd333fc3-2b78-4d69-bbf2-951aa3b29d36"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e6ef0613-6a60-4e3a-8373-a63b0e2d5bf2"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20dc192e-1af9-45b8-9414-478b1bdbb583"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a06f4320-d4ea-4ee1-8688-00b37470937a"",
                    ""path"": ""<Keyboard>/rightCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Fire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96f1ab62-8ad4-4509-af6f-cd969aff81d8"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f668c6c0-5066-4316-8f06-a5a1ac8967fd"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Keyboard&Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""543112ed-6934-4dd1-bf4e-9b6806a629d9"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f58d7afd-3c6b-4d8f-8129-cf65d22f13f4"",
                    ""path"": ""<Keyboard>/leftAlt"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Build"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // CharacterController
        m_CharacterController = asset.FindActionMap("CharacterController", throwIfNotFound: true);
        m_CharacterController_Move = m_CharacterController.FindAction("Move", throwIfNotFound: true);
        m_CharacterController_Look = m_CharacterController.FindAction("Look", throwIfNotFound: true);
        m_CharacterController_Fire = m_CharacterController.FindAction("Fire", throwIfNotFound: true);
        m_CharacterController_Jump = m_CharacterController.FindAction("Jump", throwIfNotFound: true);
        m_CharacterController_Aim = m_CharacterController.FindAction("Aim", throwIfNotFound: true);
        m_CharacterController_Build = m_CharacterController.FindAction("Build", throwIfNotFound: true);
    }

    ~PlayerController()
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

    // CharacterController
    private readonly InputActionMap m_CharacterController;
    private ICharacterControllerActions m_CharacterControllerActionsCallbackInterface;
    private readonly InputAction m_CharacterController_Move;
    private readonly InputAction m_CharacterController_Look;
    private readonly InputAction m_CharacterController_Fire;
    private readonly InputAction m_CharacterController_Jump;
    private readonly InputAction m_CharacterController_Aim;
    private readonly InputAction m_CharacterController_Build;
    public struct CharacterControllerActions
    {
        private PlayerController m_Wrapper;
        public CharacterControllerActions(PlayerController wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_CharacterController_Move;
        public InputAction @Look => m_Wrapper.m_CharacterController_Look;
        public InputAction @Fire => m_Wrapper.m_CharacterController_Fire;
        public InputAction @Jump => m_Wrapper.m_CharacterController_Jump;
        public InputAction @Aim => m_Wrapper.m_CharacterController_Aim;
        public InputAction @Build => m_Wrapper.m_CharacterController_Build;
        public InputActionMap Get() { return m_Wrapper.m_CharacterController; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterControllerActions set) { return set.Get(); }
        public void SetCallbacks(ICharacterControllerActions instance)
        {
            if (m_Wrapper.m_CharacterControllerActionsCallbackInterface != null)
            {
                Move.started -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnMove;
                Move.canceled -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnMove;
                Look.started -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnLook;
                Look.performed -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnLook;
                Look.canceled -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnLook;
                Fire.started -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnFire;
                Fire.performed -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnFire;
                Fire.canceled -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnFire;
                Jump.started -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnJump;
                Jump.performed -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnJump;
                Jump.canceled -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnJump;
                Aim.started -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnAim;
                Aim.performed -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnAim;
                Aim.canceled -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnAim;
                Build.started -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnBuild;
                Build.performed -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnBuild;
                Build.canceled -= m_Wrapper.m_CharacterControllerActionsCallbackInterface.OnBuild;
            }
            m_Wrapper.m_CharacterControllerActionsCallbackInterface = instance;
            if (instance != null)
            {
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.canceled += instance.OnMove;
                Look.started += instance.OnLook;
                Look.performed += instance.OnLook;
                Look.canceled += instance.OnLook;
                Fire.started += instance.OnFire;
                Fire.performed += instance.OnFire;
                Fire.canceled += instance.OnFire;
                Jump.started += instance.OnJump;
                Jump.performed += instance.OnJump;
                Jump.canceled += instance.OnJump;
                Aim.started += instance.OnAim;
                Aim.performed += instance.OnAim;
                Aim.canceled += instance.OnAim;
                Build.started += instance.OnBuild;
                Build.performed += instance.OnBuild;
                Build.canceled += instance.OnBuild;
            }
        }
    }
    public CharacterControllerActions @CharacterController => new CharacterControllerActions(this);
    public interface ICharacterControllerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnFire(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnBuild(InputAction.CallbackContext context);
    }
}
