using UnityEngine;

public class PlayerExample2 : BasePlayerController, IAimable, IMoveable, IExplosionAttackable
{
    [Header("Configuración")]
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionForce = 1000f; // Aumentada para 3D
    [SerializeField] private LayerMask affectedLayers;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Efectos")]
    [SerializeField] private float explosionDuration = 2f;

    public Vector2 Position { get => _aimPosition; set => _aimPosition = value; }

    public void Move(Vector2 direction)
    {
        Vector3 dir3D = new Vector3(direction.x, 0f, direction.y).normalized;
        Vector3 targetVel = dir3D * moveSpeed;
        myRigidBody.linearVelocity = new Vector3(targetVel.x, myRigidBody.linearVelocity.y, targetVel.z);
        Debug.Log("Move from " + this.name);
    }

    public void Attack(Vector2 mousePosition)
    {
        TriggerExplosion(mousePosition);
    }

    public void TriggerExplosion(Vector2 mousePosition)
    {
        if (explosionPrefab == null) return;

        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        // Versión mejorada con detección 3D precisa
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, affectedLayers))
        {
            CreateExplosion(hit.point);
        }
        else
        {
            // Plan B: Crear en un plano horizontal si no hay colisión
            CreateExplosionOnPlane(ray);
        }
    }

    private void CreateExplosion(Vector3 position)
    {
        GameObject explosion = Instantiate(explosionPrefab, position, Quaternion.identity);
        ApplyExplosionPhysics(position);
        Destroy(explosion, explosionDuration);
    }

    private void CreateExplosionOnPlane(Ray ray)
    {
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float distance;
        if (groundPlane.Raycast(ray, out distance))
        {
            CreateExplosion(ray.GetPoint(distance));
        }
    }

    private void ApplyExplosionPhysics(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, explosionRadius, affectedLayers);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(
                    explosionForce,
                    position,
                    explosionRadius,
                    0.5f, // Pequeño empuje hacia arriba
                    ForceMode.Impulse
                );
            }
        }
    }
}