using AplikacjaForum;
using System;
using System.Collections.Generic;

//Piotr Bacior 15 722 - WSEI Kraków 

namespace AplikacjaForum
{
    //Definiujemy publiczną klasę Forum, która implementuje interfejs IObservable<ForumNotification>
    public class Forum : IObservable<ForumNotification>
    {
        //Definiujemy prywatną listę subskrybentów (obserwatorów)
        private List<IObserver<ForumNotification>> obserwatorzy;

        //Teraz przechodzimy do zdefiniowania słownika pytań: klucz = questionId, wartość = pytanie
        private Dictionary<string, Pytanie> pytania;

        //Teraz przechodzimy do zdefiniowania słownik odpowiedzi: klucz = answerId, wartość = odpowiedź
        private Dictionary<string, Odpowiedź> odpowiedzi;

        //Definiujemy konstruktor klasy Forum, który inicjalizuje listę obserwatorów oraz słowniki pytań i odpowiedzi
        public Forum()
        {
            //Inicjalizacja listy obserwatorów oraz słowników pytań i odpowiedzi
            obserwatorzy = new List<IObserver<ForumNotification>>();
            pytania = new Dictionary<string, Pytanie>();
            odpowiedzi = new Dictionary<string, Odpowiedź>();
        }

        //Dodajemy funkcję AddQuestion, która dodaje nowe pytanie do forum
        public void AddQuestion(string questionId, string questionText, string authorLogin)
        {
            //Jeżeli pytanie o danym ID już istnieje, rzucamy wyjątek
            if (pytania.ContainsKey(questionId))
                throw new ArgumentException($"Pytanie o ID {questionId} już istnieje.");

            //Jeżeli pytanie o danym ID nie istnieje, tworzymy nowe pytanie i dodajemy je do słownika pytań
            var question = new Pytanie(questionId, questionText, authorLogin);

            //Dodajemy pytanie do słownika pytań
            pytania.Add(questionId, question);

            //Definiujemy powiadomienie o nowym pytaniu 
            var notification = new ForumNotification(

                //Dodajemy powiadomienie o nowym pytaniu
                NotificationType.QuestionAdded,

                //Dodajemy login autora pytania
                authorLogin,

                //Dodajemy ID pytania
                questionId,

                //Dodajemy ID odpowiedzi (null, ponieważ pytanie nie ma jeszcze odpowiedzi)
                null,

                //Dodajemy tekst czyli w zasadzie treść samego pytania
                questionText,

                //Dodajemy login autora pytania (null, ponieważ pytanie nie ma jeszcze odpowiedzi)
                null);

            //Teraz powiadamiamy wszystkich obserwatorów o nowym pytaniu za pomocą funkcji NotifyObservers
            NotifyObservers(notification);
        }

        //Definiujemy funkcję AddAnswer, która dodaje nową odpowiedź do pytania
        public void AddAnswer(string answerId, string answerText, string authorLogin, string questionId)
        {
            //Jeżeli pytanie o danym ID już istnieje, rzucamy wyjątek ArgumentException
            if (!pytania.ContainsKey(questionId))
                throw new ArgumentException($"Pytanie o ID {questionId} nie istnieje.");

            //Jeżeli odpowiedź o danym ID już istnieje, rzucamy wyjątek ArgumentException
            if (odpowiedzi.ContainsKey(answerId))
                throw new ArgumentException($"Odpowiedź o ID {answerId} już istnieje.");

            //Definiujemy nową odpowiedź i dodajemy ją do słownika odpowiedzi
            var answer = new Odpowiedź(answerId, answerText, authorLogin, questionId);

            //Dodajemy odpowiedź do słownika odpowiedzi
            odpowiedzi.Add(answerId, answer);

            //Teraz dodajemy odpowiedź do pytania za pomocą funkcji AddAnswer
            pytania[questionId].AddAnswer(answer);

            //Definiujemy powiadomienie o nowej odpowiedzi
            var notification = new ForumNotification(

                //Dodajemy powiadomienie o nowej odpowiedzi
                NotificationType.AnswerAdded,

                //Dodajemy login autora odpowiedzi
                authorLogin,

                //Dodajemy ID pytania
                questionId,

                //Dodajemy ID odpowiedzi
                answerId,

                //Dodajemy tekst odpowiedzi domyślnie null 
                null,

                //Dodajemy login autora pytania
                pytania[questionId].AuthorLogin);

            //Teraz powiadamiamy wszystkich obserwatorów o nowej odpowiedzi za pomocą funkcji NotifyObservers
            NotifyObservers(notification);
        }

        //Teraz przechodzimy do zdefiniowania funkcji GetAllQuestions, która zwraca wszystkie pytania
        public List<Pytanie> GetAllQuestions()
        {
            //Zwracamy wszystkie pytania w postaci listy
            return new List<Pytanie>(pytania.Values);
        }

        //Teraz definiujemmy funkcję GetQuestionByUser, która zwraca pytania zadane przez danego użytkownika
        public List<Pytanie> GetQuestionsByUser(string userLogin)
        {
            //Definiuy pustą listę pytań 
            var result = new List<Pytanie>();

            //Iterujemy po wszystkich pytaniach w słowniku pytania
            foreach (var q in pytania.Values)
            {
                //Jeżeli pytanie zostało zadane przez danego użytkownika, dodajemy je do listy
                if (q.AuthorLogin == userLogin)
                    result.Add(q);
            }

            //Zwracamy listę pytań
            return result;
        }

        //Definiujemy funkcję GetAnswersForQuestion, która zwraca odpowiedzi do danego pytania
        public List<Odpowiedź> GetAnswersForQuestion(string questionId)
        {
            //Jeżeli pytanie o danym ID już istnieje, rzucamy wyjątek ArgumentException
            if (!pytania.ContainsKey(questionId))
                throw new ArgumentException($"Pytanie o ID {questionId} nie istnieje.");

            //Zwracamy wszystkie odpowiedzi do po pytania 
            return pytania[questionId].Answers;
        }

        //Teraz definiujemy funkcję Subscribe, która dodaje obserwatora do listy obserwatorów
        public IDisposable Subscribe(IObserver<ForumNotification> observer)
        {
            //Jeżeli obserwatorzy nie zawierają tego obserwatora, to go dodajemy
            if (!obserwatorzy.Contains(observer))
                obserwatorzy.Add(observer);

            //Zwracamy nowy obiekt Unsubscriber, który będzie zarządzał subskrypcją
            return new Unsubscriber(obserwatorzy, observer);
        }

        //Teraz definiujemy funkcję NotifyObservers, która powiadamia wszystkich obserwatorów o nowym powiadomieniu
        private void NotifyObservers(ForumNotification notification)
        {
            //Iterujemy po wszystkich obserwatorach w liście obserwatorzy
            foreach (var observer in obserwatorzy)
            {
                //Powiadamiamy obserwatora o nowym powiadomieniu za pomocą funkcji OnNext
                observer.OnNext(notification);
            }
        }

        //Teraz definiujemy klasę Unsubscriber, która implementuje interfejs IDisposable, odpowiadzialna jest ona za zarządzanie subskrypcją
        private class Unsubscriber : IDisposable
        {
            //Definiujemy prywatną listę obserwatorów oraz prywatnego obserwatora
            private List<IObserver<ForumNotification>> _obserwatorzy;

            //Definiujemy prywatnego obserwatora
            private IObserver<ForumNotification> _obserwator;

            ////Definiujemy konstruktor klasy Unsubscriber, który inicjalizuje listę obserwatorów oraz prywatnego obserwatora
            public Unsubscriber(List<IObserver<ForumNotification>> obserwatorzy, IObserver<ForumNotification> obserwator)
            {
                //Inicjalizujemy listę obserwatorów oraz prywatnego obserwatora
                _obserwatorzy = obserwatorzy;
                _obserwator = obserwator;
            }

            //Definiujemy metodę Dispose, która usuwa obserwatora z listy obserwatorów
            public void Dispose()
            {
                //Jeżeli obserwatorzy nie są null i zawierają tego obserwatora, to go usuwamy
                if (_obserwator != null && _obserwatorzy.Contains(_obserwator))
                    _obserwatorzy.Remove(_obserwator);
            }
        }

        //Teraz przechodzimy do zdefiniowania klasy Pytanie, która reprezentuje pytanie na forum
        public class Pytanie
        {
            //Definiujemy publiczne właściwości pytania: ID pytania, tekst pytania, login autora pytania oraz listę odpowiedzi
            public string QuestionId { get; }
            public string Text { get; }
            public string AuthorLogin { get; }
            public List<Odpowiedź> Answers { get; }

            //Definiujemy konstruktor klasy Pytanie, który inicjalizuje ID pytania, tekst pytania, login autora pytania oraz listę odpowiedzi
            public Pytanie(string id, string text, string authorLogin)
            {
                QuestionId = id;
                Text = text;
                AuthorLogin = authorLogin;
                Answers = new List<Odpowiedź>();
            }

            //Definiujemy metodę AddAnswer, która dodaje odpowiedź do pytania
            public void AddAnswer(Odpowiedź answer)
            {
                Answers.Add(answer);
            }
        }

        //Teraz przechodzimy do zdefiniowania klasy Odpowiedź, która reprezentuje odpowiedź na pytanie
        public class Odpowiedź
        {
            //Definiujemy publiczne właściwości odpowiedzi: ID odpowiedzi, tekst odpowiedzi, login autora odpowiedzi oraz ID pytania
            public string AnswerId { get; }
            public string Text { get; }
            public string AuthorLogin { get; }
            public string QuestionId { get; }

            //Definiujemy konstruktor klasy Odpowiedź, który inicjalizuje ID odpowiedzi, tekst odpowiedzi, login autora odpowiedzi oraz ID pytania
            public Odpowiedź(string id, string text, string authorLogin, string questionId)
            {
                AnswerId = id;
                Text = text;
                AuthorLogin = authorLogin;
                QuestionId = questionId;
            }
        }
    }
}
