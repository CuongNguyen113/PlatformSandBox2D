using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerDashState : PlayerAbilityState {

    public bool CanDash {  get; private set; }
    private bool isHolding;
    private bool dashInputStop;

    private float lastDashTime;
    private const float indicatorSpriteAngle = 45f;

    private Vector2 dashDirection;
    private Vector2 dashDirectionInput;
    private Vector2 lastAfterImagePos;

    public PlayerDashState(Player player, PlayerStateMachine playerStateMachine, PlayerData playerData, string animBoolName) : base(player, playerStateMachine, playerData, animBoolName) {
    }
    public override void Enter() {
        base.Enter();

        CanDash = false;
        player.InputHandler.UseDashInput();

        isHolding = true;
        dashDirection = Vector2.right * player.FacingDirection;

        Time.timeScale = playerData.holdTimeScale;
        startStateTime = Time.unscaledTime;

        player.dashDirectionIndicator.gameObject.SetActive(true);
    }

    public override void Exit() {
        base.Exit();

        if (player.CurrentVelocity.y > 0) {
            player.SetVelocityY(player.CurrentVelocity.y * playerData.dashEndYMultiplier);
        }
    }

    public override void LogicUpdate() {
        base.LogicUpdate();

        if (!isExitingState) {

            player.Animator.SetFloat(PlayerAnimationConstants.X_VELOCITY, Mathf.Abs(player.CurrentVelocity.x));
            player.Animator.SetFloat(PlayerAnimationConstants.Y_VELOCITY, player.CurrentVelocity.y);

            if (isHolding) {
                dashDirectionInput = player.InputHandler.DashDirectionInput;
                dashInputStop = player.InputHandler.DashInputStop;

                if (dashDirection != Vector2.zero) {
                    dashDirection = dashDirectionInput;
                    dashDirection.Normalize();
                }

                float angle = Vector2.SignedAngle(Vector2.right, dashDirection);
                player.dashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle - indicatorSpriteAngle);

                if (dashInputStop || Time.unscaledTime >= startStateTime + playerData.maxHoldTime) {
                    isHolding = false;
                    Time.timeScale = 1f;
                    startStateTime = Time.time;
                    player.CheckIfShouldFlip(Mathf.RoundToInt(dashDirection.x));
                    player.Rigidbody.drag = playerData.drag;
                    player.SetVelocity(playerData.dashVelocity, dashDirection);
                    player.dashDirectionIndicator.gameObject.SetActive(false);
                    PlaceAfterImage();
                }
            } 
            else {
                player.SetVelocity(playerData.dashVelocity, dashDirection);
                CheckIfShouldPlaceAfterImage();

                if (Time.time >= startStateTime + playerData.dashTime) {
                    player.Rigidbody.drag = 0f;
                    isAbilityDone = true;
                    lastDashTime = Time.time;
                }
            }
        }
    }

    private void CheckIfShouldPlaceAfterImage() {
        if (Vector2.Distance(player.transform.position, lastAfterImagePos) > playerData.distanceBetweenAfterImages) {
            PlaceAfterImage();
        }
    }

    private void PlaceAfterImage() {
        PlayerAfterImagePool.Instance.GetFromPool();
        lastAfterImagePos = player.transform.position;
    }

    public bool CheckIfCanDash() {
        return CanDash && Time.time > lastDashTime + playerData.dashCoolDown;
    }

    public void ResetCanDash() => CanDash = true;

}
