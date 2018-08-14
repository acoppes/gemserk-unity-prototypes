using Gemserk.Vision;
using UnityEngine;
using NUnit.Framework;

public class CachedAbsoluteTests {

    [Test]
    public void CachedAbsoluteTestsSimplePasses() {
        // Use the Assert class to test conditions.
        var abs = new CachedIntAbsoluteValues();
        const int max = 100;
        abs.Init(max);

        for (var i = 0; i < max; i++)
        {
            for (var j = 0; j < max; j++)
            {
                Assert.AreEqual(abs.Abs(i - j), Mathf.Abs(i - j));
            }
        }

    }

}
