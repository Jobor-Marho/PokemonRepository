using Models.pokemeon;
using Models.Reviewer;

namespace Models.Review{
    public class Review
    {
        public int Id {get; set;}
        public string Title {get; set;} = "";
        public string Text {get; set;} = "";
        public int Rating {get; set;}

        //building one-to-many relationship between review and reviewer
        public  Reviewer.Reviewer? Reviewer {get; set;}

        //building one-to-many relationship between review and pokemon
        public  Pokemon? Pokemon {get; set;}

    }
}



