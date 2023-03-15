namespace HRenderer.Common; 

public class ObjectParser {
    public static int VertexCount = 0;
    private static List<float> _v = new List<float>();
    public static List<float> v => _v;
    private static List<float> _vt = new List<float>();
    public static List<float> vt => _vt;
    private static List<float> _vn = new List<float>();
    public static List<float> vn => _vn;
    private static List<float> _vp = new List<float>();
    public static List<float> vp => _vp;

    public static List<uint> f = new List<uint>();

    public static void ParseObj(string objPath) {
        var lines = File.ReadAllLines(objPath);
        // vertexStrings
        foreach (var line in lines) {
            if (line.StartsWith("v ")) {
                var strings = line.Substring(2).Split(' ');
                foreach (var s in strings) {
                    ObjectParser._v.Add(float.Parse(s));
                }
                ObjectParser.VertexCount++;
            }else if (line.StartsWith("vt ")) {
                var strings = line.Substring(4).Split(' ');
                foreach (var s in strings) {
                    ObjectParser._vt.Add(float.Parse(s));
                }
            }else if (line.StartsWith("f ")) {
                var strings = line.Substring(2).Split(' ');
                foreach (var s in strings) {
                    ObjectParser.f.Add(uint.Parse(s.Split('/')[0]));
                }
            }
        }
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