using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    public float dragSpeed = 2;
    private Vector3 _dragOrigin;
    private Vector3 _dragDelta;
    private Camera _camera;
    private bool _dragging;

    private float _zoomSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _camera = Camera.main;
        _zoomSpeed = 5f;
    }
    
    
    bool MouseOverUi()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    public void OnDrag(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _dragOrigin = GetMousePosition();
        }

        if (!MouseOverUi())
        {
            _dragging = context.started || context.performed;  
        }
        
    }

    void LateUpdate()
    {
        if (!_dragging)
        {
            return;
        }
        _dragDelta = GetMousePosition() - transform.position;
        transform.position = _dragOrigin - _dragDelta;
    }

    Vector3 GetMousePosition()
    {
        return _camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    }

    public void OnScroll(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 1f)
        {
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, 
                _camera.orthographicSize - 5f, Time.deltaTime * _zoomSpeed);
        }
        
        if (context.ReadValue<float>() == -1f)
        {
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, 
                _camera.orthographicSize + 5f, Time.deltaTime * _zoomSpeed);
        }
    }
}
