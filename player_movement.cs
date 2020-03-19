using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{


    [Header("chracter movement")]
    public float speed;
    private float HorizontalInput;
    public float jumpforce;
    public float fallmultiplier;
    public float jumptime;
    private bool isjumping;
    private float jumptimer;
    private bool onground;
    public float raydistance;
    private float slopeangleleft;
    private float slopeangleright;

    [Header("componenets")]
    public Rigidbody2D player;
    public BoxCollider2D BoxCollider;
    public LayerMask Groundlayer;
    

    boxcollider box;

    struct boxcollider
    {
        public Vector2 bottomleft, bottomright;
        public Vector2 topleft, topright;
    }
    
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        BoxCollider = GetComponent<BoxCollider2D>();
    }

    void checkground()
    {
        Vector2 Bleft = box.bottomleft;
        Vector2 Bright = box.bottomright;
        Bounds bounds = BoxCollider.bounds;
        
        RaycastHit2D BottomLeft = Physics2D.Raycast(Bleft, Vector2.down, raydistance, Groundlayer);
        RaycastHit2D Bottomright = Physics2D.Raycast(Bright, Vector2.down, raydistance, Groundlayer);

        Debug.DrawRay(Bleft, Vector2.down*raydistance,Color.green);
        Debug.DrawRay(Bright, Vector2.down*raydistance,Color.red);

       if(BottomLeft.collider == null && Bottomright.collider == null)
       {
           onground = false;
       }
       else
       {
            onground = true;
       }
    }

    void FixedUpdate()
    {


        HorizontalInput = Input.GetAxisRaw("Horizontal");

        updateboxcollider();
        checkground();
        HorizontalMotion();
        jump();
    }
    void HorizontalMotion()
    {
        player.velocity = new Vector2(speed * HorizontalInput, player.velocity.y);
    }

    void jump()
    {
        if (onground == true && Input.GetKey(KeyCode.UpArrow))
        {
            isjumping = true;
            jumptimer = jumptime;
            player.velocity = new Vector2(player.velocity.x, 0);
            player.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);
        }

        if (Input.GetKey(KeyCode.UpArrow) && isjumping == true)
        {
            if (jumptimer > 0)
            {
                player.velocity += Vector2.up * 0.05f;
                jumptimer -= Time.deltaTime;
            }
            else
            {
                isjumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            isjumping = false;
        }

        if (player.velocity.y < 0)
        {
            player.velocity += Vector2.up * Physics2D.gravity.y * (fallmultiplier - 1) * Time.deltaTime;
        }
    }

    void updateboxcollider()
    {
        Bounds bounds = BoxCollider.bounds;

        box.bottomleft = new Vector2(bounds.min.x, bounds.min.y);
        box.bottomright = new Vector2(bounds.max.x, bounds.min.y);
        box.topleft = new Vector2(bounds.min.x, bounds.max.y);
        box.topright = new Vector2(bounds.max.x, bounds.max.y);
    }
}
