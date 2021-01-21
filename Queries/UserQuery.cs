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

        internal async Task<IEnumerable<RoleResponse>> GetAvailableRoles()
        {
            return await _db.Roles
                .Select(t => _mapper.Map<Role, RoleResponse>(t))
                .ToListAsync();
        }

        internal async Task<IEnumerable<RoleResponse>> AddUserRole(int userId, int roleId)
        {
            var user = await _db.Users
                .Include(t => t.Roles)
                .Where(t => t.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null) return null;

            var role = await _db.Roles
                .Where(t => t.Id == roleId)
                .FirstOrDefaultAsync();

            if (role == null) return null;

            if(!user.Roles.Contains(role))
            {
                user.Roles.Add(role);
                _db.SaveChanges();
                return user.Roles.Select(t => _mapper.Map<Role, RoleResponse>(t));
            }
            else
            {
                return user.Roles.Select(t => _mapper.Map<Role, RoleResponse>(t));
            }
        }

        internal async Task<IEnumerable<RoleResponse>> DeleteUserRole(int userId, int roleId)
        {
            var user = await _db.Users
                .Include(t => t.Roles)
                .Where(t => t.Id == userId)
                .FirstOrDefaultAsync();

            if (user == null) return null;

            if (user.Roles.Where(t => t.Id == roleId).FirstOrDefault() != null)
            {
                user.Roles.Remove(user.Roles.First(t=>t.Id == roleId));
                _db.SaveChanges();
                return user.Roles.Select(t => _mapper.Map<Role, RoleResponse>(t));
            }
            else
            {
                return user.Roles.Select(t => _mapper.Map<Role, RoleResponse>(t));
            }
        }
    }
}
