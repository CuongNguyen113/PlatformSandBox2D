using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseStatsSO", menuName = "Stats/BaseStatsSO")]
public class BaseStatsSO : ScriptableObject {
    public int attack;
    public int maxHealth;
    public float moveSpeed;
}
