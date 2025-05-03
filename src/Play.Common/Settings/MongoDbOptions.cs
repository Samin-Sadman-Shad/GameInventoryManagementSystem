﻿namespace Play.Common.Settings
{
    public class MongoDbOptions
    {
        public const string MongoDbSettings = "MongoDbSettings";

        public string Host { get; init; }
        public int Port { get; init; }

        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }
}
