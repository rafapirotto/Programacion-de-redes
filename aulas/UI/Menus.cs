using System;
using System.Collections.Generic;

namespace UI
{
    public class Menus
    {
           public static int ClientControllerLoopMenu()
        {
            List<string> options = new List<string>(new[]{
                    "Enroll to subject",
                    "Available subjects",
                    "Upload assignment",
                    "Grades",
                    "Exit"
            });
            for (var i = 0; i < options.Count; i++)
            {
                Console.WriteLine(i + 1 + " - " + options[i]);
            }
            return Input.SelectMenuOption("Choose an option", 1, options.Count);
        }

        public static int ServerMainMenu()
        {
            List<string> options = new List<string>(new[]{
                "Show All Students",
                "Show Connected Players",
                "Exit"
            });

            for (var i = 0; i < options.Count; i++)
            {
                Console.WriteLine(i + 1 + " - " + options[i]);
            }
            return Input.SelectMenuOption("Choose an option", 1, options.Count);
        }

    }
}