using System;
using System.Collections.Generic;
using System.Linq;

class Repository<T>
{
    List<T> items = new();
    public void Add(T item) => items.Add(item);
    public List<T> GetAll() => items;
    public T? GetById(Func<T, bool> pred) => items.FirstOrDefault(pred);
    public bool Remove(Func<T, bool> pred) { var i = items.FirstOrDefault(pred); return i != null && items.Remove(i); }
}

class Patient
{
    public int Id; public string Name; public int Age; public string Gender;
    public Patient(int id, string n, int a, string g) { Id = id; Name = n; Age = a; Gender = g; }
}

class Prescription
{
    public int Id; public int PatientId; public string MedicationName; public DateTime DateIssued;
    public Prescription(int id, int pid, string m, DateTime d) { Id = id; PatientId = pid; MedicationName = m; DateIssued = d; }
}

class HealthSystemApp
{
    Repository<Patient> _patients = new();
    Repository<Prescription> _prescriptions = new();
    Dictionary<int, List<Prescription>> _map = new();

    public void SeedData()
    {
        _patients.Add(new Patient(1, "Alice", 30, "F"));
        _patients.Add(new Patient(2, "Bob", 40, "M"));
        _prescriptions.Add(new Prescription(1, 1, "DrugA", DateTime.Now));
        _prescriptions.Add(new Prescription(2, 1, "DrugB", DateTime.Now));
        _prescriptions.Add(new Prescription(3, 2, "DrugC", DateTime.Now));
    }
    public void BuildMap()
    {
        _map = _prescriptions.GetAll().GroupBy(p => p.PatientId)
               .ToDictionary(g => g.Key, g => g.ToList());
    }
    public void PrintAllPatients() => _patients.GetAll()
        .ForEach(p => Console.WriteLine($"{p.Id} - {p.Name}"));
    public void PrintPrescriptionsForPatient(int id)
    {
        if (_map.ContainsKey(id))
            _map[id].ForEach(p => Console.WriteLine($"{p.MedicationName} on {p.DateIssued}"));
    }
    static void Main()
    {
        var app = new HealthSystemApp();
        app.SeedData(); app.BuildMap(); app.PrintAllPatients();
        Console.WriteLine("Prescriptions for Patient 1:");
        app.PrintPrescriptionsForPatient(1);
    }
}

