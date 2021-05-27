using Events.Api.Models.UserManagement;
using Events.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;

namespace UserManagment.services
{
    public interface IUserService
    {
        EUser Authenticate(string username, string password);
        IEnumerable<EUser> GetAll();
        EUser GetByUsername(string username);

        EUser GetById(long username);

        EUser Create(EUser employee, string password);
        List<EUser> GetEmpsOfDepartment(long Id);
        EUser GetHead(long Department);

        void Update(EUser employee, string password = null);
        void Delete(int id);
    }

    //=======================================================================================================================
    public class UserService : IUserService
    {
        private readonly AppDbContext _ctx;
        private UserManager<EUser> _userManager;

        public UserService(AppDbContext context, UserManager<EUser> userManager)
        {
            _ctx = context;
            _userManager = userManager;
        }


        //=======================================================================================================================

        public EUser Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _ctx.Employees.SingleOrDefault(x => x.UserName == username);

            // check if username exists 
            if (user == null)
                return null;


            return user;
        }

        //=======================================================================================================================

        public IEnumerable<EUser> GetAll()
        {
            return _ctx.Employees;
        }


        //=======================================================================================================================

        public EUser GetByUsername(string username)
        {
            return _ctx.Employees.Where(x => x.UserName == username)
                .Include(x => x.Section)
                .ThenInclude(x => x.Department)
                .FirstOrDefault();

        }



        //=======================================================================================================================

        public EUser Create(EUser employee, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_ctx.Employees.Any(x => x.UserName == employee.UserName))
                throw new AppException("Username " + employee.UserName + " is already taken");


            employee.PasswordHash = password;


            _ctx.Employees.Add(employee);
            _ctx.SaveChanges();

            return employee;
        }


        //=======================================================================================================================

        public void Update(EUser empParm, string password = null)
        {
            var employee = _ctx.Employees.Find(empParm.Id);
            if (employee == null)
                throw new AppException("User Not Found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(empParm.UserName) && empParm.UserName != employee.UserName)
            {
                // throw error if the new username is already taken
                if (_ctx.Employees.Any(x => x.UserName == empParm.UserName))
                    throw new AppException("Username " + empParm.UserName + " is already taken");
                employee.UserName = empParm.UserName;
            }
            // update user properties if provided

            if (!string.IsNullOrWhiteSpace(empParm.FullName))
                employee.FullName = empParm.FullName;

            if (!string.IsNullOrWhiteSpace(password))
            {
                employee.PasswordHash = password;
            }

            _ctx.Employees.Update(employee);
            _ctx.SaveChanges();
        }



        //=======================================================================================================================


        public void Delete(int id)
        {
            var employee = _ctx.Employees.Find(id);
            if (employee != null)
            {

                _ctx.Employees.Remove(employee);
                _ctx.SaveChanges();
            }

        }

        public EUser GetHead(long Department)
        => _ctx.Employees.Where(e => e.Section.Department.Id == Department && e.IsHead).SingleOrDefault();
        

        public List<EUser> GetEmpsOfDepartment(long Id)
        => _ctx.Users.Where(e => e.Section.Department.Id == Id).ToList();

        EUser GetById(long username)
        {
            return _ctx.Employees.Where(x => x.Id == username)
            .Include(x => x.Section)
            .ThenInclude(x => x.Department)
            .FirstOrDefault();
        }

        EUser IUserService.GetById(long username)
        {
            return _ctx.Employees.Where(x => x.Id == username)
            .Include(x => x.Section)
            .ThenInclude(x => x.Department)
            .FirstOrDefault();
        }
    }
}

