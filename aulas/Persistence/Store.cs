using System.Collections.Generic;
using Entities;
using System;

namespace Persistence
{
    public class Store
    {
        public List<Entities.Client> Clients { get; set; }
        public List<Student> Students { get; set; }
        public List<Subject> Subjects { get; set; }

        public Store()
        {
            Clients = new List<Entities.Client>();
            Students = new List<Student>();
            Subjects = new List<Subject>();
        }

        //Subject
        public void AddSubject(string subjectName)
        {
            Tuple<string, SubjectState, int> newSubject = new Tuple<string, SubjectState, int>(subjectName, SubjectState.Pending, 0);
            foreach (Student student in Students)
            {
                student.TotalSubjects.Add(newSubject);
            }
            //PREGUNTA: tiene sentido tener lista de materias en el store? xq cuando agregamos o sacamos una, ademas de
            //sacarla o a agregarla a la lista de materias de cada alumno, hay que hacer lo mismo en la lista total
            //PREGUNTA 2: para consultar los cursos disponibles hay que estar logueados no?
        }
        public void RemoveSubject(string subjectName)
        {
            foreach (Student student in Students)
            {
                Tuple<string, SubjectState, int> newSubject = student.TotalSubjects.Find(s => s.Item1.Equals(subjectName));
                student.TotalSubjects.Remove(newSubject);
            }
        }
        //Student
        public Student GetStudent(string email)
        {
            return Students.Find(s => s.Email.Equals(email));
        }

        public void AddStudent(Student student)
        {
            Students.Add(student);
        }

        public List<Student> GetStudents()
        {
            return Students;
        }

        public Student GetLoggedStudent(string clientUsername)
        {
            return Students.Find(s => s.Client.Username.Equals(clientUsername));
        }

        public bool StudentExists(Student student)
        {
            return Students.Contains(student);
        }
        
        //Clients
        public Entities.Client GetClient(string username)
        {
            return Clients.Find(c => c.Username.Equals(username));
        }

        public List<Subject> GetSubjects()
        {
            return Subjects;
        }

        public bool ClientExists(Entities.Client client)
        {
            return Clients.Contains(client);
        }

        public void AddClient(Entities.Client client)
        {
            Clients.Add(client);
        }
    }
}