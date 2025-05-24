namespace AplikacjaForum
{
    //Piotr Bacior 15 722 - WSEI Kraków 

    //Definiujemy enumerację NotificationType, która będzie używana do określenia typu powiadomienia
    public enum NotificationType
    {
        //Wartość odpowiadająca dodaniu pytania
        QuestionAdded,

        //Wartość odpowiadająca dodaniu odpowiedzi
        AnswerAdded
    }

    //Definiujemy klasę ForumNotification, która będzie używana do przesyłania powiadomień o zdarzeniach na forum
    public class ForumNotification
    {
        //Typ powiadomienia (dodanie pytania lub odpowiedzi)
        public NotificationType Type { get; }

        //Właściciel akcji (użytkownik, który dodał pytanie lub odpowiedź)
        public string ActionUser { get; }

        //Id pytania, do którego dodano odpowiedź lub które zostało dodane
        public string QuestionId { get; }

        //Id odpowiedzi, która została dodana
        public string AnswerId { get; }

        //Tekst pytania, do którego dodano odpowiedź
        public string QuestionText { get; }

        //Autor oryginalnego pytania, do którego dodano odpowiedź   
        public string OriginalQuestionAuthor { get; }

        //Konstruktor klasy ForumNotification, który przyjmuje parametry określające typ powiadomienia, użytkownika akcji, id pytania, id odpowiedzi, tekst pytania oraz autora oryginalnego pytania
        public ForumNotification(
            NotificationType type,
            string actionUser,
            string questionId,
            string answerId,
            string questionText,
            string originalQuestionAuthor)
        {
            //Inicjalizacja właściwości klasy ForumNotification
            Type = type;
            ActionUser = actionUser;
            QuestionId = questionId;
            AnswerId = answerId;
            QuestionText = questionText;
            OriginalQuestionAuthor = originalQuestionAuthor;
        }
    }
}
