using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; } = null;
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public byte[]? Photo { get; set; } = null;
        public List<Project> Projects { get; set; } = new List<Project>();
        public List<Desk> Desks { get; set; } = new List<Desk>();
        public List<Task> Tasks { get; set; } = new List<Task>();
        public UserStatus Status { get; set; }
        public User() { }
        public User(string fname, string lname, string email, string password, UserStatus status = UserStatus.User, string ? phone = null, byte[]? photo = null)
        {
            FirstName = fname;
            LastName = lname;
            Email = email;
            Password = password;
            Phone = phone;
            Photo = photo;
            RegistrationDate = DateTime.Now;
            LastLoginDate = DateTime.Now;
            Status = status;
        }
        // Создается библиотека моделей UserModel.cs 
        public UserModel ToDto() // Метод позволяет на основе юезра создать модель он возвращает модель юзера
        {
            return new UserModel()
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                Password = this.Password,
                Phone = this.Phone,
                Photo = this.Photo,
                RegistrationDate = this.RegistrationDate,
                Status = this.Status
            };
        }
        // Далее переходим в UserController и в метод CreateUser добавляем UserModel 
        public User(UserModel model)
        {
            Id = model.Id;
            FirstName = model.FirstName;
            LastName = model.LastName;
            Email = model.Email;
            Password = model.Password;
            Phone = model.Phone;
            Photo = model.Photo;
            RegistrationDate = model.RegistrationDate;
            Status = model.Status;
        }
    }
}

    
