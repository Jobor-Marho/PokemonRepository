using System;
using System.Collections.Generic;
using Models.Owner;

namespace Models.Country
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Many-to-One Relationship (Many Countries -> One Owner)
        public ICollection<Owner.Owner> Owners { get; set; }

        public Country(string name)
        {
            this.Name = name;
            this.Owners = new HashSet<Owner.Owner>(); // ✅ Prevents NullReferenceException
        }
    }
}
