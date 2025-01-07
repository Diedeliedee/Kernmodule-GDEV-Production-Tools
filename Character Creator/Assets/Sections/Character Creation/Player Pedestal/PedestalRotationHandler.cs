using UnityEngine;

public class PedestalRotationHandler : MonoBehaviour
{
    [SerializeField] private float m_rotationTime = 1.0f;

    private float m_targetAngle         = 0f;
    private float m_rotationVelocity    = 0f;

    private void Start()
    {
        //  Set the target rotation to the current rotation at the start of the scene.
        m_targetAngle = transform.localEulerAngles.y;
    }

    private void Update()
    {
        //  Create an iterated rotation based on the smoothdamp function.
        var iteratedRotation = Mathf.SmoothDampAngle(transform.localEulerAngles.y, m_targetAngle, ref m_rotationVelocity, m_rotationTime);

        //  Assign the iterated rotation to the player pedestal transform.
        transform.localEulerAngles = new Vector3(0f, iteratedRotation, 0f);
    }

    public void SetTargetAngle(float _angle)
    {
        //  Invert angle because rotation to the right is minus in the Y axis.
        m_targetAngle = -_angle;
    }
}
