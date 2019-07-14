#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Icosahedron {
	class VertsTris {
		public List<Vector3> verts = new List<Vector3>();
		public List<int> tris = new List<int>();

		public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3) {
			tris.Add(verts.Count);
			verts.Add(v1);
			tris.Add(verts.Count);
			verts.Add(v2);
			tris.Add(verts.Count);
			verts.Add(v3);
		}
	}

	public static Mesh Generate(int subdivisions = 0) {
		var t = (1.0f + Mathf.Sqrt(5.0f)) / 2.0f;

		var icosahedron = new VertsTris {
			// 12 vertices of a icosahedron
			verts = new List<Vector3> {
				new Vector3(-1, t, 0).normalized,
				new Vector3(1, t, 0).normalized,
				new Vector3(-1, -t, 0).normalized,
				new Vector3(1, -t, 0).normalized,

				new Vector3(0, -1, t).normalized,
				new Vector3(0, 1, t).normalized,
				new Vector3(0, -1, -t).normalized,
				new Vector3(0, 1, -t).normalized,

				new Vector3(t, 0, -1).normalized,
				new Vector3(t, 0, 1).normalized,
				new Vector3(-t, 0, -1).normalized,
				new Vector3(-t, 0, 1).normalized
			},

			// 20 triangles of the icosahedron
			tris = new List<int> {
				// 5 faces around point 0
				0, 11, 5,
				0, 5, 1,
				0, 1, 7,
				0, 7, 10,
				0, 10, 11,

				// 5 adjacent faces
				1, 5, 9,
				5, 11, 4,
				11, 10, 2,
				10, 7, 6,
				7, 1, 8,

				// 5 faces around point 3
				3, 9, 4,
				3, 4, 2,
				3, 2, 6,
				3, 6, 8,
				3, 8, 9,

				// 5 adjacent faces
				4, 9, 5,
				2, 4, 11,
				6, 2, 10,
				8, 6, 7,
				9, 8, 1
			}
		};

		// refine triangles
		for (int i = 0; i < subdivisions; i++) {
			icosahedron = Subdivide(icosahedron);
		}

		// Convert into a Unity mesh.
		var mesh = new Mesh();
		mesh.SetVertices(icosahedron.verts);
		mesh.SetTriangles(icosahedron.tris, 0);
		return mesh;
	}

	static VertsTris Subdivide(VertsTris orig) {
		var result = new VertsTris();

		for (int i = 0; i < orig.tris.Count; i += 3) {
			var v1 = orig.verts[orig.tris[i + 0]];
			var v2 = orig.verts[orig.tris[i + 1]];
			var v3 = orig.verts[orig.tris[i + 2]];

			// replace triangle by 4 triangles
			var a = Vector3.Normalize((v1 + v2) / 2f);
			var b = Vector3.Normalize((v2 + v3) / 2f);
			var c = Vector3.Normalize((v3 + v1) / 2f);

			result.AddTriangle(v1, a, c);
			result.AddTriangle(v2, b, a);
			result.AddTriangle(v3, c, b);
			result.AddTriangle(a, b, c);
		}
		return result;
	}
}

public class IcosahedronGenerator : EditorWindow {
	int subdivisions;

	[MenuItem("Window/Icosahedron Generator")]
	static void Open() {
		GetWindow<IcosahedronGenerator>().Show();
	}

	void OnGUI() {
		GUILayout.Label("Base Settings", EditorStyles.boldLabel);
		subdivisions = EditorGUILayout.IntSlider("Subdivisions", subdivisions, 0, 10);

		if (GUILayout.Button("Generate")) {
			var path = EditorUtility.SaveFilePanelInProject(
				"Save sphere mesh", "sphere.asset", "asset", "message??");

			if (path.Length > 0) {
				AssetDatabase.CreateAsset(Icosahedron.Generate(subdivisions), path);
			}
		}
	}
}
#endif
