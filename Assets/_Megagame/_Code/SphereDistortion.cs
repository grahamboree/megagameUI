using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class SphereDistortion : MonoBehaviour {
	[SerializeField] float SecondsPerRotation = 3f;

	[Header("Noise")] [SerializeField] float NoiseMagnitude = 0.5f;
	[SerializeField] float NoiseFrequency = 1f;
	[SerializeField] float NoiseSpeed = 1f;
	[SerializeField] float NoiseThreshold = 0.75f;

	//////////////////////////////////////////////////

	MeshFilter meshFilter;
	Vector3[] originalVerts;

	//////////////////////////////////////////////////

	#region MonoBehaviour
	void Start() {
		meshFilter = GetComponent<MeshFilter>();
		SetBarycentricUVS(meshFilter.mesh);
		originalVerts = meshFilter.mesh.vertices;
	}

	void Update() {
		float norm = (Time.time % SecondsPerRotation) / SecondsPerRotation;
		transform.rotation = Quaternion.Euler(0, -360 * norm, 0);

		PerturbVertsWithNoise();
	}
	#endregion

	/// Sets the UV's on channel 0 to each vert's barycentric coordinate.
	/// Duplicates vertices because each face has a unique value.
	void SetBarycentricUVS(Mesh mesh) {
		var meshTris = mesh.triangles;
		var meshVerts = mesh.vertices;

		var uvs = new List<Vector3>(meshVerts.Length);
		var verts = new List<Vector3>(meshVerts.Length);
		var tris = new List<int>(meshTris.Length);

		for (int i = 0; i < meshTris.Length; i += 3) {
			tris.Add(verts.Count);
			verts.Add(meshVerts[meshTris[i + 0]]);
			uvs.Add(new Vector3(1, 0, 0));

			tris.Add(verts.Count);
			verts.Add(meshVerts[meshTris[i + 1]]);
			uvs.Add(new Vector3(0, 1, 0));

			tris.Add(verts.Count);
			verts.Add(meshVerts[meshTris[i + 2]]);
			uvs.Add(new Vector3(0, 0, 1));
		}

		mesh.SetVertices(verts);
		mesh.SetTriangles(tris, 0);
		mesh.SetUVs(0, uvs);
	}

	void PerturbVertsWithNoise() {
		var verts = new Vector3[originalVerts.Length];
		for (int v = 0; v < verts.Length; v++) {
			float timeOffset = Time.time * NoiseSpeed;
			float noise = Perlin.noise(
				NoiseFrequency * originalVerts[v].x + timeOffset,
				NoiseFrequency * originalVerts[v].y + timeOffset,
				NoiseFrequency * originalVerts[v].z + timeOffset
			);

			// High-pass filter noise values.
			noise -= NoiseThreshold;
			noise = Mathf.Clamp01(noise / (1 - NoiseThreshold));

			verts[v] = originalVerts[v] * (1 + NoiseMagnitude * noise);
		}

		meshFilter.mesh.vertices = verts;
	}
}
