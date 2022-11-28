using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ControlPlayer : MonoBehaviour
{ [Header("Movement")]
    public int maxLife;
    public int currentLife;
    public float speed = 2f;
    public float jumpForce;
    /*float gravity = -9.81f;*/

    [Header("Camera")]
    public float mouseSensibility;
    public float maxViewX, minViewX;
    float rotationX;
    [SerializeField]private Camera CameraPlayer;
    private Animator animator;
    [SerializeField] private ZapController zap;
    [SerializeField] private FishBonesController fishBones;
    [SerializeField] private GameObject handR;
    [SerializeField] private GameObject EnergyBar;
    GameObject targetWeapon;
    GameObject ZapWeapon, FishBonesWeapon, BombWeapon;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        FishBonesWeapon = GameObject.Find("/Player/CameraPlayer/handR/FishBones");
        ZapWeapon = GameObject.Find("/Player/CameraPlayer/handR/Zap");
        BombWeapon = GameObject.Find("/Player/CameraPlayer/handR/Bomb");
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        CameraPlayer = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        CameraView();
        if (Input.GetButtonDown("Jump")) Jump();

        if (Input.GetButton("Fire1"))
        {
            /*if (zap.canShoot() && targetWeapon == FishBonesWeapon)*/
            if (zap.canShoot() && targetWeapon == ZapWeapon)
            {
                zap.Shoot();
            }
            if (fishBones.canShoot() && targetWeapon == FishBonesWeapon)
            {
                fishBones.Shoot();
            }
        }

        if (Input.GetButton("Fire2") && targetWeapon == ZapWeapon)
        {
            zap = FindObjectOfType(typeof(ZapController)) as ZapController;
            handR.transform.localPosition = new Vector3(-0.211999997f, -0.0149999997f, 0f);

        }
        else if(Input.GetButtonUp("Fire2") && targetWeapon == ZapWeapon) handR.transform.localPosition = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            handR.transform.localPosition = new Vector3(0.107000001f, 0f, 0.100000001f);
            targetWeapon = FishBonesWeapon;
            ChangeWeapon(/*FishBonesWeapon*/);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            targetWeapon = ZapWeapon;
            handR.transform.localPosition = Vector3.zero;
            ChangeWeapon(/*ZapWeapon*/);
            EnergyBar.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            targetWeapon = BombWeapon;
            handR.transform.localPosition = Vector3.zero;
            ChangeWeapon(/*BombWeapon*/);
        }

        Cursor.lockState = CursorLockMode.Locked;


    }

    private void ChangeWeapon(/*GameObject targetWeapon*/)
    {
        FishBonesWeapon.SetActive(false);
        ZapWeapon.SetActive(false);
        BombWeapon.SetActive(false);
        EnergyBar.SetActive(false);

        targetWeapon.SetActive(true);
    }

    private void Jump()
    {
        Ray ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, 1.1f))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void CameraView()
    {
        if (!GameManager.instance.gamePaused)
        {
        //get input x & y axis from mouse
        float y = Input.GetAxis("Mouse X") * mouseSensibility;
        rotationX += Input.GetAxis("Mouse Y") * mouseSensibility;

        // set the rotation limits
        rotationX = Mathf.Clamp(rotationX, minViewX, maxViewX);

        //rotate the camera
        CameraPlayer.transform.localRotation = Quaternion.Euler(-rotationX, 0, 0);
        //rotate the player
        transform.eulerAngles += Vector3.up * y;
        }
    }

    /// <summary>
    /// Player movement input controller
    /// </summary>
    private void MovePlayer()
    {
        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;

        Vector3 direction = transform.right * x + transform.forward * z;
        direction.y = rb.velocity.y;

        rb.velocity = direction;

        direction = new Vector3(direction.x, 0, direction.z);
        animator.SetFloat("velocity", direction.magnitude);

    }

    ///
    public void DamagePlayer(int quantity)
    {

        currentLife -= quantity;

        HUDController.instance.UpdateHealthBar(currentLife, maxLife);

        if (currentLife <= 0)
        {
            Debug.Log("Game Over");
            animator.SetTrigger("death");
            handR.SetActive(false);
        }
            
        
    }
   
}
