﻿using System;
using System.Collections.Generic;
using BashSoft.Exceptions;

namespace BashSoft.Models
{
    public class Course
    {
        public const int NumberOfTasksOnExam = 5;
        public const int MaxScoreOnExamTask = 100;

        private string name;
        private Dictionary<string, Student> studentsByName;

        public Course(string name)
        {
            this.Name = name;
            this.studentsByName = new Dictionary<string, Student>();
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException(nameof(this.name), ExceptionMessages.NullOrEmptyValue);
                }

                this.name = value;
            }
        }

        public IReadOnlyDictionary<string, Student> StudentsByName
        {
            get
            {
                return this.studentsByName;
            }
        }


        public void EnrollStudent(Student student)
        {
            if (this.studentsByName.ContainsKey(student.UserName))
            {
                //OutputWriter.DisplayException(string.Format(ExceptionMessages.StudentAlreadyEnrolledInGivenCourse, student.UserName, this.name));
                throw new DuplicateEntryInStructureException(student.UserName, this.Name);
            }

            this.studentsByName.Add(student.UserName, student);
        }
    }
}
