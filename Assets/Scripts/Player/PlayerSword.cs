using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    [SerializeField]
    private int _damage;

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject gObj = col.gameObject;
        if (gObj.CompareTag(TagConst.ENEMY)) gObj.GetComponent<Enemy>().OnHit(_damage);
    }
}
