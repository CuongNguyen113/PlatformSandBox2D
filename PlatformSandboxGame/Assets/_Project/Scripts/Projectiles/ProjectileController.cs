using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private AttackDetails attackDetails;

    private float speed;
    private float travelDistance;
    private float xStartPosition;

    [SerializeField] private float gravity;
    [SerializeField] private float damageRadius;

    private bool isGravityOn;
    private bool hasHitGround;

    [SerializeField] LayerMask whatIsGround;
    [SerializeField] LayerMask whatIsPlayer;
    [SerializeField] Transform damagePosition;

    private Rigidbody2D _rb;

    private void Start() {
        _rb = GetComponent<Rigidbody2D>();

        _rb.gravityScale = 0.0f;
        _rb.velocity = transform.right * speed;

        xStartPosition = transform.position.x;

        isGravityOn = false;

    }

    private void Update() {
        if (!hasHitGround) {

            attackDetails.position = damagePosition.position;

            if (isGravityOn) {
                float angle = Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);//rotate around the arrow facing to screen(blue arrow when u move an object in 3D mode)
            } 
        }
    }

    private void FixedUpdate() {

        if (!hasHitGround) {

            Collider2D[] damageHit = Physics2D.OverlapCircleAll(damagePosition.position, damageRadius, whatIsPlayer);
            Collider2D groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);

            foreach (Collider2D hit in damageHit) {
                
                if (hit.TryGetComponent<IDamageable>(out var damageable)) {
                    damageable.Damage(attackDetails.damageAmount);
                    Destroy(gameObject);
                }
            }

            if (groundHit) {
                hasHitGround = true;
                _rb.gravityScale = 0f;
                _rb.velocity = Vector2.zero;
            }

            if (Mathf.Abs(xStartPosition - transform.position.x) >= travelDistance && !isGravityOn) {
                isGravityOn = true;
                _rb.gravityScale = gravity;
            }
        }
    }

    public void FireProjectile(float speed, float travelDistance, int damage) {
        this.speed = speed;
        this.travelDistance = travelDistance;
        attackDetails.damageAmount = damage;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }
}
