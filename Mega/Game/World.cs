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
        public const double G = 10d;
        Window _view;

        public Player Player;
        public UnitedChunk Area;
        public readonly int RenderDistance;
        public World(Player player, Window view, int renderDistance)
        {
            Player = player;
            this._view = view;
            Area = new UnitedChunk((renderDistance * renderDistance + renderDistance) * 4 + 1);
            player.world = this;
        }
        public void SetChunk(Chunk chunk, int index)
        {
            chunk.Root = Area;
            Area.AddChunk(chunk);
        }
        public void RefreshView()
        {
            var sides = new List<RenderSurface>();
            foreach (var chunk in Area.Chunks.Values)
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
            _view?.UpdateMesh(vertexArray, orders);
        }
        public void Update(double time)
        {
            UpdatePlayerPosition(time);
            UpdateSelector();
        }
        void UpdatePlayerPosition(double t)
        {
            if (Player.Jumping) // debug trap
                Console.WriteLine("test");

            //calculating movement in x-z plane
            var localG = G * t;
            var clearMove = Player.Moving * (float)(t * Player.WalkSpeed);
            var move2d = new Vector2();
            var localFront = Player.Cam.Front;
            localFront.Y = 0;
            localFront.Normalize();
            move2d += localFront.Xz * clearMove.X;
            move2d += -Player.Cam.Right.Xz * clearMove.Y;

            var playerPosition = Player.Position;

            //creating global player's move
            var move = new Vector3(move2d.X, -(float)(localG + Player.VerticalSpeed * t), move2d.Y);

            //reading adjistment colliders
            var playerBlock = (Vector3i)playerPosition;
            var nearBlocks = GetHorisontalBlocks(playerBlock);
            if (Area.GetMember(playerBlock - Vector3i.UnitY))
                nearBlocks.Add(Area.GetBlock(playerBlock - Vector3i.UnitY));
            nearBlocks.AddRange(GetHorisontalBlocks(playerBlock + Vector3i.UnitY));
            if (Area.GetMember(playerBlock + Vector3i.UnitY * 2))
                nearBlocks.Add(Area.GetBlock(playerBlock + 2 * Vector3i.UnitY));
            var colliderList = nearBlocks.Select(i => i.GetCollider()).ToList();
            Vector3 resultalMove = Vector3.Zero;
            var playerCollider = Player.GetCollider();

            //colliding
            if (colliderList.Count != 0)
                do
                {
                    foreach (var collider in colliderList)
                    {
                        playerCollider.move = move;
                        playerCollider.Collide(collider, out move, out resultalMove);
                        Player.MoveTo(move);
                        move = resultalMove;
                    }
                } while (resultalMove != Vector3.Zero);
            else
                Player.MoveTo(move);
            Player.VerticalSpeed = playerPosition.Y - Player.Position.Y;
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
            Area.UpdateBorder();

            Window.sw.Restart();

            Area.UpdateRenderSurface();
            Window.sw.Stop();
            var t = Window.sw.Elapsed;
            RefreshView();
        }
    }
}
