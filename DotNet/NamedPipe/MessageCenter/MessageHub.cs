using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace MessageCenter
{
    public class MessageHub
    {
        private readonly IDictionary<string, Type> m_MessageTypes = new Dictionary<string, Type>();
        private readonly string m_PipeName;

        public MessageHub(string pipeName)
        {
            m_PipeName = pipeName;

            RegisterAssemblyMessages(Assembly.GetExecutingAssembly());
        }

        public void RegisterAssemblyMessages(Assembly assembly)
        {
            var types = assembly.GetTypes()
                    .Where(t => !t.IsAbstract && t.IsSubclassOf(typeof(MessageBase)));

            foreach (var type in types)
            {
                var name = type.Name;
                if (m_MessageTypes.ContainsKey(name))
                    throw new InvalidOperationException("Duplicated message type name " + name);

                m_MessageTypes.Add(name, type);
            }
        }

        private NamedPipeServerStream CreatePipeServer()
        {
            return new NamedPipeServerStream(m_PipeName, PipeDirection.InOut);
        }

        private string MessageToJson(IMessage message)
        {
            var seralizer = new DataContractJsonSerializer(message.GetType());

            using (var ms = new MemoryStream())
            {
                seralizer.WriteObject(ms, message);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private IMessage JsonToMessage(Type type, string jsonContent)
        {
            var seralizer = new DataContractJsonSerializer(type);

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent)))
            {
                return (IMessage)seralizer.ReadObject(ms);
            }
        }

        public void Send(IMessage message)
        {
            if (message == null) throw new ArgumentNullException("message");
            var pipeServer = CreatePipeServer();
            Task.Run(delegate
            {
                using (var wirter = new BinaryWriter(pipeServer, Encoding.UTF8))
                {
                    pipeServer.WaitForConnection();
                    wirter.Write(message.GetType().Name);
                    wirter.Write(MessageToJson(message));
                }
            });
        }

        public IMessage Receive()
        {
            string name, content;
            using (var pipeServer = CreatePipeServer())
            using (var reader = new BinaryReader(pipeServer, Encoding.UTF8, true))
            {
                pipeServer.WaitForConnection();
                name = reader.ReadString();
                content = reader.ReadString();
            }

            if (!m_MessageTypes.ContainsKey(name))
            {
                Trace.WriteLine(
                    string.Format("Discard message {0}:{1}{2}",
                        name, Environment.NewLine, content));
                return null;
            }

            var type = m_MessageTypes[name];
            return JsonToMessage(type, content);
        }

    }
}
