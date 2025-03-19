using UnityEngine;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

public class Building : MonoBehaviour
{
    [SerializeField]
    private Collider2D _triggerCollider;
    [SerializeField]
    private Collider2D _innerCollider;

    private bool _isOnTheBuildingArea;
    private bool _isAcrossingSomething;

    public Collider2D TriggerCollider { get { return _triggerCollider; } }
    public Collider2D InnerCollider { get { return _innerCollider; } }
    public bool IsOnTheBuildingArea { get { return _isOnTheBuildingArea; } }
    public bool IsAcrossingSomething { get { return _isAcrossingSomething; } }

    private void FixedUpdate()
    {
        _isAcrossingSomething = _innerCollider.IsTouchingLayers(-1);
        _isOnTheBuildingArea = _triggerCollider.IsTouchingLayers(LayerMask
                .GetMask("Ground", "Building"));
    }
}
