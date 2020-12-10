using System;
using System.Collections.Generic;
using System.Text;

namespace lecture_9
{
  public  class InhertianceVSPolymorphism
    {
        //this is inheritance example. there is 1 base class and there are derived classes from the base class or from derived classes
        public class BaseClass
        {
            public string HelloMessage = "Hello, World!";
        }

        public class SubClass : BaseClass
        {
            public string ArbitraryMessage = "Uh, Hi!";
        }

        public class Level2SubClass : SubClass
        {
            public string MessageofLevel2Class = "Uh, Hi!";

            public Level2SubClass()
            {
                this.ArbitraryMessage = "arbitrary message of level 2 class";
                this.HelloMessage = "hello message of level 2 class";
            }
        }

        public class Test
        {
            public static List<string> Main()
            {
                BaseClass bsClassExample = new BaseClass();

                SubClass subClass = new SubClass();

                Level2SubClass lvl2Class = new Level2SubClass();

                //this below 2 is example of polimorphism
                Animal animal = new Dog();

                Animal animalTwo = new Cat();

                //animal.irDogCount // this is invalid because irDogCount property is only available to dog base class

                return new List<string>()
                {
                    $"HelloMessage of base class: {bsClassExample.HelloMessage}",
   $"ArbitraryMessage of level 1 class: {subClass.ArbitraryMessage}",
      $"HelloMessage of level 1 class: {subClass.HelloMessage}",
         $"ArbitraryMessage of level 2 class: {lvl2Class.ArbitraryMessage}",
            $"HelloMessage of level 2 class: {lvl2Class.HelloMessage}",
               $"Message of level 2 class: {lvl2Class.MessageofLevel2Class}",
              $"Dog name: {animal.Name}",
                     $"Cat name: {animalTwo.Name}"
                };
                 


            }
        }

        //********************************************

        public interface Animal
        {
            string Name { get; }
        }

        public class Dog : Animal
        {
            public string Name { get { return "Dog"; } }

            public int irDogCount { get; set; }//this property is not available in interface therefore it would not be available in objects generated from interface
        }

        public class Cat : Animal
        {
            public string Name { get { return "Cat"; } }

            public int irCatCount { get; set; }//this property is not available in interface therefore it would not be available in objects generated from interface
        }


        public class Lion : Cat //multi-level inheritance
        {
            public int irHunger { get; set; }
        }

    }
}
