using System;
using System.Collections.Generic;
using Core.Application.Users.Dtos.Patches;
using Core.Domain.Documents.Models;
using Core.Storage.Context;
using Shared.Entities;

namespace Core.Domain.Users.Models {
    public class User : Entity, ILastModified {
        public string Name     { get; set; }
        public string Email    { get; set; }
        public bool   Active   { get; set; } = true;
        public bool   Verified { get; set; } = false;

        public string         Password         { get; set; }
        public DateTime       LastModifiedDate { get; set; }
        public List<Document> Documents        { get; set; }
    }
}