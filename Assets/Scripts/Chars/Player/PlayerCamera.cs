using UnityEngine;
using Sacristan.Utils;

public class PlayerCamera : Singleton<PlayerCamera>
{
    [SerializeField]
    Transform cam;

    [SerializeField]
    float height = 2.3f;

    [SerializeField]
    float offset = 0.75f;

    [SerializeField]
    float aimingDistance = 1f;

    [SerializeField]
    float runningDistance = 3f;

    [SerializeField]
    float runningSmoothTime = 0.1f;

    [SerializeField]
    Transform dummyRig;

    [SerializeField]
    Transform dummyTarget;

    [SerializeField]
    private CursorLockMode cursorLockMode = CursorLockMode.Locked;

    // damp velocity of camera
    Vector3 _velocity;

    // camera target
    Transform _target;

    // if we are aiming or not
    bool _aiming = false;

    // current camera distance
    float _distance = 0f;

    // accumulated time for aiming transition
    float _aimingAcc = 0f;

    public Camera Camera
    {
        get { return cam.GetComponent<Camera>(); }
    }

    public System.Func<bool> GetAiming;
    public System.Func<float> GetPitch;

    #region MonoBehaviour

    protected override void Awake()
    {
        base.Awake();
        //DontDestroyOnLoad(gameObject);
        _distance = runningDistance;
    }

    private void Start()
    {
        SetTarget(GameManager.Instance.Player.transform);
        GetPitch = () => GameManager.Instance.Player.Pitch;
        GetAiming = () => GameManager.Instance.Player.IsAiming;
    }

    private void LateUpdate()
    {
        UpdateCamera(true);
    }

    #endregion

    #region Public Methods
    //public void CamShake()
    //{
    //    Camera.transform.position -= Camera.transform.forward * 1f;
    //}

    public void SetTarget(Transform target)
    {
        _target = target;
        UpdateCamera(false);
    }

    public void CalculateCameraAimTransform(Transform target, float pitch, out Vector3 pos, out Quaternion rot)
    {
        CalculateCameraTransform(target, pitch, aimingDistance, out pos, out rot);
    }

    public void CalculateCameraTransform(Transform target, float pitch, float distance, out Vector3 pos, out Quaternion rot)
    {

        // copy transform to dummy
        dummyTarget.position = target.position;
        dummyTarget.rotation = target.rotation;

        // move position to where we want it
        dummyTarget.position += new Vector3(0, height, 0);
        dummyTarget.position += dummyTarget.right * offset;

        // clamp and calculate pitch rotation
        Quaternion pitchRotation = Quaternion.Euler(pitch, 0, 0);

        pos = dummyTarget.position;
        pos += (-dummyTarget.forward * distance);

        pos = dummyTarget.InverseTransformPoint(pos);
        pos = pitchRotation * pos;
        pos = dummyTarget.TransformPoint(pos);

        // calculate look-rotation by setting position and looking at target
        dummyRig.position = pos;
        dummyRig.LookAt(dummyTarget.position);

        rot = dummyRig.rotation;
    }

    #endregion

    #region Private Methods

    private void UpdateCamera(bool allowSmoothing)
    {
        if (_target)
        {
            bool isAiming = GetAiming != null ? GetAiming() : false;
            float pitch = GetPitch != null ? GetPitch() : 0f;

            Cursor.lockState = cursorLockMode;
            Cursor.visible = false;

            if (_aiming)
            {
                if (!isAiming)
                {
                    _aiming = false;
                    _aimingAcc = 0f;
                }
            }
            else
            {
                if (isAiming)
                {
                    _aiming = true;
                    _aimingAcc = 0f;
                }
            }

            _aimingAcc += Time.deltaTime;

            if (_aiming)
            {
                _distance = Mathf.Lerp(_distance, aimingDistance, _aimingAcc / 0.4f);
            }
            else
            {
                _distance = Mathf.Lerp(_distance, runningDistance, _aimingAcc / 0.4f);
            }

            Vector3 pos;
            Quaternion rot;

            CalculateCameraTransform(_target, pitch, _distance, out pos, out rot);

            if (!_aiming || allowSmoothing)
            {
                pos = Vector3.SmoothDamp(transform.position, pos, ref _velocity, runningSmoothTime);
            }

            transform.position = pos;
            transform.rotation = rot;

            cam.transform.localRotation = Quaternion.identity;
            cam.transform.localPosition = Vector3.zero;
        }
    }
    #endregion

}
