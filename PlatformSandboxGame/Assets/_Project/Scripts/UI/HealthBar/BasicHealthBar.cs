using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class BasicHealthBar : MonoBehaviour {
    protected Image barImage, damagedBarImage;
    protected HealthSystem _healthSystem;

    [SerializeField] protected float shrinkHealthBarEffectSpeed;
    protected float damagedHealthFadeTimer;
    protected float damagedHealthShrinkTimer;

    [SerializeField] protected bool toggleHealthBarFadeEffect;
    [SerializeField] protected bool toggleHealthBarShrinkEffect;

    protected Color damagedBarColor;

    protected virtual void Awake() {
        barImage = transform.Find(UIConstants.BAR).GetComponent<Image>();
        damagedBarImage = transform.Find(UIConstants.DAMAMED_BAR).GetComponent<Image>();
        
        if (toggleHealthBarFadeEffect) {
            damagedBarColor = damagedBarImage.color;
            damagedBarColor.a = 0f;
            damagedBarImage.color = damagedBarColor;
        }
    }

    public virtual void Setup(HealthSystem healthSystem) {
        _healthSystem = healthSystem;
        SetHealth(_healthSystem.GetHealthNomalized());
        damagedBarImage.fillAmount = barImage.fillAmount;

        _healthSystem.OnDamaged += HealthSysten_OnDamaged;
        _healthSystem.OnHealed += HealthSystem_OnHealed;
    }

    protected virtual void Update() {
        if (toggleHealthBarFadeEffect) {
            HandleFadeHealthBarEffect();
        } else if (toggleHealthBarShrinkEffect) {
            HandleShrinkHealthBarEffect();
        }
    }

    protected virtual void HealthSystem_OnHealed(object sender, System.EventArgs e) {
        SetHealth(_healthSystem.GetHealthNomalized());
        if (toggleHealthBarFadeEffect) {
            // Handle heal effect for fade if needed
        } else if (toggleHealthBarShrinkEffect) {
            damagedBarImage.fillAmount = barImage.fillAmount;
        }
    }

    protected void SetHealth(float healthNormalized) {
        barImage.fillAmount = healthNormalized;
    }

    protected virtual void HealthSysten_OnDamaged(object sender, System.EventArgs e) {
        if (toggleHealthBarFadeEffect) {
            SetupFadeHealthBarEffect();
        } else if (toggleHealthBarShrinkEffect) {
            SetupShrinkHealthBarEffect();
        }
        SetHealth(_healthSystem.GetHealthNomalized());
    }

    protected virtual void SetupFadeHealthBarEffect() {
        if (damagedBarColor.a <= 0) {
            damagedBarImage.fillAmount = barImage.fillAmount;
        }
        damagedBarColor.a = 1;
        damagedBarImage.color = damagedBarColor;
        damagedHealthFadeTimer = UIConstants.DAMAGED_HEALTH_FADE_TIMER_MAX;
    }

    protected virtual void SetupShrinkHealthBarEffect() {
        damagedHealthShrinkTimer = UIConstants.DAMAGED_HEALTH_SHRINK_TIMER_MAX;
    }

    protected virtual void HandleFadeHealthBarEffect() {
        if (damagedBarColor.a > 0f) {
            damagedHealthFadeTimer -= Time.deltaTime;
            if (damagedHealthFadeTimer < 0) {
                float fadeAmount = 5f;
                damagedBarColor.a -= fadeAmount * Time.deltaTime;
                damagedBarImage.color = damagedBarColor;
            }
        }
    }

    protected virtual void HandleShrinkHealthBarEffect() {
        damagedHealthShrinkTimer -= Time.deltaTime;
        if (damagedHealthShrinkTimer < 0) {
            if (barImage.fillAmount < damagedBarImage.fillAmount) {
                damagedBarImage.fillAmount -= shrinkHealthBarEffectSpeed * Time.deltaTime;
            }
        }
    }
}