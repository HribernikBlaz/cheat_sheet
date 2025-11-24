using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

class Program
{
    static string potDoStudentov = "studenti.xml";

    static void FiltrirajPoPovprecju()
    {
        if (!File.Exists(potDoStudentov))
        {
            Console.WriteLine("\nDatoteka ne obstaja!");
            return;
        }

        Console.WriteLine("\n ---- FILTRIRANJE ŠTUDENTOV ----");
        Console.WriteLine("Vnesi minimalno povprečje: ");
        string vnos = Console.ReadLine();
        double minPovprecje;

        if (!double.TryParse(vnos, out minPovprecje))
        {
            Console.WriteLine("Napaka: vnesi veljavno številko!");
            return;
        }

        List<string> filtriraniStudenti = new List<string>();

        try
        {
            XDocument doc = XDocument.Load(potDoStudentov);

            foreach (var student in doc.Descendants("student"))
            {
                double povprecje = double.Parse(student.Element("povprecje").Value);

                if (povprecje >= minPovprecje)
                {
                    string ime = student.Element("ime").Value;
                    string priimek = student.Element("priimek").Value;

                    string izpis = $"ID: {student.Attribute("id").Value}, Ime: {ime}, Priimek: {priimek}, Povprečje: {povprecje}";
                    filtriraniStudenti.Add(izpis);
                    Console.WriteLine(izpis);
                }
            }

            if (filtriraniStudenti.Count > 0)
            {
                string datotekaRezultatov = "povprecja.txt";
                using (StreamWriter sw = new StreamWriter(datotekaRezultatov, false))
                {
                    foreach (string vrstica in filtriraniStudenti)
                    {
                        sw.WriteLine(vrstica);
                    }
                }
                Console.WriteLine($"\nRezultati shranjeni v {datotekaRezultatov}");
            }
            else
            {
                Console.WriteLine("Ni študentov, ki bi ustrezali pogojem.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Prišlo je do napake: {ex.Message}");
        }
        Console.ReadKey();
        Console.Clear();
    }

    static void PovecajPovprecja()
    {
        if (!File.Exists(potDoStudentov))
        {
            Console.WriteLine("\nDatoteka ne obstaja!");
            return;
        }

        Console.WriteLine("\n ---- POVEČANJE POVPREČIJ ----");
        Console.WriteLine("Vnesi odstotek povečanja (npr. 5 za 5%): ");
        string vnos = Console.ReadLine();
        double odstotek;

        if (!double.TryParse(vnos, out odstotek) || odstotek <= 0)
        {
            Console.WriteLine("Napaka: vnesi veljavno številko večjo od 0!");
            return;
        }

        try
        {
            XDocument doc = XDocument.Load(potDoStudentov);
            int študentiPosodobljeni = 0;

            foreach (var student in doc.Descendants("student"))
            {
                double povprecje = double.Parse(student.Element("povprecje").Value);
                double novoPovprecje = povprecje * (1 + odstotek / 100);

                if (novoPovprecje > 10.0)
                    novoPovprecje = 10.0;

                student.Element("povprecje").Value = novoPovprecje.ToString("F2");
                študentiPosodobljeni++;

                Console.WriteLine($"ID: {student.Attribute("id").Value}, Staro povprečje: {povprecje}, Novo povprečje: {novoPovprecje}");
            }

            doc.Save(potDoStudentov);
            Console.WriteLine($"\nPosodobljeno {študentiPosodobljeni} študentov.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Prišlo je do napake: {ex.Message}");
        }

        Console.ReadKey();
        Console.Clear();
    }
}
