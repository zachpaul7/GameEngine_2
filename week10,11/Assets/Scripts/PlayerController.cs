using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
    public NetworkVariable<Color> NetColor = new NetworkVariable<Color>();
    public NetworkVariable<float> JumpPower = new NetworkVariable<float>();
    private InputControlAsset inputActions;

    DateTime centuryBegin = new DateTime(2001, 1, 1);
    [SerializeField] private float jumpPower = 5.0f;
    [SerializeField] private float moveSpeed = 1.0f;

    private void Awake()
    {
        inputActions = new InputControlAsset();
        inputActions.Player.Enable();
        inputActions.Player.Jump.performed += Jump_performed;
        inputActions.Player.Move.performed += Move_performed;
        inputActions.Player.Fire.performed += Fire_performed;
        //centuryBegin = new DateTime(2001, 1, 1);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        inputActions.Player.Enable();
        inputActions.Player.Jump.performed -= Jump_performed;
        inputActions.Player.Move.performed -= Move_performed;
        inputActions.Player.Fire.performed -= Fire_performed;
    }

    private void Fire_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }
    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        throw new System.NotImplementedException();
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("[PlayerController:Jump_performed]NetworkManager.Singleton.IsServer : " + NetworkManager.Singleton.IsServer + ", IsOwner : " + IsOwner);
        if (NetworkManager.Singleton.IsServer && IsOwner) //only server can actual jump for its own object
        {
            DoJump(jumpPower);
            //Rigidbody rb = this.GetComponent<Rigidbody>();
            //rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            //JumpPower.Value = jumpPower;
        }
        else if (IsOwner) // client can only request jump to server if it is Owner
        {
            SubmitJumpRequestServerRpc(jumpPower);
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {

            Move();
            SetColor();
        }
    }

    public void SetColor()
    {
        if (NetworkManager.Singleton.IsServer) //if server, set material color and NetColor.Value, then set isColorSet to be true
        {
            var randomColor = GetRandomColor();
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.color = randomColor;
            //isColorSet = true;
            NetColor.Value = randomColor;
        }
        else
        {
            SubmitColorRequestServerRpc();
        }
    }

    public void Move()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            var randomPosition = GetRandomPositionOnPlane();
            transform.position = randomPosition;
            Position.Value = randomPosition;
        }
        else
        {
            SubmitPositionRequestServerRpc();
        }
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        Position.Value = GetRandomPositionOnPlane();
        transform.position = Position.Value;
    }
    [ServerRpc]
    void SubmitColorRequestServerRpc(ServerRpcParams rpcParams = default)
    {
        var color = GetRandomColor();
        NetColor.Value = color;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.color = color;
        //Debug.Log("[PalyerController:SubmitColorRequestServerRpc]NetColor.Value : " + NetColor.Value.ToString());
    }
    [ServerRpc]
    void SubmitJumpRequestServerRpc(float jumpPower, ServerRpcParams rpcParams = default)
    { // do actual jump at server for client request
        DoJump(jumpPower);
        //Rigidbody rb = this.GetComponent<Rigidbody>();
        //rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        //JumpPower.Value = jumpPower;
        //Debug.Log("[PalyerController:SubmitJumpRequestServerRpc]jumpPower : " + jumpPower);
    }
    [ServerRpc]
    void SubmitMoveRequestServerRpc(Vector2 dir, long ticks, ServerRpcParams rpcParams = default)
    { // do actual move at server for client request
        DoMove(dir, ticks);
    }
    void CheckMove()
    {
        InputActionPhase phase = inputActions.Player.Move.phase;
        //Debug.Log("[PalyerController:CheckMove]inputActions.Player.Move.ReadValue<Vector2>() : " + phase.ToString()+", IsServer : "+ NetworkManager.Singleton.IsServer);
        if (phase == InputActionPhase.Started)
        {
            Vector2 dir = inputActions.Player.Move.ReadValue<Vector2>();
            //Debug.Log("[PalyerController:CheckMove]inputActions.Player.Move.ReadValue<Vector2>() : " + dir.ToString());
            if (IsOwner)
            {
                if (NetworkManager.Singleton.IsServer) //only server can actual jump for its own object
                {
                    DoMove(dir, -1); // server action
                }
                else
                {
                    long elapsedTicks = DateTime.Now.Ticks - centuryBegin.Ticks;
                    SubmitMoveRequestServerRpc(dir, elapsedTicks);
                }
            }
        }
    }
    void DoMove(Vector2 dir, long ticks)
    {
        long deltaTicks = (ticks < 0) ? 0 : DateTime.Now.Ticks - centuryBegin.Ticks - ticks;
        float deltaTime = (deltaTicks > 0 ? 0 : Time.deltaTime) + (float)deltaTicks / TimeSpan.TicksPerSecond;
        //Debug.Log("[PalyerController:CheckMove]moveSpeed : " + moveSpeed.ToString()+ ", deltaTime : "+ deltaTime+", ticks : "+ticks+", delayTime : "+(float)deltaTicks/TimeSpan.TicksPerSecond+", IsServer : "+ NetworkManager.Singleton.IsServer);
        Vector3 move = new Vector3(dir.x, 0, dir.y) * moveSpeed * Time.deltaTime;
        transform.Translate(move);
        Position.Value = transform.position;
    }

    void DoJump(float jumpPower)
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        JumpPower.Value = jumpPower;
    }
    static UnityEngine.Color GetRandomColor()
    {
        var color = new UnityEngine.Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value, 1.0f); ;
        return color;
    }
    static Vector3 GetRandomPositionOnPlane()
    {
        return new Vector3(UnityEngine.Random.Range(-3f, 3f), 1f, UnityEngine.Random.Range(-3f, 3f));
    }

    void Update()
    {
        if (NetworkManager.Singleton.IsServer) // synchronize position over network clients
        {
            Position.Value = transform.position;
            Renderer renderer = GetComponent<Renderer>();
            NetColor.Value = renderer.material.color;
        }
        else
        {
            transform.position = Position.Value;
            Renderer renderer = GetComponent<Renderer>();
            renderer.material.color = NetColor.Value;
        }
        CheckMove();
    }
}