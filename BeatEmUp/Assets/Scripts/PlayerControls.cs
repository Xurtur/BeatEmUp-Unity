using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;

public class CameraRotation : MonoBehaviour
{
    [Header("References")]
    public Transform Orientation;
    public Transform Player;
    public Transform PlayerObj;
    public Rigidbody rb;
    [Header("States")]
    [SerializeField] public bool IsGrounded;
    [SerializeField] private Animator animator;
    float KbX;
    float KbY;
    public float Sens;
    public float Speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        IsGrounded = true;
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        //allows jump if grounded
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            animator.SetBool("Jump", true);
        }

    }

    // FixedUpdate for frame independent calls
    void FixedUpdate()
    {
        Vector3 CamRotation = Player.position - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Orientation.forward = CamRotation.normalized;

        KbX = Input.GetAxisRaw("Horizontal");
        KbY = Input.GetAxisRaw("Vertical");


        //get camera normal axis without rotation
        //transform.forward sucks ill make my own .forward
        //im a fucking genius
        var CameraForward = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z);
        var CameraSideward = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z);

        //rotate direction with wasd
        Vector3 InputDir = CameraForward * KbY + CameraSideward * KbX;

        rb.AddForce(rb.mass * (3 - 1) * Physics.gravity);

        //player movement relative to camera forward
        if (InputDir != Vector3.zero)
        {
            var NewForward = new Vector3(0, 0, InputDir.z);

            //player orientation that is detached from camera, rotates with input, makes rotation relative to camera
            Orientation.forward = Vector3.Slerp(Orientation.forward, NewForward, Time.deltaTime * Sens);

            //movement orientation, forward is where camera is facing, makes movement relative to camera
            Player.forward = Vector3.Slerp(Player.forward, InputDir.normalized, Time.deltaTime * Sens);

            //sprint and walk 
            rb.AddForce(Player.forward * Speed);
            var SpeedTime = .5f / Time.deltaTime;

            if (animator.GetFloat("WalkX") >= .5f)
            {
                Speed = Mathf.Lerp(Speed, 60, SpeedTime);
            }
            else if (animator.GetFloat("WalkX") <= .4f)
            {
                Speed = Mathf.Lerp(Speed, 30, SpeedTime);
            }

        }
    }

    private void Jump()
    {
        if (IsGrounded)
        {
            rb.AddForce(Vector3.up * 15, ForceMode.Impulse);
            IsGrounded = false;

            if (Input.GetMouseButton(1))
            {
                animator.SetLayerWeight(1, 1);
                animator.SetLayerWeight(2, 0);
            }

        }
    }

    //reset grounded once landed
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) {
            IsGrounded = true;
            animator.SetBool("Jump", false);

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        IsGrounded = false;
    }

}
