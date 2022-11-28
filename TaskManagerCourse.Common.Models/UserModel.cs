using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerCourse.Common.Models
{
    // Хотим хранить модели на стороне без создания лишнего кода, чтобы к нему можно было получить доступ фронту для использования библиотеки классов
    // Для этого переносим атрибуты кроме entity framework отношений из User.cs в UserModel.cs 
    public class UserModel
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
        public UserStatus Status { get; set; } // UserStatus выносится из Services Namespace делается такой же как у библиотеки 

        // Конструктор для создания UserModel
        public UserModel(string fname, string lname, string email, string password, UserStatus status, string? phone) 
        {
            FirstName = fname;
            LastName = lname;
            Email = email;
            Password = password;
            Phone = phone;
            RegistrationDate = DateTime.Now;
            Status = status;
        }
        // Пустой конструктор UserModel
        public UserModel() 
        { }
        // Создается метод ToDto в классе User.cs 


        public override string ToString()
        {
            return $"{FirstName} {LastName}";
            
        }
    }
}
