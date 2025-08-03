using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : BasicHealthBar {
    private TextMeshProUGUI healthBarValueText;
    private Transform damagedBarTemplate;
    private float beforeDamagedBarFillAmount;

    [SerializeField] private bool toggleHealthBarCutEffect;
    [SerializeField] private bool toggleHealthPointText;

    protected override void Awake() {
        base.Awake();
        healthBarValueText = transform.Find(UIConstants.HEALTH_VALUE_TEXT).GetComponent<TextMeshProUGUI>();
        damagedBarTemplate = transform.Find(UIConstants.DAMAGED_BAR_TEMPLATE);
    }

    public override void Setup(HealthSystem healthSystem) {
        base.Setup(healthSystem);
        if (toggleHealthPointText) {
            UpdateHealthText();
        }
    }

    protected override void Update() {
        base.Update();

        if (toggleHealthPointText) {
            UpdateHealthText();
        }
    }

    private void UpdateHealthText() {
        int currentHealthAmount = _healthSystem.GetCurrentHealthAmount();
        int healthAmountMax = _healthSystem.GetHealthAmountMax();
        healthBarValueText.text = currentHealthAmount + "/" + healthAmountMax;
    }

    protected override void HealthSysten_OnDamaged(object sender, System.EventArgs e) {
        if (toggleHealthBarCutEffect) {
            beforeDamagedBarFillAmount = barImage.fillAmount;
        }

        base.HealthSysten_OnDamaged(sender, e);

        if (toggleHealthBarCutEffect) {
            SetupCutHealthBarEffect();
        }
    }

    private void SetupCutHealthBarEffect() {
        Transform damagedBar = Instantiate(damagedBarTemplate, transform);
        damagedBar.gameObject.SetActive(true);
        damagedBar.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            barImage.fillAmount * UIConstants.PLAYER_BAR_WIDTH,
            damagedBar.GetComponent<RectTransform>().anchoredPosition.y);
        damagedBar.GetComponent<Image>().fillAmount = beforeDamagedBarFillAmount - barImage.fillAmount;
        damagedBar.gameObject.AddComponent<HealthBarCutDown>();
    }
}