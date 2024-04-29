﻿using System.Runtime.CompilerServices;

namespace ConsiliumTempus.Application.Common.Extensions;

public static class TaskExtensions
{
    public static TaskAwaiter<(T1, T2)> GetAwaiter<T1, T2>(this (Task<T1>, Task<T2>) tasksTuple)
    {
        return CombineTasks().GetAwaiter();

        async Task<(T1, T2)> CombineTasks()
        {
            var (task1, task2) = tasksTuple;
            await Task.WhenAll(task1, task2);
            return (task1.Result, task2.Result);
        }
    }
}