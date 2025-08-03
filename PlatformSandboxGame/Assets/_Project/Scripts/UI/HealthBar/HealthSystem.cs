using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem
{
    public event EventHandler OnDamaged;
    public event EventHandler OnHealed;

    private int healthAmount;
    private int healthAmountMax;

    public HealthSystem(int healthAmount) {
        this.healthAmount = healthAmount;
        healthAmountMax = healthAmount;
    }

    public void Damaged(int amount) {
        healthAmount -= amount;
        if (healthAmount < 0) {
            healthAmount = 0;
        }
        OnDamaged?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(int amount) {
        healthAmount += amount;

        if (healthAmount > healthAmountMax) {
            healthAmount = healthAmountMax;
        }
        OnHealed?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNomalized() {
        return (float)healthAmount / healthAmountMax;
    }

    public int GetCurrentHealthAmount() {
        return healthAmount;
    }

    public int GetHealthAmountMax() {
        return healthAmountMax;
    }
}
