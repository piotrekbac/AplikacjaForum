using System;

//Piotr Bacior 15 722 - WSEI Kraków

namespace AplikacjaForum
{
    // Definiujemy interfejs IUser, który będzie reprezentował użytkownika forum
    //Odpowiada on za reprezentację użytkownika forum, który może obserwować zdarzenia na forum
    public interface IUser : IObserver<ForumNotification>
    {
        //Deklarujemy właściwość Login, która będzie przechowywać login użytkownika
        string Login { get; }

        //Deklarujemy metodę Subscribe, która będzie subskrybować (obserwować) powiadomienia z forum
        void Subscribe(IObservable<ForumNotification> forum);

        //Deklarujemy metodę Unsubscribe, która będzie odsubskrybowywać (odobserwowywać) powiadomienia z forum
        void Unsubscribe();
    }
}
