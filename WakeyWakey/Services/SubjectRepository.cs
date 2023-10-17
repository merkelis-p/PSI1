using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WakeyWakey.Models;

namespace WakeyWakey.Services;

public class SubjectRepository
{
    private List<SubjectModel> _subjects;
    private readonly string _dataFilePath = "Subjects.csv";

    public SubjectRepository()
    {
        _subjects = LoadSubjectsFromCsv();
    }

    public IEnumerable<SubjectModel> GetAllSubjects()
    {
        return _subjects;
    }

    public SubjectModel GetSubject(int id)
    {
        return _subjects.FirstOrDefault(s => s.Id == id);
    }

    public void AddSubject(SubjectModel subject)
    {
        subject.Id = _subjects.Max(s => s.Id) + 1;
        _subjects.Add(subject);
        SaveSubjectsToCsv();
    }

    public void UpdateSubject(SubjectModel updatedSubject)
    {
        var existingSubject = _subjects.FirstOrDefault(s => s.Id == updatedSubject.Id);
        if (existingSubject != null)
        {
            // Update the properties of the existing subject.
            existingSubject.Name = updatedSubject.Name;
            existingSubject.Description = updatedSubject.Description;
            existingSubject.StartDateTime = updatedSubject.StartDateTime;
            existingSubject.EndDateTime = updatedSubject.EndDateTime;
            SaveSubjectsToCsv();
        }
    }

    public void DeleteSubject(int id)
    {
        var subjectToDelete = _subjects.FirstOrDefault(s => s.Id == id);
        if (subjectToDelete != null)
        {
            _subjects.Remove(subjectToDelete);
            SaveSubjectsToCsv();
        }
    }

    private List<SubjectModel> LoadSubjectsFromCsv()
    {
        var subjects = new List<SubjectModel>();

        if (File.Exists(_dataFilePath))
        {
            using (var reader = new StreamReader(_dataFilePath))
            {
                // Skip the header line.
                reader.ReadLine();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (values.Length >= 5)
                    {
                        var subject = new SubjectModel
                        {
                            Id = int.Parse(values[0]),
                            CourseId = int.Parse(values[1]),
                            Name = values[2],
                            Description = values[3],
                            StartDateTime = DateTime.Parse(values[4]),
                            EndDateTime = DateTime.Parse(values[5])
                        };
                        subjects.Add(subject);
                    }
                }
            }
        }

        return subjects;
    }

    private void SaveSubjectsToCsv()
    {
        using (var writer = new StreamWriter(_dataFilePath))
        {
            // Write the header line.
            writer.WriteLine("Id,CourseId,Name,Description,StartDateTime,EndDateTime");

            foreach (var subject in _subjects)
            {
                writer.WriteLine($"{subject.Id},{subject.CourseId},{subject.Name},{subject.Description},{subject.StartDateTime},{subject.EndDateTime}");
            }
        }
    }
}
