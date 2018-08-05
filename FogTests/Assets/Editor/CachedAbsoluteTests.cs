using UnityEngine;
using NUnit.Framework;

public class CachedAbsoluteTests {

    
    [Test]
    public void CachedAbsoluteTestsSimplePasses() {
        // Use the Assert class to test conditions.
        VisionSystem.CachedAbs abs = new VisionSystem.CachedAbs();
        int max = 100;
        abs.Init(max);

        for (int i = 0; i < max; i++)
        {
            for (int j = 0; j < max; j++)
            {
                Assert.AreEqual(abs.Abs(i - j), Mathf.Abs(i - j));
            }
        }

    }

}
