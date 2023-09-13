using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MeshRenderer))]

public class CharacterRotate : MonoBehaviour, IPointerUpHandler,IPointerDownHandler
{

    #region ROTATE
    public float _sensitivity = 1f;
    private Vector3 _mouseReference;
    private Vector3 _mouseOffset;
    private Vector3 _rotation = Vector3.zero;
    private bool _isRotating;

    public Vector3 ObjectRotation;
    public GameObject Character;
    private float speed = 0.6f;
    private Vector3 OriginalScale;
    private Vector3 OriginalPos;

    #endregion

//     void Update()
//     {
//         Character.transform.Rotate (ObjectRotation * speed * Time.deltaTime);
// /*      Debug.Log("character Rotategdsgsdg");
//         Debug.Log("character Rotate: "+ _isRotating);*/
//         if(_isRotating)
//         {
//             // offset
//             _mouseOffset = (ControlFreak2.CF2Input.mousePosition - _mouseReference);
//
//             // apply rotation
//             _rotation.y = -(_mouseOffset.x + _mouseOffset.y) * _sensitivity;
//              /*Debug.Log("character Rotate");*/
//             // rotate
//             Character.transform.Rotate(_rotation);
//
//             // store new mouse position
//             _mouseReference = ControlFreak2.CF2Input.mousePosition;
//         }
//     }
//
     public void OnPointerDown(PointerEventData eventData)
     {
//         // rotating flag
//         _isRotating = true;
//
//         // store mouse position
//         _mouseReference = ControlFreak2.CF2Input.mousePosition;
// //        Character.transform.localScale = OriginalScale;
     }
//
     public void OnPointerUp(PointerEventData eventData)
     {
//         // rotating flag
//         _isRotating = false;
//         OriginalScale = Character.transform.localScale;
// //        gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x,gameObject.transform.localScale.y,gameObject.transform.localScale.z);
     }

}
