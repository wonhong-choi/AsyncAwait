using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    internal class SecondClient
    {
        private readonly IEngine _engine;

        public SecondClient(IEngine engine)
        {
            _engine = engine;
        }

        public async void OnUpdateRequestMsgReceived(int id)
        {
            var response = await _engine.Receive(new UpdateRequestCmd()
            {
                Id = id,
            });

            response.Accept(this);
        }

        public void Visit(UpdateResponseCmd updateResponseCmd)
        {
            Console.WriteLine($"Update > {updateResponseCmd.Id} : {updateResponseCmd.IsSuccessful} / {updateResponseCmd.WhenUpdated}");
        }
    }
}
