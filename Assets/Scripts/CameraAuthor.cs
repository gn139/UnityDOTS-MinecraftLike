using Components;
using Unity.Entities;
using UnityEngine;

public class CameraAuthor : MonoBehaviour, IConvertGameObjectToEntity {
    public Transform player;
    public Vector3 pivotOffset = new Vector3 (0.0f, 1.0f, 0.0f); // Offset to repoint the camera.
    public Vector3 camOffset = new Vector3 (1f, 1f, -4f); // Offset to relocate the camera related to the player position.
    public float smooth = 10f; // Speed of camera responsiveness.
    public float horizontalAimingSpeed = 1f; // Horizontal turn speed.
    public float verticalAimingSpeed = 1f; // Vertical turn speed.
    public float maxVerticalAngle = 30f; // Camera max clamp angle. 
    public float minVerticalAngle = -60f; // Camera min clamp angle.

    private float angleH = 0; // Float to store camera horizontal angle related to mouse movement.
    private float angleV = 0; // Float to store camera vertical angle related to mouse movement.
    private Transform cam; // This transform.
    private Vector3 relCameraPos; // Current camera position relative to the player.
    private float relCameraPosMag; // Current camera distance to the player.
    private Vector3 smoothPivotOffset; // Camera current pivot offset on interpolation.
    private Vector3 smoothCamOffset; // Camera current offset on interpolation.
    private Vector3 targetPivotOffset; // Camera pivot offset target to iterpolate.
    private Vector3 targetCamOffset; // Camera offset target to interpolate.
    private float defaultFOV; // Default camera Field of View.
    private float targetFOV; // Target camera Field of View.
    private float targetMaxVerticalAngle; // Custom camera max vertical clamp angle.

    public void Convert (Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem) {

        if (enabled) {
            // var shape = player.GetComponent<PhysicsShapeAuthoring>();
            // var height = shape.GetCylinderProperties().Height;
            var data = new CameraControlData {
                PivotOffset = pivotOffset,
                    CameraOffset = camOffset,
                    AngleHorizontal = angleH,
                    AngleVertical = angleV,
                    RelCameraPos = relCameraPos,
                    RelCameraPosLength = relCameraPosMag,
                    SmoothCameraOffset = smoothCamOffset,
                    SmoothPivotOffset = smoothPivotOffset,
                    TargetCameraOffset = targetCamOffset,
                    TargetPivotOffset = targetPivotOffset,
                    TargetFOV = targetFOV,
                    TargetMaxVerticalAngle = targetMaxVerticalAngle,
                    Smooth = smooth,
                    HorizontalAimingSpeed = horizontalAimingSpeed,
                    VerticalAimingSpeed = verticalAimingSpeed,
                    MaxVerticalAngle = maxVerticalAngle,
                    MinVerticalAngle = minVerticalAngle,
                    ShapeHeight = 3f
            };
            dstManager.AddComponentData (entity, data);
        }
    }

    void Awake () {
        // Reference to the camera transform.
        cam = transform;

        // Set camera default position.
        cam.position = player.position + Quaternion.identity * pivotOffset + Quaternion.identity * camOffset;
        cam.rotation = Quaternion.identity;

        // Get camera position relative to the player, used for collision test.
        relCameraPos = transform.position - player.position;
        relCameraPosMag = relCameraPos.magnitude - 0.5f;

        // Set up references and default values.
        smoothPivotOffset = pivotOffset;
        smoothCamOffset = camOffset;
        defaultFOV = cam.GetComponent<Camera> ().fieldOfView;
        angleH = player.eulerAngles.y;

        ResetTargetOffsets ();
        ResetFOV ();
        ResetMaxVerticalAngle ();
    }
    public void ResetTargetOffsets () {
        targetPivotOffset = pivotOffset;
        targetCamOffset = camOffset;
    }

    // Reset the camera vertical offset.
    public void ResetYCamOffset () {
        targetCamOffset.y = camOffset.y;
    }

    public void ResetFOV () {
        this.targetFOV = defaultFOV;
    }
    // Reset max vertical camera rotation angle to default value.
    public void ResetMaxVerticalAngle () {
        this.targetMaxVerticalAngle = maxVerticalAngle;
    }

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }
}