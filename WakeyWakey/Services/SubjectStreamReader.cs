using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WakeyWakey.Models;

namespace WakeyWakey.Services;



public class SubjectStreamReader : IEnumerable<SubjectModel>
{
    private List<SubjectModel> _subjects;
    private readonly string _dataFilePath = "Services/Subjects.csv";

    //public delegate void SubjectActionDelegate(SubjectModel subject);

    public SubjectActionDelegate OnSubjectAction;

    public SubjectStreamReader()
    {
        _subjects = LoadSubjectsFromCsv();
    }

    public IEnumerable<SubjectModel> GetAllSubjects()
    {
        return _subjects;
    }

    public SubjectModel GetSubject(int id) =>
            _subjects.FirstOrDefault(s => s.Id == id);

    public void AddSubject(SubjectModel subject)
    {
        subject.Id = _subjects.Max(i => i.Id) + 1;
        subject.CourseId = _subjects.Max(i => i.CourseId) + 1;
        _subjects.Add(subject);
        SaveSubjectsToCsv();

        // Trigger the delegate after adding a subject
        OnSubjectAction?.Invoke(subject);
    }

    public void UpdateSubject(SubjectModel updatedSubject)
    {
        var existingSubject = _subjects.FirstOrDefault(s => s.Id == updatedSubject.Id);
        if (existingSubject != null)
        {
            existingSubject.Name = updatedSubject.Name;
            existingSubject.Description = updatedSubject.Description;
            existingSubject.StartDateTime = updatedSubject.StartDateTime;
            existingSubject.EndDateTime = updatedSubject.EndDateTime;
            SaveSubjectsToCsv();

            // Trigger the delegate after updating a subject
            OnSubjectAction?.Invoke(existingSubject); 
        }
    }

    public void DeleteSubject(int id)
    {
        var subjectToDelete = _subjects.FirstOrDefault(s => s.Id == id);
        if (subjectToDelete != null)
        {
            _subjects.Remove(subjectToDelete);
            SaveSubjectsToCsv();

            // Trigger the delegate after deleting a subject
            OnSubjectAction?.Invoke(subjectToDelete);
        }
    }

    private List<SubjectModel> LoadSubjectsFromCsv()
    {
        var subjects = new List<SubjectModel>();

        if (File.Exists(_dataFilePath))
        {
            using (var reader = new StreamReader(_dataFilePath))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (values.Length >= 5) {
                        var subject = new SubjectModel {
                            Id = int.Parse(values[0]),
                            CourseId = int.Parse(values[1]),
                            Name = values[2],
                            Description = values[3]
                        };

                        DateTime startDateTime;
                        if (DateTime.TryParse(values[4], out startDateTime)) {
                            subject.StartDateTime = startDateTime;
                        }
                        else {
                            subject.StartDateTime = null;
                        }

                        DateTime endDateTime;
                        if (DateTime.TryParse(values[5], out endDateTime)) {
                            subject.EndDateTime = endDateTime;
                        }
                        else {
                            subject.EndDateTime = null;
                        }

                        subjects.Add(subject);
                    }
                }
            }

        }

        return subjects;
    }

    private void SaveSubjectsToCsv()
    {
        using var writer = new StreamWriter(_dataFilePath);
        writer.WriteLine("Id,CourseId,Name,Description,StartDateTime,EndDateTime");

        foreach (var subject in _subjects)
        {
            writer.WriteLine($"{subject.Id},{subject.CourseId},{subject.Name},{subject.Description},{subject.StartDateTime},{subject.EndDateTime}");
        }
    }

    public IEnumerator<SubjectModel> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
