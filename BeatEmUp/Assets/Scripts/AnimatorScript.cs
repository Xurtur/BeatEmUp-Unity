using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class AnimatorScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] public GameObject target;
    private VisualEffect VisualEffect;
    private AudioSource SoundFx;

    [Header("Sound Array")]
    [SerializeField] public AudioClip[] WalkingSFX;
    [SerializeField] public AudioClip[] JumpingSFX;

    public CameraRotation cameraRotation;
    int PunchCache;
    float ComboTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       animator = GetComponent<Animator>();
       VisualEffect = target.GetComponent<VisualEffect>();
       SoundFx = GetComponent<AudioSource>();
       SoundFx.volume = 5f;
       PunchCache = 0;
       ComboTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        #region kb input
        var SpeedTime = 1f * Time.deltaTime;

        //animation script
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            animator.SetBool("IsWalking", true);
            animator.SetBool("Emote",false);
        }
        else if (!Input.GetKey(KeyCode.W) || !Input.GetKey(KeyCode.S) || !Input.GetKey(KeyCode.A) || !Input.GetKey(KeyCode.D)) 
        {
            animator.SetBool("IsWalking", false);
        }

        if (Input.GetKey(KeyCode.LeftShift) && animator.GetBool("IsWalking"))
        {
            animator.SetFloat("WalkX", 1f, .5f, Time.deltaTime );
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 90, SpeedTime);
            VisualEffect.Play();

        }
        else if (!Input.GetKey(KeyCode.LeftShift) && animator.GetBool("IsWalking"))
        {
            if(animator.GetFloat("WalkX") >= 0.0f)
            {
                animator.SetFloat("WalkX", 0.0f, 1f, Time.deltaTime);
                Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, SpeedTime);
                VisualEffect.Stop();
            }
        }


        if (Input.GetMouseButtonDown(0))
        {
            ComboTimer = Time.time;
            PunchCache++;
        }
        if (Time.time - ComboTimer > 1)
        {
            PunchCache = 0;
            animator.SetBool("Punch1", false);
            animator.SetBool("Punch2", false);
        }
        Combo();


        #endregion

        #region Mouse Combo
        if (Input.GetMouseButton(1))
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 40, SpeedTime);

            if (animator.GetBool("IsWalking") && cameraRotation.IsGrounded)
            {
                UpperBody();
            }
            else if (!animator.GetBool("IsWalking") && cameraRotation.IsGrounded)
            {
               WholeBody();
            }

        }
        else if (!Input.GetMouseButton(1))
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, SpeedTime);

            if (animator.GetBool("IsWalking") && cameraRotation.IsGrounded)
            {
                animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0, 5f * Time.deltaTime));

            }
            else if (!animator.GetBool("IsWalking") && cameraRotation.IsGrounded)
            {
                animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 0, 5f * Time.deltaTime));
            }
        }

        if (animator.GetCurrentAnimatorStateInfo(2).normalizedTime > .7 && animator.GetCurrentAnimatorStateInfo(2).IsName("Punch1"))
        {
            animator.SetBool("Punch1", false);
        }

        if (animator.GetCurrentAnimatorStateInfo(2).normalizedTime > .7 && animator.GetCurrentAnimatorStateInfo(2).IsName("Punch2"))
        {
            animator.SetBool("Punch2", false);
            PunchCache = 0;
        }

        if (Input.GetKeyDown(KeyCode.B) && !animator.GetBool("IsWalking"))
        {
            animator.SetBool("Emote", true);
        }

        if (animator.GetBool("Emote"))
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 120, SpeedTime);
        }
        if (!animator.GetBool("Emote"))
        {
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 60, SpeedTime);
        }


    }
    private void Combo() 
    {
        if (PunchCache == 1)
        {
            animator.SetBool("Punch1", true);
        }

        PunchCache = Mathf.Clamp(PunchCache, 0, 2);

        if (PunchCache >= 2 && animator.GetCurrentAnimatorStateInfo(2).normalizedTime > .7 && animator.GetCurrentAnimatorStateInfo(2).IsName("Punch1")) 
        {
            animator.SetBool("Punch2", true);
            animator.SetBool("Punch1", false);
        }
    }
    #endregion

    #region animation functions
    //play footstep sfx 
    public void FootStep()
    {
        int rand = Random.Range(0, WalkingSFX.Length);
        SoundFx.clip = WalkingSFX[rand];

        if (animator.GetBool("IsWalking") && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            SoundFx.Play();
        }
    }

    private void UpperBody()
    {
        animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 1, 10f * Time.deltaTime));
        animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 0, 10f * Time.deltaTime));
    }

    private void WholeBody()
    {
        animator.SetLayerWeight(2, Mathf.Lerp(animator.GetLayerWeight(2), 1, 5f * Time.deltaTime));
        animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0, 5f * Time.deltaTime));
    }

    public void Jump()
    {
        SoundFx.clip = JumpingSFX[0];
        SoundFx.Play();
    }

    public void Land()
    {
        SoundFx.clip = JumpingSFX[1];
        SoundFx.Play();
    }

    public void Stop()
    {
        SoundFx.Stop();
    }
    #endregion
}