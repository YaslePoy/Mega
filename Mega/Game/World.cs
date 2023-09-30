using Mega.Video;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mega.Game
{
    public class World
    {
        public Player Player;
        Window view;
        public UnitedChunk Area;
        //Chunk[] chunks;
        public readonly int RenderDistance;
        public World(Player player, Window view, int renderDistance)
        {
            Player = player;
            this.view = view;
            Area = new UnitedChunk((renderDistance * renderDistance + renderDistance) * 4 + 1);
        }
        public void SetChunk(Chunk chunk, int index)
        {
            chunk.Root = Area;
            Area.Chunks[index] = chunk;
        }
        public void RefreshView()
        {
            var sides = new List<RenderSurface>();
            foreach (var chunk in Area.Chunks)
            {
                if (chunk is null) continue;
                sides.AddRange(chunk.Surface);
            }
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
        }
        public void Update(double time)
        {
            UpdatePlayerPosition(time);
            UpdateSelector();
        }
        void UpdatePlayerPosition(double t)
        {
            if(Player.Jumping)
                Console.WriteLine("test");
            var localG = Utils.G * t;

            var clearMove = Player.Moving * (float)(t * Player.WalkSpeed);
            var move2d = new Vector2();

            var localFront = Player.Cam.Front;
            localFront.Y = 0;
            localFront.Normalize();
            move2d += localFront.Xz * clearMove.X;
            move2d += -Player.Cam.Right.Xz * clearMove.Y;
            var move = new Vector3(move2d.X, -(float)(localG * t), move2d.Y);
            var playerPosition = Player.Position;
            var playerBlock = (Vector3i)playerPosition;
            var nextPosition = Player.Position + move;

            var nearBlocks = GetHorisontalBlocks(playerBlock);
            nearBlocks.AddRange(GetHorisontalBlocks(playerBlock + Vector3i.UnitY));
            if (Area.GetMember(playerBlock - Vector3i.UnitY))
                nearBlocks.Add(Area.GetBlock(playerBlock - Vector3i.UnitY));
            if (Area.GetMember(playerBlock + Vector3i.UnitY * 2))
                nearBlocks.Add(Area.GetBlock(playerBlock + 2 * Vector3i.UnitY));
            var collideList = nearBlocks.Select(i => i.GetCollider());

            foreach (var collide in collideList)
            {
                if (!collide.IsContains(nextPosition))
                    continue;
                collide.MoveToPossible(ref nextPosition, playerPosition);
            }

            Player.Position = nextPosition;
            Player.UpdateCamPosition();
        }
        public List<Block> GetHorisontalBlocks(Vector3i center)
        {
            var result = new List<Block>
            {
                Area.GetBlock(center),
                Area.GetBlock(center + Vector3i.UnitX),
                Area.GetBlock(center + Vector3i.UnitZ),
                Area.GetBlock(center - Vector3i.UnitX),
                Area.GetBlock(center - Vector3i.UnitZ),
                Area.GetBlock(center + Vector3i.One),
                Area.GetBlock(center + Vector3i.UnitZ - Vector3i.UnitX),
                Area.GetBlock(center - Vector3i.One),
                Area.GetBlock(center - Vector3i.UnitZ + Vector3i.UnitX)
            };
            result.RemoveAll(i => i is null);
            return result;
        }
        void UpdateSelector()
        {
            if (Player == null) return;
            var viewDir = Player.View;
            Ray viewRay = new Ray(Player.ViewPoint, viewDir);

            foreach (var block in viewRay.GetCrossBlocks(5))
            {
                if (!Area.GetMember(block.block))
                    continue;
                if (Player.SelectedBlock != block.block)
                    Player.SelectedBlock = block.block;
                Player.Cursor = block.block - block.side;
                break;
            }
        }

        public void SetBlock(Block block)
        {
            Area.SetBlock(block);
        }
    }
}
