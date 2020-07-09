using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Loader;
using System.Reflection;
using System.Linq;

namespace TrainTime.Models
{
    public class PluginWorker
    {
        public string PluginDirectory { get; set; }

        

        public PluginWorker(string pluginDirectory)
        {
            PluginDirectory = pluginDirectory;
        }

        static public Assembly LoadPlugin(string path)
        {
            PluginLoadContext context = new PluginLoadContext(path);
            return context.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(path)));
        }

        static public IEnumerable<Plugin_Base.ITrainTime> CreateCommands(Assembly asm)
        {
            int cnt = 0;
            foreach (var type in asm.GetTypes())
            {
                if (type.GetInterfaces().Contains(typeof(Plugin_Base.ITrainTime)))
                {
                    Plugin_Base.ITrainTime instance = Activator.CreateInstance(type) as Plugin_Base.ITrainTime;
                    if(instance != null)
                    {
                        cnt++;
                        yield return instance;
                    }
                }               
            }
            if (cnt == 0)
            {
                string availableTypes = string.Join(",", asm.GetTypes().Select(t => t.FullName));
                throw new NotSupportedException($"DLLインスタンスの作成に失敗しました\n{availableTypes}");
            }
        }
    }
}
