using UnityEngine;

public class PlayerExample1 : BasePlayerController, IAimable, IMoveable, IBulletAttackable
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float moveSpeed = 5f;

    public Vector2 Position
    {
        get => _aimPosition;
        set => _aimPosition = value;
    }

    public void Move(Vector2 direction)
    {
        Vector3 delta = new Vector3(direction.x, direction.y, 0f).normalized * moveSpeed * Time.deltaTime;
        transform.position += delta;
        Debug.Log("Move from " + this.name);
    }

    public void Attack(Vector2 mousePosition)
    {
        ShootBullet(mousePosition);
    }

    public void ShootBullet(Vector2 mousePosition)
    {
        if (bulletPrefab == null || firePoint == null) return;

        // Proyectar el mouse en un plano X-Z (ignorar Y)
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane groundPlane = new Plane(Vector3.up, firePoint.position); // Plano horizontal en el punto de disparo

        if (groundPlane.Raycast(ray, out float hitDistance))
        {
            Vector3 worldPosition = ray.GetPoint(hitDistance);
            Vector3 direction = (worldPosition - firePoint.position).normalized;
            direction.y = 0f; // Asegurar disparo horizontal

            // Instanciar bala
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb != null)
            {
                bulletRb.linearVelocity = direction * bulletSpeed;
            }
        }
    }
}