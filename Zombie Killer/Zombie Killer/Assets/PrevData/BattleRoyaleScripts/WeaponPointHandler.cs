using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponPointHandler : MonoBehaviour
{
    [Space(10)]
    // [Header("Weapon Zoom")]
    // public GameObject Canvas;
    // public Vector3 OrignalPos;
    // public float Scale;

    [Space(10)]
    [Header("Weapon Rotation")]
    [Space(5)]
    public Vector3 ObjectRotation;
    public float RotationSpeed = 0.6f;
    public float _sensitivity = 1f;
    
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation = Vector3.zero;
    private bool _isRotating;
    private Vector3 OriginalScale;
    private Vector3 OriginalPos;

    public void OnMouseDown()
    {
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x,gameObject.transform.localScale.y,gameObject.transform.localScale.z);
        
        _isRotating = true;
    
        // store mouse position
        _mouseReference = ControlFreak2.CF2Input.mousePosition;
        
    }

    public void OnMouseUp()
    {
        // Canvas.SetActive(true);
        // gameObject.transform.position = OrignalPos;     
        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x,gameObject.transform.localScale.y,gameObject.transform.localScale.z);
        
        _isRotating = false;
        OriginalScale = gameObject.transform.localScale;
    }

    private void LateUpdate()
    {
        transform.Rotate (ObjectRotation * RotationSpeed * Time.deltaTime);
        if(_isRotating)
        {
            // offset
            _mouseOffset = (ControlFreak2.CF2Input.mousePosition - _mouseReference);

            // apply rotation
            _rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;

            // rotate
            gameObject.transform.Rotate(_rotation);

            // store new mouse position
            _mouseReference = ControlFreak2.CF2Input.mousePosition;
        }
    }
}
