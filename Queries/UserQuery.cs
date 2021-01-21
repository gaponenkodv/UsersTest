using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppTest.Db;
using AppTest.Db.DTO;
using AppTest.Db.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppTest.Queries
{
    public class UserQuery
    {
        readonly ApplicationContext _db;
        IMapper _mapper;

        public UserQuery(ApplicationContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        internal async Task<IEnumerable<UserResponse>> GetUsersAsync()
        {
            return await _db.Users
                .Include(t => t.Roles)
                .Select(t => _mapper.Map<User, UserResponse>(t))
                .ToListAsync();
        }

        internal async Task<UserResponse> FindByIdAsync(int id)
        {
            return await _db.Users
                .Include(t => t.Roles)
                .Where(t => t.Id == id)
                .Select(t => _mapper.Map<User, UserResponse>(t))
                .FirstOrDefaultAsync();
        }

        internal async Task<int> UpdateUserAsync(UserRequest user)
        {
            var updatingUser = await _db.Users
                .Where(t => t.Id == user.Id)
                .FirstAsync();

            updatingUser.Login = user.Login;
            updatingUser.Name = user.Name;

            _db.SaveChanges();

            return updatingUser.Id;
        }

        internal async Task<int> AddUserAsync(UserRequest user)
        {
            var savingUser = _mapper.Map<UserRequest, User>(user);

            await _db.Users.AddAsync(savingUser);
            _db.SaveChanges();

            return savingUser.Id;
        }

        internal async Task<bool> RemoveUserAsync(int id)
        {
            var user = await _db.Users.FirstAsync(t => t.Id == id);

            if (user == null)
                return false;

            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
