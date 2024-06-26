using System.Collections;

using UnityEngine;

public class LS_Enemy : MonoBehaviour
{
    [SerializeField] private GameObject p_point1;
    [SerializeField] private GameObject p_point2;
    


    [Header("Enemy Movement")]
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float MovemntSpeedPatrol;

    [Header("Enemy Stats")]
    [SerializeField] private GM_Health HealthManager;
    [SerializeField] private int Damage;

    [Header("Enemy Attack")]
    [SerializeField] private float AttackRange;

    [Header("Enemy Detection")]
    [SerializeField] private float DetectionRange;    

    private Rigidbody2D rb;
    private Transform CurrentPoint;
    private GameObject Player;
    private Animator animator;

    [SerializeField] private AudioClip EnemyHitSound;


    // Inicializamos los componentes
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        CurrentPoint = p_point2.transform;

        HealthManager = new GM_Health();

        animator = GetComponent<Animator>();


    }

    private void Update()
    {
        // Movimiento del enemigo
        if (Vector2.Distance(transform.position, Player.transform.position) > DetectionRange)
        {
            Vector2 point = CurrentPoint.position - transform.position;


            if (CurrentPoint == p_point2.transform)
            {
                rb.velocity = new Vector2(MovemntSpeedPatrol, 0);

            }
            else if (CurrentPoint == p_point1.transform)
            {
                rb.velocity = new Vector2(-MovemntSpeedPatrol, 0);

            }

            // Cambio de direccion del enemigo
            if (Vector2.Distance(transform.position, CurrentPoint.position) < 0.5f && CurrentPoint == p_point2.transform)
            {
                flip();
                CurrentPoint = p_point1.transform;
            }
            if (Vector2.Distance(transform.position, CurrentPoint.position) < 0.5f && CurrentPoint == p_point1.transform)
            {
                flip();
                CurrentPoint = p_point2.transform;

            }
        }
        else
        {

            if (Player == null)
            {
                Player = GameObject.FindGameObjectWithTag("Player");
            }

        }
    }

    // Funcion para que el enemigo reciba da�o
    public void TakeDamage(int damage)
    {
        HealthManager.TakeDamage(damage);
        animator.SetTrigger("TakeDamage");

        if (HealthManager.GetHealth() <= 0)
        {
            StartCoroutine(Die());
        }

        SoundController.Instance.EjecutarSonido(EnemyHitSound);
    }

    // Funcion de muerte del enemigo
    private IEnumerator Die()
    {
        animator.SetTrigger("Die");
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);
    }

    // Funcion para detectar y atacar al jugador
    private void FixedUpdate()
    {
        if (Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");

        }
        else
        {
            if (Vector2.Distance(transform.position, Player.transform.position) < DetectionRange)
            {
                if (Vector2.Distance(transform.position, Player.transform.position) > AttackRange)
                {
                    transform.position = Vector2.MoveTowards(transform.position, Player.transform.position, MovementSpeed * Time.deltaTime);
                }
            }
        }

    }
    
    // Funcion para cambiar la direccion del enemigo
    private void flip()
    {
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }

    // Dibujar el rango de ataque
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, DetectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }

    // Dibujar los puntos de patrullaje
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(p_point1.transform.position, 0.2f);
        Gizmos.DrawWireSphere(p_point2.transform.position, 0.2f);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(p_point1.transform.position, p_point2.transform.position);
    }
#endif

}