namespace Models.Reviewer{
    public class Reviewer{
        public int Id {get; set;}
        public string FirstName {get; set;} = "";
        public string LastName {get; set;} = "";

        //building many-to-one-relationship btw Reviewer & reviews i.e many(Reviewer) -> one(review)
        public  ICollection<Review.Review>? Reviews { get; set; }


    }
}