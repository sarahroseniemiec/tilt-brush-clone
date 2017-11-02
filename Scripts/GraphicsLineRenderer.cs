using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]


public class GraphicsLineRenderer : MonoBehaviour
{

    public Material lineMaterial;
    private Mesh meshLine;
    private Vector3 startPoint;
    private float lineSize = .1f;
    //you need two points for the mesh renderer and the first time you only get one point
    private bool firstQuad = true;

    // Use this for initialization
    void Start()
    {
        //Initializes everything
        //Mesh filter keeps track of our mesh
        meshLine = GetComponent<MeshFilter>().mesh;
        //Mesh renderer visualizes the mesh
        GetComponent<MeshRenderer>().material = lineMaterial;
    }

    public void SetWidth(float width)
    {
        lineSize = width;

    }

    public void AddPoint(Vector3 point)
    {
        if (startPoint != Vector3.zero)
        {
            AddLine(meshLine, MakeQuad(startPoint, point, lineSize, firstQuad));
            firstQuad = false;
        }
        startPoint = point;
    }


    Vector3[] MakeQuad(Vector3 startPoint, Vector3 endPoint, float width, bool all)
    {
        width = width / 2; //half on top, half on bottom

        Vector3[] quads;

        if (all) //if its the first quad you need to draw all 4 therefore the array is length 4 otherwise you are just adding 2 points so the array is 2
            quads = new Vector3[4];
        else
            quads = new Vector3[2];

        Vector3 normal = Vector3.Cross(startPoint, endPoint); //cross product of start vector and end vector that creates our plane between and we take the cross product that comes out at us
        Vector3 line = Vector3.Cross(normal, endPoint - startPoint); // this is the line we want to draw - perpendicular to the velocity and the normal
        line.Normalize();

        if (all)
        {
            quads[0] = transform.InverseTransformPoint(startPoint + line * width);
            quads[1] = transform.InverseTransformPoint(startPoint + line * -width);
            quads[2] = transform.InverseTransformPoint(endPoint + line * width);
            quads[3] = transform.InverseTransformPoint(endPoint + line * -width);
        }
        else
        {
            quads[0] = transform.InverseTransformPoint(endPoint + line * width);
            quads[1] = transform.InverseTransformPoint(endPoint + line * -width);
        }
        return quads;
    }

    void AddLine(Mesh meshLine, Vector3[] quad)
    {
        //veticies length
        int vl = meshLine.vertices.Length;

        Vector3[] verticesList = meshLine.vertices; //stores the vertices
        verticesList = resizeVertices(verticesList, 2 * quad.Length); //resizes the list to add how ever many more we want - in the case we want to duplicate the 4 we have so we can use it for the front and back

        //add the vertices with this for loop 
        for (int i = 0; i < 2 * quad.Length; i += 2)
        {
            verticesList[vl + i] = quad[i / 2];
            verticesList[vl + i + 1] = quad[i / 2];
        }
        Vector2[] uvs = meshLine.uv;
        uvs = resizeUVs(uvs, 2 * quad.Length);
        //indexes our image for each quad think of a rect specifying where each point on the rect should be. assigning (0, 0) (0, 1) (1, 0) and (1, 1) values to the line we draw
        if (quad.Length == 4)
        {
            uvs[vl] = Vector2.zero;
            uvs[vl + 1] = Vector2.zero;
            uvs[vl + 2] = Vector2.right;
            uvs[vl + 3] = Vector2.right;
            uvs[vl + 4] = Vector2.up;
            uvs[vl + 5] = Vector2.up;
            uvs[vl + 6] = Vector2.one;
            uvs[vl + 7] = Vector2.one;
        }
        else
        {
            if (vl % 8 == 0)
            {
                uvs[vl] = Vector2.zero;
                uvs[vl + 1] = Vector2.zero;
                uvs[vl + 2] = Vector2.right;
                uvs[vl + 3] = Vector2.right;
            }
            else
            {
                uvs[vl] = Vector2.up;
                uvs[vl + 1] = Vector2.up;
                uvs[vl + 2] = Vector2.one;
                uvs[vl + 3] = Vector2.one;

            }
        }


        // this actually draws the triangle 

        int tl = meshLine.triangles.Length;
        int[] ts = meshLine.triangles;
        ts = resizeTriangles(ts, 12);

        // the front facing quad is made up of 2 triangles and the back facing quad is made of 2 triangles so for each triange we give the index of the vertex we want to draw

        if (quad.Length == 2)
            vl -= 4;

        //front facing quad if you draw it counterclockwise it will be facing us, if you draw clockwise it will be facing away from the normal
        ts[tl] = vl;
        ts[tl + 1] = vl + 2;
        ts[tl + 2] = vl + 4;

        ts[tl + 3] = vl + 2;
        ts[tl + 4] = vl + 6;
        ts[tl + 5] = vl + 4;

        //back facing quad 
        ts[tl + 6] = vl + 5;
        ts[tl + 7] = vl + 3;
        ts[tl + 8] = vl + 1;

        ts[tl + 9] = vl + 5;
        ts[tl + 10] = vl + 7;
        ts[tl + 11] = vl + 3;

        //reassign them to the mesh
        meshLine.vertices = verticesList;
        meshLine.triangles = ts;
        meshLine.RecalculateBounds();
        meshLine.RecalculateNormals();

    }

    Vector3[] resizeVertices(Vector3[] ovs, int ns)
    {
        Vector3[] nvs = new Vector3[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
        return nvs;
    }

    Vector2[] resizeUVs(Vector2[] uvs, int ns)
    {
        Vector2[] nvs = new Vector2[uvs.Length + ns];
        for (int i = 0; i < uvs.Length; i++) nvs[i] = uvs[i];
        return nvs;
    }

    int[] resizeTriangles(int[] ovs, int ns)
    {
        int[] nvs = new int[ovs.Length + ns];
        for (int i = 0; i < ovs.Length; i++) nvs[i] = ovs[i];
        return nvs;
    }

    // Update is called once per frame
    void Update()
    {

    }
}