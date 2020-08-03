using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public static State State { get; private set; }


    /// <summary>
    /// 상태 변경 시 사용하는 함수
    /// </summary>
    /// <param name="newState">변경할 상태</param>
    public static void SetState(State newState)
    {
        State = newState;
        State.Start();
    }
}