using UnityEngine;
using System.Collections;

public class SlugEnemy : RandomWalkEnemy {

	public Vector3 minSize, maxSize;
	public Oscillator squishOscillator;

	protected override void Polish ()
	{
		base.Polish ();

		transform.forward = DirectionHandler.Instance.DirectionToVector(direction);
		transform.localScale = Vector3.Lerp(minSize, maxSize, squishOscillator.Displacement());
	}
}
