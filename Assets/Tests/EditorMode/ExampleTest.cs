using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ExampleTest
    {
        private GameObject _root;
        
        
        [OneTimeSetUp]
        public void Setup()
        {
            _root = new GameObject();
            _root.AddComponent<ExampleScript>();
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ExampleTestSimplePasses()
        {
            // Use the Assert class to test conditions
            Assert.AreEqual(6f, _root.GetComponent<ExampleScript>().multiply2Numbers(3f, 2f));
        }

    }
}
