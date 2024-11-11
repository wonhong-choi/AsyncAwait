using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    public class TaskCompletionSourceWrapper
    {
        private readonly TaskCompletionSource<IResponseCmd> _taskCompletionSource;
        private IRequestCmd _requestCmd;

        public TaskCompletionSourceWrapper(IRequestCmd requestCmd)
        {
            _requestCmd = requestCmd;
            _taskCompletionSource = new TaskCompletionSource<IResponseCmd>();
        }

        public Task<IResponseCmd> Task => _taskCompletionSource.Task;

        public void ProcessBy(IRequestVisitor engine)
        {
            var response = _requestCmd.Accept(engine);
            _taskCompletionSource.SetResult(response);
        }
    }
}
