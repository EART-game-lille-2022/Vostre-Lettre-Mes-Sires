using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NelowGames {
    [ExecuteAlways]
    public class PhysicalSprite : MonoBehaviour {
        public Sprite sprite;
        Sprite _s;
        public Color color = Color.white;

        Mesh _Mesh;

        public bool compensateIsoHeight = true;
        public bool flip = false;
        public bool bothSides = false;
        

        #if UNITY_EDITOR
        public Material defaultMat;
        void Reset()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if(sr) {
                sprite = sr.sprite;
                color = sr.color;
                DestroyImmediate(sr);
                MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
                renderer.material = defaultMat;
                gameObject.AddComponent<MeshFilter>();
                Generate();
            }
        }
        #endif

        private void OnValidate() {
            Generate();
        }

        private void Start() {
            Generate();
        }

        void Update() {
            if(_s != sprite) {
                Generate();
            }
        }

        // [Button]
        void Generate() { 
            if(sprite == null) return;
            _s = sprite;

            MeshFilter filter = GetComponent<MeshFilter>();
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetTexture("_MainTex", sprite.texture);
            renderer.SetPropertyBlock(block);

            if(_Mesh == null) {
                _Mesh = new Mesh();
                _Mesh.name = sprite.name;
                filter.mesh = _Mesh;
            }

            Vector2[] verts = sprite.vertices;
            ushort[] tris = sprite.triangles;



            // Vector3[] vertices = new Vector3[verts.Length];
            // Color[] cols = new Color[verts.Length];
            // if(compensateIsoHeight) {
            //     for (int i = 0; i < verts.Length; i++) {
            //         vertices[i] = verts[i];
            //         vertices[i].y *= 1.573132f;
            //         cols[i] = color;
            //     }
            // } else
            // for (int i = 0; i < verts.Length; i++) {
            //     vertices[i] = verts[i];
            //     cols[i] = color;
            // }

            // float h = compensateIsoHeight ? 1.573132f : 1f;
            float h = compensateIsoHeight ? 1.32f : 1f;
            int triCount = tris.Length;
            int vertCount = verts.Length;
            int[] triangles;
            
            Vector3[] vertices;
            Vector2[] uv;
            Color[] cols;

            if(bothSides) {
                triangles = new int[triCount*2];
                vertices = new Vector3[vertCount*2];
                cols = new Color[vertCount*2];
                uv = new Vector2[vertCount*2];
                
                for (int i = 0; i < vertCount; i++)
                    uv[i] = uv[vertCount+i] = sprite.uv[i];

                for (int i = 0; i < triCount; i+=3) {
                    triangles[i] = tris[i];
                    triangles[i+1] = tris[i+2];
                    triangles[i+2] = tris[i+1];
                }
                for (int i = 0; i < triCount; i++)
                    triangles[triCount+i] = vertCount+tris[i];

                for (int i = 0; i < vertCount*2; i++) {
                    vertices[i] = verts[i%vertCount];
                    vertices[i].y *= h;
                    cols[i] = color;
                }

            } else { 
                triangles = new int[triCount];
                vertices = new Vector3[vertCount];
                cols = new Color[vertCount];
                uv = sprite.uv;
                
                for (int i = 0; i < vertCount; i++) {
                    vertices[i] = verts[i];
                    vertices[i].y *= h;
                    cols[i] = color;
                }
                
                if(flip) {
                    for (int i = 0; i < triCount; i+=3) {
                        triangles[i] = tris[i];
                        triangles[i+1] = tris[i+2];
                        triangles[i+2] = tris[i+1];
                    }
                } else {
                    for (int i = 0; i < tris.Length; i++)
                        triangles[i] = tris[i];
                }
            }

            _Mesh.Clear();
            _Mesh.name = sprite.name;
            _Mesh.vertices = vertices;
            _Mesh.triangles = triangles;

            _Mesh.colors = cols;
            _Mesh.uv = uv;

            _Mesh.RecalculateBounds();
            _Mesh.RecalculateNormals();
            _Mesh.RecalculateTangents();
        }
    }
}
