using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTrajectory : MonoBehaviour
{

    [SerializeField]
    private LineRenderer _lineRenderer;

    [SerializeField]
    [Range(3, 500)]
    private int _lineSegmentCount = 20;

    private List<Color32> colors = new List<Color32>();

    private List<Vector3> _linePoints = new List<Vector3>();

    private Vector3 hitPoint;
    private bool hasHitSurface;

    #region Singleton

    public static DrawTrajectory Instance;


    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;

        _lineRenderer.sortingOrder = 1;
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.material.color = Color.cyan;

        colors.Add(Color.red); //1
        colors.Add(Color.yellow);
        colors.Add(Color.green); //3
    }
    private void Start()
    {
        hasHitSurface = false;
    }

    #endregion

    public void UpdateTrajectory(Vector3 forceVector, Rigidbody rigidbody, Vector3 startingPoint)
    {
        hasHitSurface = false;

        Vector3 velocity = (forceVector / rigidbody.mass) * Time.fixedDeltaTime;

        float FlightDuration = (2 * velocity.y) / Physics.gravity.y;

        float stepTime = FlightDuration / _lineSegmentCount;

        _linePoints.Clear();
        _linePoints.Add(startingPoint);

        for (int i = 1; i<_lineSegmentCount; i++)
        {
            float stepTimePassed = stepTime * i;

            Vector3 MovementVector = new Vector3(
                velocity.x * stepTimePassed,
                velocity.y * stepTimePassed - 0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed,
                velocity.z * stepTimePassed);

            Vector3 NewPointOnLine = -MovementVector + startingPoint;

            RaycastHit hit;

            if (!hasHitSurface && Physics.Raycast(_linePoints[i-1], NewPointOnLine-_linePoints[i-1], out hit, (NewPointOnLine - _linePoints[i - 1]).magnitude))
            {
                _linePoints.Add(hit.point);
                hitPoint = hit.point;
                hasHitSurface = true;
            }


            _linePoints.Add(NewPointOnLine);
        }

        _lineRenderer.positionCount = _linePoints.Count;
        _lineRenderer.SetPositions(_linePoints.ToArray());
    }

    public void HideLine()
    {
        _lineRenderer.positionCount = 0;
    }

    public Vector3 getHitPoint()
    {
        return hitPoint;
    }

    public void startCountdown(float duration)
    {
        StartCoroutine(Countdown(duration));
    }


    private IEnumerator Countdown(float duration)
    {
        float normalizedTime = 1f;

        while (normalizedTime >= 0f)
        {
            normalizedTime -= Time.deltaTime / duration;

            int color_index = (int)(normalizedTime * (float)colors.Count);

            _lineRenderer.material.color = colors[color_index];

            yield return null;
        }
    }
}
