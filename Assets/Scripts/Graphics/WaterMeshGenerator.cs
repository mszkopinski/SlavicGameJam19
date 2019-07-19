using UnityEngine;

namespace SGJ
{
	public class WaterMeshGenerator : MonoBehaviour
	{
		[SerializeField]
		float waveFrequency = 0.53f;
		[SerializeField]
		float waveHeight = 0.48f;
		[SerializeField]
		float waveLength = 0.71f;
		[SerializeField]
		bool edgeBlend = true;
		[SerializeField]
		bool forceFlatShading = true;

		readonly Vector3 waveSourceVector = new Vector3(2.0f, 0.0f, 2.0f);
		
		Mesh generatedMesh;
		MeshFilter meshFilter;
		Vector3[] currentVertices;
		Camera mainCam;
 
		void Start()
		{
			mainCam = CameraController.Instance.ActiveCamera;
			mainCam.depthTextureMode |= DepthTextureMode.Depth;
			meshFilter = GetComponent<MeshFilter>();  
			GenerateLowPolyMesh();
		}
		
		void Update()
		{ 
			CalculateWavePosition();
			SetEdgeBlend(); 
		}

		void GenerateLowPolyMesh()
		{
			generatedMesh = meshFilter.sharedMesh; 
			var oldVertices = generatedMesh.vertices;
			int[] triangles = generatedMesh.triangles;
			var newVertices = new Vector3[triangles.Length];
			for (int i = 0; i < triangles.Length; i++) {
				newVertices [i] = oldVertices [triangles [i]];
				triangles [i] = i;
			}
			generatedMesh.vertices = newVertices;
			generatedMesh.triangles = triangles;
			generatedMesh.RecalculateBounds ();
			generatedMesh.RecalculateNormals ();
			currentVertices = generatedMesh.vertices;
		}

		void SetEdgeBlend()
		{
			if (!SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth)) 
			{
				edgeBlend = false;
			}
			
			if (edgeBlend) 
			{
				Shader.EnableKeyword("WATER_EDGEBLEND_ON");
				mainCam.depthTextureMode |= DepthTextureMode.Depth;
			}
			else 
			{ 
				Shader.DisableKeyword("WATER_EDGEBLEND_ON");
			}
		}

		void CalculateWavePosition()
		{
			for (int i = 0; i < currentVertices.Length; i++) 
			{
				var v = currentVertices[i];
				v.y = 0.0f;
				float dist = Vector3.Distance (v, waveSourceVector);
				dist = dist % waveLength / waveLength;
				v.y = waveHeight * Mathf.Sin (Time.time * Mathf.PI * 2.0f * waveFrequency
				                              + Mathf.PI * 2.0f * dist);
				currentVertices [i] = v;
			}
			
			generatedMesh.vertices = currentVertices;
			generatedMesh.RecalculateNormals(); 
			generatedMesh.MarkDynamic();
	
			meshFilter.mesh = generatedMesh;
		}
	}
}