
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class CombatDummyController : MonoBehaviour {

    #region Variable
    [SerializeField] private float knockbackSpeedX, knockbackSpeedY, knockbackDeathSpeedX, knockbackDeathSpeedY, deathTorque, knockbackDuration;
    private float knockbackStart;

    [SerializeField] private bool applyKnockback;
    public bool playerOnLeft { get; private set; }
    private bool isKnockback;

    private int playerFacingDirection;

    public event Action<string, bool> OnSetAnimatorBool;
    public event Action<string> OnSetAnimatorTrigger;

    private GameObject aliveGO, brokenTopGO, brokenBotGO;
    private Rigidbody2D _rbAlive, _rbBrokenTop, _rbBrokenBot;
    private StatsController combatDummyStats;

    #endregion

    #region Start Function
    private void Awake() {

        aliveGO = transform.Find(TagConstants.ALIVE).gameObject;
        brokenTopGO = transform.Find(TagConstants.BROKEN_TOP).gameObject;
        brokenBotGO = transform.Find(TagConstants.BROKEN_BOT).gameObject;

        _rbAlive = aliveGO.GetComponent<Rigidbody2D>();
        _rbBrokenTop = brokenTopGO.GetComponent<Rigidbody2D>();

        combatDummyStats = GetComponent<StatsController>();
        _rbBrokenBot = brokenBotGO.GetComponent<Rigidbody2D>();

        aliveGO.SetActive(true);
        brokenTopGO.SetActive(false);
        brokenBotGO.SetActive(false);
    }

    private void Start() {
        CheckKnockback();
    }

    #endregion

    #region Damage Calulator
    public void Damage(AttackDetails attackDetails) {

        combatDummyStats.DecreaseCurrentHealth(attackDetails.damageAmount);

        if (attackDetails.position.x < aliveGO.transform.position.x) {
            playerFacingDirection = 1;
        } else {
            playerFacingDirection = -1;
        }

        /*//Intanitate hit particle
        var particle = HitParticlePool.Instance.GetFromPool(
            aliveGO.transform.position,
            Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f))
        );*/

        var particle = ParticleManager.Instance.SpawnObject(
            "DummyHitParticle",
            aliveGO.transform.position,
            Quaternion.Euler(0.0f, 0.0f, UnityEngine.Random.Range(0.0f, 360.0f))
        );


        if (playerFacingDirection == 1) {//1 mean left
            playerOnLeft = true;
        } else {
            playerOnLeft = false;
        }

        OnSetAnimatorBool?.Invoke(CombatDummyAnimationConstants.PlAYER_ON_LEFT, playerOnLeft);
        OnSetAnimatorTrigger?.Invoke(CombatDummyAnimationConstants.DAMAGE_TRIGGER);

        if (applyKnockback && combatDummyStats.CurrentHealth > 0) {
            //Knockback
            Knockback();
        }

        if (combatDummyStats.CurrentHealth <= 0) {
            //Die
            Die();
        }
    }

    #endregion

    #region Knockback
    private void Knockback() {
        isKnockback = true;
        knockbackStart = Time.time;

        _rbAlive.velocity = new Vector2(knockbackSpeedX * playerFacingDirection, knockbackSpeedY);

    }

    private void CheckKnockback() {
        if (Time.time >= knockbackStart + knockbackDuration && isKnockback) {
            isKnockback = false;
            _rbAlive.velocity = new Vector2(0.0f, _rbAlive.velocity.y);
        }
    }
    #endregion

    #region On Die
    private void Die() {
        aliveGO.SetActive(false);
        brokenTopGO.SetActive(true);
        brokenBotGO.SetActive(true);

        brokenTopGO.transform.position = aliveGO.transform.position;
        brokenBotGO.transform.position = aliveGO.transform.position;


        _rbBrokenBot.velocity = new Vector2(knockbackSpeedX * playerFacingDirection, knockbackSpeedY);
        _rbBrokenTop.velocity = new Vector2(knockbackDeathSpeedX * playerFacingDirection, knockbackDeathSpeedY);
        _rbBrokenTop.AddTorque(deathTorque * -playerFacingDirection, ForceMode2D.Impulse);
    }
    #endregion
}

