using System.Diagnostics;
using System.Threading.Channels;
using Mega.Game.Blocks;
using Mega.Game.Blocks;
using Mega.Video;
using Vec3 = OpenTK.Mathematics.Vector3;
using Vec3i = OpenTK.Mathematics.Vector3i;
using Vec2 = OpenTK.Mathematics.Vector2;
using Vec2i = OpenTK.Mathematics.Vector2i;

namespace Mega.Game
{
    public class World
    {
        public static Vec3 Sun = new Vec3(1, 0.5f, -1).Normalized();
        private bool redrawing = true;

        public bool Redrawing
        {
            get
            {
                //var buf = redrawing;
                //if (redrawing)
                //    redrawing = false;
                //return buf;
                return redrawing;
            }
            set { redrawing = value; }
        }

        bool isRuninig;
        Task updateThread;
        public const double G = 10d;
        int i;
        public Player Player;
        public UnitedChunk Area;
        public readonly int RenderDistance;
        int updateRate;

        public World(Player player)
        {
            Player = player;
            Area = new UnitedChunk();
            player.world = this;
        }

        public void SetChunk(Chunk chunk)
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
            updateThread = Task.Factory.StartNew(BasicPhysics);
        }

        void BasicPhysics()
        {
            var updateTime = 1f / updateRate;
            var totalUpdateTime = TimeSpan.FromSeconds(updateTime);
            DateTime next = DateTime.Now + totalUpdateTime;
            while (true)
            {
                if (!isRuninig)
                    return;
                next = DateTime.Now + totalUpdateTime;
                Update(updateTime);
                var sleep = next - DateTime.Now;
                if (sleep > TimeSpan.Zero)
                    Thread.Sleep(sleep);
            }
        }

        public void Stop()
        {
            isRuninig = false;
        }

        public void UpdateTotalMesh()
        {
            var sides = Area.Chunks.Values.Select(index => index.Surface).SumList();

            // for (int j = 0; j < sides.Length; j++)
            // {
            //     sides[j].Apply();
            // }
            Parallel.For(0, sides.Length, index => { sides[index].Apply(); });
            OmegaEngine.SetMeshShaderData(sides, (uint)sides.Length);
        }

        public void Update(float time)
        {
            if (!Player.IsActed)
                Debug.Trap();
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

            DemoWriter.TakePhoto(this);
            DemoWriter.ApplyPhoto(this);

            //calculating movement in x-z plane
            try
            {
                if (Player.Fly)
                {
                    var localG = G * t;
                    Player.VerticalSpeed -= (float)localG;
                    var clearMove = Player.Moving * (t * Player.WalkSpeed) * (Player.Fast ? 2 : 1);
                    var move2d = new Vec2();
                    var localFront = Player.Cam.Front;
                    localFront.Z = 0;
                    localFront.Normalize();
                    move2d += localFront.Xy * clearMove.X;
                    move2d += -Player.Cam.Right.Xy * clearMove.Y;


                    var playerPosition = Player.Position;

                    //is player standing?
                    var playerBlock = (Vec3i)playerPosition;
                    var nearBlocks = GetHorisontalBlocks(playerBlock - Vec3i.UnitZ);
                    var colliderList = nearBlocks.Select(i => i.GetCollider()).ToList().Where(i => i is not null)
                        .ToList();
                    var united = Collider.CreateUnitedCollider(colliderList);
                    var playerCollider = Player.GetCollider();
                    float vertical = Player.VerticalSpeed * t;
                    if (united.IsContact(playerCollider))
                    {
                        vertical = Player.Jumping ? 5f * t : 0;
                    }

                    //creating global player's move
                    var move = new Vec3(move2d.X, move2d.Y, vertical);

                    //reading adjistment colliders

                    nearBlocks.AddRange(GetHorisontalBlocks(playerBlock));
                    nearBlocks.AddRange(GetHorisontalBlocks(playerBlock + Vec3i.UnitZ));
                    nearBlocks.AddRange(GetHorisontalBlocks(playerBlock + 2 * Vec3i.UnitZ));
                    colliderList = nearBlocks.Select(i => i.GetCollider()).ToList().Where(i => i is not null).ToList();
                    united = Collider.CreateUnitedCollider(colliderList);

                    Vec3 resultalMove = Vec3.Zero;

                    Debug.LogToFile($"Startup move {move}");

                    //colliding
                    if (colliderList.Count != 0)
                        do
                        {
                            playerCollider.move = move;
                            playerCollider.Collide(united, out move, out resultalMove);
                            Player.MoveTo(move);
                            Debug.LogToFile($"Move: {move} Resultal: {resultalMove}", 1);
                            move = resultalMove;
                        } while (resultalMove != Vec3.Zero);
                    else
                    {
                        Player.MoveTo(move);
                    }

                    Player.VerticalSpeed = (Player.Position.Z - playerPosition.Z) / t;
                }
                else
                {
                    var localFront = (Player.Cam.Front / 2 * Player.Moving.X - Player.Cam.Right / 2 * Player.Moving.Y);
                    Player.MoveTo(localFront);
                }

                Player.UpdateCamPosition();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            DemoWriter.NextFrame();
        }

        public List<Block> GetHorisontalBlocks(Vec3i center)
        {
            var diagA = new Vec3i(1, 0, 1);

            var diagB = new Vec3i(1, 0, -1);
            var result = new List<Block>
            {
                Area.GetBlock(center),
                Area.GetBlock(center + Vec3i.UnitX),
                Area.GetBlock(center + Vec3i.UnitZ),
                Area.GetBlock(center - Vec3i.UnitX),
                Area.GetBlock(center - Vec3i.UnitZ),
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
            var archive = Player.SelectedBlock;
            Player.SelectedBlock = null;
            Player.Cursor = null;
            foreach (var block in viewRay.GetCrossBlocks(5))
            {
                if (!Area.IsMember(block.block))
                    continue;
                if (Player.SelectedBlock != block.block)
                    Player.SelectedBlock = block.block;
                Player.Cursor = block.block - block.side;
                break;
            }

            if (archive != Player.SelectedBlock)
                Console.WriteLine($"New Block selected {Player.SelectedBlock}");
        }

        public void SetBlock(Block block)
        {
            Area.SetBlock(block);
            Area.UpdateBorder();
            redrawing = true;
        }
    }

    public struct WorldPath
    {
        public ChunkLocation InChunk;
        public Vec2i Chunk;

        public WorldPath(Vec3i inChunk, Vec2i chunk)
        {
            InChunk = inChunk;
            Chunk = chunk;
        }

        public override string ToString()
        {
            return $"({InChunk}-{Chunk})";
        }

        public static bool operator ==(WorldPath left, WorldPath right)
        {
            return left.Chunk == right.Chunk && right.InChunk == left.InChunk;
        }

        public static bool operator !=(WorldPath left, WorldPath right)
        {
            return !(left == right);
        }
    }
}