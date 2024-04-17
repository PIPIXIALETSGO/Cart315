using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName="WeaponScriptableObject",menuName="ScriptableObject/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject prefab;
    public GameObject Prefab{get =>prefab;private set=>prefab=value;}
    [SerializeField]
    float damage;
    public float Damage{get =>damage;private set=>damage=value;}
    [SerializeField]
    float speed;
    public float Speed{get =>speed;private set=>speed=value;}
    [SerializeField]
     float cooldownDuration;
    public float CooldownDuration{get =>cooldownDuration;private set=>cooldownDuration=value;}
    [SerializeField]
    int pierce;
    public int Pierce{get =>pierce;private set=>pierce=value;}
    [SerializeField]
    int level;
    public int Level { get => level; private set => level = value; }
    [SerializeField]
    GameObject nextLevelPrefab ;
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }
    [SerializeField]
    new string name;
    public string Name { get => name; private set => name = value; }
    [SerializeField]
    string description;
    public string Description { get => description; private set => description = value; }
    [SerializeField]
    Sprite icon;
    public Sprite Icon { get => icon; private set => icon = value; }
    [SerializeField]
    int numberOfBullet;
    public int NumberOfBullet { get => numberOfBullet; private set => numberOfBullet = value; }

    [SerializeField]
    float spreadAngle;
    public float SpreadAngle { get => spreadAngle; private set => spreadAngle = value; }
}