using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ServiceLocator : MonoBehaviour
{
    [Header("Services")]
    private static readonly IDictionary<Type, MonoBehaviour> services = new Dictionary<Type, MonoBehaviour>();


    public static void Register<TService>(TService service) where TService : MonoBehaviour, new()
    {
        services[typeof(TService)] = service;
    }

    public static TService Get<TService>() where TService : MonoBehaviour
    {
        UnityEngine.Assertions.Assert.IsTrue(services.ContainsKey(typeof(TService)), "ServiceLocator :: Could not locate service: " + typeof(TService));
        return (TService)services[typeof(TService)];
    }
}