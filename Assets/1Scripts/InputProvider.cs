using Fusion;
using UnityEngine;

public class InputProvider : SimulationBehaviour, INetworkInput
{
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        PlayerInput playerInput = new PlayerInput();
        if (Input.GetKey(KeyCode.Mouse0))
        {
            playerInput.Buttons.Set(MyButtons.Attack, true);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            playerInput.Buttons.Set(MyButtons.Jump, true);
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            playerInput.Buttons.Set(MyButtons.Left, true);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            playerInput.Buttons.Set(MyButtons.Right, true);
        }
        
        if (Input.GetKey(KeyCode.W))
        {
            playerInput.Buttons.Set(MyButtons.Forward, true);
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            playerInput.Buttons.Set(MyButtons.Backward, true);
        }
        
        playerInput.MousePos = Input.mousePosition;
        bool resultInput = input.Set(playerInput);
        
        Debug.Log("Sending input : " + resultInput);
        // Reset the input struct to start with a clean slate
        // when polling for the next tick
    }
}

public struct PlayerInput : INetworkInput
{
    public NetworkButtons Buttons;
    public Vector3 MousePos;
}

enum MyButtons
{
    Forward = 0,
    Backward = 1,
    Left = 2,
    Right = 3,
    Jump = 4,
    Attack = 5,
}