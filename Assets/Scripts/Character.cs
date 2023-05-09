
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class Character : MonoBehaviour
{
    [Header("Controls")]
    [Tooltip("Speed of the character when moving")]
    public float playerSpeed = 1.0f;

    [Tooltip("Speed of the character when crouching")]
    public float crouchSpeed = 2.0f;

    [Tooltip("Speed of the character when sprint")]
    public float sprintSpeed = 7.0f;

    [Tooltip("The height player can jump")]
    public float jumpHeight = 0.8f;

    [Tooltip("The gravity multiplier. Gravity default is -9.81f")]
    public float gravityMultiplier = 2f;

    [Tooltip("The rotation speed")]
    public float rotationSpeed = 5f;

    [Tooltip("Player height when crouching")]
    public float crouchColliderHight = 1.35f;

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    [Header("Animation Smoothing")]
    [Range(0f, 1f)]
    public float speedDampTime = 0.1f;
    [Range(0f, 1f)]
    public float velocityDampTime = 0.9f;
    [Range(0f, 1f)]
    public float rotaionDampTime = 0.2f;
    [Range(0f, 1f)]
    public float airControl = 0.5f;

    public float rotationPower = 3f;
    public float rotationLerp = 0.5f;

    public StateMachine movementSM;
    public StandingState standing;
    public JumpingState jumping;
    public CrouchingState crouching;
    public LandingState landing;
    public SprintState sprinting;
    public SprintJumpState sprintjumping;


    [HideInInspector]
    public float gravityValue = -9.81f;
    [HideInInspector]
    public float normalColliderHeight;
    [HideInInspector]
    public CharacterController controller;
    [HideInInspector]
    public PlayerInput playerInput;
    [HideInInspector]
    public Transform cameraTransform;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Vector3 playerVelocity;

    private GameObject _mainCamera;

    public Vector2 _move;
    public Vector2 _look;
    public Vector3 angles;
    

    private void Start() {
        animator = GetComponent<Animator>();
        controller= GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        movementSM = new StateMachine();
        standing = new StandingState(this, movementSM); 
        jumping = new JumpingState(this, movementSM);
        crouching = new CrouchingState(this, movementSM);
        landing = new LandingState(this, movementSM);
        sprinting = new SprintState(this, movementSM);
        sprintjumping = new SprintJumpState(this, movementSM);

        movementSM.Initialize(standing);

        normalColliderHeight = controller.height;
        gravityValue *= gravityMultiplier;
    }

    private void Update() {
        movementSM.currentState.HandleInput();

        movementSM.currentState.LogicUpdate();

        #region Player Based Rotation

        //Move the player based on the X input on the controller
        //transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);

        #endregion

        #region Follow Transform Rotation

        //Rotate the Follow Target transform based on the input
        CinemachineCameraTarget.transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);

        #endregion


        #region Vertical Rotation
        CinemachineCameraTarget.transform.rotation *= Quaternion.AngleAxis(_look.y * rotationPower, Vector3.right);

        angles = CinemachineCameraTarget.transform.localEulerAngles;
        angles.z = 0;

        var angle = CinemachineCameraTarget.transform.localEulerAngles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340) {
            angles.x = 340;
        } else if (angle < 180 && angle > 40) {
            angles.x = 40;
        }


        CinemachineCameraTarget.transform.localEulerAngles = angles;
        #endregion


}

private void FixedUpdate() {
        movementSM.currentState.PhysicsUpdate();
    }

    private void Awake() {
        // get a reference to our main camera
        if (_mainCamera == null) {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }

    private void OnFootstep(AnimationEvent animationEvent) {
        if (animationEvent.animatorClipInfo.weight > 0.5f) {
            if (FootstepAudioClips.Length > 0) {
                var index = Random.Range(0, FootstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(controller.center), FootstepAudioVolume);
            }
        }
    }

    private void OnLand(AnimationEvent animationEvent) {
        if (animationEvent.animatorClipInfo.weight > 0.5f) {
            AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(controller.center), FootstepAudioVolume);
        }
    }

    public void OnLook(InputValue value) {
        _look = value.Get<Vector2>();
    }

    public void OnMove(InputValue value) {
        _move = value.Get<Vector2>();
    }

}
