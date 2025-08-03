using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsMediator {
    readonly LinkedList<StatModifier> modifiers = new();

    public event EventHandler<object> Queries;
    public void PerformQuery(object sender, object query) => Queries?.Invoke(sender, query);

    public void AddModifier(StatModifier modifier) {
        modifiers.AddLast(modifier);
        Queries += modifier.Handle;

        modifier.OnDispose += _ => {
            modifiers.Remove(modifier);
            Queries -= modifier.Handle;
        };
    }

    public void Update(float deltaTime) {
        // Update all modifiers with deltaTime
        var node = modifiers.First;
        while (node != null) {
            node.Value.Update(deltaTime);
            node = node.Next;
        }

        // Dispose any that are finished (mark and sweep)
        node = modifiers.First;
        while (node != null) {
            var nextNode = node.Next;
            if (node.Value.MarkerForRemoval) {
                node.Value.Dispose();
            }
            node = nextNode;
        }
    }
}


public class Query<T> where T : struct {
    public readonly StatType StatType;
    public StatValue<T> Value;

    public Query(StatType statType, T value) {
        StatType = statType;
        Value = value;
    }
}
