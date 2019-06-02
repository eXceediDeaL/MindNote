﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace MindNote.Server.Host.Helpers
{
    public static class RandomHelper
    {
        static readonly Random rand = new Random();

        public static string Color()
        {
            int r = rand.Next(256), g = rand.Next(256), b = rand.Next(256);
            return String.Format("#{0:X}{1:X}{2:X}", r, g, b);
        }

        public static T Choice<T>(IList<T> list)
        {
            return list[rand.Next(list.Count)];
        }
    }
}
