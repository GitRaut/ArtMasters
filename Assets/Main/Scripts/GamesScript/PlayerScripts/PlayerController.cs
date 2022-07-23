using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Parametrs:")]
    public float speed;//��������
    public float jumpPower;//���� ������
    public float hookSpeed;//�������� ������
    public float climbSpeed;//�������� ��������� �� ��������
    public float sinkSpeed;//�������� ����������� � ������
    
    [Header("�hild objects:")]
    public GameObject hook;//������ ������ ������
    public Text textField;//��������� ���� ��� ���������

    [Header("Cameras:")]
    public Camera playerCamera;//������ �����
    public Camera hookCamera;//������ ������

    [Header("Control object's Rigidbodyes2D:")]
    public Rigidbody2D rb;//RigidBody2D ������
    public Rigidbody2D hookRB;//RigidBody2D ������

    [Header("On what platform is Player:")]
    public Transform platformParent;//����������� ��������� �� ������� ����� �����

    [Header("Conditions:")]
    public bool isGrounded;//����� �� ����� �� �����
    public bool isJumping;//������� �� ����� ������

    public bool isPulling;//��������� �� �������
    public bool isClimbing;//�� �������� ������ ��� ���
    public bool isOnPlatform;//�� ��������� ������ ��� ���

    private bool canJump;
    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hookRB = hook.GetComponent<Rigidbody2D>();
        isPulling = false;
        isOnPlatform = false;
        canJump = true;
        timer = 0.5f;
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
        if (Input.GetKey(KeyCode.Space) && !isJumping)
        {
            if (isGrounded && canJump)
            {
                isJumping = true;
                rb.AddForce (transform.up * jumpPower, ForceMode2D.Impulse);
                canJump = false;
            }
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