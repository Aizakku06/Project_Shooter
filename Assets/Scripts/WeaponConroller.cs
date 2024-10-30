using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum recoilOption // Enum de opciones de retroceso (del script anterior)
{
    foward,
    right
}

public enum ShotType
{
    Manual,
    Automatic
}

public class WeaponController : MonoBehaviour
{
    public WeaponController weapon;

    [Header("References")]
    public Transform weaponMuzzle;
    public Animator animator;

    public int currentAmmo { get; private set; }
    public GameObject owner { set; get; }

    private float lastTimeShoot = Mathf.NegativeInfinity;
    private Transform cameraPlayerTransform;
    private bool isReloading;

    [Header("General")]
    public LayerMask hittableLayers;
    public GameObject bulletHolePrefab;

    [Header("Shoot Parameters")]
    public float fireRange = 200; // Rango de disparo (del script anterior)
    public float recoilForce = 4f; // Fuerza de retroceso (del script anterior)
    public float fireRate = 0.6f; // Frecuencia de disparo (del script anterior)
    public int maxAmmo = 8; // Máxima munición (del script anterior)

    [Header("Reload Parameters")]
    public float reloadTime = 1.5f; // Tiempo de recarga (del script anterior)

    [Header("Sounds & Visuals")]
    public GameObject flashEffect;

    [SerializeField] recoilOption ro; // Opción de retroceso (del script anterior)
    [SerializeField] float recoilMultiplier = -1;

    [SerializeField] recoilOption shootOption; // Opción de dirección de disparo (del script anterior)
    [SerializeField] float shootMultiplier = -1;

    public Color gizmoColor = Color.red; // Color para los Gizmos (del script anterior)

    private void Awake()
    {
        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        currentAmmo = weapon.maxAmmo;
        EventManager.current.UpdateBulletsEvent.Invoke(currentAmmo, weapon.maxAmmo);
    }

    public void SetAmmo(int newAmmo)
    {
        currentAmmo = newAmmo;
        EventManager.current.UpdateBulletsEvent.Invoke(currentAmmo, weapon.maxAmmo);
    }

    private void OnEnable()
    {
        isReloading = true;
        StartCoroutine(Draw(weapon.drawTime));
    }

    private void Update()
    {
        if (!isReloading)
        {
            if (weapon.shotType == ShotType.Manual)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    TryShoot();
                }
            }
            else if (weapon.shotType == ShotType.Automatic)
            {
                if (Input.GetButton("Fire1"))
                {
                    TryShoot();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5f);
    }

    private IEnumerator Draw(float time)
    {
        yield return new WaitForSeconds(time - 0.15f);
        isReloading = false;
    }

    private bool TryShoot()
    {
        if (lastTimeShoot + weapon.fireRate < Time.time)
        {
            if (currentAmmo >= 1)
            {
                HandleShoot();
                currentAmmo -= 1;
                EventManager.current.UpdateBulletsEvent.Invoke(currentAmmo, weapon.maxAmmo);
                return true;
            }
        }

        return false;
    }

    private void HandleShoot()
    {
       // GameObject flashClone = Instantiate(weapon.flashEffectPrefab, weaponMuzzle.position, Quaternion.Euler(transform.forward.x, transform.forward.y, transform.forward.z), transform);
        //Destroy(flashClone, 1f);

        AddRecoil();

        RaycastHit hit;
        if (Physics.Raycast(cameraPlayerTransform.position, cameraPlayerTransform.forward, out hit, weapon.fireRange, hittableLayers) && hit.collider.gameObject != owner)
        {
            GameObject bulletHoleClone = Instantiate(weapon.bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal), hit.collider.gameObject.transform);
            Destroy(bulletHoleClone, 4f);

            
        }

        AddBulletTrial();

        lastTimeShoot = Time.time;
    }

    private void AddRecoil()
    {
        transform.Rotate(-weapon.recoilForce, 0f, 0f);
        if (ro == recoilOption.foward) // Añade retroceso basado en recoilOption
        {
            transform.position = transform.position - transform.forward * recoilMultiplier * (weapon.recoilForce / 50f);
        }
        else
        {
            transform.position = transform.position - transform.right * recoilMultiplier * (weapon.recoilForce / 50f);
        }
    }

    private void AddBulletTrial()
    {
        // if (weapon.bulletTrialPrefab == null) return;
        // Efecto de trazo de bala (vacío en el script anterior)
    }

    IEnumerator Reload()
    {
        if (isReloading || currentAmmo == weapon.maxAmmo) yield break;

        isReloading = true;
        if (animator) animator.SetTrigger("Reloading");

        yield return new WaitForSeconds(weapon.reloadTime - 0.15f);
        currentAmmo = weapon.maxAmmo;
        EventManager.current.UpdateBulletsEvent.Invoke(currentAmmo, weapon.maxAmmo);
        isReloading = false;
    }

    public void Hide()
    {
        if (animator)
        {
            isReloading = false;
            animator.SetTrigger("Hiding");
        }
    }

    void OnDrawGizmos() // Nuevo método para visualizar Gizmos
    {
        Gizmos.color = gizmoColor;

        if (shootOption == recoilOption.foward)
        {
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * shootMultiplier * fireRange);
        }
        else
        {
            Gizmos.DrawLine(transform.position, transform.position + transform.right * shootMultiplier * fireRange);
        }

        Gizmos.DrawWireSphere(transform.position + transform.forward * fireRange, 0.5f);
    }
}