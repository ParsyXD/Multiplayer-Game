using UnityEngine;
using Mirror;

public class Weapon : NetworkBehaviour
{
    public GameObject muzzleFlashPrefab;
    public AudioSource syncedAudio;
    public AudioSource localAudio;
    public AudioClip gunshotSound;
    public float raycastDistance = 10f;
    public int primaryDamage = 20;
    public int secondaryDamage = 2;
    public LayerMask hitLayerMask; // Set this in the Inspector to only detect the player layer (or a custom layer for players).
    public float fireRate;
    float nextFireTime = 0f;
    PointSystem pointSystem;
    PlayerMovementAdvanced movement;

    void Start()
    {
        pointSystem = GetComponentInParent<PointSystem>();
        movement = GetComponentInParent<PlayerMovementAdvanced>();
    }
    void Update()
    {
        if (!hasAuthority)
            return;

        if (Input.GetButtonDown("PrimaryFire"))
        {
            CmdFireWeapon(primaryDamage);
        }
        if (Input.GetButton("SecondaryFire")&& Time.time >= nextFireTime)
        {
            CmdFireWeapon(secondaryDamage);
            nextFireTime = Time.time + fireRate;
        }

    }

[Command]
void CmdFireWeapon(int damage)
{
    RpcSpawnEffects();

    // Perform the raycast from the player's position forward
    if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, raycastDistance, hitLayerMask))
    {
        Debug.Log("Hit");

        // Apply damage on the server
        Health health = hit.collider.GetComponent<Health>();
        if (health != null && health.health > 0)
        {
            health.TakeDamage(damage);
            pointSystem.Damage(damage);
            Debug.Log("Damaged");

            if (health.health <= 0)
            {
                pointSystem.Killed();
                Debug.Log("Killed");
            }
        }
    }
}

void RpcSpawnEffects()
{
    Debug.Log("played sound");
    //GameObject muzzleFlash = Instantiate(muzzleFlashPrefab, position, Quaternion.LookRotation(normal));
    syncedAudio.PlayOneShot(gunshotSound, 0.13f);
}
}