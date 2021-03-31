using System;

namespace Core.Storage.Context {
    public interface ILastModified {
        DateTime LastModifiedDate { get; set; }
    }
}