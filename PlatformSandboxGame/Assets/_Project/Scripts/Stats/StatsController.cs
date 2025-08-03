using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;


[DefaultExecutionOrder(-2)]
public class StatsController : MonoBehaviour {
    [SerializeField] private BaseStatsSO baseStats;

    public event EventHandler<HealthChangedEventArgs> OnHealthChanged;
    public Stats Stats {  get; private set; }
    public int CurrentHealth { get; private set; }
    public int CurrentStunResistance {  get; private set; }


    public class HealthChangedEventArgs : EventArgs {
        public int CurrentHealth { get; }
        public int MaxHealth { get; }
        public int ChangeAmount { get; }

        public HealthChangedEventArgs(int currentHealth, int maxHealth, int changeAmount) {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
            ChangeAmount = changeAmount;
        }
    }

    protected virtual void Awake() {
        Stats = new Stats(new StatsMediator(), baseStats);
    }

    protected virtual void Start() {
        CurrentHealth = Stats.MaxHealth;
    }

    protected virtual void Update() {
        Stats.Mediator.Update(Time.deltaTime);
    }

    public virtual void DecreaseCurrentHealth(int amount) {
        CurrentHealth -= amount;
        OnHealthChanged?.Invoke(this, new HealthChangedEventArgs(CurrentHealth, Stats.MaxHealth, amount));
    }

    public virtual float GetMoveSpeed() {
        return Stats.MoveSpeed;
    }

    public virtual int GetAttack() {
        return Stats.Attack;
    }

    public virtual bool IsDie() {
        return CurrentHealth <= 0;
    }

    public virtual void ResetHealth() {
        CurrentHealth = Stats.MaxHealth;
    }
}
