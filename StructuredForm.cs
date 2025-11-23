using System.Globalization;
using System.Xml;

namespace naloga02_1_strukturiranaOblika
{

    class Program
    {
        static string potDoArtiklov = "artikli.xml";
        static string potDoDobaviteljev = "dobavitelji.xml";

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
                        Console.WriteLine("❌ Napačna izbira!");
                        break;
                }
            }
        }

        // ---------------------------------
        // USTVARJANJE DATOTEK, PODATKOV,...
        // ---------------------------------
        static void NarediDatoteke()
        {
            if (!File.Exists(potDoArtiklov))
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

            XmlDocument docDob = new XmlDocument();
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
            docArt.Save(potDoArtiklov);
        }

        static void UstvariDobavitelja(XmlDocument doc, XmlElement root, string id, string naziv, string naslov, string davcna, string kontakt, string opis)
        {
            XmlElement dobavitelj = doc.CreateElement("dobavitelj");
            dobavitelj.SetAttribute("id", id);

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

            root.AppendChild(dobavitelj);
        }

        static void UstvariArtikel(XmlDocument doc, XmlElement root, string id, string naziv, string cena, string zaloga, string dobaviteljId, string datum)
        {
            XmlElement artikel = doc.CreateElement("artikel");
            artikel.SetAttribute("id", id);

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

                string naziv = artikel.SelectSingleNode("naziv")?.InnerText ?? "";
                string cena = artikel.SelectSingleNode("cena")?.InnerText ?? "";
                string zaloga = artikel.SelectSingleNode("zaloga")?.InnerText ?? "";
                string dobaviteljId = artikel.SelectSingleNode("dobaviteljId")?.InnerText ?? "";
                string datum = artikel.SelectSingleNode("zadnjaNabava")?.InnerText ?? "";

                string dobaviteljNaziv = PridobiNazivDobavitelja(dobaviteljId);

                Console.WriteLine($"\n Artikel ID: {id}");
                Console.WriteLine($"        Naziv: {naziv}");
                Console.WriteLine($"        Cena: {cena} EUR");
                Console.WriteLine($"        Zaloga: {zaloga} kosov");
                Console.WriteLine($"        Dobavitelj: {dobaviteljNaziv} (ID: {dobaviteljId})");
                Console.WriteLine($"        Zadnja nabava: {datum}");
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
                Console.WriteLine("\nNi še nobenega dobavitelja!");
                Console.WriteLine("Najprej dodajte dobavitelja (7 opcija v menijz)!");
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


            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoArtiklov);

                XmlNode root = doc.SelectSingleNode("artikli");

                XmlElement novArtikel = doc.CreateElement("artikel");
                novArtikel.SetAttribute("id", id);

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

                root.AppendChild(novArtikel);
                doc.Save(potDoArtiklov);
                Console.WriteLine($"\nArtikel {naziv} (ID: {id}) je bil uspešno dodan!");
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

                Console.WriteLine("\n--- TRNEUTNI PODATKI ---");
                Console.WriteLine($"Naziv: {artikel.SelectSingleNode("naziv")?.InnerText}");
                Console.WriteLine($"Cena: {artikel.SelectSingleNode("cena")?.InnerText}");
                Console.WriteLine($"Zaloga: {artikel.SelectSingleNode("zaloga")?.InnerText}");
                Console.WriteLine($"Dobavitelj: {artikel.SelectSingleNode("dobaviteljId")?.InnerText}");
                Console.WriteLine("\nPusti prazno da se nič ne spremeni.");


                Console.Write("\nNov naziv:");
                string novNaziv = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novNaziv))
                {
                    artikel.SelectSingleNode("naziv").InnerText = novNaziv;
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


                doc.Save(potDoArtiklov);
                Console.WriteLine("\nArtikel uspešno posodobljen!");
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

                string naziv = dobavitelj.SelectSingleNode("naziv")?.InnerText ?? "";
                string naslov = dobavitelj.SelectSingleNode("naslov")?.InnerText ?? "";
                string davcna = dobavitelj.SelectSingleNode("davcna")?.InnerText ?? "";
                string kontakt = dobavitelj.SelectSingleNode("kontakt")?.InnerText ?? "";
                string opis = dobavitelj.SelectSingleNode("opis")?.InnerText ?? "";

                string dobaviteljNaziv = PridobiNazivDobavitelja(kontakt);

                Console.WriteLine($"\n Artikel ID: {id}");
                Console.WriteLine($"        Naziv: {naziv}");
                Console.WriteLine($"        Naslov: {naslov}");
                Console.WriteLine($"        Davcna: {davcna}");
                Console.WriteLine($"        Kontakt: {kontakt}");
                Console.WriteLine($"        Naziv: {opis}");
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

            Console.Write("Naslov: ");
            string naslov = Console.ReadLine();


            Console.Write("Davnčna: ");
            string davcna = Console.ReadLine();

            Console.Write("Kontakt: ");
            string kontakt = Console.ReadLine();

            Console.Write("Opis: ");
            string opis = Console.ReadLine();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(potDoDobaviteljev);

                XmlNode root = doc.SelectSingleNode("dobavitelji");

                XmlElement novDobavitelj = doc.CreateElement("dobavitelj");
                novDobavitelj.SetAttribute("id", id);

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

                root.AppendChild(novDobavitelj);
                doc.Save(potDoDobaviteljev);
                Console.WriteLine($"\nDobavitelj {naziv} (ID: {id}) je bil uspešno dodan!");
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
                Console.WriteLine("\nPusti prazno da se nič ne spremeni.");


                Console.Write("\nNov naziv:");
                string novNaziv = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(novNaziv))
                {
                    dobavitelj.SelectSingleNode("naziv").InnerText = novNaziv;
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


                doc.Save(potDoDobaviteljev);
                Console.WriteLine("\nDobavitelj uspešno posodobljen!");
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
