using UnityEngine;

public class Player : Character, Character.IDamageable
{
    private Vector2 inputVec;
    private float yaw;
    private float pitch;

    public float Yaw { get { return yaw; } }
    public float Pitch { get { return pitch; } }
    public bool IsAiming { get; private set; }

    public void ApplyDamage(float dmg)
    {
        throw new System.NotImplementedException();
    }

    public void Die()
    {
        throw new System.NotImplementedException();
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

        yaw += (Input.GetAxisRaw("Mouse X") * GameSettings.MOUSE_SENSITIVITY);
        yaw %= 360f;

        pitch += (-Input.GetAxisRaw("Mouse Y") * GameSettings.MOUSE_SENSITIVITY);
        pitch = Mathf.Clamp(pitch, -85f, +85f);

        transform.Rotate(Vector3.up * yaw * Time.deltaTime);
        velocity += transform.localRotation * new Vector3(inputVec.x, 0, inputVec.y) * maxSpeed * Time.deltaTime;
    }
}
