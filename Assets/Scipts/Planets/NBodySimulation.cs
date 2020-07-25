using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using Object = System.Object;

public class NBodySimulation : MonoBehaviour
{
    private CelestialBody[] _bodies;
    private static NBodySimulation _instance;

    void Awake()
    {
        _bodies = FindObjectsOfType<CelestialBody>();
        Time.fixedDeltaTime = Universe.PhysicsTimeStep;
        Debug.Log("Setting fixedDeltaTime to: " + Universe.PhysicsTimeStep);
    }

    void FixedUpdate()
    {
        for (int i = 0; i < _bodies.Length; i++)
        {
            Vector3 acceleration = CalculateAcceleration(_bodies[i].Position, _bodies[i]);
            _bodies[i].UpdateVelocity(acceleration, Universe.PhysicsTimeStep);
        }

        for (int i = 0; i < _bodies.Length; i++)
        {
            _bodies[i].UpdatePosition(Universe.PhysicsTimeStep);
        }
    }

    public static Vector3 CalculateAcceleration(Vector3 point, CelestialBody ignoreBody = null)
    {
        Vector3 acceleration = Vector3.zero;
        foreach (var body in Instance._bodies)
        {
            if (body != ignoreBody)
            {
                Vector3 bodyPosition = body.Position;

                float sqrDst = (bodyPosition - point).sqrMagnitude;
                Vector3 forceDir = (bodyPosition - point).normalized;
                acceleration += Universe.GravitationalConstant * body.Mass * forceDir / sqrDst;
            }
        }

        return acceleration;
    }

    public CelestialBody[] Bodies => Instance._bodies;

    static NBodySimulation Instance
    {
        get
        {
            if (!ReferenceEquals(_instance, null))
            {
                _instance = FindObjectOfType<NBodySimulation>();
            }

            return _instance;
        }
    }
}