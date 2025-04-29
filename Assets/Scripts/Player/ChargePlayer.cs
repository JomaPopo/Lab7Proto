using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerExample3 : BasePlayerController, IAimable, IMoveable, IChargeAttackable
{
    [SerializeField] private float chargeSpeed = 20f;
    [SerializeField] private float chargeDuration = 0.5f;
    private bool isCharging = false;
    [SerializeField] private float rotationSpeed = 100f;

    public Vector2 Position
    {
        get => _aimPosition;
        set => _aimPosition = value;
    }

    public void Move(Vector2 direction)
    {
        float yaw = direction.x * rotationSpeed * Time.deltaTime;
        float pitch = -direction.y * rotationSpeed * Time.deltaTime;
        transform.Rotate(pitch, yaw, 0f, Space.Self);
        Debug.Log("rotanto como una mariposa");
    }

    public void Attack(Vector2 mousePosition)
    {
        Charge(mousePosition);
    }

    public void Charge(Vector2 mousePosition)
    {
        if (isCharging) return;

        // Convertir el mousePosition a un punto en el plano X-Z (ignorar Y)
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position); // Plano horizontal en la posición del jugador

        if (groundPlane.Raycast(ray, out float hitDistance))
        {
            Vector3 targetPosition = ray.GetPoint(hitDistance);
            Vector3 chargeDirection = (targetPosition - transform.position).normalized;
            chargeDirection.y = 0f; // Asegurar que no hay movimiento en Y

            StartCoroutine(PerformCharge(chargeDirection));
        }
    }

    private IEnumerator PerformCharge(Vector3 direction)
    {
        isCharging = true;
        float startTime = Time.time;

        while (Time.time < startTime + chargeDuration)
        {
            if (myRigidBody != null)
            {
                myRigidBody.linearVelocity = direction * chargeSpeed;
            }
            yield return null;
        }

        if (myRigidBody != null)
        {
            myRigidBody.linearVelocity = Vector3.zero;
        }
        isCharging = false;
    }
}