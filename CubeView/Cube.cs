using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace CubeView
{
	class Cube
	{
		struct Vertex
		{
			public Vector3 position;
			public Vector3 normal;

			public Vertex(Vector3 position, Vector3 normal)
			{
				this.position = position;
				this.normal = normal;
			}
			public static readonly int Stride = Marshal.SizeOf(default(Vertex));
		}

		int vbo;
		int vao;
		int ebo;
		int vertexCount;

		public Cube(int program)
		{
			GL.GenBuffers(1, out vbo);
			Vertex[] v = new Vertex[]{
				new Vertex(new Vector3(-1.0f, 1.0f,-1.0f), new Vector3(-1.0f, 1.0f,-1.0f)),
				new Vertex(new Vector3( 1.0f, 1.0f,-1.0f), new Vector3( 1.0f, 1.0f,-1.0f)),
				new Vertex(new Vector3( 1.0f, 1.0f, 1.0f), new Vector3( 1.0f, 1.0f, 1.0f)),
				new Vertex(new Vector3(-1.0f, 1.0f, 1.0f), new Vector3(-1.0f, 1.0f, 1.0f)),
				new Vertex(new Vector3(-1.0f,-1.0f,-1.0f), new Vector3(-1.0f,-1.0f,-1.0f)),
				new Vertex(new Vector3( 1.0f,-1.0f,-1.0f), new Vector3( 1.0f,-1.0f,-1.0f)),
				new Vertex(new Vector3( 1.0f,-1.0f, 1.0f), new Vector3( 1.0f,-1.0f, 1.0f)),
				new Vertex(new Vector3(-1.0f,-1.0f, 1.0f), new Vector3(-1.0f,-1.0f, 1.0f)),
			};

			foreach(Vertex vv in v )
			{
				vv.normal.Normalize();
			}

			uint[] indices = new uint[]{
             0,1,2,
						 0,2,3,
						 1,0,4,
						 1,4,5,
						 2,1,5,
						 2,5,6,
						 3,2,6,
						 3,6,7,
						 0,3,7,
						 0,7,4,
						 6,5,4,
						 7,6,4
			};

			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData<Vertex>(BufferTarget.ArrayBuffer, new IntPtr(v.Length * Vertex.Stride), v, BufferUsageHint.StaticDraw);

			GL.GenBuffers(1, out ebo);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
			GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, new IntPtr(sizeof(uint) * indices.Length), indices, BufferUsageHint.StaticDraw);
			vertexCount = indices.Length;

			//
			GL.GenVertexArrays(1, out vao);
			GL.BindVertexArray(vao);
			
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

			int positionLocation = GL.GetAttribLocation(program, "position");
			int normalLocation = GL.GetAttribLocation(program, "normal");

			GL.EnableVertexAttribArray(positionLocation);
			GL.EnableVertexAttribArray(normalLocation);

			GL.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false, Vertex.Stride, 0);
			GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, true, Vertex.Stride, Vector3.SizeInBytes);

			GL.BindAttribLocation(program, positionLocation, "position");
			GL.BindAttribLocation(program, normalLocation, "normal");
		}

		public void Render()
		{
			GL.BindVertexArray(vao);
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
			GL.DrawElements(BeginMode.Triangles, vertexCount, DrawElementsType.UnsignedInt, 0);
		}

	}
}
