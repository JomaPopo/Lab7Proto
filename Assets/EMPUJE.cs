
using UnityEngine;

public class ExplosionForce2D : MonoBehaviour
{
    [SerializeField] private float radius = 5f;
    [SerializeField] private float force = 10f;
    [SerializeField] private LayerMask affectedLayers;

    private void Start()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, affectedLayers);

        foreach (Collider2D col in colliders)
        {
            Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (col.transform.position - transform.position).normalized;
                rb.AddForce(direction * force, ForceMode2D.Impulse);
            }
        }
        Destroy(gameObject, 2f);
    }
}