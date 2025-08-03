using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerAfterImageSprite : MonoBehaviour
{

    private Transform player;
    private SpriteRenderer SR;
    private SpriteRenderer playerSR;
    public const string PLAYER = "Player";

    //Time to active
    private float timeActivated;
    private float alpha;
    //Time work
    [SerializeField] private float activeTime = 0.1f;
    [SerializeField] private float alphaDefault = 0.8f;
    [SerializeField] private float alphaDecay = 0.85f;

    private Color color;

    private void OnEnable() {
        SR = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag(PLAYER).transform;
        playerSR = player.GetComponent<SpriteRenderer>();

        alpha = alphaDefault;
        SR.sprite = playerSR.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;

        timeActivated = Time.time;

    }


    private void Update() {
        alpha -= alphaDecay * Time.deltaTime;
        color = new Color(1f, 1f, 1f, alpha);
        SR.color = color;
        //Return object to pool after 0.1s(activeTime)
        if (Time.time >= (timeActivated + activeTime)) {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
