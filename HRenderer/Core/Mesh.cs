using System.IO;
using HRenderer.Common;

namespace HRenderer.Core {
    public class Mesh {
        // 顶点大小
        protected int _stride;
        // 顶点描述
        protected VertexFormat[] _attribInfo;
        // 顶点buffer
        protected DataStream _vertexBuffer;
        // 顶点索引buffer
        protected uint[] _indiceBuffer;

        public Mesh(VertexFormat[] attribInfo) {
            this._attribInfo = attribInfo;
            this._vertexBuffer = new DataStream(false);
        }
        
    }
    
    public enum VertexType {
        Float32,
        UInt8,
        Int32,
    }

    public class VertexFormat {
        public string name;
        public VertexType type;
        public int num;
        public bool normalize;

        public VertexFormat(string name, VertexType type, int num, bool normalize = false) {
            this.name = name;
            this.type = type;
            this.num = num;
            this.normalize = normalize;
        }
    }
}

