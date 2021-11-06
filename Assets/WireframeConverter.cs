using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireframeConverter : MonoBehaviour
{
	Mesh shadedMesh;
	Mesh wireframeMesh;

	MeshFilter meshFilter;

	bool wireframe = false;

	// Start is called before the first frame update
	void Start()
    {
		meshFilter = GetComponent<MeshFilter>();

		shadedMesh = meshFilter.mesh;
		wireframeMesh = Instantiate(shadedMesh);

		for (int subMesh = 0; subMesh < wireframeMesh.subMeshCount; ++subMesh)
		{
			int[] indices = wireframeMesh.GetIndices(subMesh);
			int[] lineIndices = new int[(indices.Length / 3) * 6];

			int line = 0;
			for (int i = 0; i < indices.Length; i += 3)
			{
				lineIndices[line * 2] = indices[i];
				lineIndices[line * 2 + 1] = indices[i + 1];
				++line;

				lineIndices[line * 2] = indices[i + 1];
				lineIndices[line * 2 + 1] = indices[i + 2];
				++line;

				lineIndices[line * 2] = indices[i + 2];
				lineIndices[line * 2 + 1] = indices[i];
				++line;
			}
			wireframeMesh.SetIndices(lineIndices, MeshTopology.Lines, subMesh);
		}
	}

    // Update is called once per frame
    void Update()
    {
        if (Wireframe.wireframeEnabled && !wireframe)
        {
			meshFilter.mesh = wireframeMesh;
			wireframe = true;
        }

		if (!Wireframe.wireframeEnabled && wireframe)
		{
			meshFilter.mesh = shadedMesh;
			wireframe = false;
		}
	}
}
