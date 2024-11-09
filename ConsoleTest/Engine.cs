using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    public interface IEngine
    {
        void Start();
        void Stop();
        Task<IResponseCmd> Receive<T>(T requestCmd) where T : IRequestCmd;
    }

    public interface ICmdVisitor
    {
        CreateResponseCmd Visit(CreateRequestCmd createRequestCmd);
        DeleteResponseCmd Visit(DeleteRequestCmd deleteRequestCmd);
        NullResponseCmd Visit(NullRequestCmd nullRequestCmd);
    }

    public class Engine : IEngine, ICmdVisitor
    {
        private const double INTERVAL_MS = 100;

        private const int IDLE = 0;
        private const int RUNNING = 1;

        private int _engineState = IDLE;

        private List<TaskCompletionSourceWrapper> _toDoLists = new List<TaskCompletionSourceWrapper>();
        private readonly object _toDoListLock = new object();

        private readonly Dictionary<int, Data> _dataById = new Dictionary<int, Data>();

        public void Start()
        {
            if (IsRunning())
            {
                return;
            }

            Interlocked.Exchange(ref _engineState, RUNNING);

            StartImpl();
        }

        private void StartImpl()
        {
            Task.Run(() =>
            {
                var stopWatch = new Stopwatch();

                while (IsRunning())
                {
                    stopWatch.Start();

                    var todos = default(List<TaskCompletionSourceWrapper>);
                    lock (_toDoListLock)
                    {
                        todos = _toDoLists;
                        _toDoLists = new List<TaskCompletionSourceWrapper>();
                    }

                    foreach (var todo in todos)
                    {
                        todo.ProcessBy(this);
                    }

                    stopWatch.Stop();
                    Thread.Sleep(TimeSpan.FromMilliseconds(Math.Max(INTERVAL_MS - stopWatch.Elapsed.TotalMilliseconds, 0)));

                    stopWatch.Reset();
                }

                Clear();
            });
        }

        private void Clear()
        {
            _dataById.Clear();
            _toDoLists.Clear();
        }

        public void Stop()
        {
            if (IsRunning())
            {
                Interlocked.Exchange(ref _engineState, IDLE);
            }
        }

        public Task<IResponseCmd> Receive<T>(T requestCmd) where T : IRequestCmd
        {
            var tcs = new TaskCompletionSourceWrapper(requestCmd);
            lock (_toDoListLock)
            {
                _toDoLists.Add(tcs);
            }

            return tcs.Task;
        }

        CreateResponseCmd ICmdVisitor.Visit(CreateRequestCmd createRequestCmd)
        {
            if (_dataById.ContainsKey(createRequestCmd.Id))
            {
                return new CreateResponseCmd()
                {
                    Id = createRequestCmd.Id,
                    IsSuccessful = false,
                    WhenCreated = DateTime.UtcNow,
                };
            }

            _dataById.Add(createRequestCmd.Id, new Data()
            {
                Id = createRequestCmd.Id,
                Name = createRequestCmd.Name,
            });

            return new CreateResponseCmd()
            {
                Id = createRequestCmd.Id,
                IsSuccessful = true,
                WhenCreated = DateTime.UtcNow,
            };
        }

        DeleteResponseCmd ICmdVisitor.Visit(DeleteRequestCmd deleteRequestCmd)
        {
            return new DeleteResponseCmd()
            {
                Id = deleteRequestCmd.Id,
                IsSuccessful = _dataById.Remove(deleteRequestCmd.Id),
                WhenDeleted = DateTime.UtcNow,
            };
        }
        NullResponseCmd ICmdVisitor.Visit(NullRequestCmd nullRequestCmd)
        {
            return new NullResponseCmd();
        }

        private bool IsRunning()
        {
            return Interlocked.CompareExchange(ref _engineState, _engineState, RUNNING) == RUNNING;
        }

    }
}
