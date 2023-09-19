using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Random = UnityEngine.Random;

public class GenerateBox : XRBaseInteractable
{
    Rigidbody m_Rigidbody;
    public Rigidbody m_Box;
    public float m_force = 1.0f;
    private bool _isHovered = false;
    private void Start()
    {
        //Debug.Log("[DEBUG-hwlee]GenBox Start() called, m_RigidBody = " + m_Rigidbody);
    }
    private void Update()
    {
        //Debug.Log("[DEBUG-hwlee]GenBox Update() called, m_RigidBody = " + m_Rigidbody);
        if (_isHovered && Input.GetKeyDown(KeyCode.Joystick1Button14))
        {
            Debug.Log("Joystick1Button14 key was pressed.");
            GenBox();
        }
        if (Input.anyKeyDown)
        {
            KeyCode keyCode = getCurrentKeyDown();
            Debug.Log("[" + keyCode + "] key was pressed.");
        }

    }
    public KeyCode getCurrentKeyDown()
    {
        KeyCode finalKeyCode = KeyCode.None;
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode))) { if (Input.GetKey(kcode)) { finalKeyCode = kcode; } }
        if (finalKeyCode == KeyCode.None)
        {
            Debug.Log("Cannot find the key.");
        }
        return finalKeyCode;
    }
    protected override void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody>();
        if (m_Rigidbody == null)
            Debug.LogError("GenerateBox Interactable does not have a required Rigidbody.", this);
    }
    public void GenBox()
    {
        Debug.Log("[DEBUG-hwlee]GenBox called, m_RigidBody = " + m_Rigidbody);
        if (!_isHovered)
        {
            Debug.Log("[DEBUG-hwlee]GenBox called, m_RigidBody NOT Hovered = " + m_Rigidbody);
            return;
        }
        Rigidbody box = Instantiate(m_Box, m_Rigidbody.transform);
        Color customColor = new Color(Random.value, Random.value, Random.value, 1.0f);
        box.GetComponent<Renderer>().material.SetColor("_Color", customColor);
        Vector3 direction = new Vector3(Random.value * 2 - 1, Random.value, Random.value * 2 - 1);
        direction.Normalize();
        Debug.Log("[DEBUG-hwlee]GenBox called, m_RigidBody = " + m_Rigidbody + ", direction = " + direction);
        box.AddForce(direction * m_force);
    }
    public void Hovered()
    {
        Debug.Log("[DEBUG-hwlee]Hovered called, m_RigidBody = " + m_Rigidbody);
    }
    public void Activated()
    {
        Debug.Log("[DEBUG-hwlee]Activated called, m_RigidBody = " + m_Rigidbody);
    }
    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        //Debug.Log("[DEBUG-hwlee]OnSelectEntered called, args = "+args);
        //GenBox();
    }
    protected override void OnSelectEntering(SelectEnterEventArgs args)
    {
        base.OnSelectEntering(args);
        //Debug.Log("[DEBUG-hwlee]OnSelectEntering called, args = "+args);
    }
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        //Debug.Log("[DEBUG-hwlee]OnHoverEntered called, args = "+args);
        _isHovered = true;
        Debug.Log("[DEBUG-hwlee]OnHoverEntered called, args = " + args + ", _isHovered = " + _isHovered);
    }
    protected override void OnHoverEntering(HoverEnterEventArgs args)
    {
        base.OnHoverEntering(args);
        Debug.Log("[DEBUG-hwlee]OnHoverEntering called, args = " + args);
        //_isHovered = true;
    }
    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        //Debug.Log("[DEBUG-hwlee]OnHoverExited called, args = " + args);
        _isHovered = false;
        Debug.Log("[DEBUG-hwlee]OnHoverExited called, args = " + args + ", _isHovered = " + _isHovered);
    }
}