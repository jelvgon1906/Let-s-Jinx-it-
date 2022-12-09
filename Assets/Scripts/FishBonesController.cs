using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBonesController : MonoBehaviour
{
    public Transform outPosition;
    public int currentAmmo;
    public int maxAmmo;
    public bool infiniteAmmo;

    public float ballSpeed;
    public float shotFrequency;

    private ObjectPool objectPool;
    private float lastShootTime;
    public bool isPlayer;

    private void Awake()
    {
        //if I am the Player
        if (GetComponent<ControlPlayer>())
            isPlayer = true;
        /*else if (GetComponent<ControlEnemy>())
        {
            shortFrequency = controlEnemy.EnemyData.shootFrequency;
        }*/

        objectPool = GetComponent<ObjectPool>();
        
    }
    private void LateUpdate()
    {
        HUDController.instance.UpdateAmmo(currentAmmo, maxAmmo);
    }
    private void OnEnable()
    {
        HUDController.instance.UpdateAmmo(currentAmmo, maxAmmo);
    }

    public bool canShoot()
    {
        if ((Time.time - lastShootTime >= shotFrequency) && !GameManager.instance.gamePaused)
        {
            if (currentAmmo > 0 || infiniteAmmo) 
                return true;
        }
        return false;
    }
    public void AddAmmo(int ammoQuantity)
    {
        currentAmmo += ammoQuantity;

        if (currentAmmo >= maxAmmo) currentAmmo = maxAmmo;

        HUDController.instance.UpdateAmmo(currentAmmo, maxAmmo);
    }

    public void Shoot()
    {
        gameObject.SetActive(false);
        lastShootTime = Time.time;

        currentAmmo--;

        GameObject ball = objectPool.GetGameObject();

        ball.transform.position = outPosition.position;
        ball.transform.rotation = outPosition.rotation;
        ball.transform.localRotation = outPosition.rotation;

        ball.GetComponent<Bullet>().isPlayer = isPlayer;


        ball.GetComponent<Rigidbody>().velocity = outPosition.forward * ballSpeed;
        gameObject.SetActive(true);

        HUDController.instance.UpdateAmmo(currentAmmo, maxAmmo);
    }

    

}
