using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour
{
    private Collider hitbox;

    [SerializeField]
    private GameObject potionRadius;
    [SerializeField]
    private GameObject splashEffect;
    [SerializeField]
    private GameObject potionModel;

    [SerializeField]
    private int heal;

    private Collider[] hitColliders;

    [SerializeField]
    private float healRadius = 5f;

    // Start is called before the first frame update
    void Start()
    {
        hitbox = potionRadius.GetComponent<SphereCollider>();
        hitbox.enabled = false;
        StartCoroutine(DisableCollider());
    }

    private IEnumerator DisableCollider()
    {
        yield return new WaitForSeconds(3.0f);

        hitbox.enabled = true;
    }

    void OnCollisionEnter(Collision other)
    {
        Heal();
    }

    private void Heal()
    {
        hitColliders = Physics.OverlapSphere(transform.position, healRadius);

        foreach (Collider hit in hitColliders)
        {
            GameObject go = hit.gameObject;

            //if (go.tag == "Enemey")
            //{
            //    go.SendMessage("TakeDamage", heal);
            //    hitbox.enabled = false;
            //}
            //else 
            if (go.tag == "Player")
            {
                HealthManager.playerHealth.Heal(heal);
                hitbox.enabled = false;
            }
        }

        potionModel.SetActive(false);
        splashEffect.SetActive(true);

        Invoke("Die", 3f);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
