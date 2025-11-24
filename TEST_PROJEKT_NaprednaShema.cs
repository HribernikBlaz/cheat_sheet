using System.Diagnostics;
using System.Globalization;
using System.Xml;
using System.Xml.Schema;

namespace test_projekt
{
    internal class Program
    {
        static string potKnjigeTXT = "knjige.txt";
        static string potDoKnjige = "knjige.xml";
        static string potClani = "clani.xml";
        static string potIzposoje = "izposoje.xml";
        static string potDoBesednjaka = "besednjak.dtd";
        static string potDoSheme = "shema.xsd";

        static List<string> validacijskeNapake = new List<string>();
        static bool validacijaOK = false;

        static void Main(string[] args)
        {
            bool konec = false;
            while (!konec)
            {
                Console.WriteLine();
                Console.WriteLine("========== MENI ==========");
                Console.WriteLine("1 - TXT Dodaj knjigo");
                Console.WriteLine("2 - TXT Izpiši vse knjige");
                Console.WriteLine("3 - TXT Filtriraj knjige po ceni (cena >= vnešena cena) -> rezultati_iskanja.txt");
                Console.WriteLine("4 - TXT Povečaj cene za % in izvoz -> posodobljene_knjige.txt");
                Console.WriteLine("5 - XML Dodaj člana");
                Console.WriteLine("6 - XML Dodaj izposojo knjige");
                Console.WriteLine("7 - XML Izpiši člane z izposojenimi knjigami");
                Console.WriteLine("8 - VALIDIRAJ XML");
                Console.WriteLine("9 - Izhod iz programa");

                //NarediDatoteke();
                //UstvariTestnePodatke();
                //NarediDatotekeXmlShema();
                //UstvariTestnePodatkeXmlShema();

                Console.Write("Vnesite možnost: ");
                string izbira = Console.ReadLine();
                switch (izbira)
                {
                    case "1":
                        DodajKnjigo();
                        break;
                    case "2":
                        IzpisiVse();
                        break;
                    case "3":
                        FiltrirajPoCeni();
                        break;
                    case "4":
                        PovecajCene();
                        break;
                    case "5":
                        DodajClana();
                        break;
                    case "6":
                        DodajIzposojo();
                        break;
                    case "7":
                        IzpisiClaneZIzposojami();
                        break;
                    case "8":
                        ValidirajXML_Shema(potClani);
                        break;
                    case "9":
                        konec = true;
                        break;
                    default:
                        Console.WriteLine("Napačna izbira!");
                        break;
                }
            }
        }
        // DTD DEL
        static void ValidirajXML(string pot)
        {
            validacijaOK = true;

            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Parse;
                settings.ValidationType = ValidationType.DTD;
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                settings.XmlResolver = new XmlUrlResolver();

                using (XmlReader reader = XmlReader.Create(pot, settings))
                {
                    while (reader.Read());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Napaka pri validaciji: {ex.Message}");
                validacijaOK = false;
            }
            if (validacijaOK)
                Console.WriteLine($"Validacija je bila uspešna");
        }

        private static void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            Console.WriteLine($"❌ Napaka pri validaciji: {e.Message}");
            validacijaOK = false;
        }

        static void NarediDatotekeDTD()
        {
            if (!File.Exists(potClani))
            {
                string xmlClani = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE clani SYSTEM ""{potDoBesednjaka}"">
<clani>
</clani>";
                File.WriteAllText(potClani, xmlClani);
            }

            if (!File.Exists(potIzposoje))
            {
                string xmlIzposoje = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE izposoje SYSTEM ""{potDoBesednjaka}"">
<izposoje>
</izposoje>";
                File.WriteAllText(potIzposoje, xmlIzposoje);
            }
        }

        static void UstvariTestnePodatkeDTD()
        {
            Console.WriteLine("\n To bo prepisalo obstoječe podatke!");
            Console.Write("Ste prepričani? (y/n): ");
            string potrditev = Console.ReadLine();

            if (potrditev?.ToLower() != "y")
            {
                Console.WriteLine("Prekinjeno");
                return;
            }

            // CLANI
            XmlDocument docClani = new XmlDocument();
            XmlDeclaration declClani = docClani.CreateXmlDeclaration("1.0", "UTF-8", null);
            docClani.AppendChild(declClani);

            XmlDocumentType doctypeClani = docClani.CreateDocumentType(
                "clani", null, potDoBesednjaka, null);
            docClani.AppendChild(doctypeClani);

            XmlElement rootClani = docClani.CreateElement("clani");
            docClani.AppendChild(rootClani);

            UstvariClanaDTD(docClani, rootClani, "C001", "ime1", "priimek1", "email1@gmail.com");
            UstvariClanaDTD(docClani, rootClani, "C002", "ime2", "priimek2", "email2@gmail.com");
            UstvariClanaDTD(docClani, rootClani, "C003", "ime3", "priimek3", "email3@gmail.com");

            docClani.Save(potClani);

            // ARTIKLI
            XmlDocument docIzposoje = new XmlDocument();
            XmlDeclaration declIzposoje = docIzposoje.CreateXmlDeclaration("1.0", "UTF-8", null);
            docIzposoje.AppendChild(declIzposoje);

            XmlDocumentType doctypeIzposoje = docIzposoje.CreateDocumentType(
                "izposoje", null, potDoBesednjaka, null);
            docIzposoje.AppendChild(doctypeIzposoje);

            XmlElement rootIzposoje = docIzposoje.CreateElement("izposoje");
            docIzposoje.AppendChild(rootIzposoje);

            // Uporablja tvojo metodo UstvariArtikel
            UstvariIzposojoDTD(docIzposoje, rootIzposoje, "C001", "K001", "naslov1", "2025-01-01");
            UstvariIzposojoDTD(docIzposoje, rootIzposoje, "C002", "K004", "naslov2", "2025-01-11");

            docIzposoje.Save(potIzposoje);

            Console.WriteLine("Testni podatki uspešno ustvarjeni!");
        }

        static void UstvariClanaDTD(XmlDocument doc, XmlElement root, string id, string ime, string priimek, string email)
        {
            XmlElement clan = doc.CreateElement("clan");
            clan.SetAttribute("id", id);

            XmlElement imeEl = doc.CreateElement("ime");
            imeEl.InnerText = ime;
            clan.AppendChild(imeEl);

            XmlElement priimekEl = doc.CreateElement("priimek");
            priimekEl.InnerText = priimek;
            clan.AppendChild(priimekEl);

            XmlElement emailEl = doc.CreateElement("email");
            emailEl.InnerText = email;
            clan.AppendChild(emailEl);

            root.AppendChild(clan);
        }

        static void UstvariIzposojoDTD(XmlDocument doc, XmlElement root, string idClana, string idIzposoje, string naslov, string datumIzposoje)
        {
            XmlElement izposoja = doc.CreateElement("izposoja");

            XmlElement idClanaEl = doc.CreateElement("idClana");
            idClanaEl.InnerText = idClana;
            izposoja.AppendChild(idClanaEl);

            XmlElement idIzposojeEl = doc.CreateElement("idIzposoje");
            idIzposojeEl.InnerText = idIzposoje;
            izposoja.AppendChild(idIzposojeEl);

            XmlElement naslovEl = doc.CreateElement("naslov");
            naslovEl.InnerText = naslov;
            izposoja.AppendChild(naslovEl);

            XmlElement datumIzposojeEl = doc.CreateElement("datumIzposoje");
            datumIzposojeEl.InnerText = datumIzposoje;
            izposoja.AppendChild(datumIzposojeEl);

            root.AppendChild(izposoja);
        }


        // XML SHEMA DEL
        static void ValidirajXML_Shema(string pot)
        {
            /*validacijaOK = true;

            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBackShema);

                // Add schema file
                settings.Schemas.Add(null, "shema.xsd");

                using (XmlReader reader = XmlReader.Create(pot, settings))
                {
                    while (reader.Read()) ;
                }

                if (validacijaOK)
                {
                    Console.WriteLine("XML is valid!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Validation error: {ex.Message}");
                validacijaOK = false;
            }*/

            validacijaOK = true;
            validacijskeNapake.Clear();

            try
            {
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.XmlResolver = new XmlUrlResolver(); // ključna vrstica — omogoči resolviranje include/redefine
                schemaSet.Add(null, potDoSheme);

                /*shema.xsd
                    ↓ include
                  tipi.xsd(EmailType, DavcnaStevilkaType, ArtikelIdType, BaseArtikelType...)
                    ↓ redefine
                  omejitve.xsd(BaseDobaviteljType → razširjen z dodatnimi elementi)*/


                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += ValidationCallBack; // kaj se nardi ob napaki
                settings.Schemas = schemaSet;
                settings.XmlResolver = new XmlUrlResolver();

                using (XmlReader reader = XmlReader.Create(pot, settings))
                {
                    Console.WriteLine($"Preverjam: {pot}");
                    while (reader.Read()) ; // prebere cel dokument, validacija poteka sproti
                }

                if (validacijaOK)
                {
                    Console.WriteLine($"Dokument {pot} je veljaven!");
                }
                else
                {
                    Console.WriteLine($"Dokument {pot} vsebuje napake:");
                    foreach (string napaka in validacijskeNapake)
                    {
                        Console.WriteLine($"  - {napaka}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Kritična napaka pri validaciji: {ex.Message}");
                validacijaOK = false;
            }
        }

        private static void ValidationCallBackShema(object sender, ValidationEventArgs e)
        {
            Console.WriteLine($"Validation error: {e.Message}");
            validacijaOK = false;
        }

        static void NarediDatotekeXmlShema()
        {
            if (!File.Exists(potClani))
            {
                string xmlClani = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<clani xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:noNamespaceSchemaLocation=""shema.xsd"">
</clani>";
                File.WriteAllText(potClani, xmlClani);
            }

            if (!File.Exists(potIzposoje))
            {
                string xmlIzposoje = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<izposoje xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
             xsi:noNamespaceSchemaLocation=""shema.xsd"">
</izposoje>";
                File.WriteAllText(potIzposoje, xmlIzposoje);
            }
        }

        static void UstvariTestnePodatkeXmlShema()
        {
            Console.WriteLine("\n To bo prepisalo obstoječe podatke!");
            Console.Write("Ste prepričani? (y/n): ");
            string potrditev = Console.ReadLine();

            if (potrditev?.ToLower() != "y")
            {
                Console.WriteLine("Prekinjeno");
                return;
            }

            // Člani
            XmlDocument docClani = new XmlDocument();
            XmlDeclaration declClani = docClani.CreateXmlDeclaration("1.0", "UTF-8", null);
            docClani.AppendChild(declClani);

            XmlElement rootClani = docClani.CreateElement("clani");
            rootClani.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            rootClani.SetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "shema.xsd");
            docClani.AppendChild(rootClani);

            UstvariClana(docClani, rootClani, "C001", "ime1", "priimek1", "email1@gmail.com");
            UstvariClana(docClani, rootClani, "C002", "ime2", "priimek2", "email2@gmail.com");
            UstvariClana(docClani, rootClani, "C003", "ime3", "priimek3", "email3@gmail.com");

            docClani.Save(potClani);

            // Izposoje
            XmlDocument docIzposoje = new XmlDocument();
            XmlDeclaration declIzposoje = docIzposoje.CreateXmlDeclaration("1.0", "UTF-8", null);
            docIzposoje.AppendChild(declIzposoje);

            XmlElement rootIzposoje = docIzposoje.CreateElement("izposoje");
            rootIzposoje.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            rootIzposoje.SetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "shema.xsd");
            docIzposoje.AppendChild(rootIzposoje);

            UstvariIzposojo(docIzposoje, rootIzposoje, "C001", "K001", "naslov1", "2025-01-01");
            UstvariIzposojo(docIzposoje, rootIzposoje, "C002", "K004", "naslov2", "2025-01-11");

            docIzposoje.Save(potIzposoje);

            Console.WriteLine("Testni podatki uspešno ustvarjeni!");
        }



        // TXT DEL
        static void DodajKnjigo()
        {
            Console.Write("Naslov: ");
            string naslov = Console.ReadLine();

            Console.Write("Avtor: ");
            string avtor = Console.ReadLine();

            Console.Write("Leto izdaje: ");
            if (!int.TryParse(Console.ReadLine(), out int leto) || leto <= 1000)
            {
                Console.WriteLine("Napaka: Leto mora biti številka > 1000.");
                return;
            }

            Console.Write("Cena: ");
            if (!double.TryParse(Console.ReadLine(), out double cena) || cena <= 0)
            {
                Console.WriteLine("Napaka: Cena mora biti > 0.");
                return;
            }

            string vrstica = $"{naslov};{avtor};{leto};{cena}";

            try
            {
                File.AppendAllText(potKnjigeTXT, vrstica + Environment.NewLine);
                Console.WriteLine("Knjiga uspešno dodana.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Napaka pri pisanju: " + ex.Message);
            }
        }

        static void IzpisiVse()
        {
            if (!File.Exists(potKnjigeTXT))
            {
                Console.WriteLine("\nDatoteka ne obstaja!");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            List<string> najdeneKnjige = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(potKnjigeTXT))
                {
                    string vrstica;
                    while ((vrstica = sr.ReadLine()) != null)
                    {
                        string[] podatki = vrstica.Split(';');

                        if (podatki.Length == 4)
                        {
                            string naslov = podatki[0];
                            string avtor = podatki[1];
                            string leto = podatki[2];
                            string cena = podatki[3];
                            IzpisKnjig(naslov, avtor, leto, cena);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Prišlo je do napake: {ex.Message}");
            }
        }

        static void IzpisKnjig(string naslov, string avtor, string leto, string cena)
        {
            Console.WriteLine($"[{avtor}] {naslov} ({leto}) - {cena} EUR");
        }

        static void FiltrirajPoCeni()
        {
            if (!File.Exists(potKnjigeTXT))
            {
                Console.WriteLine("\nDatoteka ne obstaja!");
                return;
            }

            Console.WriteLine("\n ---- ISKANJE KNJIG po ceni ----");
            Console.WriteLine("Ta filter bo vrnil vse knige, katerih cena je večja od vnešene cene");
            Console.WriteLine();
            Console.Write("Minimalna cena: ");

            if (!double.TryParse(Console.ReadLine(), out double minCena))
            {
                Console.WriteLine("Napaka: Vnesi številko.");
                return;
            }

            List<string> najdeneKnjige = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(potKnjigeTXT))
                {
                    string vrstica;

                    while ((vrstica = sr.ReadLine()) != null)
                    {
                        string[] podatki = vrstica.Split(';');

                        if (podatki.Length == 4)
                        {
                            string naslov = podatki[0];
                            string avtor = podatki[1];
                            string leto = podatki[2];
                            double cena = double.Parse(podatki[3]);


                            if (cena >= minCena)
                            {
                                IzpisKnjig(naslov, avtor, leto.ToString(), cena.ToString());
                            }
                        }
                    }

                    if (najdeneKnjige.Count > 0)
                    {
                        Console.WriteLine($"Najdenih je bilo {najdeneKnjige.Count} knjig");

                        using (StreamWriter sw = new StreamWriter("rezultati_iskanja.txt", false))
                        {
                            foreach (string knjiga in najdeneKnjige)
                            {
                                sw.WriteLine(knjiga);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Prišlo je do napake pri branju: {ex.Message}");
            }
            Console.ReadKey();
            Console.Clear();
        }

        static void PovecajCene()
        {
            Console.WriteLine("Vnesite odstotek povišanja (npr. 10 za 10%): ");

            if (!double.TryParse(Console.ReadLine(), out double odstotek))
            {
                Console.WriteLine("Napaka: Vnesi številko.");
                return;
            }

            if (odstotek <= 0 || odstotek > 100)
            {
                Console.WriteLine("\nOdstotek mora biti med 0 in 100");
                return;
            }

            List<string> podrazaneKnjige = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(potKnjigeTXT))
                {
                    string vrstica;
                    while ((vrstica = sr.ReadLine()) != null)
                    {
                        string[] podatki = vrstica.Split(";");
                        if (podatki.Length == 4)
                        {
                            string naslov = podatki[0];
                            string avtor = podatki[1];
                            string leto = podatki[2];
                            double cena = double.Parse(podatki[3]);

                            double novaCena = Math.Round(cena * (1 + odstotek / 100), 2);

                            string novaVrstica =$"{naslov};{avtor};{leto};{novaCena.ToString("F2")}";

                            podrazaneKnjige.Add(novaVrstica);
                        }
                    }
                }

                Directory.CreateDirectory("out");
                using (StreamWriter sw = new StreamWriter("out/posodobljene_knjige.txt", false))
                {
                    sw.WriteLine($"PODRAŽITEV - {odstotek}%");
                    sw.WriteLine($"Datum: {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}");
                    sw.WriteLine();

                    foreach (string knjiga in podrazaneKnjige)
                    {
                        sw.WriteLine(knjiga);
                    }
                    Console.WriteLine("Knjige so bile uspešno podražane in shranjene v out/posodobljene_knjige.txt");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nPrišlo je do napake: {ex.Message}");
            }
        }



        static void NarediDatoteke()
        {
            if (!File.Exists(potClani))
            {
                XmlDocument document = new XmlDocument();
                XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
                document.AppendChild(declaration);

                XmlElement root = document.CreateElement("clani");
                document.AppendChild(root);
                document.Save(potClani);
            }

            if (!File.Exists(potIzposoje))
            {
                XmlDocument document = new XmlDocument();
                XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
                document.AppendChild(declaration);

                XmlElement root = document.CreateElement("izposoje");
                document.AppendChild(root);
                document.Save(potIzposoje);
            }

        }

        static void UstvariTestnePodatke()
        {
            // ČLANI
            XmlDocument docClani = new XmlDocument();
            XmlDeclaration declClani = docClani.CreateXmlDeclaration("1.0", "UTF-8", null);
            docClani.AppendChild(declClani);
            XmlElement rootDob = docClani.CreateElement("clani");
            docClani.AppendChild(rootDob);

            UstvariClana(docClani, rootDob, "C001", "ime1", "priimek1", "email1");
            UstvariClana(docClani, rootDob, "C002", "ime2", "priimek2", "email2");
            docClani.Save(potClani);

            // IZPOSOJE
            XmlDocument docIzposoje = new XmlDocument();
            XmlDeclaration declIzposoje = docIzposoje.CreateXmlDeclaration("1.0", "UTF-8", null);
            docIzposoje.AppendChild(declIzposoje);
            XmlElement rootArt = docIzposoje.CreateElement("izposoje");
            docIzposoje.AppendChild(rootArt);

            UstvariIzposojo(docIzposoje, rootArt, "C001", "K010", "naslov1", "2025-11-11");
            UstvariIzposojo(docIzposoje, rootArt, "C002", "K011", "naslov2", "2025-12-11");
            docIzposoje.Save(potIzposoje);
        }

        static void UstvariClana(XmlDocument doc, XmlElement root, string id, string ime, string priimek, string email)
        {
            XmlElement clan = doc.CreateElement("clan");
            clan.SetAttribute("id", id);

            XmlElement imeEl = doc.CreateElement("ime");
            imeEl.InnerText = ime;
            clan.AppendChild(imeEl);

            XmlElement priimekEl = doc.CreateElement("priimek");
            priimekEl.InnerText = priimek;
            clan.AppendChild(priimekEl);

            XmlElement emailEl = doc.CreateElement("email");
            emailEl.InnerText = email;
            clan.AppendChild(emailEl);

            root.AppendChild(clan);
        }

        static void UstvariIzposojo(XmlDocument doc, XmlElement root, string idClana, string idKnjige, string naslov, string datumIzposoje)
        {
            XmlElement clan = doc.CreateElement("izposoja");

            XmlElement idClanaEl = doc.CreateElement("idClana");
            idClanaEl.InnerText = idClana;
            clan.AppendChild(idClanaEl);

            XmlElement idKnjigeEl = doc.CreateElement("idKnjige");
            idKnjigeEl.InnerText = idKnjige;
            clan.AppendChild(idKnjigeEl);

            XmlElement naslovEl = doc.CreateElement("naslov");
            naslovEl.InnerText = naslov;
            clan.AppendChild(naslovEl);

            XmlElement datumIzposojeEl = doc.CreateElement("datumIzposoje");
            datumIzposojeEl.InnerText = datumIzposoje;
            clan.AppendChild(datumIzposojeEl);

            root.AppendChild(clan);
        }


      



        // XML DEL
        static void DodajClana()
        {
            Console.Write("\nID clana (npr. C001): ");
            string id = Console.ReadLine();

            Console.Write("Ime: ");
            string ime = Console.ReadLine();

            Console.Write("Priimek: ");
            string priimek = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potClani);

                XmlElement root = doc.SelectSingleNode("//clani") as XmlElement;

                UstvariClana(doc, root, id, ime, priimek, email);
                /*doc.Save(potClani);

                Console.WriteLine($"\nČlan {ime} uspešno dodan!");*/

                // Shranjevanje v začasno datoteko
                string tempPath = "temp_clani.xml";
                doc.Save(tempPath);

                ValidirajXML_Shema(tempPath);

                if (validacijaOK)
                {
                    File.Copy(tempPath, "clani.xml", true);
                    File.Delete(tempPath);
                    Console.WriteLine("\nČlan uspešno dodan in validiran!");
                }
                else
                {
                    File.Delete(tempPath);
                    Console.WriteLine("\nČlan ni bil dodan zaradi napake pri validaciji!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nNapaka: {ex.Message}");
            }
        }

        static void DodajIzposojo()
        {
            Console.Write("\nID clana (npr. C001): ");
            string idClana = Console.ReadLine();
            /*
            if (AliClanObstaja(idClana))
            {
                Console.WriteLine($"Član z id {idClana} že obstaja");
                return;
            }*/

            Console.Write("ID knjige (npr. K001): ");
            string idKnjige = Console.ReadLine();
            /*
            if (AliKnjigaOBstaja(idKnjige))
            {
                Console.WriteLine($"Knjiga z id {idKnjige} že obstaja");
                return;
            }*/


            Console.Write("Naslov: ");
            string naslov = Console.ReadLine();

            Console.Write("Datum: ");
            string datumIzposoje = Console.ReadLine();


            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potIzposoje);

                XmlElement root = doc.SelectSingleNode("//izposoje") as XmlElement;

                UstvariIzposojo(doc, root, idClana, idKnjige, naslov, datumIzposoje);
                string tempPath = "temp_izposoje.xml";
                doc.Save(tempPath);

                ValidirajXML_Shema(tempPath);

                if (validacijaOK)
                {
                    File.Copy(tempPath, "izposoje.xml", true);
                    File.Delete(tempPath);
                    Console.WriteLine("\nIzposoja uspešno dodan in validiran!");
                }
                else
                {
                    File.Delete(tempPath);
                    Console.WriteLine("\nIzposoja ni bil dodan zaradi napake pri validaciji!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nNapaka: {ex.Message}");
            }
        }

        static void IzpisiClaneZIzposojami()
        {
            if (!File.Exists(potClani) || !File.Exists(potIzposoje))
            {
                Console.WriteLine("Manjkajo XML datoteke!");
                return;
            }

            XmlDocument docClani = new XmlDocument();
            docClani.Load(potClani);

            XmlDocument docIzposoje = new XmlDocument();
            docIzposoje.Load(potIzposoje);

            XmlNodeList clani = docClani.SelectNodes("//clan");

            foreach (XmlNode clan in clani)
            {
                string id = clan.Attributes["id"].Value;
                string ime = clan.SelectSingleNode("ime")?.InnerText ?? "";
                string priimek = clan.SelectSingleNode("priimek")?.InnerText ?? "";
                string email = clan.SelectSingleNode("email")?.InnerText ?? "";

                Console.WriteLine($"\n[{id}] {ime} {priimek} ({email})");

                XmlNodeList izposoje = docIzposoje.SelectNodes($"//izposoja[idClana='{id}']");

                if (izposoje.Count == 0)
                {
                    Console.WriteLine("  Ni izposojenih knjig");
                    continue;
                }

                Console.WriteLine("  Izposojene knjige:");

                foreach (XmlNode iz in izposoje)
                {
                    string idKnjige = iz.SelectSingleNode("idKnjige")?.InnerText ?? "";
                    string naslov = iz.SelectSingleNode("naslov")?.InnerText ?? "";
                    string datum = iz.SelectSingleNode("datumIzposoje")?.InnerText ?? "";

                    Console.WriteLine($"  - [{idKnjige}] {naslov} (izposojeno: {datum})");
                }
            }

            Console.WriteLine();
        }

        static bool AliClanObstaja(string id)
        {
            if (!File.Exists(potClani)) return false;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potClani);

                XmlNode clan = doc.SelectSingleNode($"//clan[@id='{id}']");
                return clan != null;
            }
            catch
            {
                return false;
            }
        }
        static bool AliKnjigaOBstaja(string id)
        {
            if (!File.Exists(potDoKnjige)) return false;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoKnjige);

                XmlNode knjiga = doc.SelectSingleNode($"//knjiga[@id='{id}']");
                return knjiga != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
