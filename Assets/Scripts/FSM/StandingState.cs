using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class StandingState : State
{

    float gravityValue;
    bool jump;
    bool crouch;
    Vector3 currentVelocity;
    bool grounded;
    bool sprint;
    bool inAir;
    float playerSpeed;

    Vector3 nextPosition;
    Quaternion nextRotation;

    public float rotationLerp = 0.5f;

    GameObject followTransform;

    public StandingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine) {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter() {
        base.Enter();

        jump = false;
        crouch = false;
        sprint = false;
        input = Vector2.zero;
        velocity = Vector3.zero;
        currentVelocity = Vector3.zero;
        gravityVelocity.y = 0;
        followTransform = character.CinemachineCameraTarget;

        playerSpeed = character.playerSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;
    }

    public override void HandleInput() {
        base.HandleInput();

        if (jumpAction.triggered) {
            jump = true;
        }

        if (crouchAction.triggered) {
            crouch = true;
        }

        if (sprintAction.triggered) { 
            sprint = true; 
        }

        input = moveAction.ReadValue<Vector2>().normalized;
        velocity = new Vector3(input.x, 0.0f, input.y);

        velocity = velocity.x * character.CinemachineCameraTarget.transform.right.normalized + velocity.z * character.CinemachineCameraTarget.transform.forward.normalized;
        
        velocity.y = 0f;

        velocity = velocity.normalized;
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        character.animator.SetFloat("speed", input.magnitude, character.speedDampTime, Time.deltaTime);

        if (sprint) {
            stateMachine.ChangeState(character.sprinting);
        }
        if (jump) {
            stateMachine.ChangeState(character.jumping);
        }
        if (crouch) {
            stateMachine.ChangeState(character.crouching);
        }
    }

    public override void PhysicsUpdate() {
        base.PhysicsUpdate();

        nextRotation = Quaternion.Lerp(followTransform.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);

        if (input.x == 0 && input.y == 0) {
            nextPosition = character.transform.position;

            return;
        }
        float moveSpeed = playerSpeed / 100f;
        Vector3 position = (character.transform.forward * input.y * moveSpeed) + (character.transform.right * input.x * moveSpeed);
        nextPosition = character.transform.position + position;


        //Set the player rotation based on the look transform
        character.transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
        //reset the y rotation of the look transform
        followTransform.transform.localEulerAngles = new Vector3(character.angles.x, 0, 0);
        character.transform.position = nextPosition;
    }



public override void Exit() {
        base.Exit();
        gravityVelocity.y = 0f;
        character.playerVelocity = new Vector3(input.x, 0, input.y);

        if(velocity.sqrMagnitude > 0) {
            character.transform.rotation = Quaternion.LookRotation(velocity);
        }
    }


}
