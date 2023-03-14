// See https://aka.ms/new-console-template for more information

using HRenderer.Core;
using HRenderer.DrawTriangle;

Console.WriteLine("Hello, World!");

var texture = new Texture("./Assets/001.jpg");
var mesh = new SpriteMesh();
var shader = new SpriteShader();
var material = new Material(texture, mesh, shader);

var renderer = new Renderer(500, 500);
renderer.materials.Add(material);
renderer.Render();