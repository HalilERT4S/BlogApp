﻿using BlogApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string userName);

        Task AddUserAsync(User user);
    }
}
