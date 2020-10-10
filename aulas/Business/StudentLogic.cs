using Entities;
using Persistence;
using System;
using System.Collections.Generic;

namespace Business
{
    public class StudentLogic
    {
        private Store Store { get; set; }
        public StudentLogic(Store store)
        {
            Store = store;
        }

        public void EnrollToSubject(string studentEmail, string subjectName)
        {
            Student storeStudent = Store.GetStudent(studentEmail);
            Tuple<string, SubjectState, int> oldSubject = storeStudent.TotalSubjects.Find(s => s.Item1.Equals(subjectName));
            storeStudent.TotalSubjects.Remove(oldSubject);
            Tuple<string, SubjectState, int> newSubject = new Tuple<string, SubjectState, int>(subjectName, SubjectState.Enrolled, 0);
            storeStudent.TotalSubjects.Add(newSubject);
        }
        public List<Tuple<string, SubjectState, int>> GetPassedSubjects(string studentEmail)
        {
            Student storeStudent = Store.GetStudent(studentEmail);
            List<Tuple<string, SubjectState, int>> passedSubjects = storeStudent.TotalSubjects.FindAll(s => s.Item2.Equals(SubjectState.Passed));
            return passedSubjects;
        }
    }
}