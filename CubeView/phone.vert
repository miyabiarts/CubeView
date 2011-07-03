attribute vec3 position;
attribute vec3 normal;
uniform mat4 world;
uniform mat4 viewProjection;
uniform vec3 lightDir;
varying vec4 diffuseColor;
void main(void)
{
	gl_Position = viewProjection * world * vec4(position, 1.0);
	/*
	mat3 m;
	m[0][0] = world[0][0];
	m[0][1] = world[0][1];
	m[0][2] = world[0][2];

	m[1][0] = world[1][0];
	m[1][1] = world[1][1];
	m[1][2] = world[1][2];

	m[2][0] = world[2][0];
	m[2][1] = world[2][1];
	m[2][2] = world[2][2];
	diffuseColor = vec4(max(0.0,dot(m * normal,lightDir)));
	*/
	diffuseColor = vec4(max(0,dot(mat3(world) * normal,-lightDir)));
	diffuseColor.a = 1.0;
}
