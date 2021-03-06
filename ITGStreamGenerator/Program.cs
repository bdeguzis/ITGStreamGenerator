﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;//ITG Auto-generator for streams
//Left off trying to figure out how to stop the generation of long uncomfortable patterns like 8-notes being repeated over and over, and long anchors 
//For the bad pattern checker, the "fourth" note is the newest note in the pattern and the "first" is the oldest
//tests

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ITGStreamGenerator
{
    class Program
    {

        static string randomPattern(int rn, bool first)
        {
            string rand = "0000";
            if (first) //Ensures first note is on left or right
            {
                if (rn == 1 || rn == 2)
                    rand = "1000";
                else
                    rand = "0001";
            }
            else
            {
                if (rn == 1)
                    rand = "1000";
                if (rn == 2)
                    rand = "0100";
                if (rn == 3)
                    rand = "0010";
                if (rn == 4)
                    rand = "0001";
            }
            return rand;
        }


        static bool isBad(List<string> currentPatt, string fourth, string foot)
        {
            string first = currentPatt[0];
            string second = currentPatt[1];
            string third = currentPatt[2];
            /*Template
            if (first == "000" && second == "000" && third == "000" && fourth == "000")
            return true;
            */
            //Jacks
            if ((first == "1000" && second == "1000")
                || (first == "0100" && second == "0100")
                || (first == "0010" && second == "0010")
                || (first == "0001" && second == "0001"))
                return true;
            if ((second == "1000" && third == "1000")
                || (second == "0100" && third == "0100")
                || (second == "0010" && third == "0010")
                || (second == "0001" && third == "0001"))
                return true;
            if ((third == "1000" && fourth == "1000")
                || (third == "0100" && fourth == "0100")
                || (third == "0010" && fourth == "0010")
                || (third == "0001" && fourth == "0001"))
                return true;

            //Crossovers
            if (second == "1000" && third == "0100" && fourth == "0001")
                return true;
            if (second == "1000" && third == "0010" && fourth == "0001")
                return true;
            if (second == "0001" && third == "0010" && fourth == "1000")
                return true;
            if (second == "0001" && third == "0100" && fourth == "1000")
                return true;
            if (first == "1000" && second == "0100" && third == "0010" && fourth == "1000")
                return true;
            if (first == "0001" && second == "0010" && third == "0100" && fourth == "0001")
                return true;
            if (foot == "right")
            {
                if (first == "0010" && second == "0100" && third == "0010" && fourth == "1000")
                    return true;
                if (first == "1000" && second == "0010" && third == "0100" && fourth == "1000")
                    return true;
                if (first == "0100" && second == "0010" && third == "0100" && fourth == "1000")
                    return true;

            }
            if (foot == "left")
            {
                if (first == "0100" && second == "0010" && third == "0100" && fourth == "0001")
                    return true;
                if (first == "0001" && second == "0100" && third == "0010" && fourth == "0001")
                    return true;
                if (first == "0010" && second == "0100" && third == "0010" && fourth == "0001")
                    return true;
            }


            //Towers
            if (first == "1000" && second == "0100" && third == "1000" && fourth == "0100")
                return true;
            else if (first == "0100" && second == "1000" && third == "0100" && fourth == "1000")
                return true;
            else if (first == "1000" && second == "0010" && third == "1000" && fourth == "0010")
                return true;
            else if (first == "0010" && second == "1000" && third == "0010" && fourth == "1000")
                return true;
            else if (first == "1000" && second == "0001" && third == "1000" && fourth == "0001")
                return true;
            else if (first == "0001" && second == "1000" && third == "0001" && fourth == "1000")
                return true;
            else if (first == "0100" && second == "0010" && third == "0100" && fourth == "0010")
                return true;
            else if (first == "0010" && second == "0100" && third == "0010" && fourth == "0100")
                return true;
            else if (first == "0010" && second == "0001" && third == "0010" && fourth == "0001")
                return true;
            else if (first == "0001" && second == "0010" && third == "0001" && fourth == "0010")
                return true;
            else if (first == "0100" && second == "0001" && third == "0100" && fourth == "0001")
                return true;
            else if (first == "0001" && second == "0100" && third == "0001" && fourth == "0100")
                return true;
            return false;
        }
        static void Main(string[] args)
        {
            string path = @"E:\Programming\ITG Autogen\Happy Time\Happy Time.sm";
            List<string> lines = new List<string>();
            List<int> markers = new List<int>();
            Random r = new Random();
            int linecount = 0;
            int changecount = 0;
            using (StreamReader sr = File.OpenText(path))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    lines.Add(s);
                    linecount++;
                    if (s == "1000")
                    {
                        changecount++;
                        markers.Add(1);
                    }
                    else if (s == ",")
                        markers.Add(2);
                    else
                        markers.Add(0);

                }
                sr.Close();
            }

            int totalNoteCount = 0;
            string lastPattern = "0000";
            string nextPattern = "0000";
            bool firstPattern = true;
            string lastFoot = "left";
            string nextFoot = "left";
            //pattern checker variables
            string firstNote = "0000";
            string secondNote = "0000";
            string thirdNote = "0000";
            string fourthNote = "0000";
            int noteCount = 1;
            List<string> currentPattern = new List<string>();
            currentPattern.Add(firstNote);
            currentPattern.Add(secondNote);
            currentPattern.Add(thirdNote);
            currentPattern.Add(fourthNote);
            string currentFoot = "";
            bool badPattern = false;
            for (int i = 0; i < linecount; i++)
            {
                if (markers[i] == 1)
                {
                    if (firstPattern == true)
                    {
                        lastPattern = lines[i];
                    }
                    bool solved = false;
                    while (solved == false)
                    {
                        nextPattern = randomPattern(r.Next(1, 5), firstPattern);
                        if (firstPattern)
                        {
                            firstPattern = false;
                            if (nextPattern == "1000")
                                currentFoot = "left";
                            else
                                currentFoot = "right";
                        }
                        // badPattern = isBad(currentPattern[1], currentPattern[2], currentPattern[3], nextPattern, currentFoot); 
                        badPattern = isBad(currentPattern, nextPattern, currentFoot);
                        if (!badPattern)
                        {

                            lines[i] = nextPattern;
                            lastPattern = lines[i];
                            totalNoteCount++;
                            currentPattern.Add(nextPattern);
                            currentPattern.RemoveAt(0);
                            solved = true;
                            if (currentFoot == "left")
                                currentFoot = "right";
                            else
                                currentFoot = "left";
                        }

                    }

                }
                else if (markers[i] == 0)
                {
                    currentPattern[0] = "0000";
                    currentPattern[1] = "0000";
                    currentPattern[2] = "0000";
                    currentPattern[3] = "0000";
                    firstPattern = true;
                }

                //    Console.WriteLine(lines[i]);
            }
            using (StreamWriter sw = File.CreateText(path))
            {
                foreach (string s in lines)
                    sw.WriteLine(s);
            }


        }
    }
}


namespace ITGStreamGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
