using System;

//Piotr Bacior 15 722 - WSEI Kraków

namespace AplikacjaForum
{
    //Definiujemy interfejs IForum, który będzie reprezentował forum, dziedziłący interfejs IObservable<ForumNotification>
    //Dzięki implementacji tego interfejsu, forum będzie mogło wysyłać powiadomienia do subskrybentów (użytkowników)
    public interface IForum : IObservable<ForumNotification>
    {
        //Definiujemy metodę AddQuestion, która będzie dodawać nowe pytanie do forum, przyjmując jako argumenty id pytania, tekst pytania oraz login autora
        void AddQuestion(string questionId, string questionText, string authorLogin);

        //Definiujemy metodę AddAnswer, która będzie dodawać nową odpowiedź do forum, przyjmując jako argumenty id odpowiedzi, tekst odpowiedzi, login autora oraz id pytania, do którego odpowiedź jest przypisana
        void AddAnswer(string answerId, string answerText, string authorLogin, string questionId);
    }
}
