using ConsoleTest.DTOs;
using ConsoleTest.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest.Services
{
    public interface IEngine
    {
        event Action<PlatformInfo> PlatformInfoUpdated;
        event Action<MissileInfo> MissileInfoUpdated;

        void Start();
        void Stop();
        Task<IResponseCmd> Receive<T>(T requestCmd) where T : IRequestCmd;
    }

    public interface IRequestVisitor
    {
        PlatformCreateResponseCmd Visit(PlatformCreateRequestCmd createRequestCmd);
        PlatformDeleteResponseCmd Visit(PlatformDeleteRequestCmd deleteRequestCmd);
        NullResponseCmd Visit(NullRequestCmd nullRequestCmd);
        PlatformSyncResponseCmd Visit(PlatformSyncRequestCmd updateRequestCmd);
        MissileShootResponseCmd Visit(MissileShootRequestCmd missileShootRequestCmd);
    }

    public class Engine : IEngine, IRequestVisitor
    {
        public const double INTERVAL_MS = 100;

        public event Action<PlatformInfo> PlatformInfoUpdated;
        public event Action<MissileInfo> MissileInfoUpdated;

        private const int IDLE = 0;
        private const int RUNNING = 1;
        private int _engineState = IDLE;

        private Task _task = null;

        private List<RequestCmdTaskWrapper> _toDoLists = new List<RequestCmdTaskWrapper>();
        private readonly object _toDoListLock = new object();

        private readonly Dictionary<int, Platform> _platformById = new Dictionary<int, Platform>();
        private readonly Dictionary<int, Missile> _missileById = new Dictionary<int, Missile>();


        public void Start()
        {
            if (IsRunning())
            {
                return;
            }

            Interlocked.Exchange(ref _engineState, RUNNING);

            _task = StartImpl();
        }

        public void Stop()
        {
            if (!IsRunning())
            {
                return;
            }
            
            Interlocked.Exchange(ref _engineState, IDLE);
            
            _task.Wait(TimeSpan.FromSeconds(1.0));
            
            Clear();
        }

        private Task StartImpl()
        {
            return Task.Run(() =>
            {
                var stopWatch = new Stopwatch();

                int counter = 0;

                while (IsRunning())
                {
                    stopWatch.Start();

                    var todos = default(List<RequestCmdTaskWrapper>);
                    lock (_toDoListLock)
                    {
                        todos = _toDoLists;
                        _toDoLists = new List<RequestCmdTaskWrapper>();
                    }

                    foreach (var todo in todos)
                    {
                        todo.SetResultBy(this);
                    }

                    _platformById.Values.AsParallel().ForAll(each => each.Move());
                    _missileById.Values.AsParallel().ForAll(each => each.Move());

                    counter++;
                    if(counter >= 10)
                    {
                        RaisePlatformInfoUpdated();
                        RaiseMissileInfoUpdated();

                        counter = 0;
                    }

                    stopWatch.Stop();
                    Thread.Sleep(TimeSpan.FromMilliseconds(Math.Max(INTERVAL_MS - stopWatch.Elapsed.TotalMilliseconds, 0)));

                    stopWatch.Reset();
                }

                void RaisePlatformInfoUpdated()
                {
                    PlatformInfoUpdated?.Invoke(new PlatformInfo()
                    {
                        Items = _platformById.Select(each => new PlatformInfo.PlatformInfoItem()
                        {
                            Id = each.Key,
                            LatLonAlt = each.Value.LatLonAlt,
                            Heading = each.Value.Heading,
                            Speed = each.Value.Speed,
                            PlatformState = each.Value.State,
                        }).ToList(),
                    });
                }
                void RaiseMissileInfoUpdated()
                {
                    MissileInfoUpdated?.Invoke(new MissileInfo()
                    {
                        Items = _missileById.Select(each => new MissileInfo.MissileInfoItem()
                        {
                            Id = each.Key,
                            LatLonAlt = each.Value.LatLonAlt,
                            Heading = each.Value.Heading,
                            Speed = each.Value.Speed,
                            MissileState = each.Value.State,
                        }).ToList(),
                    });
                }
            });
        }

        private void Clear()
        {
            _platformById.Clear();
            _toDoLists.Clear();

            _task?.Dispose();
            _task = null;
        }

        private bool IsRunning()
        {
            return Interlocked.CompareExchange(ref _engineState, _engineState, RUNNING) == RUNNING;
        }

        public Task<IResponseCmd> Receive<T>(T requestCmd) where T : IRequestCmd
        {
            var tcs = new RequestCmdTaskWrapper(requestCmd);
            lock (_toDoListLock)
            {
                _toDoLists.Add(tcs);
            }

            return tcs.Task;
        }

        PlatformCreateResponseCmd IRequestVisitor.Visit(PlatformCreateRequestCmd platformCreateRequestCmd)
        {
            if (_platformById.ContainsKey(platformCreateRequestCmd.Id))
            {
                return new PlatformCreateResponseCmd()
                {
                    Id = platformCreateRequestCmd.Id,
                    IsSuccessful = false,
                    WhenCreated = DateTime.UtcNow,
                };
            }

            _platformById.Add(platformCreateRequestCmd.Id, new Platform(platformCreateRequestCmd.LatLonAlt, new HoldingStrategy()));

            return new PlatformCreateResponseCmd()
            {
                Id = platformCreateRequestCmd.Id,
                IsSuccessful = true,
                WhenCreated = DateTime.UtcNow,
            };
        }

        PlatformDeleteResponseCmd IRequestVisitor.Visit(PlatformDeleteRequestCmd PlatformdeleteRequestCmd)
        {
            return new PlatformDeleteResponseCmd()
            {
                Id = PlatformdeleteRequestCmd.Id,
                IsSuccessful = _platformById.Remove(PlatformdeleteRequestCmd.Id),
                WhenDeleted = DateTime.UtcNow,
            };
        }
        
        PlatformSyncResponseCmd IRequestVisitor.Visit(PlatformSyncRequestCmd platformSyncRequestCmd)
        {
            bool targetExist = _platformById.TryGetValue(platformSyncRequestCmd.TargetId, out var target);
            bool originatorExist = _platformById.TryGetValue(platformSyncRequestCmd.OriginatorId, out var originator);
            if (targetExist && originatorExist)
            {
                target.SetMoveStrategy(new SyncWithOriginStrategy(originator));
            }

            return new PlatformSyncResponseCmd()
            {
                Id = platformSyncRequestCmd.TargetId,
                IsSuccessful = targetExist && originatorExist,
                WhenSynced = DateTime.UtcNow,
            };
        }

        NullResponseCmd IRequestVisitor.Visit(NullRequestCmd nullRequestCmd)
        {
            return new NullResponseCmd();
        }

        MissileShootResponseCmd IRequestVisitor.Visit(MissileShootRequestCmd missileShootRequestCmd)
        {
            bool missileIdAlreadyExist = _missileById.ContainsKey(missileShootRequestCmd.MissileId);

            bool srcExist = _platformById.TryGetValue(missileShootRequestCmd.SrcId, out var srcPlatform);
            bool targetExist = _platformById.TryGetValue(missileShootRequestCmd.TargetId, out var targetPlatform);

            if (missileIdAlreadyExist || !srcExist || !targetExist)
            {
                return new MissileShootResponseCmd()
                {
                    MissileId = missileShootRequestCmd.MissileId,
                    SrcId = missileShootRequestCmd.SrcId,
                    TargetId = missileShootRequestCmd.TargetId,
                    IsSuccessful = false,
                    WhenShot = DateTime.UtcNow,
                };
            }

            _missileById.Add(missileShootRequestCmd.MissileId, new Missile(srcPlatform.LatLonAlt, new ChasingStrategy(targetPlatform)));

            return new MissileShootResponseCmd()
            {
                MissileId = missileShootRequestCmd.MissileId,
                SrcId = missileShootRequestCmd.SrcId,
                TargetId = missileShootRequestCmd.TargetId,
                IsSuccessful = true,
                WhenShot = DateTime.UtcNow,
            };
        }
    }
}
