namespace HRenderer.Common;

public class ObjectModel {
    public int vertexCount;
    public List<float> positions;
    public List<float> uvs;
    public List<uint> indices;
}

public class ObjectParser {
    
    public static ObjectModel ParseObj(string objPath) {
        var lines = File.ReadAllLines(objPath);
        var vLines = new List<string>();
        var vtLines = new List<string>();
        var vertexCount = 0;
        var position = new List<float>();
        var uv = new List<float>();
        var indices = new List<uint>();
        uint idx = 0;
        // vertexStrings
        foreach (var line in lines) {
            if (line.StartsWith("v ")) {
                var l = line.Substring(2).Trim();
                vLines.Add(l);;
            }else if (line.StartsWith("vt ")) {
                var l = line.Substring(4).Trim();
                vtLines.Add(l);
            }else if (line.StartsWith("f ")) {

                var strings = line.Substring(2).Split(' ');
                foreach (var s in strings) {
                    var face = s.Split('/');
                    var vIdx = int.Parse(face[0])-1;
                    var vtIdx = int.Parse(face[1])-1;

                    var vs = vLines[vIdx].Split(' ');
                    foreach (var v in vs) {
                        position.Add(float.Parse(v));
                    }
                    var vts = vtLines[vtIdx].Split(' ');
                    foreach (var vt in vts) {
                        uv.Add(float.Parse(vt));
                    }
                    
                    vertexCount++;
                    
                    indices.Add(idx);
                    idx++;
                }
            }
        }

        return new ObjectModel() {vertexCount = vertexCount, positions = position, uvs = uv, indices = indices};
    }
}

/***

# List of geometric vertices, with (x,y,z[,w]) coordinates, w is optional and defaults to 1.0. 
v 0.123 0.234 0.345 1.0 
v ... 
... 
# List of texture coordinates, in (u, v [,w]) coordinates, these will vary between 0 and 1, w is optional and defaults to 0. 
vt 0.500 1 [0] 
vt ... 
... 
# List of vertex normals in (x,y,z) form; normals might not be unit vectors. 
vn 0.707 0.000 0.707 
vn ... 
... 
# Parameter space vertices in ( u [,v] [,w] ) form; free form geometry statement ( see below ) 
vp 0.310000 3.210000 2.100000 
vp ... 
... 
# Polygonal face element (see below) 
f 1 2 3 
f 3/1 4/2 5/3 
f 6/4/1 3/5/3 7/6/5 
f 7//1 8//2 9//3 
f ... 
...

 */