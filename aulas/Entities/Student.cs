using System;
using System.Collections.Generic;

namespace Entities
{
    public class Student
    {
        public Client Client { get; set; }
        public int Number { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        //nombre, estado (por defecto es pending), puntaje (por defecto es cero)
        public List<Tuple<string, SubjectState, int>> TotalSubjects { get; set; }
        private List<Session> Sessions { get; }

        public Student(string email, string password)
        {
            Email = email;
            Password = password;
            TotalSubjects = new List<Tuple<string, SubjectState, int>>();
        }
    }
}