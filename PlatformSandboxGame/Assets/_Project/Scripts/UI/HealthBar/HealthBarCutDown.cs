using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarCutDown : MonoBehaviour
{
    private RectTransform rectTransform;

    private float fallCutEffectSpeed = UIConstants.FALL_CUT_EFFECT_SPEED;
    private float alphaFadeCutEffectSpeed = UIConstants.FADE_CUT_EFFECT_SPEED;
    private float fallDownTimer;
    private float fadeTimer;
    private Image image;    
    private Color color;

    private void Awake() {
        rectTransform = transform.GetComponent<RectTransform>();
        image = transform.GetComponent<Image>();
        color = image.color;
        fallDownTimer = 0.3f;
        fadeTimer = 0.1f;
    }

    private void Update() {
        fallDownTimer -= Time.deltaTime;
        if (fallDownTimer < 0f) {
            rectTransform.anchoredPosition += Vector2.down * fallCutEffectSpeed * Time.deltaTime;

            fadeTimer -= Time.deltaTime;
            if (fadeTimer < 0) {
                color.a -= alphaFadeCutEffectSpeed * Time.deltaTime;
                image.color = color;

                if (color.a < 0) {
                    Destroy(gameObject);
                }
            }
        }
    }
}
