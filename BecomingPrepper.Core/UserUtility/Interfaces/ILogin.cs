﻿namespace BecomingPrepper.Core.UserUtility
{
    public interface ILogin
    {
        string AccountId { get; set; }
        bool Authenticate(string username, string password);
    }
}
