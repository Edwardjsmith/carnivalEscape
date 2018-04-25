using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class unitTests {

	[Test]
	public void unitTestsSimplePasses() {

        var player = new playerMove();
        var spotlight = player.playerSpotlight;
        // Use the Assert class to test conditions.
        Assert.AreEqual(player.transform.position.x, player.playerSpotlight.transform.position.x);
        
	}

	// A UnityTest behaves like a coroutine in PlayMode
	// and allows you to yield null to skip a frame in EditMode
	[UnityTest]
	public IEnumerator unitTestsWithEnumeratorPasses() {
		// Use the Assert class to test conditions.
		// yield to skip a frame
		yield return null;
	}
}
