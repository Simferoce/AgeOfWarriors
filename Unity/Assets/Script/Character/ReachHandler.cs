using UnityEngine;

namespace Game
{
    public class ReachHandler : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D boxCollider2D;

        public void SetReach(float reach)
        {
            boxCollider2D.offset = new Vector2(reach / 2f, boxCollider2D.offset.y);
            boxCollider2D.size = new Vector2(reach, boxCollider2D.size.y);
        }
    }
}
