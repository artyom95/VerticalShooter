using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class BorderLineBehaviour: MonoBehaviour
{
    private void OnTriggerEnter(Collider someCollider)
    {
        if (someCollider== null )
        {
            return;
        }

        if ( someCollider.TryGetComponent<Bullet>(out var bullet))
        {
            Destroy(bullet.gameObject);
        }
    }   
}