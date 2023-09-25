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
        public UnitedChunk DrawArea;
        //Chunk[] chunks;
        public readonly int RenderDistance;
        public World(Player player, Window view, int renderDistance)
        {
            Player = player;
            this.view = view;
            DrawArea = new UnitedChunk((renderDistance * renderDistance + renderDistance) * 4 + 1);
        }
        public void SetChunk(Chunk chunk, int index)
        {
            DrawArea.Chunks[index] = chunk;
        }
        public void RefreshView()
        {
            var sides = new List<RenderSurface>();
            foreach (var chunk in DrawArea.Chunks)
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
            var localG = Utils.G * t;
            var clearMove = Player.Moving * (float)(t * Player.WalkSpeed);
            var move2d = new Vector2();

            var localFront = Player.Cam.Front;
            localFront.Y = 0;
            localFront.Normalize();
            move2d += localFront.Xz * clearMove.X;
            move2d += -Player.Cam.Right.Xz * clearMove.Y;
            var move = new Vector3(move2d.X, 0, move2d.Y);
            var playerBlock = Player.Position;
            //var startPlayerBlock = (Vector3i)playerBlock;
            //var nextPosition = player.Position + move;
            //var nextPlayerBlock = (Vector3i)nextPosition;
            if (Player.Jumping)
                Console.WriteLine();
            if (!DrawArea.GetMember((Vector3i)(Player.Position)))
            {

                Player.VerticalSpeed += (float)localG;

            }
            else
            {
                Player.VerticalSpeed = Player.Jumping ? -7 : 0;
                move.Y = MathF.Round(Player.Position.Y) - Player.Position.Y;
            }
            move.Y -= Player.VerticalSpeed * (float)t;
            var nextPosition = Player.Position + move;
            var plBlock = (Vector3i)Player.Position;
            if (DrawArea.GetMember((Vector3i)(nextPosition)))
            {
                var sign = MathF.Sign(move.X);
                float newX;
                if (sign > 0)
                {
                    newX = playerBlock.X - float.Epsilon;
                }
                else
                {
                    newX = playerBlock.X + float.Epsilon;
                }
                move.X = move.X - (nextPosition.X - newX);
            }
            if (DrawArea.GetMember((Vector3i)(nextPosition)))
            {
                var sign = MathF.Sign(move.Z);
                float newZ;
                if (sign > 0)
                {
                    newZ = playerBlock.Z - float.Epsilon;
                }
                else
                {
                    newZ = playerBlock.Z + float.Epsilon;
                }
                move.Z = move.Z - (nextPosition.Z - newZ);
            }
            Player.Position += move;
            Player.UpdateCamPosition();
        }
        void UpdateSelector()
        {
            return;
            if (Player == null) return;
            var viewDir = Player.View;
            Ray viewRay = new Ray(Player.ViewPoint, viewDir);

            foreach (var block in viewRay.GetCrossBlocks(5))
            {
                if (!DrawArea.GetMember(block.block))
                    continue;
                if (Player.SelectedBlock != block.block)
                    Player.SelectedBlock = block.block;
                Player.Cursor = block.block - block.side;
                break;
            }
        }
    }
}
