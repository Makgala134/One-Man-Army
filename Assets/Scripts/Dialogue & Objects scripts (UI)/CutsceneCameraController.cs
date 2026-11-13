using UnityEngine;
using System.Collections;

public class CutsceneCameraController : MonoBehaviour
{
    [Header("Camera")]
    public Camera cutsceneCamera;

    [Header("Camera Positions")]
    public Transform widePosition;
    public Transform luckPosition;
    public Transform danePosition;
    public Transform spikePosition;

    [Header("Transition Settings")]
    public float transitionDuration = 1f;

    private Coroutine cameraTransition;

    private void Start()
    {
        MoveImmediatelyTo(widePosition);
    }

    public void ShowWideShot()
    {
        MoveToPosition(widePosition);
    }

    public void ShowLuck()
    {
        MoveToPosition(luckPosition);
    }

    public void ShowDane()
    {
        MoveToPosition(danePosition);
    }

    public void ShowSpike()
    {
        MoveToPosition(spikePosition);
    }

    private void MoveToPosition(Transform target)
    {
        if (target == null || cutsceneCamera == null)
            return;

        if (cameraTransition != null)
            StopCoroutine(cameraTransition);

        cameraTransition = StartCoroutine(SmoothCameraMove(target));
    }

    private void MoveImmediatelyTo(Transform target)
    {
        if (target == null || cutsceneCamera == null)
            return;

        cutsceneCamera.transform.position = target.position;
        cutsceneCamera.transform.rotation = target.rotation;
    }

    private IEnumerator SmoothCameraMove(Transform target)
    {
        Vector3 startPosition = cutsceneCamera.transform.position;
        Quaternion startRotation = cutsceneCamera.transform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsedTime / transitionDuration);
            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            cutsceneCamera.transform.position =
                Vector3.Lerp(startPosition, target.position, smoothProgress);

            cutsceneCamera.transform.rotation =
                Quaternion.Slerp(startRotation, target.rotation, smoothProgress);

            yield return null;
        }

        cutsceneCamera.transform.position = target.position;
        cutsceneCamera.transform.rotation = target.rotation;

        cameraTransition = null;
    }
}
