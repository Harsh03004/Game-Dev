using UnityEngine;
using UnityEngine.UI;

public class ARCombatSystem : MonoBehaviour
{
    public Transform enemy;
    public int enemyHealth = 3;
    public float attackDistance = 3f;
    public HealthBar healthBar;
    public GameObject hitAffect;
    public Button attackButton; // Reference to UI Attack Button

    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        if(attackButton!= null)
        {
            attackButton.onClick.AddListener(Attack);
        }
    }

    //void Update()
    //{
    //    // Check for mobile touch input
    //    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
    //    {
    //        Attack();
    //    }

    //    // Check for left mouse click input (for PC)
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Attack();
    //    }
    //}

    public void Attack()
    {
        if (enemy == null) return;

        float distance = Vector3.Distance(cam.transform.position, enemy.position);

        if (distance < attackDistance)
        {
            enemyHealth-=10;
            healthBar.SetHealth(enemyHealth);
            if (hitAffect != null)
            {
                Instantiate(hitAffect, enemy.position, Quaternion.identity);
            }
            Debug.Log("Enemy hit! Remaining health: " + enemyHealth);
        }
    }
}
