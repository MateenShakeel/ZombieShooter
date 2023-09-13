using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]

public class rotateController : MonoBehaviour 
{

    #region ROTATE
    public float _sensitivity = 1f;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation = Vector3.zero;
    private bool _isRotating;

    public Vector3 ObjectRotation;
    private float speed = 0.6f;
    private Vector3 OriginalScale;
    private Vector3 OriginalPos;

    #endregion

    void Update()
    {
            transform.Rotate (ObjectRotation * speed * Time.deltaTime);
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

    void OnMouseDown()
    {
        // rotating flag
        _isRotating = true;

        // store mouse position
        _mouseReference = ControlFreak2.CF2Input.mousePosition;
//        gameObject.transform.localScale = OriginalScale;
    }

    void OnMouseUp()
    {
        // rotating flag
        _isRotating = false;
        OriginalScale = gameObject.transform.localScale;
//        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x,gameObject.transform.localScale.y,gameObject.transform.localScale.z);
    }

}