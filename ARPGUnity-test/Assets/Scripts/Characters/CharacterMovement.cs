using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class CharacterMovement : MonoBehaviour
{
    protected bool CanMove;
    protected bool isMoving;
    public abstract void MovePlayer(Vector3 pos);
    public abstract void MovePlayer(Vector2 axys);

    public abstract void ChangeMoveState(bool canMove);
}