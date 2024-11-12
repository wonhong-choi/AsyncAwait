using ConsoleTest.DTOs;
using ConsoleTest.Services;
using ConsoleTest.VOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest.DAOs
{
    public class PlatformDAO
    {
        private readonly IEngine _engine;

        public PlatformDAO(IEngine engine)
        {
            _engine = engine;
            _engine.PlatformInfoUpdated += OnPlatformInfoUpdated;
        }


        public async void OnPlatformCreateRequestReceived(int id, LatLonAlt initialPosition)
        {
            var response = await _engine.Receive(new PlatformCreateRequestCmd()
            {
                Id = id,
                LatLonAlt = initialPosition,
            });

            response.Accept(this);
        }

        public async void OnDeleteRequestMsgReceived(int id)
        {
            var response = await _engine.Receive(new PlatformDeleteRequestCmd()
            {
                Id = id,
            });

            response.Accept(this);
        }

        public async void OnPlatformSyncMsgRequestReceived(int targetId, int originatorId)
        {
            var response = await _engine.Receive(new PlatformSyncRequestCmd()
            {
                TargetId = targetId,
                OriginatorId = originatorId,
            });

            response.Accept(this);
        }

        public void Visit(PlatformCreateResponseCmd platformCreateResponseCmd)
        {
            Console.WriteLine($"Created > {platformCreateResponseCmd.Id} : {platformCreateResponseCmd.IsSuccessful} / {platformCreateResponseCmd.WhenCreated}");
        }

        public void Visit(PlatformDeleteResponseCmd platformDeleteResponseCmd)
        {
            Console.WriteLine($"Deleted > {platformDeleteResponseCmd.Id} : {platformDeleteResponseCmd.IsSuccessful} / {platformDeleteResponseCmd.WhenDeleted}");
        }

        public void Visit(PlatformSyncResponseCmd platformSyncResponseCmd)
        {
            Console.WriteLine($"Synced > {platformSyncResponseCmd.Id} : {platformSyncResponseCmd.IsSuccessful} / {platformSyncResponseCmd.WhenSynced}");
        }

        private void OnPlatformInfoUpdated(PlatformInfo info)
        {
            foreach(var item in info.Items)
            {
                Console.WriteLine($"[Plf {item.Id}] : {item.LatLonAlt.Lat:N2}, " +
                    $"{item.LatLonAlt.Lon:N2}, " +
                    $"{item.LatLonAlt.Alt:N2}, " +
                    $"{item.Heading:N2}, " +
                    $"{item.Speed:N2}, " +
                    $"{item.PlatformState}");
            }
        }
    }
}