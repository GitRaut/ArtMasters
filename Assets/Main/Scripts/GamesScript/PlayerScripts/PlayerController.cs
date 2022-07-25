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

    [Header("On what platform is Player:")]
    public Transform platformParent;//двигающаяся платформа на которой стоит игрок

    [Header("Conditions:")]
    public bool isGrounded;//стомт ли игрок на земле
    public bool isJumping;//прыгает ли игрок сейчас

    public bool isPulling;//управляет ли крючком
    public bool isClimbing;//на лестнице сейчас или нет
    public bool isOnPlatform;//на платформе сейчас или нет

    private bool canJump;
    private float timer;
    public Animator animator;
    private SpriteRenderer sprite;
    public LineRenderer line;

    private void Start()
    {
        sprite = transform.gameObject.GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        hookRB = hook.GetComponent<Rigidbody2D>();
        animator = transform.gameObject.GetComponent<Animator>();
        isPulling = false;
        isOnPlatform = false;
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

        textField.transform.position 
            = new Vector2(transform.position.x, textField.transform.position.y);
    }

    private void FixedUpdate()
    {
        if (isPulling) PullMove();
        else if (isClimbing) LadderMove();
        else if (isOnPlatform) PlatformMove();
        else if (!isClimbing && !isPulling && !isOnPlatform) Move();
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

        rb.velocity = new Vector2(move, rb.velocity.y);
    }

    private void PlatformMove()
    {
        if (isOnPlatform)
        {
            float move = Input.GetAxisRaw("Horizontal") * speed;
            if (Input.GetAxis("Horizontal") != 0 || isJumping)
            {
                animator.SetBool("IsWalking", true);
                transform.SetParent(null);
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                animator.SetBool("IsWalking", false);
                transform.SetParent(platformParent);
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.velocity = Vector2.zero;
            }

            if (Input.GetAxisRaw("Horizontal") < 0)
                sprite.flipX = true;
            else if (Input.GetAxisRaw("Horizontal") > 0)
                sprite.flipX = false;

            rb.velocity = new Vector2(move, rb.velocity.y);
        }
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
                line.SetPosition(line.positionCount - 1, hook.transform.position);
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
                isOnPlatform = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
                break;
            case "pull":
                line.positionCount = 2;
                line.SetPosition(line.positionCount - 2, transform.position);
                line.SetPosition(line.positionCount - 1, hook.transform.position);
                isClimbing = false;
                isPulling = true;
                isOnPlatform = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
                break;
            case "ladder":
                isClimbing = true;
                isPulling = false;
                isOnPlatform = false;
                rb.bodyType = RigidbodyType2D.Kinematic;
                break;
            case "platform":
                isClimbing = false;
                isPulling = false;
                isOnPlatform = true;
                break;
        }
    }
}
