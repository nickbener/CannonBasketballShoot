using UnityEngine;

public class TrignometricRotation : MonoBehaviour
{

    [SerializeField] CannonController _cannonController;
    [SerializeField] BonusManager _bonusManager;
    [Range(0f, 1f)]
    [SerializeField] float _sensitivity = 0.1f;
    public Vector3 RotationLimit;
    public Vector3 RotationFrequency;
    public Vector3 StartRotation;
    private Vector3 FinalPosition;

    [Header("It is for learning game!")]
    public GameObject panel;
    public bool isLearnSceneEnable;
    public float StopPosition;

    private float _time = 0f;

    public bool IsStoped { get; set; }

    // Use this for initialization

    void Start()
    {
        StartRotation = transform.localEulerAngles;
        if (isLearnSceneEnable)
        {
            Time.timeScale = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsStoped)
            return;
        if (_bonusManager != null && _bonusManager.CannonBoostIsActive)
        {
            if(_cannonController.IsPressed)
            {
                //Vector3 rotVec = new Vector3(FinalPosition.x, FinalPosition.y, _cannonController.Delta.y * _sensitivity);
                //transform.Rotate(rotVec * Time.deltaTime);
                FinalPosition.z = StartRotation.z + Mathf.Sin(_cannonController.Delta.y * _sensitivity * RotationFrequency.z) * RotationLimit.z;
            }
        }
        else
            FinalPosition.z = StartRotation.z + Mathf.Sin(CalculateTime() * RotationFrequency.z) * RotationLimit.z;
            //FinalPosition.z = StartRotation.z + Mathf.Sin(Time.timeSinceLevelLoad * RotationFrequency.z) * RotationLimit.z;
        transform.localEulerAngles = new Vector3(FinalPosition.x, FinalPosition.y, FinalPosition.z);
    }

    private float CalculateTime()
    {
        _time += Time.deltaTime;
        return _time;
    }

    public void OnClick()
    {
        Time.timeScale = 1.0f;
    }
}
