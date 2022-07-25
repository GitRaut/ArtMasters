using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ColisChecker : MonoBehaviour
{
    private PlayerController player;
    private float memberSpeed;
    private float swampTimer;
    private Scene scene;

    private void Start()
    {
        player = transform.parent.gameObject.GetComponent<PlayerController>();
        scene = SceneManager.GetActiveScene();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                player.isGrounded = true;
                break;
            case "PullPlace":
                if (Input.GetKey(KeyCode.E))
                {
                    player.animator.SetBool("IsPull", true);
                    player.textField.text = null;
                    Vector2 startPos = collision.transform.position;
                    player.hook.transform.position
                        = new Vector2(startPos.x, startPos.y - 0.5f);
                    player.hook.SetActive(true);
                    player.ChangeView(player.playerCamera, player.hookCamera);
                    player.ChangeMove("pull");
                }
                break;
            case "Ladder":
                if (Input.GetAxis("Vertical") != 0)
                {
                    float startX = collision.transform.position.x;
                    player.transform.position
                        = new Vector2(startX, player.transform.position.y);
                    player.ChangeMove("ladder");
                }
                break;
            case "Swamp":
                if (player.speed > 0.2f) player.speed -= 0.1f;
                swampTimer -= Time.deltaTime;
                if(swampTimer <= 0)
                {
                    SceneManager.LoadScene(scene.name);
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "PullPlace":
                player.textField.text = "ֽאזלטעו 'ֵ'";
                break;
            case "Ladder":
                player.textField.text = "'W', 'S'";
                break;
            case "MovingPlatform":
                player.isGrounded = true;
                player.transform.SetParent(collision.transform);
                player.ChangeMove("platform");
                break;
            case "Swamp":
                swampTimer = 3f;
                memberSpeed = player.speed;
                player.rb.gravityScale = player.sinkSpeed * 0.01f;
                player.rb.velocity = new Vector2(player.rb.velocity.x, 0);
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Ground":
                player.isGrounded = false;
                break;
            case "PullPlace":
                player.textField.text = null;
                break;
            case "Ladder":
                player.textField.text = null;
                player.ChangeMove("simple");
                break;
            case "MovingPlatform":
                player.transform.SetParent(null);
                player.isGrounded = false;
                player.ChangeMove("simple");
                break;
            case "Swamp":
                swampTimer = 3f;
                player.speed = memberSpeed;
                player.rb.gravityScale = 1;
                break;
        }
    }

}
