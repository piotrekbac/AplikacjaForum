using AplikacjaForum;
using System;
using System.Collections.Generic;

//Piotr Bacior 15 722 - WSEI Kraków

namespace AplikacjaForum
{
    //Definiujemy klasę ForumStatistics, która implementuje interfejs IObserver<ForumNotification>
    //Klasa ta będzie nam służyć do zbierania statystyk dotyczących pytań i odpowiedzi na forum
    public class ForumStatistics : IObserver<ForumNotification>
    {
        //Przechodzimy do zdefiniowania publiczncyh właściwości, które będą przechowywać różne statystyki dotyczące pytań i odpowiedzi na forum
        public int TotalQuestions { get; private set; }
        public int TotalAnswers { get; private set; }
        public double AverageAnswersPerQuestion { get; private set; }
        public int UnansweredQuestions { get; private set; }
        public int AnsweredQuestions { get; private set; }

        //Dokonujemy zdefiniowania prywatnej właściwości, która będzie przechowywać liczbę odpowiedzi dla każdego pytania (answersPerQuestion)
        private Dictionary<string, int> answersPerQuestion;

        //Teraz definiujemy konstruktor klasy ForumStatistics, który będzie inicjował wszystkie właściwości do wartości początkowych
        public ForumStatistics()
        {
            TotalQuestions = 0;
            TotalAnswers = 0;
            AverageAnswersPerQuestion = 0.0;
            UnansweredQuestions = 0;
            AnsweredQuestions = 0;
            answersPerQuestion = new Dictionary<string, int>();
        }

        //Definiujemy metodę OnNext, która będzie wywoływana przy każdym nowym powiadomieniu o zdarzeniu na forum
        public void OnNext(ForumNotification notification)
        {
            //Jeżeli powiadomienie dotyczy dodania pytania, zwiększamy liczbę pytań i dodajemy nowe pytanie do słownika answersPerQuestion
            if (notification.Type == NotificationType.QuestionAdded)
            {
                //Zwiększamy liczbę wszystkich pytań o 1
                TotalQuestions++;

                //Nowe pytanie - na początku bez odpowiedzi
                answersPerQuestion[notification.QuestionId] = 0;

                //Jeżeli pytanie nie miało odpowiedzi, zwiększamy liczbę pytań bez odpowiedzi
                UnansweredQuestions++;

                //Aktualizujemy średnią odpowiedzi na pytanie
                UpdateAverage();
            }

            //Jeżeli powiadomienie dotyczy dodania odpowiedzi do istniejącego pytania, zwiększamy liczbę odpowiedzi i aktualizujemy słownik answersPerQuestion
            else if (notification.Type == NotificationType.AnswerAdded)
            {
                //Zwiększamy liczbę wszystkich odpowiedzi o 1
                TotalAnswers++;

                //Dokonujemy sprawdzenia czy pytanie istnieje w słowniku answersPerQuestion
                if (answersPerQuestion.ContainsKey(notification.QuestionId))
                {
                    //Pobieramy dotychczasową ilość odpowiedzi na to pytanie
                    int oldCount = answersPerQuestion[notification.QuestionId];

                    //Zwiększamy ilość odpowiedzi na to pytanie o 1
                    answersPerQuestion[notification.QuestionId] = oldCount + 1;

                    //Jeżeli wcześniej pytanie miało 0 odpowiedzi, zmniejszamy liczbę pytań bez odpowiedzi i zwiększamy liczbę pytań z odpowiedzią
                    if (oldCount == 0)
                    {
                        //Zmniejszamy liczbę pytań bez odpowiedzi o 1
                        UnansweredQuestions--;

                        //Zwiększamy liczbę pytań z odpowiedzią o 1
                        AnsweredQuestions++;
                    }
                }

                //Aktualizujemy średnią odpowiedzi na pytanie
                UpdateAverage();
            }
        }

        //Teraz definiujemy prywatną metodę UpdateAverage, która będzie aktualizować średnią odpowiedzi na pytanie
        private void UpdateAverage()
        {
            //Jeżeli nie ma pytań, średnia odpowiedzi na pytanie wynosi 0
            if (TotalQuestions == 0)

                //Ustawiamy średnią odpowiedzi na pytanie na 0
                AverageAnswersPerQuestion = 0.0;

            //Jeżeli są pytania, obliczamy średnią odpowiedzi na pytanie
            else

                //Ustawiamy średnią odpowiedzi na pytanie na liczbę wszystkich odpowiedzi podzieloną przez liczbę wszystkich pytań
                AverageAnswersPerQuestion = (double)TotalAnswers / TotalQuestions;
        }

        //Definiujemy metody OnError i OnCompleted, które są wymagane przez interfejs IObserver<ForumNotification>, ale nie są używane w tej klasie 
        public void OnError(Exception error) { }
        public void OnCompleted() { }
    }
}
