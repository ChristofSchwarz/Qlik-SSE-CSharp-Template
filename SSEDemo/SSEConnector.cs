namespace SSEDemo
{
    using Grpc.Core;
    #region Usings
    using NLog;
    using Qlik.Sse;
    using System;
    using System.Collections.Generic;
    using System.Text;
    #endregion

    public class SseConnector
    {
        #region Logger
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        #region Properties & Variables
        private Server server;
        private SseEvaluator sseEvaluator;
        #endregion

        public void Start()
        {
            try
            {
                logger.Debug("Service running...");
                logger.Debug($"Start Service on Port \"50047\" with Host \"localhost");
                logger.Debug($"Server start...");

                using (sseEvaluator = new SseEvaluator())
                {
                    server = new Server()
                    {
                        Services = { Connector.BindService(sseEvaluator) },
                        Ports = { new ServerPort("localhost", 50047, ServerCredentials.Insecure) },
                    };

                    server.Start();
                    logger.Info($"gRPC listening on port 50047 on Host localhost");
                    logger.Info($"Ready...");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Service could not be started.");
            }
        }

        public void Stop()
        {
            try
            {
                logger.Info("Shutdown SSEDemo...");
                server?.ShutdownAsync().Wait();
                sseEvaluator.Dispose();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Service could not be stoppt.");
            }
        }
    }
}
