using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntitySO", menuName ="Scriptable Object/Entity SO/Base SO")]
public class EntitySO : ScriptableObject {
    public float attackHop = 10f;

    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 0.4f;
    public float groundCheckRadius = 0.3f;
    public float wallBehindCheckDistance = 1f;
    public float obstructedViewRadius = 1f;


    public float maxAgroDistance = 4f;
    public float minAgroDistance = 3f;

    public float closeRangeActionDistance = 1f;

    public ParticleData hitParticleData;

    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;
}
