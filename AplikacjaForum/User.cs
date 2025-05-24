using AplikacjaForum;
using System;

//Piotr Bacior 15 722 - WSEI Kraków

namespace AplikacjaForum
{
    //Definiujemy klasę Użytkownik (User), która implementuje interfejs IObserver<ForumNotification>
    //Klasa ta reprezentuje użytkownika forum, który może subskrybować powiadomienia o zdarzeniach na forum
    public class Użytkownik : IObserver<ForumNotification>
    {
        //Login użytkownika - identyfikator użytkownika na forum
        public string Login { get; }

        //Czy użytkownik chce obserwować wszystkie zdarzenia na forum (true - obserwuje wszystkie, false - nie)
        private bool ObserwujWszystko { get; }

        //Czy użytkownik chce obserwować tylko odpowiedzi na swoje pytania (true - obserwuje tylko swoje, false - nie)
        private bool ObserwujTylkoSwojePytania { get; }

        //Prywatne pole przechowujące obiekt do zarządzania subskrypcją (pozwala na wypisanie się z obserwacji)
        private IDisposable wypisanie;

        //Teraz przechodzimy do zdefiniowania konstruktora klasy Użytkownik - ustawia login oraz tryb obserwacji
        public Użytkownik(string login, bool obserwujWszystko = false, bool obserwujTylkoSwojePytania = false)
        {
            Login = login;                                   //Przypisujemy login użytkownika
            ObserwujWszystko = obserwujWszystko;             //Przypisujemy tryb obserwowania wszystkich zdarzeń
            ObserwujTylkoSwojePytania = obserwujTylkoSwojePytania; //Przypisujemy tryb obserwowania tylko własnych pytań
        }

        //Definiujemy metodę do subskrybowania powiadomień z forum
        public void Subskrybuj(IObservable<ForumNotification> forum)
        {
            //Jeżeli forum nie jest null, rozpoczynamy subskrypcję i zapisujemy obiekt do wypisania
            if (forum != null)
                wypisanie = forum.Subscribe(this);
        }

        //Teraz definiujemy metodę do wypisania się z subskrypcji (opcjonalna, ale zalecana)
        public void Wypisz()
        {
            //Jeżeli mamy aktywną subskrypcję, wywołujemy Dispose aby się od niej odłączyć
            wypisanie?.Dispose();
        }

        //Definiujemy metodę wywoływaną automatycznie, gdy pojawi się nowe powiadomienie na forum
        public void OnNext(ForumNotification powiadomienie)
        {
            //Jeżeli użytkownik obserwuje całe forum (wszystkie zdarzenia), zawsze wyświetlamy powiadomienie
            if (ObserwujWszystko)
            {
                //Wyświetlamy powiadomienie o zdarzeniu na forum
                WyswietlPowiadomienie(powiadomienie); 
                return;
            }

            //Jeżeli użytkownik obserwuje tylko odpowiedzi na swoje pytania 
            if (ObserwujTylkoSwojePytania)
            {
                //Jeżeli przyszła odpowiedź na jego własne pytanie
                if (powiadomienie.Type == NotificationType.AnswerAdded && powiadomienie.OriginalQuestionAuthor == Login)
                {
                    //Wyświetlamy specjalny komunikat, że ktoś odpowiedział na jego pytanie
                    Console.WriteLine($"Użytkownik {powiadomienie.ActionUser} udzielił odpowiedzi {powiadomienie.AnswerId} na Twoje pytanie {powiadomienie.QuestionId}.");
                }
                return;
            }

            //Jeśli użytkownik nie obserwuje nic konkretnego, nie robimy nic (nie wyświetlamy komunikatu)
        }

        //Prywatna metoda do wyświetlania powiadomień o wszystkich zdarzeniach na forum (dla trybu ObserwujWszystko)
        private void WyswietlPowiadomienie(ForumNotification powiadomienie)
        {
            //Wybieramy rodzaj powiadomienia na podstawie typu zdarzenia
            switch (powiadomienie.Type)
            {
                //Dodajemy przypadek dla dodania pytania
                case NotificationType.QuestionAdded:

                    //Wyświetlamy komunikat o dodaniu nowego pytania
                    Console.WriteLine($"Użytkownik {powiadomienie.ActionUser} dodał pytanie {powiadomienie.QuestionId}.");
                    break;

                //Dodajemy przypadek dla dodania odpowiedzi
                case NotificationType.AnswerAdded:

                    //Wyświetlamy komunikat o dodaniu odpowiedzi do pytania
                    Console.WriteLine($"Użytkownik {powiadomienie.ActionUser} udzielił odpowiedzi {powiadomienie.AnswerId} na pytanie {powiadomienie.QuestionId} zadane przez użytkownika {powiadomienie.OriginalQuestionAuthor}.");
                    break;
            }
        }

        //Poniższe metody są wymagane przez interfejs IObserver, ale nie są używane w tej klasie

        //Obsługa błędów - pusta implementacja
        public void OnError(Exception blad) { }

        //Powiadomienie o zakończeniu - pusta implementacja
        public void OnCompleted() { }                
    }
}