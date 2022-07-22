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

    private Rigidbody2D rb;//RigidBody2D игрока
    private Rigidbody2D hookRB;//RigidBody2D крючка

    private Transform platformParent;//двигающаяся платформа на которой стоит игрок

    private bool isGrounded;//стомт ли игрок на земле
    private bool isJumping;//прыгает ли игрок сейчас

    private bool isPulling;//управляет ли крючком
    private bool isClimbing;//на лестнице сейчас или нет
    private bool isOnPlatform;//на платформе сейчас или нет

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hookRB = hook.GetComponent<Rigidbody2D>();
        isPulling = false;
        isOnPlatform = false;
    }

    private void Update()
    {
        if(!isPulling)Jump();

        textField.transform.position 
            = new Vector2(transform.position.x, textField.transform.position.y);
    }

    private void FixedUpdate()
    {
        if (isPulling) PullMove();
        else if (isClimbing) LadderMove();
        else if (isOnPlatform) PlatformMove();
        else if (!isClimbing && !isPulling && ! isOnPlatform) Move();
    }

    private void Move()
    {
        float move = Input.GetAxisRaw("Horizontal") * speed;
        rb.velocity = new Vector2(move, rb.velocity.y);
    }

    private void PlatformMove()
    {
        if (isOnPlatform)
        {
            float move = Input.GetAxisRaw("Horizontal") * speed;
            if (Input.GetAxis("Horizontal") != 0 || isJumping)
            {
                transform.SetParent(null);
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                transform.SetParent(platformParent);
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.velocity = Vector2.zero;
            }
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
        }
        if (Input.GetKey(KeyCode.E))
        {
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
        if (Input.GetKey(KeyCode.Space))
        {
            if (isGrounded)
            {
                isJumping = true;
                rb.AddForce (transform.up * jumpPower, ForceMode2D.Impulse);
            }
        }
        else isJumping = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                isGrounded = true;
                break;
            case "PullPlace":
                if (Input.GetKey(KeyCode.E))
                {
                    textField.text = null;
                    Vector2 startPos = collision.transform.position;
                    hook.transform.position 
                        = new Vector2(startPos.x, startPos.y - 0.5f);
                    hook.SetActive(true);
                    ChangeView(playerCamera, hookCamera);
                    ChangeMove("pull");
                }
                break;
            case "Ladder":
                if(Input.GetAxis("Vertical") != 0)
                {
                    float startX = collision.transform.position.x;
                    transform.position 
                        = new Vector2(startX, transform.position.y);
                    ChangeMove("ladder");
                }
                break;
            case "Swamp":
                if (speed > 0.2f) speed -= 0.1f;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "PullPlace":
                textField.text = "Нажмите 'Е'";
                break;
            case "Ladder":
                textField.text = "'W', 'S'";
                break;
            case "MovingPlatform":
                isGrounded = true;
                isOnPlatform = true;
                ChangeMove("platform");
                platformParent = collision.transform;
                break;
            case "Swamp":
                rb.gravityScale = sinkSpeed * 0.01f;
                rb.velocity = new Vector2(rb.velocity.x, 0);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                isGrounded = false;
                break;
            case "PullPlace":
                textField.text = null;
                break;
            case "Ladder":
                textField.text = null;
                ChangeMove("simple");
                break;
            case "MovingPlatform":
                isOnPlatform = false;
                isGrounded = false;
                ChangeMove("simple");
                break;
        }
    }

    private void ChangeView(Camera oldCamera, Camera newCamera)
    {
        oldCamera.gameObject.SetActive(false);
        newCamera.gameObject.SetActive(true);
    }

    private void ChangeMove(string name)
    {
        switch (name)
        {
            case "simple":
                isPulling = false;
                isClimbing = false;
                isOnPlatform = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
                break;
            case "pull":
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
