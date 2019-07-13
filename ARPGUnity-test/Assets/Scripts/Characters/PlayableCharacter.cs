using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player.Scripts
{
    //Status component
//    [RequireComponent(typeof(CharacterHealth))]

    //Movement components
//    [RequireComponent(typeof(NavMeshAgent))]
//    [RequireComponent(typeof(CharacterMovement))]

    //Combat Components

    public abstract class PlayableCharacter : Character
    {
        protected override void Start()
        {
           // Debug.Log("PlayableCharacter");
        }

        protected override void Update()
        {
        }
    }
}