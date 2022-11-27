using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private Zap zap;
    [SerializeField] private GameObject handR;

    

    
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        CameraPlayer = Camera.main;
        zap = FindObjectOfType(typeof(Zap)) as Zap;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        CameraView();
        if (Input.GetButtonDown("Jump")) Jump();

        if (Input.GetButton("Fire1"))
        {
            
            if (zap.canShoot())
            {
                zap.Shoot();
            }
        }

        if (Input.GetButton("Fire2"))
        {
            zap.transform.localPosition = new Vector3(-0.170000002f, -0.188999996f, 0.238000005f);

        }
        else zap.transform.localPosition = new Vector3(0.0399000011f, -0.188999996f, 0.238000005f);

        Cursor.lockState = CursorLockMode.Locked;


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
