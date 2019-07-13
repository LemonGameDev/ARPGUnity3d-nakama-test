using UnityEngine;

namespace Characters
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Character : MonoBehaviour
    {
        // Start is called before the first frame update
        protected abstract void  Start();

        // Update is called once per frame
        protected abstract void Update();
    }
}
      