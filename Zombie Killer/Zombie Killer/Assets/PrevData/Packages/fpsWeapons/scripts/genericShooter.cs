using System;
using UnityEngine;
using System.Collections;
using ControlFreak2;
using TacticalAI;
using Random = UnityEngine.Random;


public class genericShooter : MonoBehaviour
{

    public Vector3 normalposition;
    public Vector3 aimposition;
    public Vector3 retractPos;

    public float aimFOV = 45f;
    public float normalFOV = 65f;
    public float weaponnormalFOV = 32f;
    public float weaponaimFOV = 20f;
    public float speed = 1f;


    public AudioSource myAudioSource;

    public AudioSource fireAudioSource;
    public AudioClip emptySound;
    public AudioClip fireSound;

    public AudioClip readySound;
    public AudioClip reloadSound;


    public int projectilecount = 1;
    private float inaccuracy = 0.02f;
    public float spreadNormal = 0.08f;
    public float spreadAim = 0.02f;
    public float force = 500f;
    public float damage = 0f;
    public float range = 100f;

    public float recoil;
    public float maxrecoil;


    public AnimationClip fireAnim;
    public float fireAnimSpeed = 1.1f;

    public AnimationClip reloadAnim;
    public AnimationClip readyAnim;
    // public AnimationClip idleAnim;
    // public AnimationClip stayAnim;

    public AnimationClip hideAnim;
    public GameObject shell;

    public Transform shellPos;

    public float shellejectdelay = 0;
    public int ammo = 200;
    public int currentammo = 20;
    public int clipSize = 20;

    public Transform muzzle;
    public Transform muzzlesmoke;
    public Transform clipShell;


    public Camera weaponcamera;
    public Transform recoilCamera;
    public float runXrotation = 20f;
    public float runYrotation = 0f;
    public Vector3 runposition = Vector3.zero;
    private float nextField;
    private float weaponnextfield;


    private Vector3 wantedrotation;
    private bool canaim = true;

    private bool canfire = true;
    private bool canreload = true;
    private bool retract = false;
    private bool isreloading = false;

    public Transform grenadethrower;
    public Transform meleeweapon;
    public Transform rayfirer;
    public Transform player;

    public raycastfire weaponfirer;
    playercontroller playercontrol;
    weaponselector inventory;
    camerarotate cameracontroller;
    Animation myanimation;

    public Transform bulletTransform;
    public float autoShootDistance;


    private void Awake()
    {
        weaponfirer = rayfirer.GetComponent<raycastfire>();
        playercontrol = player.GetComponent<playercontroller>();
        myanimation = GetComponent<Animation>();
        cameracontroller = recoilCamera.GetComponent<camerarotate>();
        PlayerPrefs.SetInt("Totalammo", clipSize);
    }

    private void Start()
    {
        clipSize = currentammo;
        weaponselector.Instance.CurrentClipSize(clipSize);
        nextField = normalFOV;
        weaponnextfield = weaponnormalFOV;
        // myanimation.Stop();
        //Invoke(nameof(Onstart), 0.1f);
        Onstart();
        
    }

    RaycastHit hit;
    private Outline enemy;
    private bool isOutline = true;

    private void Update()
    {
        if (ControlFreak2.CF2Input.GetButton("Reload"))
        {
            if (currentammo != clipSize && ammo > 0)
            {
                Reload();

            }
        }

        if (Physics.Raycast(muzzle.position, transform.TransformDirection(Vector3.forward), out hit, autoShootDistance))
        {
            //AutoShoot and Aim Color Changed
            Debug.DrawRay(muzzle.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //if (PlayerPrefs.GetInt("AutoShoot") == 1)
            //{ 
            //	if (hit.collider.CompareTag("HitBox") && hit.collider.gameObject.layer == 14 || hit.collider.CompareTag("uzAIZombie") && hit.collider.gameObject.layer == 8)
            //	{
            //		if (currentammo == 0)
            //		{
            //			Reload();
            //		}
            //		else
            //		{
            //			Fire();
            //			InGameProperties.Instance.CrossAssistColor();
            //		}
            //	}
            //	else
            //	{
            //		InGameProperties.Instance.CrossAimColor();
            //	}

            //}
            if(hit.collider.gameObject.layer == 8)
            {
                if (GameplayHandler.Instance.IsAutoShoot)
                    Fire();
            }
            // Outline 
            if (hit.collider.CompareTag("HitBox") && hit.collider.gameObject.layer == 14)
            {
                enemy = hit.transform.GetComponent<HitBox>().myScript.transform.GetComponent<HealthScript>().EnemyMeshRender.GetComponent<Outline>();

                // playerrotate.Instance.Pointer_Aim_Assist();
                enemy.enabled = true;
            }
            else
            {
                if (enemy != null)
                {
                    enemy.enabled = false;
                    if (PlayerPrefs.GetString("Mode") != "MultiPlayer")
                    {
                        LevelsController.Instance.EnemiesInList();
                    }
                    enemy = null;
                }
                // playerrotate.Instance.Normel_Aim_Assist();
            }
        }


        float step = speed * Time.deltaTime;
        weaponfirer.inaccuracy = inaccuracy;
        float newField = Mathf.Lerp(Camera.main.fieldOfView, nextField, Time.deltaTime * 2);
        float newfieldweapon = Mathf.Lerp(weaponcamera.fieldOfView, weaponnextfield, Time.deltaTime * 2);
        Camera.main.fieldOfView = newField;
        weaponcamera.fieldOfView = newfieldweapon;

        inventory.currentammo = currentammo;
        if (ControlFreak2.CF2Input.GetButton("ThrowGrenade") && !myanimation.isPlaying && inventory.grenade > 0 && canfire)
        {
            if (Time.timeSinceLevelLoad > (inventory.lastGrenade + 1))
            {
                inventory.lastGrenade = Time.timeSinceLevelLoad;
                StartCoroutine(SetThrowGrenade());
            }
        }
        // if (ControlFreak2.CF2Input.GetButton("Knife") && !myanimation.isPlaying && inventory.grenade>0 && canfire)
        // {
        // 	// if(Time.timeSinceLevelLoad>(inventory.lastGrenade+1)){
        // 	// 	inventory.lastGrenade=Time.timeSinceLevelLoad;			
        // 		StartCoroutine(SetPunchHitter());
        // 	// }
        // }
        if (ControlFreak2.CF2Input.GetButton("Melee") && !myanimation.isPlaying && canfire)
        {
            StartCoroutine(SetMelee());
        }
        if (retract)
        {
            canfire = false;
            canaim = false;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, retractPos, step * 2f);
            weaponnextfield = weaponnormalFOV;
            nextField = normalFOV;
        }
        else if (playercontrol.running)
        {
            canfire = false;
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, runposition, step);
            wantedrotation = new Vector3(runXrotation, runYrotation, 0f);
            weaponnextfield = weaponnormalFOV;
            nextField = normalFOV;
        }
        else
        {
            canfire = true;
            wantedrotation = Vector3.zero;

            if (((ControlFreak2.CF2Input.GetButton("Fire2") || ControlFreak2.CF2Input.GetAxis("Aim") > 0.1)) && canaim && !playercontrol.running)
            {
                // stopIdleAnim();
                inaccuracy = spreadAim;
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, aimposition, step);
                weaponnextfield = weaponaimFOV;
                nextField = aimFOV;
                recoil = 0f;     //Recoil Minimum
                                 // zoomEnabled = true;																				// add for zoom is enabled
            }
            else
            {
                // zoomEnabled = false;				// add for zoom is not enabled
                inaccuracy = spreadNormal;
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, normalposition, step);
                weaponnextfield = weaponnormalFOV;
                nextField = normalFOV;
                recoil = maxrecoil;       //Recoil Maximum
            }

        }

        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(wantedrotation), step * 3f);

        if (currentammo == 0 || currentammo <= 0)
        {

            if (ammo <= 0)
            {
                canfire = false;
                canreload = false;
                if ((ControlFreak2.CF2Input.GetButton("Fire2") || ControlFreak2.CF2Input.GetAxis("Fire1") > 0.1) && !myAudioSource.isPlaying)
                {
                    if (!myAudioSource.isPlaying)
                    {
                        myAudioSource.PlayOneShot(emptySound);
                    }
                }

            }
            else
            {
                Reload();
            }
        }

        if (!isreloading && canfire)
        {
            if (weaponnextfield == weaponaimFOV)
                inventory.ShowAim(false);
            else
                inventory.ShowAim(true);
            if ((ControlFreak2.CF2Input.GetButton("Fire2") || ControlFreak2.CF2Input.GetAxis("Fire1") > 0.1))
            {
                Fire();
            }
        }
        else
        {
            inventory.ShowAim(true);
        }

    }

    private void DoRetract()
    {
        myanimation.Play(hideAnim.name);
        Reload();
    }

    private void Onstart()
    {
        myAudioSource.Stop();
        fireAudioSource.Stop();

        if (weaponfirer == null) weaponfirer = rayfirer.GetComponent<raycastfire>();
        weaponfirer.inaccuracy = inaccuracy;
        weaponfirer.damage = damage;
        weaponfirer.range = range;
        weaponfirer.force = force;
        weaponfirer.projectilecount = projectilecount;

        if (inventory == null)
        {
            inventory = player.GetComponent<weaponselector>();
            //Init the Current Weapon with ammo value
        }
        inventory.ShowAim(false);

        myanimation.Stop();
        if (isreloading)
        {
            Reload();
        }
        else
        {
            clipShell.gameObject.SetActive(true);
            myAudioSource.clip = readySound;
            myAudioSource.loop = false;
            myAudioSource.volume = 1;
            myAudioSource.Play();
            myanimation.Play(readyAnim.name);
            canaim = true;
            canfire = true;
            inventory.ShowAim(true);
        }

       // UIHandler.Instance.UpdateAmmoText();

    }

    private void Fire()
    {
        if (!myanimation.isPlaying)
        {
            float randomZ = Random.Range(-0.05f, -0.01f);

            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + randomZ);

            cameracontroller.dorecoil(recoil);

            StartCoroutine(Flashthemuzzle());
            weaponfirer.currentmuzzle = muzzle;
            weaponfirer.fire();

            fireAudioSource.clip = fireSound;
            fireAudioSource.pitch = 0.9f + 0.1f * Random.value;
            Debug.Log("Play Sound");
            fireAudioSource.Play();

            myanimation[fireAnim.name].speed = fireAnimSpeed;
            myanimation.Play(fireAnim.name);

            currentammo -= 1;
            StartCoroutine(Ejectshell(shellejectdelay));
            if (currentammo <= 0)
            {
                Reload();
            }


           UIHandler.Instance.UpdateAmmoText();
        }

    }




    public void Reload()
    {
        if (
            !myanimation.isPlaying && canreload && !isreloading)
        {
            StartCoroutine(Setreload(myanimation[reloadAnim.name].length));
            StartCoroutine(DeactivateShell(myanimation[reloadAnim.name].length * 0.5f));
            myAudioSource.clip = reloadSound;
            myAudioSource.loop = false;
            myAudioSource.volume = 1;
            myAudioSource.Play();
            myanimation.Play(reloadAnim.name);
            InGameProperties.Instance.ReloadStart();

           UIHandler.Instance.UpdateAmmoText();
            //UIHandler.Instance.ReloadBulletUpdate();


            Debug.Log("Dddddddddd");
        }
    }

    private void DoNormal()
    {
        Onstart();
    }

    private IEnumerator Ejectshell(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        GameObject shellInstance;
        shellInstance = Instantiate(shell, shellPos.transform.position, shellPos.transform.rotation) as GameObject;
        yield return null;
        shellInstance.GetComponent<Rigidbody>().AddRelativeForce(60, 70, 0);
        shellInstance.GetComponent<Rigidbody>().AddRelativeTorque(500, 20, 800);
        shellInstance.transform.localRotation = transform.localRotation * Quaternion.Euler(0, Random.Range(-90f, 90f), 0);
    }

    private IEnumerator DeactivateShell(float waitTime)
    {
        clipShell.gameObject.SetActive(false);
        yield return new WaitForSeconds(waitTime);
        clipShell.gameObject.SetActive(true);
    }

    private IEnumerator Setreload(float waitTime)
    {

        playercontrol.canclimb = false;
        inventory.canswitch = false;
        int oldammo = currentammo;
        isreloading = true;

        canaim = false;
        yield return new WaitForSeconds(waitTime * .5f);
        currentammo = 0 + (Mathf.Clamp(clipSize, clipSize - oldammo, ammo));
        ammo -= Mathf.Clamp(clipSize, clipSize, ammo);
        //ammo -= (clipSize - currentammo);
        yield return new WaitForSeconds(waitTime * .5f);
        isreloading = false;
        canaim = true;
        inventory.canswitch = true;
        playercontrol.canclimb = true;

        InGameProperties.Instance.ReloadEnd();
    }

    private IEnumerator Flashthemuzzle()
    {

        ParticleSystem smoke = muzzlesmoke.GetComponent<ParticleSystem>();
        smoke.Emit(1);
        muzzle.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
        muzzle.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzle.gameObject.SetActive(false);

    }

    private IEnumerator SetThrowGrenade()
    {

        canfire = false;
        retract = true;
        grenadethrower.gameObject.SetActive(true);
        grenadethrower.gameObject.BroadcastMessage("throwstuff");
        Animation throwerAnimation = grenadethrower.GetComponent<Animation>();

        yield return new WaitForSeconds(throwerAnimation.clip.length);
        retract = false;
        canaim = true;
        canfire = true;
        grenadethrower.gameObject.SetActive(false);

    }
    // private IEnumerator SetPunchHitter()
    // {
    // 	
    // 	canfire = false;
    // 	retract = true;
    // 	punch.gameObject.SetActive(true);
    // 	// grenadethrower.gameObject.BroadcastMessage("throwstuff");
    // 	Animation throwerAnimation = punch.GetComponent<Animation> ();
    //
    // 	yield return new WaitForSeconds(throwerAnimation.clip.length);
    // 	retract = false;
    // 	canaim = true;
    // 	canfire = true;
    // 	punch.gameObject.SetActive(false);
    // 	
    // }

    private void PickAmmo(int inventoryAmmo)
    {

        ammo = inventoryAmmo;

    }

    private IEnumerator SetMelee()
    {
        if (!meleeweapon.gameObject.activeInHierarchy)
        {
            retract = true;
            canfire = false;
            meleeweapon.gameObject.SetActive(true);
            meleeweapon.gameObject.BroadcastMessage("melee");
            Animation meleeAnimation = meleeweapon.GetComponent<Animation>();
            yield return new WaitForSeconds(meleeAnimation.clip.length);
            retract = false;
            canaim = true;
            canfire = true;
            meleeweapon.gameObject.SetActive(false);
        }
    }


}

