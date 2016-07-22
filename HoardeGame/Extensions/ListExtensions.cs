// <copyright file="ListExtensions.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;

namespace HoardeGame.Extensions
{
    /// <summary>
    /// List extension methods
    /// </summary>
    public static class ListExtensions
    {
        private static readonly Random Rng = new Random();

        /// <summary>
        /// Sheffles a list
        /// </summary>
        /// <typeparam name="T">Type of the list</typeparam>
        /// <param name="list">List to shuffle</param>
        /// <returns>Shuffled list</returns>
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = Rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }

            return list;
        }
    }
}