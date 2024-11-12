using ConsoleTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.DTOs
{
    public class RequestCmdTaskWrapper
    {
        private readonly TaskCompletionSource<IResponseCmd> _taskCompletionSource;
        private IRequestCmd _requestCmd;

        public RequestCmdTaskWrapper(IRequestCmd requestCmd)
        {
            _requestCmd = requestCmd;
            _taskCompletionSource = new TaskCompletionSource<IResponseCmd>();
        }

        public Task<IResponseCmd> Task => _taskCompletionSource.Task;

        public void SetResultBy(IRequestVisitor engine)
        {
            var response = _requestCmd.Accept(engine);
            _taskCompletionSource.SetResult(response);
        }
    }
}
