using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Sword, Range };
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider2D meleeArea;
    Animator Animator;
    //public TrailRenderer trailEffect;

    public void Use()
    {
        if (type == Type.Sword)
        {

        }
    }

    // Update is called once per frame
}
