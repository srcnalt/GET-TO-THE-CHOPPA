using UnityEngine;

public class Player : GoodGuy, IDamageable
{
    [SerializeField]
    private WeaponBase[] weapons;

    private Vector2 inputVec;
    private float yaw;
    private float pitch;

    private Animator _animator;

    public float Yaw { get { return yaw; } }
    public float Pitch { get { return pitch; } }
    public bool IsAiming { get; private set; }

    private WeaponBase CurrentActiveWeapon
    {
        get { return weapons[0]; }
    }
		
    protected override void Awake()
    {
        base.Awake();

        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        yaw = transform.rotation.eulerAngles.y % 360;
    }

    protected override void Update()
    {
        PlayerControls();
        base.Update();
    }

    private void PlayerControls()
    {
        if (Input.GetKey(KeyCode.A)) inputVec.x = -1;
        else if (Input.GetKey(KeyCode.D)) inputVec.x = 1;
        else inputVec.x = 0;

        if (Input.GetKey(KeyCode.S)) inputVec.y = -1;
        else if (Input.GetKey(KeyCode.W)) inputVec.y = 1;
        else inputVec.y = 0;

        if (inputVec.magnitude > 1) inputVec = inputVec.normalized;

        bool isRunningInput = inputVec.sqrMagnitude > float.Epsilon;
        _animator.SetBool("IsRunning", isRunningInput);

        yaw += (Input.GetAxisRaw("Mouse X") * GameSettings.MOUSE_SENSITIVITY);
        yaw %= 360f;

        pitch += (-Input.GetAxisRaw("Mouse Y") * GameSettings.MOUSE_SENSITIVITY);
        pitch = Mathf.Clamp(pitch, -85f, +85f);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (CurrentActiveWeapon.CanFire())
            {
                GameUI.Instance.CrosshairUI.TriggerFX();
                CurrentActiveWeapon.Fire(this);
            }
        }

        //transform.Rotate(Vector3.up * yaw * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(0, yaw, 0);
        velocity += transform.localRotation * new Vector3(inputVec.x, 0, inputVec.y) * maxSpeed * Time.deltaTime;
    }

    public void ApplyDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0f) Die();
    }

    public void Die()
    {
        throw new System.NotImplementedException();
    }
}
