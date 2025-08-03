using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBarController : MonoBehaviour
{
    [SerializeField] private StatsController statsController;
    private HealthSystem healthSystem;
    private PlayerHealthBar healthBar;

    private void Awake() {
        healthSystem = new HealthSystem(statsController.Stats.MaxHealth);
        healthBar = GameObject.Find(GameObjectNameConstants.PLAYER_HEALTH_BAR).GetComponent<PlayerHealthBar>();

        statsController.OnHealthChanged += OnHealthChanged;
    }

    private void Start() {
        healthBar.Setup(healthSystem);
    }

    private void OnHealthChanged(object sender, StatsController.HealthChangedEventArgs e) {
        healthSystem.Damaged(e.ChangeAmount);
    }
}
