using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    public float speed;
    public AudioSource musicSource;
    public AudioClip background;
    public AudioClip won;
    Animator anim;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI livesText;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    private int score;
    private int lives;
    private float moveSpeed;
    private float jumpforce;
    private bool isJumping;
    private bool facingRight = true;
    private float moveHorizontal;
    private float moveVertical;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = gameObject.GetComponent<Rigidbody2D>();

        moveSpeed = 2f;
        jumpforce = 50f;
        isJumping = false;

        score = 0;
        lives = 3;

        SetCountText ();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);

        anim = GetComponent<Animator>();

        musicSource.clip = background;
        musicSource.Play();
        musicSource.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }

        if (facingRight == false && moveHorizontal > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveHorizontal < 0)
        {
            Flip();
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetInteger("State", 0);
        }

        if (isJumping == true && moveVertical > 0)
        {
            anim.SetInteger("State", 2);
        }

        if (isJumping == false && moveVertical < 0)
        {
            anim.SetInteger("State", 0);
        }

    }


    void FixedUpdate()
    {
        if (lives == 0)
        {
            Destroy(this.gameObject);
        }

        if(moveHorizontal > 0.1f || moveHorizontal <-0.1f)
        {
            rd2d.AddForce(new Vector2(moveHorizontal * moveSpeed, 0f), ForceMode2D.Impulse);
        }

        if(!isJumping && moveVertical > 0.1f)
        {
            rd2d.AddForce(new Vector2(0f, moveVertical * jumpforce), ForceMode2D.Impulse);
        }
        
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            isJumping = false;
        }

       if (collision.collider.tag == "Coin")
        {
            Destroy(collision.collider.gameObject);
            score = score + 1;

            SetCountText();
        }

        else if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.SetActive(false);

            lives = lives - 1;

            SetCountText();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
     if (collision.gameObject.tag == "Ground")
        {
            isJumping = true;
        }
    }

    void SetCountText ()
    {
        livesText.text = "Lives: " + lives.ToString();
        if (lives == 0)
        {
            loseTextObject.SetActive(true);
        }

        scoreText.text = "Score: " + score.ToString();
        
        if (score == 4)
        {
            transform.position = new Vector2(37.0f,0.0f);
            lives = 3;
            SetCountText();
        }

        if (score == 8)
        {
            winTextObject.SetActive(true);
            musicSource.clip = background;
            musicSource.Stop();
            Destroy(this.gameObject);
        }
    }
}