﻿using System.Runtime.InteropServices;
using Mega.Game.Blocks;
using Mega.Video;
using OpenTK.Mathematics;
using StbImageSharp;

namespace OmegaTester;

static class Startup
{
    public static void Main(string[] args)
    {
        ImageResult image;
        using (Stream stream = File.OpenRead("0.png"))
        {
            image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        }

        OmegaGE.SetMainRenderTexture(image.Data, image.Width, image.Height);
        using (Stream stream = File.OpenRead("1.png"))
        {
            image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
        }
        var rs = new RenderSurface(Block.MeshSides[5], new[] { Vector2.Zero, new(0, 1), new(1, 1), new(1, 0) },
            Vector3.Zero, Vector3.UnitZ, 1);


        var arr = new List<RenderSurface> { rs };
        OmegaGE.InitWindow(900, 400, "Re test name");
        OmegaGE.Start();

        OmegaGE.SetMeshShaderData(arr.ToArray(), 1);

        var state = OmegaGE.GetWindowCloseState();
        int iters = 0;
        while (state == 0)
        {
            OmegaGE.UpdateKeyboardState();
            OmegaGE.PollWindowEvents();

            // if (iters++ % 1_000 == 0)
            // {
            //     float level = RandomNumberGenerator.GetInt32(-100, 100) / 50f;
            //     arr.Add(new RenderSurface(Block.MeshSides[5],
            //         new[] { Vector2.Zero, new(0, 1), new(1, 1), new(1, 0) },
            //         Vector3.UnitZ * level, Vector3.UnitZ, 1));
            //     Console.WriteLine($"Added new {level}");
            //     OmegaGE.SetMeshShaderData(arr.ToArray(), (uint)arr.Count);
            // }
            if (OmegaGE.IsKeyPressed((int)GLFWKeys.W))
            {
                OmegaGE.UpdateMainRenderTexture(image.Data, image.Width, image.Height);
            }


            OmegaGE.Draw();
            state = OmegaGE.GetWindowCloseState();
        }
    }
}

public static class OmegaGE
{
    private const string Library =
        @"C:\Users\Mimm\Projects\VisualStudioProjects\Mega\OmegaGE\bin\x64\Release\OmegaGE.dll";

    [DllImport(Library)]
    public static extern int Add(int a, int b);

    [DllImport(Library, CharSet = CharSet.Ansi)]
    public static extern void InitWindow(uint width, uint height, string name);

    [DllImport(Library)]
    public static extern void Start();

    [DllImport(Library)]
    public static extern void Close();

    [DllImport(Library)]
    public static extern int GetWindowCloseState();

    [DllImport(Library)]
    public static extern void PollWindowEvents();

    [DllImport(Library)]
    public static extern void Draw();

    [DllImport(Library)]
    public static extern void SetMainRenderTexture([In] [Out] byte[] data, int width, int height);
    [DllImport(Library)]
    public static extern void UpdateMainRenderTexture([In] [Out] byte[] data, int width, int height);
    [DllImport(Library)]
    public static extern void SetMeshShaderData([In] [Out] RenderSurface[] polygons, uint surfaceCount);

    [DllImport(Library)]
    public static extern void UpdateKeyboardState();

    [DllImport(Library)]
    public static extern bool IsKeyPressed(int key);

    [DllImport(Library)]
    public static extern bool IsKeyReleased(int key);

    [DllImport(Library)]
    public static extern bool IsKeyDown(int key);

    [DllImport(Library)]
    public static extern bool IsKeyUp(int key);
}