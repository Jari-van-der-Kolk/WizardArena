using System;
using Job.SpellSystem.Data;
using UnityEngine;

namespace Job.SpellSystem.Spells
{
    public class ElementBall : Spell
    {
        ElementBallData _data;
        public override void CastSpell()
        {
            _data = Holder.spellData.GetData<ElementBallData>();
            Instantiate(_data.particle, transform);

            SphereCollider collider = gameObject.AddComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.radius = _data.radius;
            
            Rigidbody rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = _data.useGravity;
            
            rb.AddForce(transform.forward * _data.launchSpeed, ForceMode.Impulse);
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(_data.damageOnContact);
            }
        }
    }
}