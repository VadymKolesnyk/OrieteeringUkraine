﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using OrienteeringUkraine.Data;
using OrienteeringUkraine.Models;

namespace OrienteeringUkraine
{
    public class EFDataManager : IDataManager
    {
        private readonly EFContext db;
        public EFDataManager(EFContext db)
        {
            this.db = db;
        }

        public Task AddNewUserAsync(AccountRegisterData data)
        {
            throw new NotImplementedException();
        }

        public Task<AccountUser> GetUserAsync(string login)
        {
            throw new NotImplementedException();
        }

        public Task<AccountUser> GetUserAsync(string login, string password)
        {
            throw new NotImplementedException();
        }
    }
}
