using Mega.Video;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    public class World
    {
        public Player player;
        public static readonly Vector3i Size = new Vector3i(32, 64, 32);
        Block[,,] worldData;
        public bool[,,] Border;
        public bool[,,] Members;
        public bool[,,] BorderMembers;
        public List<Vector3i> MembersList;
        public List<Vector3i> BorderMembersList;


        public float[] RawVertexes;
        public uint[] RawOrder;
        public RenderSurface[] Surface;
        List<(int x, int y, int z)> notAir;
        Window view;
        public World(Window window)
        {
            worldData = new Block[Size.X, Size.Y, Size.Z];
            view = window;

            Border = new bool[Size.X, Size.Y, Size.Z];
            Members = new bool[Size.X, Size.Y, Size.Z];
            BorderMembers = new bool[Size.X, Size.Y, Size.Z];
            MembersList = new List<Vector3i>();
            BorderMembersList = new List<Vector3i>();
        }

        public static World GenerateFlat(int level, Window window)
        {
            World world = new World(window);
            var blocks = new List<Block>();
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Z; j++)
                {
                    blocks.Add(new Block(new Vector3i(i, level, j), 0));
                }
            }

            world.GenerateFromBlocks(blocks);
            return world;
        }

        public void SetBlock(Vector3i location, int blockId)
        {
            if (Members.Get(location))
                return;
            var block = new Block(location, blockId);
            worldData.Set(location, block);

            add(block);
            RebuildMesh();
        }

        void add(Block block)
        {

            MembersList.Add(block.Position);
            Members.Set(block.Position, true);

            Border.Set(block.Position, false);
            BorderMembers.Set(block.Position, true);
            BorderMembersList.Add(block.Position);
            var addingBorder = block.LocalNeibs;

            for (int i = 0; i < addingBorder.Length; i++)
                TrySetBorder(addingBorder[i]);


        }

        void TrySetBorder(Vector3i location)
        {
            if (Members.Get(location))
                return;
            Border.Set(location, true);
            if (BorderMembers.Get(location))
            {
                BorderMembersList.Remove(location);
            }
        }
        public void RebuildMesh()
        {
            var sides = new List<RenderSurface>();
            foreach (var borderBlock in BorderMembersList)
            {
                sides.AddRange(worldData.Get(borderBlock).GetDrawingMesh(this));
            }
            Block cursorBolck = new Block();
            uint indOffset = 0;
            var vertexArray = new float[sides.Count * 20];
            Dictionary<int, List<uint>> orders = new Dictionary<int, List<uint>>();
            for (int i = 0; i < sides.Count; i++)
            {
                var side = sides[i];

                var v = side.GetRaw();
                v.CopyTo(vertexArray, 20 * i);

                if (!orders.ContainsKey(side.TextureID))
                    orders.Add(side.TextureID, new List<uint>());
                orders[side.TextureID].Add(indOffset);
                orders[side.TextureID].Add(1 + indOffset);
                orders[side.TextureID].Add(3 + indOffset);
                orders[side.TextureID].Add(1 + indOffset);
                orders[side.TextureID].Add(2 + indOffset);
                orders[side.TextureID].Add(3 + indOffset);

                indOffset += 4;
            }
            view?.UpdateMesh(vertexArray, orders);
            Surface = sides.ToArray();
            RawVertexes = vertexArray;
        }

        public void GenerateFromBlocks(List<Block> blocks)
        {
            foreach (var block in blocks)
            {
                worldData.Set(block.Position, block);
                Members.Set(block.Position, true);
                MembersList.Add(block.Position);
            }
            foreach (var member in blocks)
            {
                var neibs = member.LocalNeibs;
                foreach (var neighbour in neibs)
                {
                    if (!neighbour.IsInRange(0, Size.X))
                        continue;
                    if (!Members.Get(neighbour))
                        Border.Set(neighbour, true);
                    else
                    if (!BorderMembers.Get(neighbour))
                    {
                        BorderMembers.Set(neighbour, true);
                        BorderMembersList.Add(neighbour);
                    }
                }
            }
            RebuildMesh();
        }

        public Block Get(Vector3i pos)
        {
            return worldData.Get(pos);
        }

        public void Update(double t)
        {

            UpdatePlayerPosition(t);
            UpdateSelector();
        }

        void UpdatePlayerPosition(double t)
        {
            var localG = Utils.G * t;
            var clearMove = player.Moving * (float)(t * Player.WalkSpeed);
            var move2d = new Vector2();
            move2d += player.Cam.Front.Xz * clearMove.X;
            move2d += -player.Cam.Right.Xz * clearMove.Y;
            var move = new Vector3(move2d.X, 0, move2d.Y);
            var playerBlock = player.Position;
            var startPlayerBlock = (Vector3i)playerBlock;
            var nextPosition = player.Position + move;
            var nextPlayerBlock = (Vector3i)nextPosition;
            var floorBlock = nextPlayerBlock - Vector3i.UnitY;
            if (floorBlock.IsInRange(0, Size.X) && !Members.Get(floorBlock))
            {

                player.VerticalSpeed += player.Jumping ? -10 : (float)localG;

                nextPosition.Y -= player.VerticalSpeed * (float)t;
            }
            if (player.Jumping)
            {
                Console.WriteLine();
            }
            if (nextPlayerBlock.IsInRange(0, Size.X))
            {
                if (Members.Get(nextPlayerBlock))
                {
                    var delta = nextPlayerBlock - startPlayerBlock;
                    //if (delta.X != 0)
                    //{
                    //    if (delta.X > 0)
                    //        nextPosition.X = MathF.Floor(nextPosition.X);
                    //    else
                    //        nextPosition.X = MathF.Ceiling(nextPosition.X);
                    //}
                    //else
                    //{
                    //    if(move2d.X > 0)
                    //        nextPosition.X = MathF.Ceiling(nextPosition.X);
                    //    else if(move2d.X < 0)
                    //        nextPosition.X = MathF.Floor(nextPosition.X);

                    //}
                    //if (delta.Z != 0)
                    //{
                    //    if (delta.Z > 0)
                    //        nextPosition.Z = MathF.Floor(nextPosition.Z);
                    //    else
                    //        nextPosition.Z = MathF.Ceiling(nextPosition.Z);
                    //}
                    //else
                    //{

                    //}
                    if (move2d.X != 0)
                        nextPosition.X = MathF.Round(nextPosition.X);
                    if (move2d.Y != 0)
                        nextPosition.Z = MathF.Round(nextPosition.Z);
                }
            }
            player.Position = nextPosition;

        }
        void UpdateSelector()
        {
            if (player == null) return;
            var viewDir = player.View;
            Ray viewRay = new Ray(player.Position, viewDir);

            foreach (var block in viewRay.GetCrossBlocks(5))
            {
                if (!block.block.IsInRange(0, Size.X) || !Members.Get(block.block))
                    continue;
                if (player.SelectedBlock != block.block)
                    player.SelectedBlock = block.block;
                player.Cursor = block.block - block.side;
                break;
            }
        }
    }
}
