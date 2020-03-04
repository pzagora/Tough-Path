namespace ToughPath.Gameplay {
    using UnityEngine;

    [CreateAssetMenu(fileName = "ObjectPool", menuName = "Create Object Pool Template")]
    public class Pool: ScriptableObject, ISpawnable {

        public string     Name;
        public GameObject Prefab;
        public int        Size;

        public GameObject Spawn() {
            return Instantiate(Prefab);
        }

    }
}
