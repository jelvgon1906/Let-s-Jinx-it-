using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using System;

public class Zap : MonoBehaviour
{
    [SerializeField]private Transform outPosition;
    public float currentAmmo;
    public int maxAmmo;
    public bool infiniteAmmo;

    public float ballSpeed;
    public float shotFrequency;

    [SerializeField] private ObjectPool objectPool;
    private float lastShootTime;
    public bool isPlayer;
    private ControlEnemy controlEnemy;
    [SerializeField] private Camera CameraPlayer;

    public TextMeshProUGUI ammunitionText;
    public Image currentEnergyBar;
    public float RechargeVelocity = 0.1f;
    public float aimZone = 50f;

    

    private void Awake()
    {
        //if I am the Player
        if (GetComponent<ControlPlayer>())
            isPlayer = true;
        /*else if (GetComponent<ControlEnemy>())
        {
            shortFrequency = controlEnemy.EnemyData.shootFrequency;
        }
*/

        ammunitionText.text = currentAmmo.ToString("00") + "/" + maxAmmo.ToString("00");

        objectPool = GetComponent<ObjectPool>();
    }

    public bool canShoot()
    {
        if ((Time.time - lastShootTime >= shotFrequency) && !GameManager.instance.gamePaused)
        {
            if (currentAmmo >= 1 || infiniteAmmo) 
                return true;
        }
        return false;
    }

    private void Update()
    {
        Vector3 aimSpot = CameraPlayer.transform.position;
        //You will want to play around with the 50 to make it feel accurate.
        aimSpot += CameraPlayer.transform.forward * aimZone;
        transform.LookAt(aimSpot);

        if (currentAmmo >= maxAmmo || infiniteAmmo)
        {
            currentAmmo = maxAmmo;
            currentEnergyBar.fillAmount = (float)currentAmmo / (float)maxAmmo;
            currentEnergyBar.color = Color.yellow;
        }else Recharge();
    }

    void Recharge()
    {
        currentAmmo = (float)currentAmmo + RechargeVelocity * maxAmmo * Time.deltaTime;
        currentEnergyBar.fillAmount = (float)currentAmmo / (float)maxAmmo;
        
    }

    public void Shoot()
    {

        lastShootTime = Time.time;
        currentAmmo--;
        currentEnergyBar.color = Color.blue;

        currentEnergyBar.fillAmount = (float)currentAmmo / (float)maxAmmo;
        ammunitionText.text = currentAmmo.ToString("00") + "/" + maxAmmo.ToString("00");

       
        GameObject ammo = objectPool.GetGameObject();

        ammo.transform.position = outPosition.position;
        ammo.transform.rotation = outPosition.rotation;
        ammo.transform.localRotation = outPosition.rotation;



        ammo.GetComponent<Rigidbody>().velocity = outPosition.forward * ballSpeed;
        gameObject.SetActive(true);

        /*GameObject ammo1 = objectPool1.GetGameObject();

        ammo1.transform.position = outPosition.position;
        ammo1.transform.rotation = outPosition.rotation;
        ammo1.transform.localRotation = outPosition.rotation;



        ammo1.GetComponent<Rigidbody>().velocity = outPosition.forward * ballSpeed;
        gameObject.SetActive(true);*/

        
    }

}
