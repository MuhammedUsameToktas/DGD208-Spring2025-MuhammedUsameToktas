using System;

namespace PetSimulator
{
    public static class AsciiArt
    {
        public static string GetHomeArt()
        {
            return @"
    /\___/\
   (  o o  )
   (  =^=  ) 
    (____)
   Pet Home";
        }

        public static string GetPetArt(PetType type)
        {
            return type switch
            {
                PetType.Dog => @"
    / \__
   (    @\___
   /         O
  /   (_____/
 /_____/   U",
                PetType.Cat => @"
 /\_/\
( o.o )
 > ^ <",
                PetType.Dragon => @"
    /\
   /  \
  /    \
 /      \
/________\
    ||
    ||",
                _ => @"
    /\
   /  \
  /    \
 /      \
/________\"
            };
        }
    }
} 