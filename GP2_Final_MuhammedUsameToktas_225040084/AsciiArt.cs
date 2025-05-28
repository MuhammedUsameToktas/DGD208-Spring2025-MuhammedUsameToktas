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
          .==.        .==.
           //`^\\      //^`\\
          // ^ ^\(\__/)/^ ^^\\
         //^ ^^ ^/6  6\ ^^ ^^\\
        //^ ^^ ^ ( .. ) ^ ^^^ \\
       // ^^ ^/\//v""v\\/\^ ^ ^\\
      // ^^/\/  / `~~` \  \/\^ ^\\
      \\^ /    / ,    , \    \^ //
       \\/    ( (      ) )    \//
        ^      \ \.__./ /      ^
               (((`  ')))",
                PetType.Rabbit => @"
    (\_/)
    (•ᴗ•)
    / >  \",
                PetType.Hamster => @"
    (\,/)
    (o o)
    /   \",
                PetType.Bird => @"
     /\
    /  \
   /    \
  /      \
 /________\
    ||
    ||",
                PetType.Turtle => @"
    _____
   /     \
  /  ^_^  \
 /  /   \  \
/__/     \__\",
                PetType.Fish => @"
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