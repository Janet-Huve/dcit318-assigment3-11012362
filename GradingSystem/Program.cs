using System;
using System.Collections.Generic;
using System.IO;

class Student
{
    public int Id;
    public string FullName;
    public int Score;
    public string GetGrade() =>
        Score >= 80 ? "A" :
        Score >= 70 ? "B" :
        Score >= 60 ? "C" :
        Score >= 50 ? "D" : "F";
}

class InvalidScoreFormatException : Exception { public InvalidScoreFormatException(string msg) : base(msg) { } }
class MissingFieldException : Exception { public MissingFieldException(string msg) : base(msg) { } }

class StudentResultProcessor
{
    public List<Student> ReadStudentsFromFile(string path)
    {
        var students = new List<Student>();
        foreach (var line in File.ReadAllLines(path))
        {
            var parts = line.Split(',');
            if (parts.Length < 3) throw new MissingFieldException("Missing data in line");
            if (!int.TryParse(parts[2], out int score)) throw new InvalidScoreFormatException("Score format invalid");
            students.Add(new Student { Id = int.Parse(parts[0]), FullName = parts[1], Score = score });
        }
        return students;
    }

    public void WriteReportToFile(List<Student> students, string path)
    {
        using var sw = new StreamWriter(path);
        foreach (var s in students)
            sw.WriteLine($"{s.FullName} (ID: {s.Id}): Score = {s.Score}, Grade = {s.GetGrade()}");
    }
}

class Program
{
    static void Main()
    {
        try
        {
            var processor = new StudentResultProcessor();
            var students = processor.ReadStudentsFromFile("input.txt");
            processor.WriteReportToFile(students, "output.txt");
            Console.WriteLine("Report generated successfully.");
        }
        catch (FileNotFoundException) { Console.WriteLine("Error: Input file not found."); }
        catch (InvalidScoreFormatException ex) { Console.WriteLine($"Error: {ex.Message}"); }
        catch (MissingFieldException ex) { Console.WriteLine($"Error: {ex.Message}"); }
        catch (Exception ex) { Console.WriteLine($"Unexpected error: {ex.Message}"); }
    }
}

