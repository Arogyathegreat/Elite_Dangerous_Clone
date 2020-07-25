using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Rigidbody))]
public class CelestialBody : MonoBehaviour
{
    public float radius;
    public float surfaceGravity;
    public Vector3 initialVelocity;
    public string bodyName = "UNNAMED";
    Transform _meshHolder; 
    public Vector3 Velocity { get; private set; }
    public float Mass { get; private set; }
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.mass = Mass;
        Velocity = initialVelocity;
    }

    public void UpdateVelocity(CelestialBody[] allBodies, float timeStep)
    {
        foreach (var otherBody in allBodies)
        {
            if (otherBody != this)
            {
                Vector3 otherPosition = otherBody._rb.position;
                Vector3 mPosition = _rb.position;

                float sqrDst = (otherPosition - mPosition).sqrMagnitude;
                Vector3 forceDir = (otherPosition - mPosition).normalized;

                // F = Gm1m2/r^2; F = m1a, a = F/m1; a = F/m1 * Gm1m2/r^2 = FGm2/r^2 
                Vector3 acceleration = forceDir * Universe.GravitationalConstant * otherBody.Mass / sqrDst;
                Velocity += acceleration * timeStep;
            }
        }
    }

    public void UpdateVelocity(Vector3 acceleration, float timeStep)
    {
        Velocity += acceleration * timeStep;
    }

    public void UpdatePosition(float timeStep)
    {
        _rb.MovePosition(_rb.position + Velocity * timeStep);
    }

    private void OnValidate()
    {
        Mass = surfaceGravity * radius * radius / Universe.GravitationalConstant;
        _meshHolder = transform.GetChild(0);
        _meshHolder.localScale = Vector3.one * radius;
        gameObject.name = bodyName;
    }


    public Rigidbody Rigidbody
    {
        get { return _rb; }
    }

    public Vector3 Position
    {
        get { return _rb.position; }
    }
}