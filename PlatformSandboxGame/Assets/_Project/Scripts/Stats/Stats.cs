using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType {
    Attack,
    MaxHealth,
    StunResistance,
    StunRecoverTime,
    MoveSpeed
}

// Generic stat value container
public struct StatValue<T> where T : struct {
    public T Value;

    public StatValue(T value) {
        Value = value;
    }

    public static implicit operator StatValue<T>(T value) => new StatValue<T>(value);
    public static implicit operator T(StatValue<T> statValue) => statValue.Value;
}

public class Stats {
    protected readonly StatsMediator mediator;
    protected readonly BaseStatsSO baseStats;

    public StatsMediator Mediator => mediator;

    public StatValue<int> Attack => GetStat<int>(StatType.Attack, baseStats.attack);
    public StatValue<int> MaxHealth => GetStat<int>(StatType.MaxHealth, baseStats.maxHealth);
    public StatValue<float> MoveSpeed => GetStat<float>(StatType.MoveSpeed, baseStats.moveSpeed);

    public Stats(StatsMediator mediator, BaseStatsSO baseStats) {
        this.mediator = mediator;
        this.baseStats = baseStats;
    }

    protected StatValue<T> GetStat<T>(StatType statType, T baseValue) where T : struct {
        var q = new Query<T>(statType, baseValue);
        mediator.PerformQuery(this, q);
        return q.Value;
    }


}



