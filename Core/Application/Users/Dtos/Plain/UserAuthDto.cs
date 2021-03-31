using System;

namespace Core.Application.Users.Dtos.Plain {
    public class UserAuthDto {
        public long     Id                  { get; set; }
        public string   Name                { get; set; }
        public DateTime LastModified        { get; set; }
        public string[] Roles { get; set; }
    }
}