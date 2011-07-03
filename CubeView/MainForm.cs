using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace CubeView
{
	public partial class MainForm : Form
	{
		int program;
		Cube cube;

		float lightAngle = 0.0f;

		public MainForm()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			viewport.MakeCurrent();

			program = CreateShader(Encoding.UTF8.GetString(Properties.Resources.phone_vert), Encoding.UTF8.GetString(Properties.Resources.phone_frag));
			cube = new Cube(program);
		}

		// レンダリング
		public void Render()
		{
			viewport.MakeCurrent();

			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.CullFace);
			GL.FrontFace(FrontFaceDirection.Cw);
			GL.CullFace(CullFaceMode.Back);

			GL.Viewport(0, 0, viewport.Width, viewport.Height);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.UseProgram(program);

			Vector3 eyePos = new Vector3(5.0f, 5.0f, 5.0f);
			Vector3 lookAt = new Vector3(0.0f, 0.0f, 0.0f);
			Vector3 eyeUp = new Vector3(0.0f, 1.0f, 0.0f);
			Matrix4 viewMatrix = Matrix4.LookAt(eyePos, lookAt, eyeUp);
			Matrix4 projectionMatrix = Matrix4.CreatePerspectiveFieldOfView((float)System.Math.PI / 4.0f, (float)viewport.Width / (float)viewport.Height, 0.1f, 10.0f);
			Matrix4 viewProjectionMatrix = viewMatrix * projectionMatrix;
			GL.UniformMatrix4(GL.GetUniformLocation(program, "viewProjection"), false, ref viewProjectionMatrix);

			Matrix4 worldMatrix = Matrix4.Identity;
			GL.UniformMatrix4(GL.GetUniformLocation(program, "world"), false, ref worldMatrix);

			Vector3 lightDir = new Vector3((float)Math.Cos((float)lightAngle), -1.0f, (float)Math.Sin((float)lightAngle));
			lightDir.Normalize();
			GL.Uniform3(GL.GetUniformLocation(program, "lightDir"), lightDir);
			lightAngle += 0.001f;

			cube.Render();

			viewport.SwapBuffers();
		}

		// シェーダを作成
		int CreateShader(string vertexShaderCode, string fragmentShaderCode)
		{
			int vshader = GL.CreateShader(ShaderType.VertexShader);
			int fshader = GL.CreateShader(ShaderType.FragmentShader);

			string info;
			int status_code;

			// Vertex shader
			GL.ShaderSource(vshader, vertexShaderCode);
			GL.CompileShader(vshader);
			GL.GetShaderInfoLog(vshader, out info);
			GL.GetShader(vshader, ShaderParameter.CompileStatus, out status_code);
			if (status_code != 1)
			{
				throw new ApplicationException(info);
			}

			// Fragment shader
			GL.ShaderSource(fshader, fragmentShaderCode);
			GL.CompileShader(fshader);
			GL.GetShaderInfoLog(fshader, out info);
			GL.GetShader(fshader, ShaderParameter.CompileStatus, out status_code);
			if (status_code != 1)
			{
				throw new ApplicationException(info);
			}

			int program = GL.CreateProgram();
			GL.AttachShader(program, vshader);
			GL.AttachShader(program, fshader);

			GL.LinkProgram(program);

			return program;
		}

	}
}
