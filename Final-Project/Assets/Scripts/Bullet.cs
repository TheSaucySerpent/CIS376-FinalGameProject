using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision objectHit) {
        // destroy the bullet when it collides with a target
        if (objectHit.gameObject.CompareTag("Target")) {
            print("hit " + objectHit.gameObject.name + " !"); 
            CreateBulletImpactEffect(objectHit);
            Destroy(gameObject);
        }

        // destroy the bullet when it collides with a wall
        if (objectHit.gameObject.CompareTag("Wall")) {
            print("hit a wall"); 
            CreateBulletImpactEffect(objectHit);
            Destroy(gameObject);
        }

        // destroy the bullet when it collides with a wall
        if (objectHit.gameObject.CompareTag("Beer Bottle")) {
            print("hit a beer bottle"); 
            // call the shatter function for the hit bottle
            objectHit.gameObject.GetComponent<BeerBottle>().Shatter();

            // We will not destory the bullet on impact, it will get destroyed
            // according to its own lifetime (allows bullet to pass through the bottle)
        }
    }

    void CreateBulletImpactEffect(Collision objectHit) {
        ContactPoint contact = objectHit.contacts[0]; // get the first contact point

        // cannot instantiate effect since bullet is being instantiated itself
        // instead need a global reference to the bullet impact effect
        // You can’t directly assign an effect prefab from within the bullet prefab because Unity doesn’t allow self-contained prefab references to other prefabs being instantiated at runtime (think of it like a circular reference issue)
        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point, // point of contact (to instantiate at)
            Quaternion.LookRotation(contact.normal) // rotation when we hit the target
        );

        // scale down the size of the bullet hole
        hole.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

        // make the hole a child of what was hit
        hole.transform.SetParent(objectHit.gameObject.transform);
    }  
}
