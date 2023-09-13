using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class playerrotate : MonoBehaviour {
	public GameData GData;
	#region GlobalInstance
	public static playerrotate LocalInstance;
	public static playerrotate Instance
	{
		get
		{
			LocalInstance = GameObject.FindObjectOfType<playerrotate>();
			return LocalInstance;
		}
	}
	#endregion
	public float  sensitivityX  = 5.5f;
	
	public float minimumX = -360f;
	public float maximumX  = 360f;
	public float rotationX = 0f;
	public float aimSens= 2f;
	
	public float normalSens= 0f;
	
	public bool climbing = false;
	public float smooth = 0.5f;
	bool stop=false;

	float offsetX = 0f;

	float totalOffsetX = 0f;

	float resetSpeed = 1f;
	float resetDelay = 0f;

	float maxKickback = 0f;

	float smoothFactor = 2f;

	private Quaternion tRotation;
	public Vector3 ladderforward;


	private Quaternion originalRotation;
	public bool climb = false;
	private Quaternion[] temp;
	private Quaternion smoothRotation;
	private Quaternion ladderrotation;
	
	public Slider SensitivityBar;
	public Text SensPercenetage;
	public Vector2[] zomobieModeRotation;
	private void Awake()
	{
		
		originalRotation = transform.localRotation;
		//if (!PlayerPrefs.HasKey("TouchChange"))
		//{
			//PlayerPrefs.SetFloat("TouchSens",2f);
		//}
		
		normalSens = 2;
		//SensitivityBar.value = 2;
		//SensPercenetage.text = Mathf.RoundToInt(SensitivityBar.value * 10 )+"%";

	}

	// public void Start()
	// {
	// 	// if (PlayerPrefs.GetString("Mode") == "Zombie")
	// 	// {
	// 	// 	minimumX = zomobieModeRotation[GameManager.Instance.currentLevel-1].x;
	// 	// 	maximumX = zomobieModeRotation[GameManager.Instance.currentLevel-1].y;
	// 	// }
	// }

	//public void SensApply()
	//{
	//	PlayerPrefs.SetFloat("TouchSens",SensitivityBar.value);
	//	normalSens = PlayerPrefs.GetFloat("TouchSens");
	//	SensPercenetage.text = Mathf.RoundToInt(SensitivityBar.value * 10 )+"%";
	//	PlayerPrefs.SetInt("TouchChange",1);
	//}
	
	private void Update () {

		if(ControlFreak2.CF2Input.GetKey(KeyCode.Escape) ){
			stop=!stop;
		}
		if(stop)
		{
			ControlFreak2.CFCursor.lockState = CursorLockMode.Confined;
			ControlFreak2.CFCursor.visible=true;
		}
		else
		{
			ControlFreak2.CFCursor.lockState = CursorLockMode.Locked;
		}

		if (ControlFreak2.CF2Input.GetButton("Aim"))
		{
			sensitivityX = aimSens;
		}
		else
		{
			sensitivityX = normalSens;
		}



		rotationX += ControlFreak2.CF2Input.GetAxis("Mouse X") * sensitivityX;

		float xDecrease;

		if(totalOffsetX > 0){
			xDecrease = Mathf.Clamp(resetSpeed*3f, 0, totalOffsetX);
		} 
		else 
		{
				
			xDecrease = Mathf.Clamp(resetSpeed*-3f, totalOffsetX, 0);

		}

		if(resetDelay > 0)
		{

			xDecrease = 0;

			resetDelay = Mathf.Clamp(resetDelay-3f, 0, resetDelay);

		}

		if(Random.value < .5)
				offsetX *= -1;

		if((totalOffsetX < maxKickback && totalOffsetX >= 0) || (totalOffsetX > -maxKickback && totalOffsetX <= 0))
		{

			totalOffsetX += offsetX;

		} 
		else 
		{


			resetDelay *= .5f;

		}

		rotationX = ClampAngle (rotationX, minimumX, maximumX)+ offsetX - xDecrease;

		if((ControlFreak2.CF2Input.GetAxis("Mouse X") * sensitivityX) < 0)
		{
			totalOffsetX += ControlFreak2.CF2Input.GetAxis("Mouse X") * sensitivityX;

		}



		totalOffsetX -= xDecrease;

		if(totalOffsetX < 0) 
		{

			totalOffsetX = 0;
		}





		if (climbing)
		{
			ladderrotation = Quaternion.LookRotation(ladderforward,Vector3.up);
			tRotation = ladderrotation;
			originalRotation = ladderrotation;
			rotationX = 0f;
		}
		else
		{
			Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
			tRotation =  originalRotation * xQuaternion;
		}



		float offsetVal = Mathf.Clamp(totalOffsetX * smoothFactor,1, smoothFactor);

		transform.localRotation=Quaternion.Slerp(transform.localRotation,tRotation,Time.deltaTime*30/smoothFactor*offsetVal);


	}




	float ClampAngle(float angle,float min, float max)
	{

		if (angle < -360F)

			angle += 360F;

		if (angle > 360F)

			angle -= 360F;

		return Mathf.Clamp (angle, min, max);

	}


	public void Pointer_Aim_Assist()
	{
		normalSens = 0.3f;
		aimSens = 0.01f;
		camerarotate.Instance.normalSens = 0.3f;
		camerarotate.Instance.aimSens = 0.01f;
	}

	public void Normel_Aim_Assist()
	{
		normalSens = GData.touchSens;
		aimSens = 1f;
		camerarotate.Instance.normalSens = normalSens;
		camerarotate.Instance.aimSens = aimSens;
	}
}

