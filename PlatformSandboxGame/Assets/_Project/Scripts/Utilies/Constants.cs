using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerAnimationConstants {
    //State
    public const string IS_WALKING = "isWalking";
    public const string IS_GROUNDED = "IsGrounded";
    public const string IS_WALL_SLIDING = "isWallSliding";
    public const string Y_VELOCITY = "yVelocity";
    public const string X_VELOCITY = "xVelocity";
    public const string CAN_CLIMB_LEDGE = "canClimbLedge";
    public const string IS_JUMPING = "isJumping";
    public const string IS_FALLING = "isFalling";
    public const string SPRINT_TOGGLE_ON = "sprintToggleOn";
    public const string CAN_ATTACK = "canAttack";
    public const string ATTACK = "attack";
    public const string FIRST_ATTACK = "firstAttack";
    public const string IS_ATTACKING = "isAttacking";

}

public static class UIConstants {
    public const string BAR = "bar";
    public const string DAMAMED_BAR = "damagedBar";
    public const string DAMAGED_BAR_TEMPLATE = "damagedBarTemplate";
    public const string HEALTH_VALUE_TEXT = "healthPointText";
    public const string BACKGROUND = "background";
    public const string BORDER = "border";
    public const float FALL_CUT_EFFECT_SPEED = 100f;
    public const float FADE_CUT_EFFECT_SPEED = 5f;
    public const float DAMAGED_HEALTH_FADE_TIMER_MAX = 1f;
    public const float DAMAGED_HEALTH_SHRINK_TIMER_MAX = 0.6f;
    public const float PLAYER_BAR_WIDTH = 400f;
    public const float ENEMY_BAR_WIDTH = 150f;
}
public static class TagConstants {
    public const string PLAYER = "Player";
    public const string ALIVE = "Alive";
    public const string BROKEN_TOP = "BrokenTop";
    public const string BROKEN_BOT = "BrokenBottom";
    public const string RED_LUKER = "RedLuker";
}

public static class CombatDummyAnimationConstants {
    public const string PlAYER_ON_LEFT = "playerOnLeft";
    public const string DAMAGE_TRIGGER = "damage";
}


public static class RedLukerAniamtionConstants {
    public const string KNOCKBACK = "knockback";
}

public static class GameObjectNameConstants {
    public const string PLAYER_CAMERA = "Player Camera";
    public const string GAME_MANAGER = "GameManager";
    public const string PLAYER = "Player";
    public const string PLAYER_HEALTH_BAR = "PlayerHealthBar";
    public const string ENEMY_HEALTH_BAR = "EnemyHealthBar";
    public const string BASIC_HEALTH_BAR = "BasicHealthBar";
    public const string DASH_DIRECTION_INDICATOR = "DashDirectionIndicator";
    public const string BASE = "Base";
    public const string WEAPON = "Weapon";
}

public static class EnemyStateConstants {
    public const string IDLE = "idle";
    public const string MOVE = "move";
    public const string PLAYER_DETECTED = "playerDetected";
    public const string CHARGE = "charge";
    public const string LOOK_FOR_PLAYER = "lookForPlayer";
    public const string MELEE_ATTACK = "meleeAttack";
    public const string STUN = "stun";
    public const string DEAD = "dead";
    public const string DODGE = "dodge";
    public const string RANGED_ATTACK = "rangedAttack";
}

public static class PlayerStateConstants {
    public const string IDLE = "idle";
    public const string MOVE = "move";
    public const string IN_AIR = "inAir";
    public const string LAND = "land";
    public const string WALL_SLIDE = "wallSlide";
    public const string WALL_GRAB = "wallGrab";
    public const string WALL_CLIMB = "wallClimb";
    public const string LEDGE_CLIMB = "ledgeClimb";
    public const string LEDGE_CLIMB_STATE = "ledgeClimbState";
    public const string CROUCH_IDLE = "crouchIdle";
    public const string CROUCH_MOVE = "crouchMove";
    public const string IS_TOUCHING_CEILING = "isTouchingCeiling";
    public const string ATTACK = "attack";
    public const string ATTACK_COUNTER = "attackCounter";
}

public static class ControlSchemeConstants {
    public const string KEYBOARD = "Keyboard";
}

