using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [SerializeField] private CinemachineVirtualCamera[] virtualCameras;
    [SerializeField] private CinemachineVirtualCamera startingCamera;

    [SerializeField] private float fallPanAmount = 0.1f;
    [SerializeField] private float fallYPanTime = 0.35f;
    public float fallSpeedYDampingChangeThreshold = -15f;

    private float normYPanAmount;

    private Vector3 startingTrackedObjectOffset;

    public bool isLerpingYDamping { get; private set; }
    public bool lerpedFromPlayerFalling { get; set; }

    private Coroutine lerpYPanCoroutine;
    private Coroutine panCameraCoroutine;

    private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTransposer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        /*
        for (int i = 0; i < virtualCameras.Length; i++)
        {
            if (virtualCameras[i].enabled)
            {
                currentCamera = virtualCameras[i];
                framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }
        */
        SwapCameraToStartCamera();

        normYPanAmount = framingTransposer.m_YDamping;
        startingTrackedObjectOffset = framingTransposer.m_TrackedObjectOffset;
    }

    #region Lerp Y Damping
    public void LerpYDamping(bool isPlayerFalling)
    {
        lerpYPanCoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
    }

    private IEnumerator LerpYAction(bool isPlayerFalling)
    {
        isLerpingYDamping = true;

        float startDampingAmount = framingTransposer.m_YDamping;
        float endDampAmount = 0f;

        if (isPlayerFalling)
        {
            endDampAmount = fallPanAmount;
            lerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = normYPanAmount;
        }

        float elapsedTime = 0f;
        while (elapsedTime < fallYPanTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampingAmount, endDampAmount, (elapsedTime / fallYPanTime));
            framingTransposer.m_YDamping = lerpedPanAmount;

            yield return null;
        }

        isLerpingYDamping = false;
    }
    #endregion

    #region Pan Camera

    public void PanCameraOnContact(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        panCameraCoroutine = StartCoroutine(PanCamera(panDistance, panTime, panDirection, panToStartingPos));
    }

    private IEnumerator PanCamera(float panDistance, float panTime, PanDirection panDirection, bool panToStartingPos)
    {
        Vector2 endPos = Vector2.zero;
        Vector2 startingPos = Vector2.zero;

        // Handle panning from trigger
        if (!panToStartingPos)
        {
            switch (panDirection)
            {
                case PanDirection.Up:
                    endPos = Vector2.up;
                    break;
                case PanDirection.Down:
                    endPos = Vector2.down;
                    break;
                case PanDirection.Left:
                    endPos = Vector2.left;
                    break;
                case PanDirection.Right:
                    endPos = Vector2.right;
                    break;
                default:
                    break;
            }

            endPos *= panDistance;

            startingPos = startingTrackedObjectOffset;
            endPos += startingPos;
        }
        // Handle directions of panning back to start pos
        else
        {
            startingPos = framingTransposer.m_TrackedObjectOffset;
            endPos = startingTrackedObjectOffset;
        }

        // Actual panning
        float elapsedTime = 0f;
        while (elapsedTime < panTime)
        {
            elapsedTime += Time.deltaTime;

            Vector3 panLerp = Vector3.Lerp(startingPos, endPos, (elapsedTime / panTime));
            framingTransposer.m_TrackedObjectOffset = panLerp;

            yield return null;
        }
    }
    #endregion

    #region Swap Cameras

    public void SwapCamera(CinemachineVirtualCamera cameraFromLeft, CinemachineVirtualCamera cameraFromRight, Vector2 triggerExitDirection)
    {
        Debug.Log("Current camera: " + currentCamera + " --- Camera from left: " + cameraFromLeft + " --- Camera from right: " + cameraFromRight);
        // If the current camera is left and trigger exit is on the right
        if (currentCamera == cameraFromLeft && triggerExitDirection.x > 0f)
        {
            Debug.Log("Switching camera from left to right");
            // activate new camera
            cameraFromRight.enabled = true;
            // deactivate old camera
            cameraFromLeft.enabled = false;
            // set new camera as current
            currentCamera = cameraFromRight;
            // update composer variable
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
        // If the current camera is right and trigger exit is on the left
        if (currentCamera == cameraFromRight && triggerExitDirection.x < 0f)
        {
            Debug.Log("Switching camera from right to left");
            // activate new camera
            cameraFromLeft.enabled = true;
            // deactivate old camera
            cameraFromRight.enabled = false;
            // set new camera as current
            currentCamera = cameraFromLeft;
            // update composer variable
            framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    public void SwapCameraToStartCamera()
    {
        startingCamera.enabled = true;
        if (currentCamera != null && currentCamera != startingCamera)
        {
            currentCamera.enabled = false;
        }

        currentCamera = startingCamera;
        Debug.Log(currentCamera);
        framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
    }
    #endregion
}
