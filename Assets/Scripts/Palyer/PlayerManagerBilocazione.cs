using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerManagerBilocazione : MonoBehaviour
{
    [SerializeField] private Transform spawn;
    [SerializeField] private AudioClip diamondFx;
    [SerializeField] private GameObject particleDiamond;
    [SerializeField] private float speedRuning;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private AnimationCurve jumpCurve;

    private bool moveFoward;
    private bool slide; //serve per sapere se già slide
    private bool jumping; //serve per sapere se già saltando

    private Rigidbody rb;
    private Animator animator;

    private float colHeight, colRadius, colCenterY, colCenterZ; //per il collider dello slide

    private float jumpTimer;
    private float yPos;

    private float altezza;

    // Start is called before the first frame update
    void Start()
    {
        ResetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        moveFoward = BilocazioneManager.current.bilocazione;
        if (!moveFoward)
            return;

        GetActions();

        if (jumping)
        {
            yPos = altezza+jumpCurve.Evaluate(jumpTimer);
            jumpTimer += Time.deltaTime;

            if (jumpTimer > 1f)
            {
                jumpTimer = 0f;
                jumping = false;
                animator.SetBool("saltar", jumping);
            }
        }

        // Movimento tra corsie
        float acceleration = InputManager.Instance.GetAccelorometer().x * Time.deltaTime * moveSpeed;
        transform.Translate(acceleration, 0, 0);

        // Muovo il player
        if (moveFoward)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, yPos, transform.position.z) + transform.forward, Time.deltaTime * speedRuning);
    }

    #region ACTIONS
    private void GetActions()
    {
        Debug.Log("Bilocazione GetActions");
        Swipe swipe = InputManager.Instance.GetAction();

        if (swipe == Swipe.Up)
            Jump();
        else if (swipe == Swipe.Down)
            Slide();
        else if (swipe == Swipe.Left)
            TurnLeft();
        else if (swipe == Swipe.Right)
            TurnRight();

        InputManager.Instance.SetAction(Swipe.None);
    }


    public void Jump()
    {
        if (BilocazioneManager.current.bilocazione)
        {
            if (!jumping)
            {
                jumping = true; //Se non sta saltando lo imposto a true
                //animator.SetTrigger("jump");
                animator.SetBool("saltar", jumping);
            }
            else
            {
                //se c'è lo swipe verso sopra di nuovo, divento invisibile
                PoteriManager.current.TurnInvisible();
            }
        }
    }

    public void TurnLeft()
    {
        if (BilocazioneManager.current.bilocazione)
            rb.transform.Rotate(0.0f, -90.0f, 0.0f);
    }

    public void TurnRight()
    {
        if (BilocazioneManager.current.bilocazione)
            rb.transform.Rotate(0.0f, 90.0f, 0.0f);
    }

    public void Slide()
    {
        if (!slide && !jumping && BilocazioneManager.current.bilocazione)
        {
            slide = true;
            animator.SetTrigger("slide");
            CapsuleCollider coll = gameObject.GetComponent<CapsuleCollider>();

            //salvo i valori del collaider per poterli ripristinare più avanti
            colHeight = coll.height;
            colRadius = coll.radius;
            colCenterY = coll.center.y;
            colCenterZ = coll.center.z;

            //vado a modificare il capsule collaider
            coll.height = 1f;
            coll.radius = 0.7f;
            coll.center = new Vector3(0, 0.72f, 0.34f);

            Invoke("ExitSlide", 1f);
        }
    }

    void ExitSlide()
    {
        CapsuleCollider coll = gameObject.GetComponent<CapsuleCollider>();
        //vado a ripristinare il capsule collaider
        coll.height = colHeight;
        coll.radius = colRadius;
        coll.center = new Vector3(0, colCenterY, colCenterZ);

        slide = false;
    }
    #endregion

    #region TRIGGERS
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "fence")
        {
            moveFoward = false;
            animator.SetBool("idle", false);
            PoteriManager.current.EndBilocazione();
        }
        else if (other.gameObject.tag == "fall")
        {
            moveFoward = false;
            animator.SetBool("idle", false);
            PoteriManager.current.EndBilocazione();
        }
        else if (other.gameObject.tag == "diamond")
        {
            //if (!PlatformSpawnerScript.current.gameOver)
            if (true)
            {
                Destroy(other.gameObject);
                ScoreManagerScript.current.DiamondScore();
                AudioManageScript.current.PlaySound(diamondFx);

                // Lo facciamo in questo modo perchè lo instaziamo momentaneamente, in quanto dopo lo andiamo a distruggere
                GameObject part = Instantiate(particleDiamond, new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y, other.gameObject.transform.position.z + 4f), Quaternion.identity) as GameObject;
                Destroy(part, 2f);
            }
        }
    }
    #endregion

    private void ResetPosition()
    {
        moveFoward = true;
        jumping = false;

        altezza = spawn.transform.position.y;
        yPos = spawn.transform.position.y;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        animator.SetBool("idle", false);
    }
}
