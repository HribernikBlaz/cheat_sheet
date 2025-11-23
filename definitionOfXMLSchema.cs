using System.Globalization;
using System.Xml;
using System.Xml.Schema;

namespace naloga04_DefinicijaXMLSheme
{

    class Program
    {
        static string potDoArtiklov = "artikli.xml";
        static string potDoDobaviteljev = "dobavitelji.xml";
        static string potDoSheme = "shema.xsd";
        static bool validacijaOK = true;

        static void Main(string[] args)
        {
            NarediDatoteke();

            while (true)
            {
                Console.WriteLine("\n===== UPRAVLJANJE ARTIKLOV IN DOBAVITELJEV =====");
                Console.WriteLine("1) Izpis artiklov");
                Console.WriteLine("2) Dodaj artikel");
                Console.WriteLine("3) Uredi artikel");
                Console.WriteLine("4) Izbriši artikel");
                Console.WriteLine("5) Išči artikle po dobavitelju");
                Console.WriteLine("6) Izpis dobaviteljev");
                Console.WriteLine("7) Dodaj dobavitelja");
                Console.WriteLine("8) Uredi dobavitelja");
                Console.WriteLine("9) Izbriši dobavitelja");
                Console.WriteLine("10) Ustvari testne podatke");
                Console.WriteLine("0) Izhod");

                Console.Write("\nIzbira: ");
                string izbira = Console.ReadLine();

                switch (izbira)
                {
                    case "1": IzpisArtiklov(); break;
                    case "2": DodajArtikel(); break;
                    case "3": UrediArtikel(); break;
                    case "4": IzbrisiArtikel(); break;
                    case "5": IsciArtiklePoDobavitelju(); break;
                    case "6": IzpisDobaviteljev(); break;
                    case "7": DodajDobavitelja(); break;
                    case "8": UrediDobavitelja(); break;
                    case "9": IzbrisiDobavitelja(); break;
                    case "10": UstvariTestnePodatke(); break;
                    case "0":
                        Console.WriteLine("Program se končuje...");
                        return;
                    default:
                        Console.WriteLine("Napačna izbira!");
                        break;
                }
            }
        }


        // ---------------------------------
        // DTD IN SCHEMA VALIDACIJA
        // ---------------------------------
        static void ValidirajXML(string pot)
        {
            // DTD: 
            /*validacijaOK = true;

            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.DtdProcessing = DtdProcessing.Parse;
                settings.ValidationType = ValidationType.DTD;
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                settings.XmlResolver = new XmlUrlResolver();

                XmlReader reader = XmlReader.Create(pot, settings);

                while (reader.Read()) ;
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Napaka pri validaciji: {ex.Message}");
                validacijaOK = false;
            }*/

            //SCHEMA
            validacijaOK = true;

            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

                settings.Schemas.Add(null, potDoSheme);

                XmlReader reader = XmlReader.Create(pot, settings);

                while (reader.Read()) ;
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Napaka pri validaciji: {ex.Message}");
                validacijaOK = false;
            }
        }

        private static void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            Console.WriteLine($"❌ Napaka pri validaciji: {e.Message}");
            validacijaOK = false;
        }



        // ---------------------------------
        // USTVARJANJE DATOTEK, PODATKOV,...
        // ---------------------------------
        static void NarediDatoteke()
        {
            // Brez DTD in Scheme
            /*if (!File.Exists(potDoArtiklov))
            {
                XmlDocument document = new XmlDocument();
                XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
                document.AppendChild(declaration);

                XmlElement root = document.CreateElement("artikli");
                document.AppendChild(root);
                document.Save(potDoArtiklov);
            }
            if (!File.Exists(potDoDobaviteljev))
            {
                XmlDocument document = new XmlDocument();
                XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "UTF-8", null);
                document.AppendChild(declaration);

                XmlElement root = document.CreateElement("dobavitelji");
                document.AppendChild(root);
                document.Save(potDoDobaviteljev);
            }*/


            // DTD
            /*if (!File.Exists(potDoArtiklov))
            {
                string xmlArtikli = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE artikli SYSTEM ""{potDoBesednjaka}"">
<artikli>
</artikli>";
                File.WriteAllText(potDoArtiklov, xmlArtikli);
            }

            if (!File.Exists(potDoDobaviteljev))
            {
                string xmlDobavitelji = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE dobavitelji SYSTEM ""{potDoBesednjaka}"">
<dobavitelji>
</dobavitelji>";
                File.WriteAllText(potDoDobaviteljev, xmlDobavitelji);
            }*/


            // Schema
            if (!File.Exists(potDoArtiklov))
            {
                string xmlArtikli = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<artikli xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xsi:noNamespaceSchemaLocation=""shema.xsd"">
</artikli>";
                File.WriteAllText(potDoArtiklov, xmlArtikli);
            }

            if (!File.Exists(potDoDobaviteljev))
            {
                string xmlDobavitelji = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<dobavitelji xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
             xsi:noNamespaceSchemaLocation=""shema.xsd"">
</dobavitelji>";
                File.WriteAllText(potDoDobaviteljev, xmlDobavitelji);
            }
        }

        static void UstvariTestnePodatke()
        {
            Console.WriteLine("\n To bo prepisalo obstoječe podatke!");
            Console.Write("Ste prepričani? (y/n): ");
            string potrditev = Console.ReadLine();

            if (potrditev?.ToLower() != "y")
            {
                Console.WriteLine("Prekinjeno");
                return;
            }

            // Brez DTD in Sheme
            /*XmlDocument docDob = new XmlDocument();
            XmlDeclaration declDob = docDob.CreateXmlDeclaration("1.0", "UTF-8", null);
            docDob.AppendChild(declDob);
            XmlElement rootDob = docDob.CreateElement("dobavitelji");
            docDob.AppendChild(rootDob);

            UstvariDobavitelja(docDob, rootDob, "1", "dobavitelj1", "naslov1", "davcna1", "kontakt1", "opis1");
            UstvariDobavitelja(docDob, rootDob, "2", "dobavitelj2", "naslov2", "davcna2", "kontakt2", "opis2");
            UstvariDobavitelja(docDob, rootDob, "3", "dobavitelj3", "naslov3", "davcna3", "kontakt3", "opis3");
            UstvariDobavitelja(docDob, rootDob, "4", "dobavitelj4", "naslov4", "davcna4", "kontakt4", "opis4");
            UstvariDobavitelja(docDob, rootDob, "5", "dobavitelj5", "naslov5", "davcna5", "kontakt5", "opis5");
            docDob.Save(potDoDobaviteljev);


            XmlDocument docArt = new XmlDocument();
            XmlDeclaration declArt = docArt.CreateXmlDeclaration("1.0", "UTF-8", null);
            docArt.AppendChild(declArt);
            XmlElement rootArt = docArt.CreateElement("artikli");
            docArt.AppendChild(rootArt);

            UstvariArtikel(docArt, rootArt, "A001", "artikel1", "149.99", "34", "2", "2025-11-11");
            UstvariArtikel(docArt, rootArt, "A002", "artikel2", "129.99", "11", "1", "2025-11-11");
            UstvariArtikel(docArt, rootArt, "A003", "artikel3", "189.99", "5", "1", "2025-11-11");
            UstvariArtikel(docArt, rootArt, "A004", "artikel4", "19.99", "78", "5", "2025-11-11");
            UstvariArtikel(docArt, rootArt, "A005", "artikel5", "49.99", "35", "4", "2025-11-11");
            docArt.Save(potDoArtiklov);*/


            // DTD
            /* XmlDocument docDob = new XmlDocument();
             XmlDeclaration declDob = docDob.CreateXmlDeclaration("1.0", "UTF-8", null);
             docDob.AppendChild(declDob);

             XmlDocumentType doctypeDob = docDob.CreateDocumentType("dobavitelji", null, potDoBesednjaka, null);
             docDob.AppendChild(doctypeDob);

             XmlElement rootDob = docDob.CreateElement("dobavitelji");
             docDob.AppendChild(rootDob);

             // Uporablja tvojo metodo UstvariDobavitelja
             UstvariDobavitelja(docDob, rootDob, "D1", "malo", "Dobavitelj 1", "Naslov 1", "12345678", "kontakt1@email.com", "Opis dobavitelja 1", "da");
             UstvariDobavitelja(docDob, rootDob, "D2", "srednje", "Dobavitelj 2", "Naslov 2", "87654321", "kontakt2@email.com", "Opis dobavitelja 2", "da");
             UstvariDobavitelja(docDob, rootDob, "D3", "mikro", "Dobavitelj 3", "Naslov 3", "11223344", "kontakt3@email.com", "Opis dobavitelja 3", "ne");
             UstvariDobavitelja(docDob, rootDob, "D4", "veliko", "Dobavitelj 4", "Naslov 4", "44332211", "kontakt4@email.com", "Opis dobavitelja 4", "da");
             UstvariDobavitelja(docDob, rootDob, "D5", "srednje", "Dobavitelj 5", "Naslov 5", "55667788", "kontakt5@email.com", "Opis dobavitelja 5", "da");

             docDob.Save(potDoDobaviteljev);

             // ARTIKLI
             XmlDocument docArt = new XmlDocument();
             XmlDeclaration declArt = docArt.CreateXmlDeclaration("1.0", "UTF-8", null);
             docArt.AppendChild(declArt);

             XmlDocumentType doctypeArt = docArt.CreateDocumentType("artikli", null, potDoBesednjaka, null);
             docArt.AppendChild(doctypeArt);

             XmlElement rootArt = docArt.CreateElement("artikli");
             docArt.AppendChild(rootArt);

             // Uporablja tvojo metodo UstvariArtikel
             UstvariArtikel(docArt, rootArt, "A001", "elektronika", "Artikel 1", "149.99", "34", "D2", "2025-11-11", "da");
             UstvariArtikel(docArt, rootArt, "A002", "oblacila", "Artikel 2", "129.99", "11", "D1", "2025-11-11", "da");
             UstvariArtikel(docArt, rootArt, "A003", "obutev", "Artikel 3", "189.99", "5", "D1", "2025-11-11", "da");
             UstvariArtikel(docArt, rootArt, "A004", "hrana", "Artikel 4", "19.99", "78", "D5", "2025-11-11", "da");
             UstvariArtikel(docArt, rootArt, "A005", "drugo", "Artikel 5", "49.99", "35", "D4", "2025-11-11", "da");

             docArt.Save(potDoArtiklov);

             Console.WriteLine("✅ Testni podatki uspešno ustvarjeni!");*/


            // Schema
            // DOBAVITELJI
            XmlDocument docDob = new XmlDocument();
            XmlDeclaration declDob = docDob.CreateXmlDeclaration("1.0", "UTF-8", null);
            docDob.AppendChild(declDob);

            XmlElement rootDob = docDob.CreateElement("dobavitelji");
            rootDob.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            rootDob.SetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "shema.xsd");
            docDob.AppendChild(rootDob);

            UstvariDobavitelja(docDob, rootDob, "D1", "malo", "Dobavitelj 1", "Naslov 1", "12345678", "kontakt1@email.com", "Opis dobavitelja 1", "da");
            UstvariDobavitelja(docDob, rootDob, "D2", "srednje", "Dobavitelj 2", "Naslov 2", "87654321", "kontakt2@email.com", "Opis dobavitelja 2", "da");
            UstvariDobavitelja(docDob, rootDob, "D3", "mikro", "Dobavitelj 3", "Naslov 3", "11223344", "kontakt3@email.com", "Opis dobavitelja 3", "ne");
            UstvariDobavitelja(docDob, rootDob, "D4", "veliko", "Dobavitelj 4", "Naslov 4", "44332211", "kontakt4@email.com", "Opis dobavitelja 4", "da");
            UstvariDobavitelja(docDob, rootDob, "D5", "srednje", "Dobavitelj 5", "Naslov 5", "55667788", "kontakt5@email.com", "Opis dobavitelja 5", "da");

            docDob.Save(potDoDobaviteljev);

            // ARTIKLI
            XmlDocument docArt = new XmlDocument();
            XmlDeclaration declArt = docArt.CreateXmlDeclaration("1.0", "UTF-8", null);
            docArt.AppendChild(declArt);

            XmlElement rootArt = docArt.CreateElement("artikli");
            rootArt.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            rootArt.SetAttribute("noNamespaceSchemaLocation", "http://www.w3.org/2001/XMLSchema-instance", "shema.xsd");
            docArt.AppendChild(rootArt);

            UstvariArtikel(docArt, rootArt, "A001", "elektronika", "Artikel 1", "149.99", "34", "D2", "2025-11-11", "da");
            UstvariArtikel(docArt, rootArt, "A002", "oblacila", "Artikel 2", "129.99", "11", "D1", "2025-11-11", "da");
            UstvariArtikel(docArt, rootArt, "A003", "obutev", "Artikel 3", "189.99", "5", "D1", "2025-11-11", "da");
            UstvariArtikel(docArt, rootArt, "A004", "hrana", "Artikel 4", "19.99", "78", "D5", "2025-11-11", "da");
            UstvariArtikel(docArt, rootArt, "A005", "drugo", "Artikel 5", "49.99", "35", "D4", "2025-11-11", "da");

            docArt.Save(potDoArtiklov);

            Console.WriteLine("✅ Testni podatki uspešno ustvarjeni!");
        }

        static void UstvariDobavitelja(XmlDocument doc, XmlElement root, string id, string tip,
            string naziv, string naslov, string davcna, string kontakt, string opis, string aktiven)
        {
            XmlElement dobavitelj = doc.CreateElement("dobavitelj");
            dobavitelj.SetAttribute("id", id);
            dobavitelj.SetAttribute("tip", tip);

            XmlElement nazivEl = doc.CreateElement("naziv");
            nazivEl.InnerText = naziv;
            dobavitelj.AppendChild(nazivEl);

            XmlElement naslovEl = doc.CreateElement("naslov");
            naslovEl.InnerText = naslov;
            dobavitelj.AppendChild(naslovEl);

            XmlElement davcnaEl = doc.CreateElement("davcna");
            davcnaEl.InnerText = davcna;
            dobavitelj.AppendChild(davcnaEl);

            XmlElement kontaktEl = doc.CreateElement("kontakt");
            kontaktEl.InnerText = kontakt;
            dobavitelj.AppendChild(kontaktEl);

            XmlElement opisEl = doc.CreateElement("opis");
            opisEl.InnerText = opis;
            dobavitelj.AppendChild(opisEl);

            // EMPTY element z atributom
            XmlElement aktivenEl = doc.CreateElement("aktiven");
            aktivenEl.SetAttribute("status", aktiven);
            dobavitelj.AppendChild(aktivenEl);

            root.AppendChild(dobavitelj);
        }

        static void UstvariArtikel(XmlDocument doc, XmlElement root, string id, string kategorija,
            string naziv, string cena, string zaloga, string dobaviteljId, string datum, string razpolozljiv)
        {
            XmlElement artikel = doc.CreateElement("artikel");
            artikel.SetAttribute("id", id);
            artikel.SetAttribute("kategorija", kategorija);

            XmlElement nazivEl = doc.CreateElement("naziv");
            nazivEl.InnerText = naziv;
            artikel.AppendChild(nazivEl);

            XmlElement cenaEl = doc.CreateElement("cena");
            cenaEl.InnerText = cena;
            artikel.AppendChild(cenaEl);

            XmlElement zalogaEl = doc.CreateElement("zaloga");
            zalogaEl.InnerText = zaloga;
            artikel.AppendChild(zalogaEl);

            XmlElement dobaviteljIdEl = doc.CreateElement("dobaviteljId");
            dobaviteljIdEl.InnerText = dobaviteljId;
            artikel.AppendChild(dobaviteljIdEl);

            XmlElement datumEl = doc.CreateElement("zadnjaNabava");
            datumEl.InnerText = datum;
            artikel.AppendChild(datumEl);

            // EMPTY element z atributom
            XmlElement razpolozljivEl = doc.CreateElement("razpolozljiv");
            razpolozljivEl.SetAttribute("stanje", razpolozljiv);
            artikel.AppendChild(razpolozljivEl);

            root.AppendChild(artikel);
        }


        // ---------------
        // ARTIKLI - IZPIS
        // ---------------
        static void IzpisArtiklov()
        {
            if (!File.Exists(potDoArtiklov))
            {
                Console.WriteLine($"Datoteka {potDoArtiklov} ne obstaja!");
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(potDoArtiklov);

            XmlNodeList artikli = doc.SelectNodes("//artikel");

            if (artikli.Count == 0)
            {
                Console.WriteLine("\nNi še nobenega artikla!");
                return;
            }

            Console.WriteLine("\n=============SEZNAM ARTIKLOV=============");

            foreach (XmlNode artikel in artikli)
            {
                string id = artikel.Attributes["id"]?.Value ?? "?";
                string kategorija = artikel.Attributes["kategorija"]?.Value ?? "?";

                string naziv = artikel.SelectSingleNode("naziv")?.InnerText ?? "";
                string cena = artikel.SelectSingleNode("cena")?.InnerText ?? "";
                string zaloga = artikel.SelectSingleNode("zaloga")?.InnerText ?? "";
                string dobaviteljId = artikel.SelectSingleNode("dobaviteljId")?.InnerText ?? "";
                string datum = artikel.SelectSingleNode("zadnjaNabava")?.InnerText ?? "";

                XmlNode razpolozljivNode = artikel.SelectSingleNode("razpolozljiv");
                string razpolozljiv = razpolozljivNode?.Attributes["stanje"]?.Value ?? "?";

                string dobaviteljNaziv = PridobiNazivDobavitelja(dobaviteljId);

                Console.WriteLine($"\nArtikel ID: {id}");
                Console.WriteLine($"   Naziv: {naziv}");
                Console.WriteLine($"   Kategorija: {kategorija}");
                Console.WriteLine($"   Cena: {cena} EUR");
                Console.WriteLine($"   Zaloga: {zaloga} kosov");
                Console.WriteLine($"   Razpoložljiv: {razpolozljiv}");
                Console.WriteLine($"   Dobavitelj: {dobaviteljNaziv} (ID: {dobaviteljId})");
                Console.WriteLine($"   Zadnja nabava: {datum}");
            }

            Console.WriteLine("\n=========================================");
            Console.WriteLine($"Skupaj {artikli.Count} artiklov");
            Console.ReadKey();
            Console.Clear();
        }

        // -------------------
        // ARTIKLI - DODAJANJE
        // -------------------
        static void DodajArtikel()
        {
            Console.WriteLine("\n=============DODAJ ARTIKEL=============");

            if (!ImaDobavitelje())
            {
                Console.WriteLine("\n⚠Ni še nobenega dobavitelja! Najprej dodaj dobavitelja.");
                Console.ReadKey();
                Console.Clear();
                return;
            }

            Console.WriteLine("\nRazpoložljivi dobavitelji:");
            PrikaziDobavitelje();

            Console.Write("\nID artikla (npr. A002): ");
            string id = Console.ReadLine();

            if (AliArtikelObstaja(id))
            {
                Console.WriteLine($"\n Artikel z id {id} že obstaja!");
                return;
            }

            Console.Write("Naziv artikla: ");
            string naziv = Console.ReadLine();


            Console.WriteLine("\nIzberi kategorijo:");
            Console.WriteLine("1) elektronika");
            Console.WriteLine("2) obutev");
            Console.WriteLine("3) oblacila");
            Console.WriteLine("4) hrana");
            Console.WriteLine("5) drugo");
            Console.Write("Izbira (1-5): ");
            string kategorijaIzbira = Console.ReadLine();

            string kategorija = kategorijaIzbira switch
            {
                "1" => "elektronika",
                "2" => "obutev",
                "3" => "oblacila",
                "4" => "hrana",
                "5" => "drugo",
                _ => "drugo"
            };

            Console.Write("Cena (EUR): ");
            string cenaVnos = Console.ReadLine();

            decimal cena;
            if (!decimal.TryParse(cenaVnos, NumberStyles.Any, CultureInfo.InvariantCulture, out cena))
            {
                Console.WriteLine("\nNapačen format cene!");
                return;
            }


            Console.Write("zaloga (kosi): ");
            string zalogaVnos = Console.ReadLine();

            int zaloga;
            if (!int.TryParse(zalogaVnos, out zaloga))
            {
                Console.WriteLine("\nNapačen format cene!");
                return;
            }



            Console.Write("ID dobavitelja: ");
            string dobaviteljId = Console.ReadLine();

            if (!AliDobaviteljObstaja(dobaviteljId))
            {
                Console.WriteLine($"\nDobavitelj z ID {dobaviteljId} ne obstaja!");
                return;
            }

            Console.Write("Vnesi datum zadnje nabave (YYYY-MM-DD): ");
            string datum = Console.ReadLine();

            Console.Write("Ali je artikel razpoložljiv? (da/ne): ");
            string razpolozljiv = Console.ReadLine();


            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoArtiklov);

                XmlNode root = doc.SelectSingleNode("artikli");

                XmlElement novArtikel = doc.CreateElement("artikel");
                novArtikel.SetAttribute("id", id);
                novArtikel.SetAttribute("kategorija", kategorija);

                XmlElement nazivEl = doc.CreateElement("naziv");
                nazivEl.InnerText = naziv;
                novArtikel.AppendChild(nazivEl);

                XmlElement cenaEl = doc.CreateElement("cena");
                cenaEl.InnerText = cena.ToString("F2", CultureInfo.InvariantCulture);
                novArtikel.AppendChild(cenaEl);

                XmlElement zalogaEl = doc.CreateElement("zaloga");
                zalogaEl.InnerText = zaloga.ToString();
                novArtikel.AppendChild(zalogaEl);

                XmlElement dobEl = doc.CreateElement("dobaviteljId");
                dobEl.InnerText = dobaviteljId;
                novArtikel.AppendChild(dobEl);

                XmlElement datumEl = doc.CreateElement("zadnjaNabava");
                datumEl.InnerText = DateTime.Now.ToString("yyyy-MM-dd");
                novArtikel.AppendChild(datumEl);


                XmlElement razpolozljivEl = doc.CreateElement("razpolozljiv");
                razpolozljivEl.SetAttribute("stanje", razpolozljiv);
                novArtikel.AppendChild(razpolozljivEl);

                root.AppendChild(novArtikel);


                string tempPath = "temp_artikli.xml";
                doc.Save(tempPath);

                ValidirajXML(tempPath);

                if (validacijaOK)
                {
                    File.Copy(tempPath, potDoArtiklov, true);
                    File.Delete(tempPath);
                    Console.WriteLine("\nArtikel uspešno dodan in validiran!");
                }
                else
                {
                    File.Delete(tempPath);
                    Console.WriteLine("\nArtikel ni bil dodan zaradi napake pri validaciji!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nNapaka pri dodajanju: {ex.Message}");
            }

            Console.ReadKey();
            Console.Clear();
        }

        // -------------------
        // ARTIKLI - UREJANJE
        // -------------------
        static void UrediArtikel()
        {
            Console.WriteLine("SHRANJENI ARTIKLI:");
            PrikaziArtikle();
            Console.WriteLine("\n=============UREDI ARTIKEL=============");
            Console.Write("Vnesi ID artikla:");
            string id = Console.ReadLine();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoArtiklov);

                XmlNode artikel = doc.SelectSingleNode($"//artikel[@id='{id}']");

                if (artikel == null)
                {
                    Console.WriteLine($"\nArtikel z ID {id} ne obstaja!");
                    return;
                }
                string staraKategorija = artikel.Attributes["kategorija"]?.Value ?? "";
                Console.WriteLine("\n--- TRNEUTNI PODATKI ---");
                Console.WriteLine($"Naziv: {artikel.SelectSingleNode("naziv")?.InnerText}");
                Console.WriteLine($"Cena: {artikel.SelectSingleNode("cena")?.InnerText}");
                Console.WriteLine($"Zaloga: {artikel.SelectSingleNode("zaloga")?.InnerText}");
                Console.WriteLine($"Dobavitelj: {artikel.SelectSingleNode("dobaviteljId")?.InnerText}");
                Console.WriteLine($"Zadnja nabava: {artikel.SelectSingleNode("zadnjaNabava")?.InnerText}");
                Console.WriteLine($"Kategorija: {staraKategorija}");
                Console.WriteLine($"Razpoložljiv: {artikel.SelectSingleNode("razpolozljiv").Attributes["stanje"].Value}"); // Branje atributa
                Console.WriteLine("\nPusti prazno da se nič ne spremeni.");


                Console.Write("\nNov naziv:");
                string novNaziv = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novNaziv))
                {
                    artikel.SelectSingleNode("naziv").InnerText = novNaziv;
                }

                Console.WriteLine($"\nTrenutna kategorija: {staraKategorija}");
                Console.WriteLine("Izberi novo kategorijo:");
                Console.WriteLine("1) elektronika");
                Console.WriteLine("2) obutev");
                Console.WriteLine("3) oblacila");
                Console.WriteLine("4) hrana");
                Console.WriteLine("5) drugo");
                Console.Write("Izbira (Enter za ohranitev): ");
                string kategorijaIzbira = Console.ReadLine();

                string novaKategorija = staraKategorija;
                if (!string.IsNullOrWhiteSpace(kategorijaIzbira))
                {
                    novaKategorija = kategorijaIzbira switch
                    {
                        "1" => "elektronika",
                        "2" => "obutev",
                        "3" => "oblacila",
                        "4" => "hrana",
                        "5" => "drugo",
                        _ => staraKategorija
                    };
                }


                Console.Write("Nova cena (EUR): ");
                string novaCenaVnos = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novaCenaVnos))
                {
                    decimal novaCena;
                    if (decimal.TryParse(novaCenaVnos, NumberStyles.Any, CultureInfo.InvariantCulture, out novaCena))
                    {
                        artikel.SelectSingleNode("cena").InnerText = novaCena.ToString("F2", CultureInfo.InvariantCulture);
                    }
                }


                Console.Write("Nova zaloga: ");
                string novaZalogaVnos = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novaZalogaVnos))
                {
                    int novaZaloga;
                    if (int.TryParse(novaZalogaVnos, out novaZaloga))
                    {
                        artikel.SelectSingleNode("zaloga").InnerText = novaZaloga.ToString();
                    }
                }



                Console.Write("Nov dobavitelj: ");
                string novDobaviteljVnos = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novDobaviteljVnos))
                {
                    if (AliDobaviteljObstaja(novDobaviteljVnos))
                    {
                        artikel.SelectSingleNode("dobaviteljId").InnerText = novDobaviteljVnos;
                    }
                    else Console.WriteLine($"Dobavitelj z id {novDobaviteljVnos} ne obstaja!");
                }

                Console.Write($"Nov datum: ");
                string novDatum = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novDatum))
                {
                    artikel.SelectSingleNode("zadnjaNabava").InnerText = novDatum;
                }

                Console.Write($"Razpoložljiv (da/ne): ");
                string novRazpolozljiv = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novRazpolozljiv))
                {
                    artikel.SelectSingleNode("razpolozljiv").Attributes["stanje"].Value = novRazpolozljiv;
                }





                string tempPath = "temp_artikli.xml";
                doc.Save(tempPath);

                ValidirajXML(tempPath);

                if (validacijaOK)
                {
                    File.Copy(tempPath, potDoArtiklov, true);
                    File.Delete(tempPath);
                    Console.WriteLine("\nArtikel uspešno posodobljen in validiran!");
                }
                else
                {
                    File.Delete(tempPath);
                    Console.WriteLine("\nSpremembe niso bile shranjene zaradi napake pri validaciji!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nNapaka pri urejanju: {ex.Message}");
            }
            Console.ReadKey();
            Console.Clear();
        }


        // ------------------
        // ARTIKLI - BRISANJE
        // ------------------
        static void IzbrisiArtikel()
        {
            Console.WriteLine("SHRANJENI ARTIKLI:");
            PrikaziArtikle();
            Console.WriteLine("\n=============IZBRIŠI ARTIKEL=============");
            Console.Write("Vnesi ID artikla: ");
            string id = Console.ReadLine();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoArtiklov);


                XmlNode artikel = doc.SelectSingleNode($"//artikel[@id='{id}']");
                if (artikel == null)
                {
                    Console.WriteLine($"\nArtikel z ID '{id}' ne obstaja!");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }

                string naziv = artikel.SelectSingleNode("naziv")?.InnerText ?? "Neznan";


                Console.Write($"\nRes želiš izbrisati artikel '{naziv}'? (y/n): ");
                string potrditev = Console.ReadLine();

                if (potrditev?.ToLower() == "y")
                {
                    artikel.ParentNode.RemoveChild(artikel);
                    doc.Save(potDoArtiklov);

                    Console.WriteLine($"\nArtikel '{naziv}' (ID: {id}) uspešno izbrisan!");
                }
                else
                {
                    Console.WriteLine("\nBrisanje preklicano.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nNapaka pri brisanju: {ex.Message}");
            }
            Console.ReadKey();
            Console.Clear();
        }


        // --------------------------------
        // ARTIKLI - ISKANJE PO DOBAVITELJU
        // --------------------------------

        static void IsciArtiklePoDobavitelju()
        {
            Console.WriteLine("SHRANJENI DOBAVITELJI:");
            PrikaziDobavitelje();
            Console.WriteLine("\n=============ISKANJE ARTIKLOV=============");

            Console.Write("ID dobavitelja: ");
            string dobID = Console.ReadLine();


            Console.Write("Maksimalna zaloga (išči artikle z manj kot X kosov): ");
            string maxZalogaVnos = Console.ReadLine();


            int maxZaloga;
            if (!int.TryParse(maxZalogaVnos, out maxZaloga))
            {
                Console.WriteLine("\nNapačen format zaloge!");
                return;
            }

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoArtiklov);

                XmlNodeList vsiArtikliDobavitelja = doc.SelectNodes($"//artikel[dobaviteljId='{dobID}']");


                if (vsiArtikliDobavitelja.Count == 0)
                {
                    Console.WriteLine($"\nNi artiklov za dobavitelja {dobID}.");
                    return;
                }


                Console.WriteLine($"\n========== REZULTATI ISKANJA ==========");
                Console.WriteLine($"Dobavitelj: {dobID}");
                Console.WriteLine($"Pogoj: Zaloga < {maxZaloga}");
                Console.WriteLine("=======================================");

                int stevec = 0;


                foreach (XmlNode artikel in vsiArtikliDobavitelja)
                {
                    int zaloga = int.Parse(artikel.SelectSingleNode("zaloga")?.InnerText ?? "0");

                    if (zaloga < maxZaloga)
                    {
                        stevec++;
                        string id = artikel.Attributes["id"]?.Value ?? "?";
                        string naziv = artikel.SelectSingleNode("naziv")?.InnerText ?? "";
                        string cena = artikel.SelectSingleNode("cena")?.InnerText ?? "";

                        Console.WriteLine($"\n{stevec}. {naziv} (ID: {id})");
                        Console.WriteLine($"   Cena: {cena} EUR");
                        Console.WriteLine($"   Zaloga: {zaloga} kosov");
                    }
                }

                if (stevec == 0)
                {
                    Console.WriteLine("\nVsi artikli tega dobavitelja imajo zadostno zalogo.");
                }
                else
                {
                    Console.WriteLine($"\n=======================================");
                    Console.WriteLine($"Skupaj najdeno: {stevec} artiklov z nizko zalogo");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nNapaka pri iskanju: {ex.Message}");
            }

        }



        // ---------------
        // DOBAVITELJI - IZPIS
        // ---------------
        static void IzpisDobaviteljev()
        {
            if (!File.Exists(potDoDobaviteljev))
            {
                Console.WriteLine($"Datoteka {potDoDobaviteljev} ne obstaja!");
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(potDoDobaviteljev);

            XmlNodeList dobavitelji = doc.SelectNodes("//dobavitelj");

            if (dobavitelji.Count == 0)
            {
                Console.WriteLine("\nNi še nobenega dobavitelja!");
                return;
            }

            Console.WriteLine("\n=============SEZNAM DOBAVITELJEV=============");

            foreach (XmlNode dobavitelj in dobavitelji)
            {
                string id = dobavitelj.Attributes["id"]?.Value ?? "?";
                string tip = dobavitelj.Attributes["tip"]?.Value ?? "?";

                string naziv = dobavitelj.SelectSingleNode("naziv")?.InnerText ?? "";
                string naslov = dobavitelj.SelectSingleNode("naslov")?.InnerText ?? "";
                string davcna = dobavitelj.SelectSingleNode("davcna")?.InnerText ?? "";
                string kontakt = dobavitelj.SelectSingleNode("kontakt")?.InnerText ?? "";
                string opis = dobavitelj.SelectSingleNode("opis")?.InnerText ?? "";

                XmlNode aktivenNode = dobavitelj.SelectSingleNode("aktiven");
                string aktiven = aktivenNode?.Attributes["status"]?.Value ?? "?";

                string dobaviteljNaziv = PridobiNazivDobavitelja(kontakt);



                Console.WriteLine($"\n Dobavitelj ID: {id}");
                Console.WriteLine($"        Naziv: {naziv}");
                Console.WriteLine($"        Tip: {tip}");
                Console.WriteLine($"        Naslov: {naslov}");
                Console.WriteLine($"        Davcna: {davcna}");
                Console.WriteLine($"        Kontakt: {kontakt}");
                Console.WriteLine($"        Naziv: {opis}");
                Console.WriteLine($"        Opis: {opis}");
                Console.WriteLine($"        Aktiven: {aktiven}");
            }

            Console.WriteLine("\n=========================================");
            Console.WriteLine($"Skupaj {dobavitelji.Count} dobaviteljev");
            Console.ReadKey();
            Console.Clear();
        }


        // -------------------
        // DOBAVITELJI - DODAJANJE
        // -------------------
        static void DodajDobavitelja()
        {
            Console.WriteLine("\n=============DODAJ DOBAVITELJA=============");


            Console.Write("\nID dobavitelja (npr. 1): ");
            string id = Console.ReadLine();

            if (AliDobaviteljObstaja(id))
            {
                Console.WriteLine($"\n Dobavitelj z id {id} že obstaja!");
                return;
            }

            Console.Write("Naziv: ");
            string naziv = Console.ReadLine();

            Console.WriteLine("\nIzberi tip podjetja:");
            Console.WriteLine("1) mikro");
            Console.WriteLine("2) malo");
            Console.WriteLine("3) srednje");
            Console.WriteLine("4) veliko");
            Console.Write("Izbira (1-4): ");
            string tipIzbira = Console.ReadLine();

            string tip = tipIzbira switch
            {
                "1" => "mikro",
                "2" => "malo",
                "3" => "srednje",
                "4" => "veliko",
                _ => "malo"
            };



            Console.Write("Naslov: ");
            string naslov = Console.ReadLine();


            Console.Write("Davnčna: ");
            string davcna = Console.ReadLine();

            Console.Write("Kontakt: ");
            string kontakt = Console.ReadLine();

            Console.Write("Opis: ");
            string opis = Console.ReadLine();


            Console.Write("Ali je dobavitelj aktiven? (da/ne): ");
            string aktiven = Console.ReadLine();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoDobaviteljev);

                XmlNode root = doc.SelectSingleNode("dobavitelji");

                XmlElement novDobavitelj = doc.CreateElement("dobavitelj");
                novDobavitelj.SetAttribute("id", id);
                novDobavitelj.SetAttribute("tip", tip);

                XmlElement nazivEl = doc.CreateElement("naziv");
                nazivEl.InnerText = naziv;
                novDobavitelj.AppendChild(nazivEl);

                XmlElement naslovEl = doc.CreateElement("naslov");
                naslovEl.InnerText = naslov;
                novDobavitelj.AppendChild(naslovEl);

                XmlElement davcnaEl = doc.CreateElement("davcna");
                davcnaEl.InnerText = davcna;
                novDobavitelj.AppendChild(davcnaEl);

                XmlElement kontaktEl = doc.CreateElement("kontakt");
                kontaktEl.InnerText = kontakt;
                novDobavitelj.AppendChild(kontaktEl);

                XmlElement opisEl = doc.CreateElement("opis");
                opisEl.InnerText = opis;
                novDobavitelj.AppendChild(opisEl);

                XmlElement aktivenEl = doc.CreateElement("aktiven");
                aktivenEl.SetAttribute("status", aktiven);
                novDobavitelj.AppendChild(aktivenEl);

                root.AppendChild(novDobavitelj);

                string tempPath = "temp_dobavitelji.xml";
                doc.Save(tempPath);

                ValidirajXML(tempPath);

                if (validacijaOK)
                {
                    File.Copy(tempPath, potDoDobaviteljev, true);
                    File.Delete(tempPath);
                    Console.WriteLine("\nDobavitelj uspešno dodan in validiran!");
                }
                else
                {
                    File.Delete(tempPath);
                    Console.WriteLine("\nDobavitelj ni bil dodan zaradi napake pri validaciji!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nNapaka pri dodajanju: {ex.Message}");
            }

            Console.ReadKey();
            Console.Clear();
        }


        // ------------------
        // DOBAVITELJI - UREJANJE
        // ------------------
        static void UrediDobavitelja()
        {
            Console.WriteLine("SHRANJENI DOBAVITELJI:");
            PrikaziDobavitelje();
            Console.WriteLine("\n=============UREDI DOBAVITELJA=============");
            Console.Write("Vnesi ID dobavitelja:");
            string id = Console.ReadLine();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoDobaviteljev);

                XmlNode dobavitelj = doc.SelectSingleNode($"//dobavitelj[@id='{id}']");

                if (dobavitelj == null)
                {
                    Console.WriteLine($"\nDobavitelj z ID {id} ne obstaja!");
                    return;
                }

                Console.WriteLine("\n--- TRNEUTNI PODATKI ---");
                Console.WriteLine($"Naziv: {dobavitelj.SelectSingleNode("naziv")?.InnerText}");
                Console.WriteLine($"Naslov: {dobavitelj.SelectSingleNode("naslov")?.InnerText}");
                Console.WriteLine($"Davčna: {dobavitelj.SelectSingleNode("davcna")?.InnerText}");
                Console.WriteLine($"Kontakt: {dobavitelj.SelectSingleNode("kontakt")?.InnerText}");
                Console.WriteLine($"Opis: {dobavitelj.SelectSingleNode("opis")?.InnerText}");
                Console.WriteLine($"Tip: {dobavitelj.Attributes["tip"].Value}");
                Console.WriteLine($"Aktiven: {dobavitelj.SelectSingleNode("aktiven").Attributes["status"].Value}");
                Console.WriteLine("\nPusti prazno da se nič ne spremeni.");


                Console.Write("\nNov naziv:");
                string novNaziv = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novNaziv))
                {
                    dobavitelj.SelectSingleNode("naziv").InnerText = novNaziv;
                }

                Console.WriteLine("Izberi nov tip:");
                Console.WriteLine("1) mikro");
                Console.WriteLine("2) malo");
                Console.WriteLine("3) srednje");
                Console.WriteLine("4) veliko");
                Console.Write("Izbira (Enter za ohranitev): ");
                string tipIzbira = Console.ReadLine();

                string novTip = dobavitelj.Attributes["tip"].Value;
                if (!string.IsNullOrWhiteSpace(tipIzbira))
                {
                    novTip = tipIzbira switch
                    {
                        "1" => "mikro",
                        "2" => "malo",
                        "3" => "srednje",
                        "4" => "veliko",
                        _ => dobavitelj.Attributes["tip"].Value
                    };
                }


                Console.Write("Nov naslov:");
                string novNaslov = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novNaslov))
                {
                    dobavitelj.SelectSingleNode("naslov").InnerText = novNaslov;
                }

                Console.Write("Nova davčna:");
                string novaDavcna = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novaDavcna))
                {
                    dobavitelj.SelectSingleNode("davcna").InnerText = novaDavcna;
                }

                Console.Write("Nov kontakt:");
                string novKontakt = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novKontakt))
                {
                    dobavitelj.SelectSingleNode("kontakt").InnerText = novKontakt;
                }

                Console.Write("Nov opis:");
                string novOpis = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novOpis))
                {
                    dobavitelj.SelectSingleNode("opis").InnerText = novOpis;
                }


                Console.Write($"Aktiven (da/ne): ");
                string novAktivenStr = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novAktivenStr))
                {
                    dobavitelj.SelectSingleNode("aktiven").Attributes["status"].Value = novAktivenStr;
                }


                string tempPath = "temp_dobavitelji.xml";
                doc.Save(tempPath);

                ValidirajXML(tempPath);

                if (validacijaOK)
                {
                    File.Copy(tempPath, potDoDobaviteljev, true);
                    File.Delete(tempPath);
                    Console.WriteLine("\nDobavitelj uspešno posodobljen in validiran!");
                }
                else
                {
                    File.Delete(tempPath);
                    Console.WriteLine("\nSpremembe niso bile shranjene zaradi napake pri validaciji!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nNapaka pri urejanju: {ex.Message}");
            }
            Console.ReadKey();
            Console.Clear();
        }



        // ------------------
        // DOBAVITELJI - BRISANJE
        // ------------------
        static void IzbrisiDobavitelja()
        {
            Console.WriteLine("SHRANJENI DOBAVITELJI:");
            PrikaziDobavitelje();
            Console.WriteLine("\n=============IZBRIŠI DOBAVITELJA=============");
            Console.Write("Vnesi ID dobavitelja: ");
            string id = Console.ReadLine();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoDobaviteljev);


                XmlNode dobavitelj = doc.SelectSingleNode($"//dobavitelj[@id='{id}']");
                if (dobavitelj == null)
                {
                    Console.WriteLine($"\nDobavitelj z ID '{id}' ne obstaja!");
                    Console.ReadKey();
                    Console.Clear();
                    return;
                }

                string naziv = dobavitelj.SelectSingleNode("naziv")?.InnerText ?? "Neznan";


                Console.Write($"\nRes želiš izbrisati dobavitelja '{naziv}'? (y/n): ");
                string potrditev = Console.ReadLine();

                if (potrditev?.ToLower() == "y")
                {
                    dobavitelj.ParentNode.RemoveChild(dobavitelj);
                    doc.Save(potDoDobaviteljev);

                    Console.WriteLine($"\nDobavitelj '{naziv}' (ID: {id}) uspešno izbrisan!");
                }
                else
                {
                    Console.WriteLine("\nBrisanje preklicano.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nNapaka pri brisanju: {ex.Message}");
            }
            Console.ReadKey();
            Console.Clear();
        }



        // --------------------------------
        // ARTIKLI - POMOZNE METODE
        // --------------------------------
        static string PridobiNazivDobavitelja(string dobaviteljId)
        {
            if (!File.Exists(potDoDobaviteljev)) return "Neznano";

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoDobaviteljev);

                XmlNode dobavitelj = doc.SelectSingleNode($"//dobavitelj[@id='{dobaviteljId}']");

                if (dobavitelj != null)
                {
                    return dobavitelj.SelectSingleNode("naziv")?.InnerText ?? "Neznano";
                }
            }
            catch
            {

            }
            return "Neznano";
        }

        static bool ImaDobavitelje()
        {
            if (!File.Exists(potDoDobaviteljev)) return false;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoDobaviteljev);

                XmlNodeList dobavitelji = doc.SelectNodes("//dobavitelj");
                return dobavitelji.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        static bool ImaArtikle(string dobaviteljID)
        {
            if (!File.Exists(potDoArtiklov)) return false;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoArtiklov);

                XmlNodeList artikli = doc.SelectNodes($"//artikel[dobaviteljId='{dobaviteljID}']");
                return artikli.Count > 0;
            }
            catch
            {
                return false;
            }
        }

        static void PrikaziDobavitelje()
        {
            if (!File.Exists(potDoDobaviteljev)) return;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoDobaviteljev);

                XmlNodeList dobavitelji = doc.SelectNodes("//dobavitelj");

                foreach (XmlNode dobavitelj in dobavitelji)
                {
                    string id = dobavitelj.Attributes["id"]?.Value ?? "?";
                    string naziv = dobavitelj.SelectSingleNode("naziv")?.InnerText ?? "?";
                    Console.WriteLine($" [{id}] - {naziv}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Napaka pri branju dobaviteljev: {ex.Message}");
            }
        }

        static void PrikaziArtikle()
        {
            if (!File.Exists(potDoArtiklov)) return;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoArtiklov);

                XmlNodeList artikli = doc.SelectNodes("//artikel");

                foreach (XmlNode artikel in artikli)
                {
                    string id = artikel.Attributes["id"]?.Value ?? "?";
                    string naziv = artikel.SelectSingleNode("naziv")?.InnerText ?? "?";
                    Console.WriteLine($" [{id}] - {naziv}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Napaka pri branju artiklov: {ex.Message}");
            }
        }

        static bool AliArtikelObstaja(string id)
        {
            if (!File.Exists(potDoArtiklov)) return false;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoArtiklov);

                XmlNode artikel = doc.SelectSingleNode($"//artikel[@id='{id}']");
                return artikel != null;
            }
            catch
            {
                return false;
            }
        }

        static bool AliDobaviteljObstaja(string id)
        {
            if (!File.Exists(potDoDobaviteljev)) return false;

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoDobaviteljev);

                XmlNode dobavitelj = doc.SelectSingleNode($"//dobavitelj[@id='{id}']");
                return dobavitelj != null;
            }
            catch
            {
                return false;
            }
        }

    }
}
