using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Parametrs:")]
    public float speed;//скорость
    public float jumpPower;//сила прыжка
    public float hookSpeed;//скорость крючка
    public float climbSpeed;//скорость забирания на лестницу
    public float sinkSpeed;//скорость затягивания в болото
    
    [Header("Сhild objects:")]
    public GameObject hook;//объект крючок удочки
    public Text textField;//текстовое поле для подсказок

    [Header("Cameras:")]
    public Camera playerCamera;//камера игрка
    public Camera hookCamera;//камера крючка

    [Header("Control object's Rigidbodyes2D:")]
    public Rigidbody2D rb;//RigidBody2D игрока
    public Rigidbody2D hookRB;//RigidBody2D крючка

    [Header("Conditions:")]
    public bool isGrounded;//стомт ли игрок на земле
    public bool isJumping;//прыгает ли игрок сейчас

    public bool isPulling;//управляет ли крючком
    public bool isClimbing;//на лестнице сейчас или нет

    private bool canJump;
    private float timer;
    public Animator animator;
    private SpriteRenderer sprite;
    public LineRenderer line;
    public GameObject pullPlace;

    public AudioSource pullDown;
    public AudioSource steps;

    private void Start()
    {
        sprite = transform.gameObject.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        hookRB = hook.GetComponent<Rigidbody2D>();
        animator = transform.gameObject.GetComponent<Animator>();
        isPulling = false;
        canJump = true;
        timer = 0.5f;

        line.endWidth = 0.07f;
        line.startWidth = 0.07f;
    }

    private void Update()
    {
        if (!canJump) timer -= Time.deltaTime;
        if (timer < 0)
        {
            canJump = true;
            timer = 0.5f;
        }

        if(!isPulling) Jump();

        if(Input.GetKeyUp(KeyCode.E) && pullPlace)
        {
            pullDown.Play();
            if (isPulling)
            {
                animator.SetBool("IsPull", false);
                hook.SetActive(false);
                ChangeView(hookCamera, playerCamera);
            }
            else
            {
                animator.SetBool("IsPull", true);
                textField.text = null;
                Vector2 startPos = pullPlace.transform.position;
                hook.transform.position = new Vector2(startPos.x, startPos.y - 0.5f);
                hook.SetActive(true);
                ChangeView(playerCamera, hookCamera);
                ChangeMove("pull");
            }
            pullPlace = null;
        }

        textField.transform.position 
            = new Vector2(transform.position.x, textField.transform.position.y);
    }

    private void FixedUpdate()
    {
        if (isPulling) PullMove();
        else if (isClimbing) LadderMove();
        else if (!isClimbing && !isPulling) Move();
    }

    private void Move()
    {
        float move = Input.GetAxisRaw("Horizontal") * speed;

        if (Input.GetAxisRaw("Horizontal") != 0)
            animator.SetBool("IsWalking", true);
        else animator.SetBool("IsWalking", false);

        if (Input.GetAxisRaw("Horizontal") < 0)
            sprite.flipX = true;
        else if(Input.GetAxisRaw("Horizontal") > 0)
            sprite.flipX = false;

        if (rb.velocity.magnitude != 0)
        {
            if (!steps.isPlaying && !isJumping)
            {
                steps.Play();
            }
        }
        else
        {
            steps.Stop();
        }

        rb.velocity = new Vector2(move, rb.velocity.y);
    }

    private void PullMove()
    {
        if (isPulling)
        {
            float moveX = Input.GetAxisRaw("Horizontal") * hookSpeed;
            float moveY = Input.GetAxisRaw("Vertical") * hookSpeed;
            hookRB.velocity = new Vector2(moveX, moveY);
            if (Input.GetAxisRaw("Vertical") != 0 || Input.GetAxisRaw("Horizontal") != 0)
            {
                Vector2 vec = new Vector2(hook.transform.position.x, hook.transform.position.y + 0.25f);
                line.positionCount++;
                line.SetPosition(line.positionCount - 1, vec);
            }
        }
        if (Input.GetKey(KeyCode.E))
        {
            animator.SetBool("IsPull", false);
            hook.SetActive(false);
            ChangeView(hookCamera, playerCamera);
            ChangeMove("simple");
        }
    }

    private void LadderMove()
    {
        float move = Input.GetAxis("Vertical") * climbSpeed;
        rb.velocity = new Vector2(rb.velocity.x, move);
        if(Input.GetAxis("Horizontal") != 0)
        {
            ChangeMove("simple");
        }
    }
    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            if (isGrounded && canJump)
            {
                isJumping = true;
                animator.SetBool("IsJumping", true);
                rb.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
                canJump = false;
            }
        }
        if (canJump)
        {
            animator.SetBool("IsJumping", false);
        }
        if (isJumping && isGrounded)
        {
            isJumping = false;
        }
    }

    public void ChangeView(Camera oldCamera, Camera newCamera)
    {
        oldCamera.gameObject.SetActive(false);
        newCamera.gameObject.SetActive(true);
    }

    public void ChangeMove(string name)
    {
        switch (name)
        {
            case "simple":
                line.positionCount = 0;
                isPulling = false;
                isClimbing = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
                break;
            case "pull":
                rb.velocity = Vector2.zero;
                line.positionCount = 2;
                line.SetPosition(line.positionCount - 2, transform.position);
                line.SetPosition(line.positionCount - 1, hook.transform.position);
                isClimbing = false;
                isPulling = true;
                rb.bodyType = RigidbodyType2D.Dynamic;
                break;
            case "ladder":
                isClimbing = true;
                isPulling = false;
                rb.bodyType = RigidbodyType2D.Kinematic;
                break;
            case "platform":
                isClimbing = false;
                isPulling = false;
                break;
        }
    }
}
