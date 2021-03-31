using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.Dtos;
using Shared.Entities;
using Shared.Exceptions;

namespace Shared.Repositories {
    public static class PathExtensions {
        // public static T ApplyToNewInstance<T>(this IPatch<T> t, DbContext context) where T : Entity {
        //     var instance = Activator.CreateInstance<T>();
        //
        //     t?.Apply(instance, context);
        //
        //     return instance;
        // }
        //
        // public static void AttachAndApply<T>(this IPatch<T> t, DbContext context, long? id = null)
        //     where T : Entity {
        //     var instance = Activator.CreateInstance<T>();
        //
        //     if (id is null) {
        //         if (t is DtoWithId dtoWithId)
        //             instance.Id = dtoWithId.Id;
        //         else
        //             throw new InvalidOperationException();
        //     }
        //     else {
        //         if (t is DtoWithId dtoWithId) {
        //             if (dtoWithId.Id != id)
        //                 throw new NotAllowedException("Impossível alterar id FIXO");
        //
        //             instance.Id = dtoWithId.Id;
        //         }
        //         else
        //             instance.Id = (long) id;
        //
        //         t.Apply(context.Attach(instance).Entity, context);
        //     }
        // }
    }
}