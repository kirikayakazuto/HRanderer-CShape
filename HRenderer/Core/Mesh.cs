using System;
using System.Collections.Generic;
using HRenderer.Common;


namespace HRenderer.Core {
    
    public class Mesh {
        // 顶点大小
        protected uint _stride;
        public uint stride => this._stride;
        // 顶点描述
        protected VertexFormat[] _attribInfo;
        // 顶点buffer
        protected float[] _vertexBuffer;
        // 顶点索引buffer
        protected uint[] _indiceBuffer;
        // 
        public uint[] indiceBuffer => this._indiceBuffer;
        public float[] vertexBuffer => this._vertexBuffer;

        public VertexFormat[] attribInfo => this._attribInfo;

        public Mesh(VertexFormat[] attribInfo) {
            this._attribInfo = attribInfo;
            for (var i = 0; i < attribInfo.Length; i++) {
                this._stride += attribInfo[i].num;
            }
        }

        public string[] GetAttribNames() {
            var names = new string[this.attribInfo.Length];
            for (var i = 0; i < names.Length; i++) {
                names[i] = this.attribInfo[i].name;
            }
            return names;
        }
        public void GetVertexAttribs(uint v, in Dictionary<string, Vector4> dict) {
            foreach (var attrib in this.attribInfo) {
                var vec = Vector4.Create();
                Array.Copy(this.vertexBuffer, v, vec.data, 0, attrib.num);
                if (dict.ContainsKey(attrib.name)) {
                    dict[attrib.name] = vec;
                }
                else {
                    dict.Add(attrib.name, vec);    
                }
            }
        }
    }
    
    public enum VertexType {
        Float32,
        UInt8,
        Int32,
    }

    public class VertexFormat {
        
        public string name;
        public uint num;
        
        public VertexFormat(string name, uint num) {
            this.name = name;
            this.num = num;
        }
    }
}

