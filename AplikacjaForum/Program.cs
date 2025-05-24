//Program.cs
//Główny plik programu demonstrujący działanie forum pytań i odpowiedzi
using AplikacjaForum;
using System;

//Piotr Bacior 15 722 - WSEI Kraków

namespace AplikacjaForum
{
    //Definiujemy klasę Program z metodą Main - punkt wejścia do aplikacji konsolowej
    class Program
    {
        //Metoda Main - tutaj uruchamiamy całą logikę programu
        static void Main(string[] args)
        {
            //Wyświetlamy informację o autorze i uczelni
            Console.WriteLine("Piotr Bacior 15 722 - WSEI Kraków\n");

            //Inicjalizujemy forum (tworzymy nowy obiekt klasy Forum)
            Forum forum = new Forum();

            //Inicjalizujemy obserwatora statystyk i subskrybujemy go do forum
            ForumStatistics obserwatorStatystyk = new ForumStatistics();
            forum.Subscribe(obserwatorStatystyk);

            //Tworzymy użytkowników z różnymi trybami obserwacji
            Użytkownik kasia = new Użytkownik("Kasia", obserwujWszystko: true);                          // obserwuje całe forum
            Użytkownik piotr = new Użytkownik("Piotr", obserwujTylkoSwojePytania: true);                 // obserwuje tylko odpowiedzi na swoje pytania
            Użytkownik michal = new Użytkownik("Michał", obserwujWszystko: true);                        // obserwuje całe forum
            Użytkownik krzysiu = new Użytkownik("Krzysiu", obserwujTylkoSwojePytania: true);             // obserwuje tylko odpowiedzi na swoje pytania
            Użytkownik czesiu = new Użytkownik("Czesiu");                                                // nie obserwuje nic
            Użytkownik marek = new Użytkownik("Marek");                                                  // nie obserwuje nic

            //Rejestrujemy użytkowników jako obserwatorów forum
            forum.Subscribe(kasia);
            forum.Subscribe(piotr);
            forum.Subscribe(michal);
            forum.Subscribe(krzysiu);
            forum.Subscribe(czesiu);
            forum.Subscribe(marek);

            //Wyświetlamy informację o zarejestrowanych użytkownikach
            Console.WriteLine("Zarejestrowani użytkownicy:");
            Console.WriteLine("- Kasia (obserwuje wszystko)");
            Console.WriteLine("- Piotr (obserwuje tylko odpowiedzi na swoje pytania)");
            Console.WriteLine("- Michał (obserwuje wszystko)");
            Console.WriteLine("- Krzysiu (obserwuje tylko odpowiedzi na swoje pytania)");
            Console.WriteLine("- Czesiu (nie obserwuje powiadomień)");
            Console.WriteLine("- Marek (nie obserwuje powiadomień)\n");

            //Dodajemy pytania do forum - oryginalne, nietypowe treści pytań
            forum.AddQuestion("P1", "Jakie są praktyczne zastosowania liczby PI w codziennym życiu?", "Kasia");
            forum.AddQuestion("P2", "Czy istnieje sposób na domowe wykrycie pola magnetycznego bez użycia kompasu?", "Piotr");
            forum.AddQuestion("P3", "Dokąd nocą tupta jeż?", "Michał");
            forum.AddQuestion("P4", "Dlaczego chleb tostowy pleśnieje szybciej od zwykłego chleba?", "Krzysiu");

            //Dodajemy odpowiedzi do pytań - oryginalne, nietypowe odpowiedzi
            forum.AddAnswer("O1", "Liczba PI jest wykorzystywana przy obliczaniu powierzchni pizzy i długości ogrodu z okrągłym trawnikiem.", "Czesiu", "P1");
            forum.AddAnswer("O2", "Wystarczy zawiesić igłę na nitce i obserwować jej reakcję w pobliżu przewodów elektrycznych.", "Marek", "P2");
            forum.AddAnswer("O3", "A któż to wie, a któż to wie...", "Piotr", "P3");
            forum.AddAnswer("O4", "Chleb tostowy zawiera więcej wilgoci i często jest pakowany szczelnie, co sprzyja rozwojowi pleśni.", "Kasia", "P4");
            forum.AddAnswer("O5", "Można też wykorzystać aplikację na smartfona z czujnikiem pola magnetycznego.", "Michał", "P2");

            //Wyświetlamy statystyki forum
            Console.WriteLine("\n=== Statystyki forum ===");
            Console.WriteLine($"Całkowita liczba pytań: {obserwatorStatystyk.TotalQuestions}");
            Console.WriteLine($"Całkowita liczba odpowiedzi: {obserwatorStatystyk.TotalAnswers}");
            Console.WriteLine($"Średnia liczba odpowiedzi na pytanie: {obserwatorStatystyk.AverageAnswersPerQuestion:F2}");
            Console.WriteLine($"Pytania bez odpowiedzi: {obserwatorStatystyk.UnansweredQuestions}");
            Console.WriteLine($"Pytania z co najmniej jedną odpowiedzią: {obserwatorStatystyk.AnsweredQuestions}");

            //Wyświetlamy wszystkie pytania i odpowiedzi do nich (prezentacja logiki forum)
            Console.WriteLine("\n=== Lista pytań i odpowiedzi ===");

            //Inicjalizujemy zmienną do numerowania pytań
            int nrPytania = 1;

            //Iterujemy przez wszystkie pytania na forum i wyświetlamy je
            foreach (var pytanie in forum.GetAllQuestions())
            {
                //Wyświetlamy pytanie i jego autora
                Console.WriteLine($"{nrPytania}. [{pytanie.QuestionId}] {pytanie.Text} (autor: {pytanie.AuthorLogin})");

                //Pobieramy odpowiedzi do pytania i wyświetlamy je
                var odpowiedzi = forum.GetAnswersForQuestion(pytanie.QuestionId);

                //Jeżeli pytanie nie ma odpowiedzi, wyświetlamy komunikat
                if (odpowiedzi.Count == 0)
                {
                    //Wyświetlamy komunikat o braku odpowiedzi
                    Console.WriteLine("Brak odpowiedzi.");
                }

                //Jeżeli pytanie ma odpowiedzi, wyświetlamy je
                else
                {
                    //Iterujemy przez wszystkie odpowiedzi i wyświetlamy je
                    foreach (var odp in odpowiedzi)
                    {
                        //Wyświetlamy odpowiedź i jej autora
                        Console.WriteLine($"   - [{odp.AnswerId}] {odp.Text} (autor: {odp.AuthorLogin})");
                    }
                }

                //Przechodzimy do następnego pytania
                nrPytania++;
            }
        }
    }
}