using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;


public class turret : MonoBehaviour
{
    public float timer = 10f;
    private float shootTimer;

    public Rigidbody2D rb;
    public GameObject bulletPrefab;
    public Transform weaponBarrel;

    void Start()
    {
        shootTimer = timer;
    }
    void FixedUpdate()
    {
        shootTimer--; 

        if (shootTimer <= 0)
        {
            FireWeapon();
            shootTimer = Random.Range(timer, 90f);
        }
    }
    public void FireWeapon()
    {
        //shoots prefab, tracks position and rotation of the tip of the weapon
        GameObject bullet = Instantiate(bulletPrefab, weaponBarrel.position, weaponBarrel.rotation);
    }
}
