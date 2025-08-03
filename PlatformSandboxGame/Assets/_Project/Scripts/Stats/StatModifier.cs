using System;


public class IntStatModifier : StatModifier {
    readonly StatType type;
    readonly Func<int, int> operation;

    public IntStatModifier(StatType type, float duration, Func<int, int> operation) : base(duration) {
        this.type = type;
        this.operation = operation;
    }

    public override void Handle(object sender, object query) {
        if (query is Query<int> q && q.StatType == type) {
            q.Value = operation(q.Value.Value);
        }
    }
}

public class FloatStatModifier : StatModifier {
    readonly StatType type;
    readonly Func<float, float> operation;

    public FloatStatModifier(StatType type, float duration, Func<float, float> operation) : base(duration) {
        this.type = type;
        this.operation = operation;
    }

    public override void Handle(object sender, object query) {
        if (query is Query<float> q && q.StatType == type) {
            q.Value = operation(q.Value.Value);
        }
    }
}

public abstract class StatModifier : IDisposable {
    public bool MarkerForRemoval { get; private set; }
    public event Action<StatModifier> OnDispose = delegate { };

    readonly CountdownTimer timer;

    protected StatModifier(float duration) {
        if (duration <= 0) return;

        timer = new CountdownTimer(duration);
        timer.OnTimerStop += () => MarkerForRemoval = true;
        timer.Start();
    }

    public void Update(float deltaTime) => timer?.Tick(deltaTime);
    public abstract void Handle(object sender, object query);
    public void Dispose() => OnDispose?.Invoke(this);
}