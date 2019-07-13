using System;
using UnityEngine;


namespace Characters
{
    public interface ISync<T>
    {
        Action<T> OnSendData { get; set; }
        void MoveFromServer(Vector3 pos);
    }
}