using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowPowController : MonoBehaviour
{
    public Transform outPosition;
    public Transform outPosition1;
    public Transform outPosition2;
    public int currentAmmo;
    public int maxAmmo;
    public bool infiniteAmmo;

    public float ballSpeed;
    public float shotFrequency;

    private ObjectPool objectPool;
    private float lastShootTime;
    public bool isPlayer;
    private ControlEnemy controlEnemy;

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


        objectPool = GetComponent<ObjectPool>();
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

    public void Shoot()
    {
        lastShootTime = Time.time;

        currentAmmo--;

        GameObject ball = objectPool.GetGameObject();
        /*ball.GetComponent<TrailRenderer>().time = 0;*/

        ball.transform.position = outPosition2.position;
        ball.transform.rotation = outPosition2.rotation;
        ball.transform.localRotation = outPosition2.rotation;

        ball.GetComponent<Bullet>().isPlayer = isPlayer;


        ball.GetComponent<Rigidbody>().velocity = outPosition2.forward * ballSpeed;
        /*ball.GetComponent<TrailRenderer>().time = 0.1f;*/
        HUDController.instance.UpdateAmmo(currentAmmo, maxAmmo);
        StartCoroutine(Shoot2());
    }

    private IEnumerator Shoot2()
    {
        yield return new WaitForSeconds(0.2f);
        currentAmmo--;

        GameObject ball = objectPool.GetGameObject();

        ball.transform.position = outPosition2.position;
        ball.transform.rotation = outPosition2.rotation;
        ball.transform.localRotation = outPosition2.rotation;

        ball.GetComponent<Bullet>().isPlayer = isPlayer;


        ball.GetComponent<Rigidbody>().velocity = outPosition2.forward * ballSpeed;
        HUDController.instance.UpdateAmmo(currentAmmo, maxAmmo);
        StartCoroutine(Shoot3());
    }
    private IEnumerator Shoot3()
    {
        yield return new WaitForSeconds(0.2f);
        currentAmmo--;

        GameObject ball = objectPool.GetGameObject();

        ball.transform.position = outPosition2.position;
        ball.transform.rotation = outPosition2.rotation;
        ball.transform.localRotation = outPosition2.rotation;

        ball.GetComponent<Bullet>().isPlayer = isPlayer;


        ball.GetComponent<Rigidbody>().velocity = outPosition2.forward * ballSpeed;

        HUDController.instance.UpdateAmmo(currentAmmo, maxAmmo);
    }


}
