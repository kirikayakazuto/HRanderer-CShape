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
        protected float[] _vbo;
        public float[] Vbo => this._vbo;
        
        // 顶点索引buffer
        protected uint[] _ibo;
        public uint[] Ibo => this._ibo;
        

        public VertexFormat[] attribInfo => this._attribInfo;

        public Mesh(VertexFormat[] attribInfo) {
            this._attribInfo = attribInfo;
            foreach (var vertexFormat in attribInfo) {
                this._stride += vertexFormat.num;
            }
        }

        public string[] GetAttribNames() {
            var names = new string[this.attribInfo.Length];
            for (var i = 0; i < names.Length; i++) {
                names[i] = this.attribInfo[i].name;
            }
            return names;
        }
        public void GetVertexAttribs(uint v, in Dictionary<string, Vector4> vec4Dict, in Dictionary<string, Vector2> vec2Dict) {
            uint offset = 0;
            foreach (var attrib in this.attribInfo) {
                switch (attrib.num) {
                    case 4:
                        var vec4 = vec4Dict.ContainsKey(attrib.name) ? vec4Dict[attrib.name] : Vector4.Create();
                        Array.Copy(this.Vbo, v + offset, vec4.data, 0, 4);
                        vec4Dict[attrib.name] = vec4;
                        break;
                    case 2:
                        var vec2 = vec2Dict.ContainsKey(attrib.name) ? vec2Dict[attrib.name] : Vector2.Create();
                        Array.Copy(this.Vbo, v + offset, vec2.data, 0, 2);
                        vec2Dict[attrib.name] = vec2;
                        break;
                    default:
                        break;
                }
                offset += attrib.num;
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

