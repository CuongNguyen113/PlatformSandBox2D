using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBarController : MonoBehaviour {
    private StatsController statsController;
    private HealthSystem healthSystem;
    private BasicHealthBar healthBar;
    [SerializeField] private Transform pfHealthBar;
    [SerializeField] private Transform canvas;
    private void Awake() {
        statsController = GetComponent<StatsController>();
        healthSystem = new HealthSystem(statsController.Stats.MaxHealth);

        Transform healthBarTransform = Instantiate(pfHealthBar, canvas.transform);
        RectTransform rectTransform = healthBarTransform.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(0, 10);

        healthBar = healthBarTransform.GetComponent<BasicHealthBar>();
        statsController.OnHealthChanged += OnHealthChanged;


    }

    private void Start() {
        healthBar.Setup(healthSystem);
    }

    private void OnHealthChanged(object sender, StatsController.HealthChangedEventArgs e) {
        healthSystem.Damaged(e.ChangeAmount);
    }

    private void LateUpdate() {
        healthBar.transform.rotation = Quaternion.identity;
    }
}
