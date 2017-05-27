﻿using System.Collections.Generic;
using System.Data;
using System.Data.Linq;

namespace Rocket.API.Providers.Database
{
    [RocketProvider]
    public interface IDatabaseProvider : IRocketDataProviderBase
    {
        IDbConnection Connection { get; }
        void Setup(string connectionString);
        ContextInitializationResult InitializeContext(DatabaseContext context);
        bool TableExists(string name);
        List<DatabaseContext> DatabaseContexts { get; }
    }
}