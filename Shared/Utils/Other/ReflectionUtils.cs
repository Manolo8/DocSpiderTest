﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Shared.Utils.Other {
    public static class ReflectionUtils {
        public static Action<T, TProperty> GetSetter<T, TProperty>(Expression<Func<T, TProperty>> expression) {
            var memberExpression = (MemberExpression) expression.Body;
            var property         = (PropertyInfo) memberExpression.Member;
            var setMethod        = property.GetSetMethod();

            var parameterT         = Expression.Parameter(typeof(T), "x");
            var parameterTProperty = Expression.Parameter(typeof(TProperty), "y");

            var newExpression =
                Expression.Lambda<Action<T, TProperty>>(
                    Expression.Call(parameterT, setMethod, parameterTProperty),
                    parameterT,
                    parameterTProperty
                );

            return newExpression.Compile();
        }
    }
}