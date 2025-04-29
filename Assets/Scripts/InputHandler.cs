using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private GameObject playerController;

    private IAimable _characterAim;
    private IMoveable _characterMovement;
    private IAttackable _characterAttack;
    private Vector2 _movementInput;

    private void Awake()
    {
        _characterAim = playerController.GetComponent<IAimable>();
        _characterMovement = playerController.GetComponent<IMoveable>();
        _characterAttack = playerController.GetComponent<IAttackable>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        _movementInput = context.ReadValue<Vector2>();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        _characterAim.Position = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _characterAttack.Attack(_characterAim.Position);
        }
    }
    private void Update()
    {
        if (_movementInput != Vector2.zero)
        {
            _characterMovement.Move(_movementInput);
        }
    }
}