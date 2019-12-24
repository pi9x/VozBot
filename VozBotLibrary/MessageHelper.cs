using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace VozBotLibrary
{
    public static class MessageHelper
    {
        /// <summary>
        /// Replace all keywords (ex: [firstname]) with actual user information
        /// </summary>
        /// <param name="source"> Source string </param>
        /// <param name="firstname"> Chat user firstname </param>
        /// <param name="lastname"> Chat user lastname </param>
        /// <param name="username"> Chat user username </param>
        /// <param name="title"> Title of the chat group </param>
        /// <returns> Unmasked string, ready to be pushed to the chat </returns>
        public static string Unmasking(string source, string firstname, string lastname, string username, string title)
        {
            char[] delims = { '[', ']' };
            string[] splittedStrings = source.Split(delims, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < splittedStrings.Length; i++)
            {
                if (splittedStrings[i].ToUpper() == "FIRSTNAME")
                    if (firstname == null) splittedStrings[i] = "";
                    else splittedStrings[i] = firstname;
                if (splittedStrings[i].ToUpper() == "LASTNAME")
                    if (lastname == null) splittedStrings[i] = "";
                    else splittedStrings[i] = lastname;
                if (splittedStrings[i].ToUpper() == "USERNAME")
                    if (username == null) splittedStrings[i] = "";
                    else splittedStrings[i] = "@" + username;
                if (splittedStrings[i].ToUpper() == "TITLE")
                    if (title == null) splittedStrings[i] = "";
                    else splittedStrings[i] = title;
            }
            return Reform(String.Join("", splittedStrings));
        }

        /// <summary>
        /// Remove all long spaces and spaces before dots/commas
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Reform(string source)
        {
            source = String.Join(" ", source.Split(' ', StringSplitOptions.RemoveEmptyEntries));
            source = String.Join(".", source.Split(" ."));
            source = String.Join(",", source.Split(" ,"));
            source = String.Join("?", source.Split(" ?"));
            source = String.Join("!", source.Split(" !"));
            source = String.Join(":", source.Split(" :"));
            return source;
        }

        /// <summary>
        /// Check the matching rate of the input message and the keywords
        /// </summary>
        /// <param name="message"> Input message from the chat </param>
        /// <param name="keys"> List of keywords </param>
        /// <returns> Matching rate </returns>

        //public static double MatchingRate(string message, List<string> keys)
        //{
        //    int count = 0;
        //    foreach (string key in keys)
        //    {
        //        if (message.ToUpper().Contains(key.ToUpper()))
        //            count++;
        //    }
        //    return count / keys.Count;
        //}

        public static double MatchingRate(string message, List<string> keys)
        {
            int count = 0;
            foreach (string key in keys)
            {
                string pattern = @$"\b{key.ToUpper()}\b";
                Regex rgx = new Regex(pattern);
                count += rgx.Matches(message.ToUpper()).Count;
            }
            return count / keys.Count;
        }
    }
}
