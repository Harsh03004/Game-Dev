using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class CalculateDistance : MonoBehaviour
{
    public Transform enemy;
    public Animator animator;
    public Text distText;
    private Camera arcamera;

    public float attackDistance = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        arcamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy != null && arcamera != null){
            float distance = Vector3.Distance(arcamera.transform.position, enemy.position);


            if (distText != null)
            {
                distText.text = "Distance: " + distance.ToString() + "m";
            }

            if (distance < attackDistance)
            {
                animator.SetBool("Attack",true);
            }

            else if(distance > attackDistance)
            {
                animator.SetBool("Attack", false);
            }
        }
    }
}
