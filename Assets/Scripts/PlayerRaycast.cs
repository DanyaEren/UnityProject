using UnityEngine;

public class PlayerRaycast : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    [SerializeField] float _raycastDistance = 2f;
    [SerializeField] private LayerMask _raycastLayerMask;
    private DraggableObject _currentlyDraggedObject = null;
    [SerializeField] private float _draggableObjectDistance = 1f;
    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)) 
        {
            RaycastHit hit;
            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward,out hit, _raycastDistance, _raycastLayerMask))
            {
                Debug.Log("We hit " + hit.collider.gameObject.name);
                if (hit.collider.TryGetComponent(out OpenableObject openable))
                {
                    openable.OpenorClose();
                }
            }
            else
            {
                Debug.Log("We hit NOTHING");
            }
        }
        Debug.DrawLine(_mainCamera.transform.position, _mainCamera.transform.position + _mainCamera.transform.forward * _raycastDistance, Color.white);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out hit, _raycastDistance))
                LayerMask.GetMask("DraggableObject");
            {
                if (hit.collider.TryGetComponent(out DraggableObject draggableObject))
                {
                    draggableObject.StartFollowingObject();
                    _currentlyDraggedObject = draggableObject;
                    Debug.Log(_currentlyDraggedObject);
                }
            }
        }

        if (_currentlyDraggedObject != null)
        {
            Vector3 targetPosition = _mainCamera.transform.position + _mainCamera.transform.forward * _draggableObjectDistance;
            _currentlyDraggedObject.SetTargetPosition(targetPosition);
        }

        if(Input.GetMouseButtonUp(0))
        {
            if(_currentlyDraggedObject != null)
            {
                _currentlyDraggedObject.StopFollowingObject();
                Debug.Log("StopFollowingObject");
                _currentlyDraggedObject = null;
            }
        }


    }
}
