using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Enemy Data", menuName = "Enemy/Data")]
public class EnemyData : ScriptableObject
{
    public string enemyName;
    public string description;
    public float speed;
    public float shootFrequency;
    public Material enemyMaterial;
    public int enemyLife;
    /*[SerializeField] public int enemyLife { get; private set; }*/





}
