using UnityEngine;

[CreateAssetMenu(fileName = "NewSpellData", menuName = "Spell System/Spell Data")]
public class ProjectileSpellStratagy : SpellStratagy
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float duration = 5f;
    [SerializeField] private float radius = 7.5f;
    public override void CastSpell(Transform origin)
    {
        var shield = Instantiate(_projectilePrefab, origin.position, Quaternion.identity, origin);   
    }
}

