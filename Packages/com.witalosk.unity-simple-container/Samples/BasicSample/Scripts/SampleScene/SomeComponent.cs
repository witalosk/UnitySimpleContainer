using System;
using UnityEngine;

namespace UnitySimpleContainer.Sample
{
    public class SomeComponent : MonoBehaviour, ISomeComponent
    {
        private void Start()
        {
            Debug.Log("Some Component Start");
        }

        public void DoSomething()
        {
            Debug.Log("Done Something!");
        }
    }
}