﻿using System;

namespace Core.Logging
{
    public interface ILogger
    {
        void Log(string message);
        void Log(Exception ex);
    }
}