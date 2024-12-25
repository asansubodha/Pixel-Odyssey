using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Text coinText;
    public int currentCoin = 0; // the current coin of the player

    public int maxHealth = 3; // the maximum health of the player
    public Text health; // to display the health of the player

    public Transform attackPoint; // to check the attack point of the player
    public float attackRange = 1f; // the attack range of the player
    public LayerMask enemyLayer; // to check the layer of the enemy

    public Animator animator;

    public Rigidbody2D rb;
    public float jumpForce = 5f;
    public bool isGround = true;

    private float movement;
    public float moveSpeed = 5f;
    private bool facingRight = true; // to check if the player is facing right or not
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check if the player is dead
        if (maxHealth <= 0)
        {
            //destroy the enemy
            Die();
        }

        coinText.text = currentCoin.ToString(); // display the coin of the player
        health.text = maxHealth.ToString(); // display the health of the player

        // Movement
        movement = Input.GetAxis("Horizontal");

        // Flip the player
        if (movement < 0 && facingRight)
        {
            transform.eulerAngles = new Vector3(0, -180f, 0);
            facingRight = false;
        }
        else if (movement > 0 && !facingRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            facingRight = true;
        }

        // Jump
        if (Input.GetKey(KeyCode.Space) && isGround)
        {
            jump();
            isGround = false;
            animator.SetBool("Jump", true);
        }

        if (Mathf.Abs(movement) > .1f)
        {
            animator.SetFloat("Run", 1f);
        }
        else if (movement < .1f)
        {
            animator.SetFloat("Run", 0f);
        }

        if (Input.GetMouseButtonDown(0)) // if the player clicks the left mouse button
        {
            FindObjectOfType<AudioManager>().PlayAudio(); // play the audio
            animator.SetTrigger("Attack"); // set the attack animation to true
        }
    }
        
    private void FixedUpdate()
    {
        // Move the player
        transform.position += new Vector3(movement, 0f, 0f) * Time.deltaTime * moveSpeed;
    }

    void jump()
    {
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    // Check if the player is on the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") // if the player is on the ground
        {
            isGround = true;
            animator.SetBool("Jump", false); // set the jump animation to false
        }
    }

    public void Attack()
    {
        //Detect enemies in range of attack
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRange, enemyLayer);
        if (collInfo)
        {
            //Debug.Log(collInfo.gameObject.name + "takes Damages"); // log the enemy name
            if (collInfo.gameObject.GetComponent<PatrolEnamy>() != null)
            {
                collInfo.gameObject.GetComponent<PatrolEnamy>().TakeDamage(1); // call the take damage function of the enemy
            }
        }
    }
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange); // draw a wire sphere to check the attack range
    }

    // Function to take damage
    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0)
        {
            return;//if the player is dead, return
        }
        maxHealth -= damage; // decrease the health of the player
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Coin") // if the player collides with the coin
        {
            currentCoin++; // increase the coin of the player
            other.gameObject.transform.GetChild(0).GetComponent<Animator>().SetTrigger("Collected"); // play the collect animation of the coin
            Destroy(other.gameObject, 1f);
            FindObjectOfType<AudioManager>().PlayCoinAudio(); // play the coin audio
        }

        if (other.gameObject.tag == "VictoryPoint") // if the player collides with the victory point
        {
            //Debug.Log("Victory");
            FindObjectOfType<SceneManagement>().LoadLevel2(); // load the next level
        }
        if (other.gameObject.tag == "Victory2") // if the player collides with the victory point
        {
            //Debug.Log("Victory");
            FindObjectOfType<SceneManagement>().LoadMenu(); // load the next level

        }
    }

    // Function to Die
    void Die()
    {
        //Debug.Log("player Died");
        FindObjectOfType<GameManager>().isGameActive = false; // set the game active to false
        Destroy(this.gameObject); // destroy the player
    }
}
