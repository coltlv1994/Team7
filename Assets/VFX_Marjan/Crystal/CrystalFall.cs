using UnityEngine;

public class CrystalFall : MonoBehaviour
{
    public GameObject wholeCrystal;
    public GameObject fracturedCrystal;
    public ParticleSystem breakEffect; // Reference to the particle effect
    public float fallDelay = 1f;
    public float breakForceThreshold = 5f;

    private Rigidbody rb;
    private bool hasFallen = false;

    void Start()
    {
        rb = wholeCrystal.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        fracturedCrystal.SetActive(false);

        if (breakEffect != null)
            breakEffect.Stop(); // Ensure particle effect doesn't play at start
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasFallen)
        {
            hasFallen = true;
            Invoke("DropCrystal", fallDelay);
        }
    }

    void DropCrystal()
    {
        rb.isKinematic = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > breakForceThreshold)
        {
            BreakCrystal(collision.contacts[0].point); // Pass impact position
        }
    }

    void BreakCrystal(Vector3 impactPoint)
    {
        wholeCrystal.SetActive(false);
        fracturedCrystal.SetActive(true);

        foreach (Rigidbody piece in fracturedCrystal.GetComponentsInChildren<Rigidbody>())
        {
            piece.isKinematic = false;
            piece.AddExplosionForce(5f, impactPoint, 2f);
        }

        if (breakEffect != null)
        {
            breakEffect.transform.position = impactPoint; // Move effect to impact point
            breakEffect.Play(); // Trigger the effect
        }
    }
}
