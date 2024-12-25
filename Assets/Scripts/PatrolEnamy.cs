using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEnamy : MonoBehaviour
{
    public int maxHealth = 5; // the maximum health of the enemy
    private bool facingLeft = true;
    public float moveSpeed = 2f;
    public Transform checkPoint;
    public float distance = 0.5f;
    public LayerMask layerMask; // to check the layer of the object

    private bool inRange = false;
    public Transform player;
    public float chaseRange = 5f;
    public float attackRange = 2f;
    public float chaseSpeed = 4f;

    public Animator animator;

    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask attackLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<GameManager>().isGameActive == false) return; // if the game is not active, return
        //check if the enemy is dead
        if (maxHealth <= 0)
        {
            Die();
        }

        //check if the player is in range
        if (Vector2.Distance(transform.position, player.position) <= chaseRange)
        {
            inRange = true;
        }
        else
        {
            inRange = false;
        }

        //if the player is in range, chase the player
        if (inRange)
        {
            //flip the enemy to face the player
            if (player.position.x > transform.position.x && facingLeft)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;
            }
            else if (player.position.x < transform.position.x && !facingLeft)
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }

            //Debug.Log("Chase Player");
            if (Vector2.Distance(transform.position, player.position) > attackRange)
            {
                animator.SetBool("Attack", false);
                //chase the player
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }
            else
            {
                //attack the player
                animator.SetBool("Attack", true);;
            }
        }
        else
        {
            //enemy movement
            transform.Translate(Vector2.left * 2 * Time.deltaTime * moveSpeed);

            //check if the enemy is grounded
            RaycastHit2D hit = Physics2D.Raycast(checkPoint.position, Vector2.down, distance, layerMask);

            //if the enemy is not grounded, flip the enemy
            if (hit == false && facingLeft)
            {
                //Debug.Log("Flip Enemy");
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;
            }
            else if (hit == false && !facingLeft)
            {
                //Debug.Log("Flip Enemy");
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }
        }

    }

   public void Attack()
    {
        //detect the player in the attack range
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position, attackRadius, attackLayer);

        if (collInfo)
        {
            if (collInfo.gameObject.GetComponent<Player>() != null)
            {
                collInfo.gameObject.GetComponent<Player>().TakeDamage(1); // take damage from the player
            }
        }
    }
    public void TakeDamage(int damage)
    {
        if (maxHealth <= 0) return; // if the health is less than or equal to 0, return
        maxHealth -= damage; // decrease the health
    }

    //draw a line to check the distance of the enemy
    private void OnDrawGizmosSelected()
    {
        //draw a line to check the distance of the enemy
        if (checkPoint == null)
        {
            return;
        }
        //draw a line to check the distance of the enemy
        Gizmos.color = Color.red;
        Gizmos.DrawRay(checkPoint.position, Vector2.down * distance);

        //draw a circle to check the chase range of the enemy
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        
        if (attackPoint == null) return; // if the attack point is not set, return
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    void Die()
    {
        Debug.Log(this.transform.name +" Enemy Died");
        Destroy(this.gameObject);
        FindObjectOfType<AudioManager>().PlayEnemyDyingAudio(); // play the enemy dying audio
    }

}
