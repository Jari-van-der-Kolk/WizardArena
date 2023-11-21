using UnityEngine;

public class EnemyFunctions
{
    public static void KnockBack(Rigidbody2D rb, Vector2 dir)
    {
        rb.velocity = Vector2.zero;
        rb.velocity = dir;
    }
}
