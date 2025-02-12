using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    Animator animator;
    Collider weaponcCollider;

    [SerializeField] private int damageAmount = 10;

    bool canSwing = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
        weaponcCollider = GetComponent<Collider>();
        weaponcCollider.enabled = false;
    }
    private void Update()
    {
        if (GameStateManager.Instance != null && GameStateManager.Instance.CurrentGameState == GameState.Pause) return;

            if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Yo");
            animator.SetTrigger("Swing");
            canSwing = false;
        }

    }
    private void StartSwing()
    {
        //animator.SetTrigger("Swing");
    }

    public void EnableCollider()
    {
        weaponcCollider.enabled = true;
    }

    public void DisableCollider()
    {
        weaponcCollider.enabled = false;
        canSwing = true;
    }


    private void OnTriggerEnter(Collider other)
    {
        EnemyGuy enemy = other.GetComponent<EnemyGuy>();
        CS_EnemyScript collidedEnemy = other.GetComponent<CS_EnemyScript>();

        if (enemy != null)
        {
            enemy.TakeDamage(damageAmount, true);
        }
        if(collidedEnemy != null)
        {   
          CapsuleCollider capsuleCollider = other.GetComponent<CapsuleCollider>();
            if (other == capsuleCollider)
            {
                if (collidedEnemy.m_lungingAtPlayer)
                { 
                    if(!collidedEnemy.stopTime && !collidedEnemy.startTime) collidedEnemy.stopTime = true;
                    if (collidedEnemy.StunStopping == false)
                    {
                        collidedEnemy.TakingDamage(damageAmount);
                    }
                }
                else
                {
                    collidedEnemy.TakingDamage(damageAmount);
                }
            }
        }

        if (other.name.Contains("Crate"))
        {
            Debug.Log(other.name);
             
            Vector3 direction = (other.transform.position - transform.position);
            direction.y = 0;
            direction.Normalize();

            other.GetComponent<Rigidbody>().AddForce(400 * direction);

            
        }
    }
}