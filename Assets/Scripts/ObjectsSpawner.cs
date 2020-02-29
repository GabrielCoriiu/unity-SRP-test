using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ObjectsSpawner : MonoBehaviour
{
	static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");

	[SerializeField] private int amount = 100;
	[SerializeField] private float spawnRate = 1;
	[SerializeField] private float spread = 1;
	[SerializeField] private GameObject prefab = null;

	[Header("GFX")]
	[SerializeField] private bool randomizeColor = false;
	[SerializeField] private Gradient randomizeLUT = new Gradient();

	private int count = 0;
	private float lastSpawnedAtTime = -1f;
	private MaterialPropertyBlock propertyBlock = null;

	protected void Awake()
	{
		propertyBlock = new MaterialPropertyBlock();
	}

	protected void Update()
    {
		float spawnInterval = 1f / spawnRate;
		if (count < amount && Time.timeSinceLevelLoad - lastSpawnedAtTime >= spawnInterval && SpawnObject())
		{
			lastSpawnedAtTime = Time.timeSinceLevelLoad;
			++count;
		}
	}

	protected bool SpawnObject()
	{
		if (prefab == null)
		{
			Debug.LogError("Please assign a prefab");
			return false;
		}

		GameObject obj = Instantiate(
			prefab,
			transform.position + Random.insideUnitSphere * spread,
			Quaternion.identity
		);

		Renderer renderer = obj.GetComponent<Renderer>();
		if (renderer != null)
		{
			renderer.GetPropertyBlock(propertyBlock);
			propertyBlock.SetColor(
				BaseColorID,
				randomizeColor ? randomizeLUT.Evaluate(Random.value) : randomizeLUT.Evaluate(0f)
			);
			renderer.SetPropertyBlock(propertyBlock);
		}

		return true;
	}
}
