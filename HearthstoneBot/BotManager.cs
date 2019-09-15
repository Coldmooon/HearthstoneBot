﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HearthstoneBot.Common;

namespace HearthstoneBot
{
    public interface IRunnable
    {
        // Token: 0x06001173 RID: 4467
        void Start();

        // Token: 0x06001174 RID: 4468
        void Tick();

        // Token: 0x06001175 RID: 4469
        void Stop();
    }

    public interface IBase
    {
        // Token: 0x0600118F RID: 4495
        void Initialize();

        // Token: 0x06001190 RID: 4496
        void Deinitialize();
    }

    public interface IBot : IRunnable, IBase
    {
    }

    class BotManager
    {
        public delegate void BotEvent(IBot bot);

        public static event BotEvent PreStart;

        public static event BotEvent PostStop;

        private static string String_0
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }
        }

        public static string BotsPath
        {
            get
            {
                return Path.Combine(String_0, "Bots");
            }
        }
        private static readonly object object_0;
        public static List<IBot> Bots;

        public static bool Load()
        {
            try
            {
                string botsPath = BotsPath;
                object obj = object_0;
                lock (obj)
                {
                    if (Bots != null)
                    {
                        //BotManager.ilog_0.ErrorFormat("[Load] This function can only be called once.", Array.Empty<object>());
                        return false;
                    }
                    if (!Directory.Exists(botsPath))
                    {
                        Directory.CreateDirectory(botsPath);
                    }
                    Bots = new List<IBot>();

                    AssemblyLoader<IBot> loader = new AssemblyLoader<IBot>(botsPath, false);
                    var types = loader.Instances.AsReadOnly();
                    foreach (IBot bot in types)
                    {
                        try
                        {
                            //Utility.smethod_0(bot);
                            bot.Initialize();
                            Bots.Add(bot);
                        }
                        catch (Exception exception)
                        {
                            //BotManager.ilog_0.Debug("[Load] Exception thrown when initializing " + bot.Name + ". Bot  will not be loaded.", exception);
                            //Utility.smethod_1(bot);
                            bot.Deinitialize();
                        }
                    }
                    return true;
                }
            }
            catch (Exception arg)
            {
                //BotManager.ilog_0.ErrorFormat("[Load] An exception occurred: {0}.", arg);
            }
            return false;
        }

    }
}