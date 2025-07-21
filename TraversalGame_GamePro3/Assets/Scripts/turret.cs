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
    public float projectileSpeed;

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
            //Shoot1();
            shootTimer = Random.Range(timer, 90f);
        }
    }
    public void FireWeapon()
    {
        //shoots prefab, tracks position and rotation of the tip of the weapon
        
        GameObject bullet = Instantiate(bulletPrefab, weaponBarrel.position, weaponBarrel.rotation);
    }
    void Shoot()
    {
        // Instantiate the bullet at the fire point
        GameObject bullet = Instantiate(bulletPrefab, weaponBarrel.position, weaponBarrel.rotation);

        // Get the Rigidbody component of the bullet
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        // Apply velocity to the bullet
        if (rb != null)
        {
            rb.linearVelocity = weaponBarrel.forward * projectileSpeed;
        }
    }

    void Shoot1()
    {
        GameObject projectile = Instantiate(bulletPrefab, weaponBarrel.position, weaponBarrel.rotation);
        bullet projectileScript = projectile.GetComponent<bullet>();
        if (projectileScript != null)
        {
            projectileScript.Launch(projectileSpeed);
        }

    }
}
