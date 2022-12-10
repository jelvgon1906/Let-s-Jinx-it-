using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;
using Cursor = UnityEngine.Cursor;

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
    [SerializeField] private PowPowController powPow;
    [SerializeField] private GameObject hands;
    [SerializeField] private GameObject handRight;
    [SerializeField] private GameObject EnergyBar;
    [SerializeField] private GameObject textAmmo;
    [SerializeField] private GameObject Cannons;
    GameObject targetWeapon;
    GameObject ZapWeapon, FishBonesWeapon, PowPowWeapon, BombWeapon;
    public AudioSource audioZap, audioBomb, audioCannon;
    Rigidbody rb;

    [SerializeField] private float cannonRotationSpeed;
    [SerializeField] private bool rotateCanyon;
    float x = 0;
    private bool pause;
    public static ControlPlayer instance;
    [SerializeField] private GameObject endGame; 
    

    // Start is called before the first frame update
    void Start()
    {
        FishBonesWeapon = GameObject.Find("/Game/Player/CameraPlayer/hands/FishBones");
        ZapWeapon = GameObject.Find("/Game/Player/CameraPlayer/hands/Zap");
        PowPowWeapon = GameObject.Find("/Game/Player/CameraPlayer/hands/PowPow");
        /*Cannons = GameObject.Find("/Game/Player/CameraPlayer/hands/PowPow/Jinx_PowPow/Cannons");*/
        BombWeapon = GameObject.Find("/Game/Player/CameraPlayer/hands/Bomb");
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        CameraPlayer = Camera.main;

        
        handRight.SetActive(true);
        targetWeapon = FishBonesWeapon;
        ChangeWeapon();
        textAmmo.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        CameraView();
        if (Input.GetButtonUp("Cancel"))
        {
            if (pause) pause = false;
            else if (!pause) pause = true;
        }
        if (Input.GetButtonDown("Jump")) Jump();

        if (Input.GetButton("Fire1") && !pause)
        {
            Cursor.lockState = CursorLockMode.Locked;
            /*if (zap.canShoot() && targetWeapon == FishBonesWeapon)*/
            if (zap.canShoot() && targetWeapon == ZapWeapon)
            {
                zap.Shoot();
                audioZap.Play();
            }
            if (fishBones.canShoot() && targetWeapon == FishBonesWeapon)
            {
                fishBones.Shoot();
                audioBomb.Play();
            }
            if (powPow.canShoot() && targetWeapon == PowPowWeapon)
            {
                powPow.Shoot();
                audioCannon.Play();
            }
            rotateCanyon = true;
        }else rotateCanyon = false;

        if (rotateCanyon && !pause) cannonRotation();

        if (Input.GetButton("Fire2") && targetWeapon == ZapWeapon)
        {
            zap = FindObjectOfType(typeof(ZapController)) as ZapController;
            hands.transform.localPosition = new Vector3(-0.211999997f, -0.0149999997f, 0f);

        }
        else if(Input.GetButtonUp("Fire2") && targetWeapon == ZapWeapon) hands.transform.localPosition = Vector3.zero;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            handRight.SetActive(true);
            hands.transform.localPosition = new Vector3(0.107000001f, 0f, 0.100000001f);
            targetWeapon = FishBonesWeapon;
            ChangeWeapon(/*FishBonesWeapon*/);
            textAmmo.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            handRight.SetActive(false);
            targetWeapon = PowPowWeapon;
            ChangeWeapon();
            textAmmo.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            handRight.SetActive(true);
            targetWeapon = ZapWeapon;
            hands.transform.localPosition = Vector3.zero;
            ChangeWeapon(/*ZapWeapon*/);
            EnergyBar.SetActive(true);

        }
        /*if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            handRight.SetActive(true);
            targetWeapon = BombWeapon;
            hands.transform.localPosition = Vector3.zero;
            ChangeWeapon(*//*BombWeapon*//*);
        }*/
    }

    private void ChangeWeapon(/*GameObject targetWeapon*/)
    {
        FishBonesWeapon.SetActive(false);
        ZapWeapon.SetActive(false);
        BombWeapon.SetActive(false);
        EnergyBar.SetActive(false);
        textAmmo.SetActive(false);
        PowPowWeapon.SetActive(false);

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
            hands.SetActive(false);
            Invoke(nameof(GameOver), 1.5f);
        }else if(currentLife > maxLife)
        {
            currentLife = maxLife;
        }
            
        
    }

    void GameOver()
    {
        pause = true;
        endGame.SetActive(true);
        
        GameManager.instance.EndGame();
    }

    private void cannonRotation()
    {

        x += cannonRotationSpeed;

        if (x > 360.0f)
        {
            x = 0.0f;
        }
        /*Cannons.transform.localEulerAngles*/
        Cannons.transform.localRotation =  Quaternion.Euler(x, -90, 90);
        /*Cannons.transform.Rotate(new Vector3(x, Cannons.transform.rotation.y, Cannons.transform.rotation.z), Space.Self);*/

        /*Cannons.transform.RotateAround(Pivot.transform.position, Vector3.right, 1f);*/

    }

}
