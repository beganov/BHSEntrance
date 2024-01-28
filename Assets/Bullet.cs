using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Bullet : MonoBehaviour
{
    public float timeDestroy = 3f;
    public Rigidbody rb;
    private TextMeshProUGUI HealthTxt;
    private Transform target;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        Vector3 targetDirection = target.position - transform.position;
        Vector3 rotate = transform.eulerAngles;
        rotate.z = Mathf.Atan(-transform.position.x / (transform.position.y - target.position.y)) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(rotate);
        Invoke("DestroyBullet", timeDestroy);
        rb.velocity = targetDirection*1f;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
        if (collision.transform == target)
        {
            HealthTxt = GameObject.FindGameObjectWithTag("FagTMP").GetComponent<TextMeshProUGUI>();
            if (int.Parse(HealthTxt.text)>0)
                HealthTxt.text = (int.Parse(HealthTxt.text) - 1).ToString();
        }
    }
    void DestroyBullet()
    {
        Destroy(this.gameObject);
    }
}