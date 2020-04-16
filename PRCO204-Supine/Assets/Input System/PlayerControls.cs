// GENERATED AUTOMATICALLY FROM 'Assets/Input System/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Gameplay"",
            ""id"": ""c5b59e3c-15f4-47e7-aafa-910bf83c8023"",
            ""actions"": [
                {
                    ""name"": ""PlayerMoveX"",
                    ""type"": ""Value"",
                    ""id"": ""0fbcc303-8bb6-4f4a-86c1-8209fae554ac"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerMoveZ"",
                    ""type"": ""Value"",
                    ""id"": ""092d8343-2340-4dcc-b88e-20bb99cb6298"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerJump"",
                    ""type"": ""Button"",
                    ""id"": ""eb3829af-1966-4c7a-b8b6-78621a82de41"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerRun"",
                    ""type"": ""Button"",
                    ""id"": ""32cc8910-2525-4b4f-8ef1-a0d44c69b32f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerCrouch"",
                    ""type"": ""Button"",
                    ""id"": ""61cfd98d-3873-4337-9dd2-130f385c88c8"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraRotate"",
                    ""type"": ""Value"",
                    ""id"": ""f6ba4a6e-7970-44ea-9484-c5d8d78789a9"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PlayerRotY"",
                    ""type"": ""Value"",
                    ""id"": ""09a099f2-9125-4502-965d-47ebaf48057d"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""219d7c91-30c0-43df-a2b8-a4cb87ab3034"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMoveX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""774ebc34-3468-4673-a371-e18d866f9bae"",
                    ""path"": ""<Gamepad>/leftStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerMoveZ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0d13bc84-1328-4f63-befe-4ff9a3c623ac"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerJump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ea16a623-7621-4a86-b611-3604b7c36080"",
                    ""path"": ""<Gamepad>/leftStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerRun"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20044d31-98ac-42ad-a6d4-61c0f2bf9d44"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerCrouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""20be2de1-9d4d-4bff-93d1-d24ce19a4c01"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraRotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c39a695c-5834-4e12-8a38-972c191bb33b"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PlayerRotY"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gameplay
        m_Gameplay = asset.FindActionMap("Gameplay", throwIfNotFound: true);
        m_Gameplay_PlayerMoveX = m_Gameplay.FindAction("PlayerMoveX", throwIfNotFound: true);
        m_Gameplay_PlayerMoveZ = m_Gameplay.FindAction("PlayerMoveZ", throwIfNotFound: true);
        m_Gameplay_PlayerJump = m_Gameplay.FindAction("PlayerJump", throwIfNotFound: true);
        m_Gameplay_PlayerRun = m_Gameplay.FindAction("PlayerRun", throwIfNotFound: true);
        m_Gameplay_PlayerCrouch = m_Gameplay.FindAction("PlayerCrouch", throwIfNotFound: true);
        m_Gameplay_CameraRotate = m_Gameplay.FindAction("CameraRotate", throwIfNotFound: true);
        m_Gameplay_PlayerRotY = m_Gameplay.FindAction("PlayerRotY", throwIfNotFound: true);
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

    // Gameplay
    private readonly InputActionMap m_Gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private readonly InputAction m_Gameplay_PlayerMoveX;
    private readonly InputAction m_Gameplay_PlayerMoveZ;
    private readonly InputAction m_Gameplay_PlayerJump;
    private readonly InputAction m_Gameplay_PlayerRun;
    private readonly InputAction m_Gameplay_PlayerCrouch;
    private readonly InputAction m_Gameplay_CameraRotate;
    private readonly InputAction m_Gameplay_PlayerRotY;
    public struct GameplayActions
    {
        private @PlayerControls m_Wrapper;
        public GameplayActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @PlayerMoveX => m_Wrapper.m_Gameplay_PlayerMoveX;
        public InputAction @PlayerMoveZ => m_Wrapper.m_Gameplay_PlayerMoveZ;
        public InputAction @PlayerJump => m_Wrapper.m_Gameplay_PlayerJump;
        public InputAction @PlayerRun => m_Wrapper.m_Gameplay_PlayerRun;
        public InputAction @PlayerCrouch => m_Wrapper.m_Gameplay_PlayerCrouch;
        public InputAction @CameraRotate => m_Wrapper.m_Gameplay_CameraRotate;
        public InputAction @PlayerRotY => m_Wrapper.m_Gameplay_PlayerRotY;
        public InputActionMap Get() { return m_Wrapper.m_Gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                @PlayerMoveX.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerMoveX;
                @PlayerMoveX.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerMoveX;
                @PlayerMoveX.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerMoveX;
                @PlayerMoveZ.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerMoveZ;
                @PlayerMoveZ.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerMoveZ;
                @PlayerMoveZ.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerMoveZ;
                @PlayerJump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerJump;
                @PlayerJump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerJump;
                @PlayerJump.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerJump;
                @PlayerRun.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerRun;
                @PlayerRun.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerRun;
                @PlayerRun.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerRun;
                @PlayerCrouch.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerCrouch;
                @PlayerCrouch.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerCrouch;
                @PlayerCrouch.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerCrouch;
                @CameraRotate.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraRotate;
                @CameraRotate.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraRotate;
                @CameraRotate.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnCameraRotate;
                @PlayerRotY.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerRotY;
                @PlayerRotY.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerRotY;
                @PlayerRotY.canceled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPlayerRotY;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @PlayerMoveX.started += instance.OnPlayerMoveX;
                @PlayerMoveX.performed += instance.OnPlayerMoveX;
                @PlayerMoveX.canceled += instance.OnPlayerMoveX;
                @PlayerMoveZ.started += instance.OnPlayerMoveZ;
                @PlayerMoveZ.performed += instance.OnPlayerMoveZ;
                @PlayerMoveZ.canceled += instance.OnPlayerMoveZ;
                @PlayerJump.started += instance.OnPlayerJump;
                @PlayerJump.performed += instance.OnPlayerJump;
                @PlayerJump.canceled += instance.OnPlayerJump;
                @PlayerRun.started += instance.OnPlayerRun;
                @PlayerRun.performed += instance.OnPlayerRun;
                @PlayerRun.canceled += instance.OnPlayerRun;
                @PlayerCrouch.started += instance.OnPlayerCrouch;
                @PlayerCrouch.performed += instance.OnPlayerCrouch;
                @PlayerCrouch.canceled += instance.OnPlayerCrouch;
                @CameraRotate.started += instance.OnCameraRotate;
                @CameraRotate.performed += instance.OnCameraRotate;
                @CameraRotate.canceled += instance.OnCameraRotate;
                @PlayerRotY.started += instance.OnPlayerRotY;
                @PlayerRotY.performed += instance.OnPlayerRotY;
                @PlayerRotY.canceled += instance.OnPlayerRotY;
            }
        }
    }
    public GameplayActions @Gameplay => new GameplayActions(this);
    public interface IGameplayActions
    {
        void OnPlayerMoveX(InputAction.CallbackContext context);
        void OnPlayerMoveZ(InputAction.CallbackContext context);
        void OnPlayerJump(InputAction.CallbackContext context);
        void OnPlayerRun(InputAction.CallbackContext context);
        void OnPlayerCrouch(InputAction.CallbackContext context);
        void OnCameraRotate(InputAction.CallbackContext context);
        void OnPlayerRotY(InputAction.CallbackContext context);
    }
}
