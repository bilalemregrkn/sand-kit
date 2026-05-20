using UnityEngine;

namespace SandFall
{
    [CreateAssetMenu(fileName = "SpritePack", menuName = "SandFall/SpritePack")]
    public class SpritePack : ScriptableObject
    {
        public Sprite[] sprites;

        public Sprite Random() =>
            sprites != null && sprites.Length > 0
                ? sprites[UnityEngine.Random.Range(0, sprites.Length)]
                : null;
    }
}
