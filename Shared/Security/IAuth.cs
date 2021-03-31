using System;

namespace Shared.Security {
    public interface IAuth {
        long     Id           { get; }
        string   Name         { get; }
        DateTime LastModified { get; }

        bool HasRole(string name);
    }
}