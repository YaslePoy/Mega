using System.Threading.Channels;
using Mega.Game.Blocks;
using Mega.Video;
using OpenTK.Mathematics;

namespace Mega.Game
{
    public class World
    {
        public static Vector3 Sun = new Vector3(1, -1, 0.5f).Normalized();
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
            set
            {
                redrawing = value;
            }
        }

        bool isRuninig;
        Task updateThread;
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

        public void GenerateMesh(out Dictionary<int, List<uint>> indeces, out float[] verteces)
        {
            var sides = Area.Chunks.Values.Select(i => i.Surface).SumList();

            verteces = new float[sides.Length * 4 * 8];
            var bufferSize = 2048 * 4 * 8;
            Span<float> buffer = stackalloc float[bufferSize];
            float[] arrayed;
            indeces = new Dictionary<int, List<uint>>();
            Span<float> raw = stackalloc float[4 * 8];
            for (int i = 0; i < sides.Length; i++)
            {
                int inBuffer = i % 2048;
                var side = sides[i];
                side.GetRawPolygon(raw);
                raw.CopyTo(buffer.Slice(inBuffer * 4 * 8, 4 * 8));

                if (!indeces.ContainsKey(side.TextureID))
                    indeces.Add(side.TextureID, new List<uint>(6));
                var indOffset = (uint)(i * 4);
                indeces[side.TextureID].AddRange(new[] { indOffset, 1 + indOffset, 3 + indOffset, 1 + indOffset, 2 + indOffset, 3 + indOffset });
                if (inBuffer != 0 || i == 0)
                    continue;
                arrayed = buffer.ToArray();
                arrayed.CopyTo(verteces, bufferSize * ((i * 4 * 8) / bufferSize - 1));

            }
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
                    var clearMove = Player.Moving * (float)(t * Player.WalkSpeed) * (Player.Fast ? 2 : 1);
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
                    var colliderList = nearBlocks.Select(i => i.GetCollider()).ToList().Where(i => i is not null).ToList();
                    var united = Collider.CreateUnitedCollider(colliderList);
                    var playerCollider = Player.GetCollider();
                    float vertical = (float)(Player.VerticalSpeed * t);
                    if (united.IsContact(playerCollider))
                    {
                        vertical = Player.Jumping ? 5f * t : 0;
                    }

                    //creating global player's move
                    var move = new Vector3(move2d.X, vertical, move2d.Y);
                    //reading adjistment colliders

                    nearBlocks.AddRange(GetHorisontalBlocks(playerBlock));
                    nearBlocks.AddRange(GetHorisontalBlocks(playerBlock + Vector3i.UnitY));
                    nearBlocks.AddRange(GetHorisontalBlocks(playerBlock + 2 * Vector3i.UnitY));
                    colliderList = nearBlocks.Select(i => i.GetCollider()).ToList().Where(i => i is not null).ToList();
                    united = Collider.CreateUnitedCollider(colliderList);

                    Vector3 resultalMove = Vector3.Zero;

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

                        } while (resultalMove != Vector3.Zero);
                    else
                    {
                        Player.MoveTo(move);
                    }
                    Player.VerticalSpeed = (Player.Position.Y - playerPosition.Y) / t;
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
            var archive = Player.SelectedBlock;
            Player.SelectedBlock = null;
            Player.Cursor = null;
            foreach (var block in viewRay.GetCrossBlocks(5))
            {
                if (!Area.GetMember(block.block))
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
        public Vector2i Chunk;

        public WorldPath(Vector3i inChunk, Vector2i chunk)
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
