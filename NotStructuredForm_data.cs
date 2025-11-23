namespace naloga01_nestrukturiraniPodatki
{
    class Program
    {
        // Pot do datoteke kot konstanta, da jo lahko vsi deli programa uporabljajo
        static string potDoDatoteke = "artikli.txt";
        static void Main(string[] args)
        {
            bool konec = false;
            while (!konec)
            {
                Console.WriteLine("\n===== UPRAVLJANJE ARTIKLOV ====");
                Console.WriteLine("1 - Shrani nov artikel");
                Console.WriteLine("2 - Išči artikle po dobavitelju in zalogi");
                Console.WriteLine("3 - Prikaži vse artikle");
                Console.WriteLine("4 - Znižaj ceno artiklov");
                Console.WriteLine("0 - Izhod iz apliakcije");
                Console.WriteLine("\nIzberi možnost: ");

                string izbira = Console.ReadLine();

                switch (izbira)
                {
                    case "1":
                        ShraniArtikel();
                        break;
                    case "2":
                        IsciArtikle();
                        break;
                    case "3":
                        BeriVseArtikle();
                        break;
                    case "4":
                        ZnizajCene();
                        break;
                    case "0":
                        konec = true;
                        Console.WriteLine("Program se končuje...");
                        break;
                    default:
                        Console.WriteLine("Napačna izbira!");
                        break;
                            
                }
            }
        }


        static void ShraniArtikel()
        {
            Console.WriteLine("\n ---- Vnos novega artikla ----");

            Console.WriteLine("Ime izdelka: ");
            string ime = Console.ReadLine();

            Console.WriteLine("Cena izdelka: ");
            string cena = Console.ReadLine();

            Console.WriteLine("Zaloga: ");
            string zaloga = Console.ReadLine();

            Console.WriteLine("Dobavitelj: ");
            string dobavitelj = Console.ReadLine();

            // ustvarjanje vrstice z vsemi podatki, ločeno s podpičjem
            string vrstica = $"{ime};{cena};{zaloga};{dobavitelj}";

            try
            {
                // Streamwriter s true parametrom odpre datoteko v append načinu,
                // kar pomeni, da ne prepiše obstoječe vsebine, ampak doda nov vnos na konec
                using (StreamWriter sw = new StreamWriter(potDoDatoteke, true))
                {
                    sw.WriteLine(vrstica);  // zapiše vrstico in doda novo vrstico na konec
                }
                Console.WriteLine("\nArtikel je bil uspešno shranjen!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nPrišlo je do izjeme {ex.Message}");
            }
            Console.ReadKey();
            Console.Clear();
        }
    
        
        static void IsciArtikle()
        {
            if (!File.Exists(potDoDatoteke))
            {
                Console.WriteLine("\nDatoteka ne obstaja!");
                return;
            }

            Console.WriteLine("\n ---- ISKANJE ARTIKLOV ----");
            Console.WriteLine("Vnesi ime dobavitelja: ");
            string iskanDobavitelj = Console.ReadLine();

            Console.WriteLine("Vnesi maximalno zalog: ");
            string vnosMaxZalog = Console.ReadLine();

            int maksZaloga = int.Parse(vnosMaxZalog);

            List<string> najdeniArtikli = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(potDoDatoteke))
                {
                    string vrstica;

                    while ((vrstica = sr.ReadLine()) != null)
                    {
                        // Razdelimo vrstico na dele glede na podpičje. Split ustvari array stringov
                        string[] podatki = vrstica.Split(';');

                        if(podatki.Length == 4)
                        {
                            string ime = podatki[0];
                            string cena = podatki[1];
                            int zaloga = int.Parse(podatki[2]);
                            string dobavitelj = podatki[3];

                            // ujemanje dobavitelja in zaloga manjša od vnešene
                            if((dobavitelj.ToLower() == iskanDobavitelj.ToLower()) && (zaloga < maksZaloga))
                            {
                                najdeniArtikli.Add(vrstica);
                            }
                        }
                    }

                    if (najdeniArtikli.Count > 0)
                    {
                        Console.WriteLine($"Najdenih je bilo {najdeniArtikli.Count} artiklov: ");
                        Console.WriteLine("-------------------------------");

                        foreach (string artikel in najdeniArtikli)
                        {
                            string[] podatki = artikel.Split(";");
                            Console.WriteLine($"Izdelek: {podatki[0]}");
                            Console.WriteLine($"Cena: {podatki[1]} EUR");
                            Console.WriteLine($"Zaloga: {podatki[2]} kosov");
                            Console.WriteLine($"Dobavitelj: {podatki[3]}");
                            Console.WriteLine("-------------------------------");
                        }

                        string datotekaRezultatov = "rezultati_iskanja.txt";
                        using (StreamWriter sw = new StreamWriter(datotekaRezultatov, false))
                        {
                            Console.WriteLine($"REZULTATI ISKANJA - Dobavitelj {iskanDobavitelj}, Zaloga < {maksZaloga}");
                            Console.WriteLine("================================");

                            foreach (string artikel in najdeniArtikli)
                            {
                                sw.WriteLine(artikel);
                            }
                        }
                        Console.WriteLine($"\n Rezultati shrnajeni v {datotekaRezultatov}");

                    }
                    else
                    {
                        Console.WriteLine("Ni najdenih artilklov, ki bi ustrezali pogojem.");
                    }
                }
            }
            catch (Exception ex){
                Console.WriteLine($"Prišlo je do napake pri branju: {ex.Message}");
            }
            Console.ReadKey();
            Console.Clear();
        }
    

        static void BeriVseArtikle()
        {
            if (!File.Exists(potDoDatoteke))
            {
                Console.WriteLine("Datoteka artiklov ne obstaja!");
                return;
            }

            Console.WriteLine("\n ---- VSI ARTIKLI ----");
            Console.WriteLine("==============================================");
            try
            {
                using(StreamReader sr = new StreamReader(potDoDatoteke))
                {
                    string vrstica;
                    int stevilka = 1;

                    while ((vrstica = sr.ReadLine()) != null)
                    {
                        string[] podatki = vrstica.Split(";");

                        if(podatki.Length == 4)
                        {
                            Console.WriteLine($"\n{stevilka}. {podatki[0]}");
                            Console.WriteLine($"    Cena: {podatki[1]} EUR");
                            Console.WriteLine($"    Zaloga: {podatki[2]} kosov");
                            Console.WriteLine($"    Dobavitelj: {podatki[3]}");
                            stevilka++;
                        }
                    }

                    Console.WriteLine("\n==============================================");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Napaka pri branju {ex.Message}");
            }
            Console.ReadKey();
            Console.Clear();
        }
    
    
        static void ZnizajCene()
        {
            if (!File.Exists(potDoDatoteke))
            {
                Console.WriteLine("Datoteka artiklov ne obstaja!");
                return;
            }

            Console.WriteLine("\n ---- ZNIŽANJE CEN ----");
            Console.WriteLine("Vnesite odstotek znižanja (npr. 10 za 10%): ");
            string vnosOdstotka = Console.ReadLine();

            double odstotek = double.Parse(vnosOdstotka);

            if(odstotek <= 0 || odstotek > 100)
            {
                Console.WriteLine("\n Odstotek mora biti med 0 in 100");
                return;
            }

            List<string> akcijskiArtikli = new List<string>();
            int stArtklov = 0;

            try
            {
                using (StreamReader sr = new StreamReader(potDoDatoteke))
                {
                    string vrstica;

                    while ((vrstica = sr.ReadLine()) != null)
                    {
                        string[] podatki = vrstica.Split(";");
                        if (podatki.Length == 4)
                        {
                            string ime = podatki[0];
                            double staraCena = double.Parse(podatki[1]);
                            string zaloga = podatki[2];
                            string dobavitelj = podatki[3];


                            double novaCena = staraCena * (1 - odstotek / 100);
                            novaCena = Math.Round(novaCena, 2);

                            string novaVrstica = $"{ime};{novaCena.ToString("F2")};{zaloga};{dobavitelj}";

                            akcijskiArtikli.Add(novaVrstica);
                            stArtklov++;

                            Console.WriteLine($"\n{ime}");
                            Console.WriteLine($"    Stara cena: {staraCena.ToString("F2")} EUR");
                            Console.WriteLine($"    Nova cena: {novaCena.ToString("F2")} EUR, znižano za {odstotek}%");
                        }

                    }
                }

                string datotekaAkcije = "artikli_akcija.txt";

                using (StreamWriter sw = new StreamWriter(datotekaAkcije, false))
                {
                    sw.WriteLine($"AKCIJA - Znižanje {odstotek}%");
                    sw.WriteLine($"Datum: {DateTime.Now.ToString("dd.MM.yyyy HH:mm")}");
                    sw.WriteLine($"Število artiklov {stArtklov}");
                    sw.WriteLine();

                    foreach (string artikel in akcijskiArtikli)
                    {
                        sw.WriteLine(artikel);
                    }
                }

                Console.WriteLine($"Uspešno znižanih {stArtklov} artiklov");
                Console.WriteLine($"Akcijski artikli shranjeni v {datotekaAkcije}");
            }
            catch (FormatException)
            {
                Console.WriteLine("\nNapaka: Cena v datoteki ni veljavno število!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nPrišlo je do napake: {ex.Message}");
            }
        }
    
    }
}
