using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisChecker : MonoBehaviour
{
    private PlayerController player;

    private void Start()
    {
        player = transform.parent.gameObject.GetComponent<PlayerController>();
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
                player.isOnPlatform = true;
                player.ChangeMove("platform");
                player.platformParent = collision.transform;
                break;
            case "Swamp":
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
                player.isOnPlatform = false;
                player.isGrounded = false;
                player.ChangeMove("simple");
                break;
        }
    }

}
