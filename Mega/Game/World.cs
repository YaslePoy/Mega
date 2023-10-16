using Mega.Video;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mega.Game
{
    public class World
    {
        bool isRuninig;
        Thread updateThread;
        public const double G = 10d;
        Window _view;
        int i;
        public Player Player;
        public UnitedChunk Area;
        public readonly int RenderDistance;
        int updateRate;
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

        public void Start(int updateRate)
        {
            if (isRuninig)
                return;
            isRuninig = true;
            this.updateRate = updateRate;
            updateThread = new Thread(() =>
            {
                Stopwatch timer = new Stopwatch();
                var updateTime = 1f / updateRate;
                var totalUpdateTime = TimeSpan.FromSeconds(updateTime);
                DateTime next = DateTime.Now + totalUpdateTime;
                while (true)
                {
                    if (!isRuninig)
                        return;
                    while (next > DateTime.Now) ;
                    next += totalUpdateTime;
                    Update(updateTime);
                }
            })
            {
                Name = "World run"
            };
            updateThread.Start();
        }
        public void Stop()
        {
            isRuninig = false;
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
        public void Update(float time)
        {
            UpdateSelector();

            UpdatePlayerPosition(time);
        }
        void UpdatePlayerPosition(float t)
        {
            if (!Player.IsActed)
            {
                Player.IsActed = true;
                Player.PlaceBlock();
            }

            //calculating movement in x-z plane
            var localG = G * t;
            Player.VerticalSpeed -= (float)localG;
            var clearMove = Player.Moving * (float)(t * Player.WalkSpeed);
            var move2d = new Vector2();
            var localFront = Player.Cam.Front;
            localFront.Y = 0;
            localFront.Normalize();
            move2d += localFront.Xz * clearMove.X;
            move2d += -Player.Cam.Right.Xz * clearMove.Y;

            var playerPosition = Player.Position;

            //is player standing?
            var playerBlock = (Vector3i)playerPosition;
            var nearBlocks = GetHorisontalBlocks(playerBlock - Vector3i.UnitY);
            var colliderList = nearBlocks.Select(i => i.GetCollider()).ToList();
            var united = Collider.CreateUnitedCollider(colliderList);
            var playerCollider = Player.GetCollider();
            float vertical = (float)(Player.VerticalSpeed * t);
            if (united.IsContact(playerCollider))
            {
                vertical = Player.Jumping ? 5f * t : 0;

            }
            else
            {
                move2d /= 4;
            }

            //creating global player's move
            var move = new Vector3(move2d.X, vertical, move2d.Y);
            //reading adjistment colliders

            nearBlocks.AddRange(GetHorisontalBlocks(playerBlock));
            nearBlocks.AddRange(GetHorisontalBlocks(playerBlock + Vector3i.UnitY));
            nearBlocks.AddRange(GetHorisontalBlocks(playerBlock + 2 * Vector3i.UnitY));
            colliderList = nearBlocks.Select(i => i.GetCollider()).ToList();
            united = Collider.CreateUnitedCollider(colliderList);

            Vector3 resultalMove = Vector3.Zero;

            //colliding
            if (colliderList.Count != 0)
                do
                {
                    playerCollider.move = move;
                    playerCollider.Collide(united, out move, out resultalMove);
                    Player.MoveTo(move);
                    move = resultalMove;

                } while (resultalMove != Vector3.Zero);
            else
            {
                Player.MoveTo(move);
            }
            Player.VerticalSpeed = (Player.Position.Y - playerPosition.Y) / t;
            Player.UpdateCamPosition();
            var delta = playerPosition - Player.Position;


        }
        public List<Block> GetHorisontalBlocks(Vector3i center)
        {
            var diagA = new Vector3i(1, 0, 1);

            var diagB = new Vector3i(1, 0, -1);
            var result = new List<Block>
            {
                Area.GetBlock(center),
                Area.GetBlock(center + Vector3i.UnitX),
                Area.GetBlock(center + Vector3i.UnitZ),
                Area.GetBlock(center - Vector3i.UnitX),
                Area.GetBlock(center - Vector3i.UnitZ),
                Area.GetBlock(center + diagA),
                Area.GetBlock(center - diagA),
                Area.GetBlock(center + diagB),

                Area.GetBlock(center - diagB)


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
            Area.UpdateRenderSurface();
            RefreshView();
        }
    }
}
