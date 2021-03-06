﻿using MasterDrums.Exception;
using System;
using System.Collections.Generic;
using System.IO;

namespace MasterDrums.Model
{
    /// <summary>
    /// The game class contains the game state informations.
    /// </summary>
    public class Game : IGame
    {
        private string _playerName = null;
        private int _bpm = -1;

        private int _score = 0;
        private int _wastedNotes = 0;
        private int _hittedNotes = 0;

        private List<Tuple<int, String>> _results = new List<Tuple<int, String>>();

        /// <summary>
        /// Creates the game instance, sets the initial bpm and loads the records internally
        /// </summary>
        /// <param name="initialBpm">The initial bpm</param>
        public Game(int initialBpm) : base()
        {
            this._results = LoadBestResults();
            this._bpm = initialBpm;
        }

        /// <summary>
        /// Sets the player's name.
        /// null is used if the player name is an empty string
        /// </summary>
        public string PlayerName {
            get => this._playerName;
            set => this._playerName = string.IsNullOrWhiteSpace(value) ? null : value;
        }

        /// <summary>
        /// The BPM at which the user is playing
        /// </summary>
        public int Bpm => this._bpm;

        /// <summary>
        /// The user score
        /// </summary>
        public int Score => this._score;

        /// <summary>
        /// A note is considered as wasted if it's perfomed 50ms before or after it would naturally occur
        /// </summary>
        public int NoteWastedMs
        {
            get => 50;
        }

        /// <summary>
        /// Method called when a note has been hit.
        /// Every 5 notes hitted the bpm is increased by 1,
        /// Every 2ms of delay from the perfect hit represents a 1 point penalty
        /// </summary>
        /// <param name="note">The note hitted</param>
        /// <param name="deltaT">The distance in time from the perfect hit time</param>
        public void Hit(INote note, double deltaT)
        {
            // every 2ms a 1 point penalty is added 
            int penalty = (int)Math.Round(deltaT / 2.0);
            this._score += (note.HitPoint - penalty);

            this._hittedNotes++;
            if ((this._hittedNotes % 5) == 0)
                this._bpm++;
        }

        /// <summary>
        /// Called when an empty hit has been performed
        /// </summary>
        public void Hit()
        {
            this._wastedNotes++;

            if (this._wastedNotes >= 20)
                throw new GameEndedException();
        }

        /// <summary>
        /// Serialize the user score to a file
        /// </summary>
        public void SerializeScore()
        {
            this._results = Game.LoadBestResults();
            
            Tuple<int, String> t = new Tuple<int, String>(this._score, this._playerName);
            if (this._score != 0)
            {
                this.AddAndOrderResult(t);
                this.AddResultToFile();
            }
            
        }

        /// <summary>
        /// Wrong hits remaining until the game ends
        /// </summary>
        /// <returns>The number of wrong hits that can be executed</returns>
        public int WrongHitsRemaining
        {
            get => (20 - this._wastedNotes);
        }

        /// <summary>
        /// Loads the highscores from the file containg them
        /// </summary>
        /// <returns>A list of record in the format score - name</returns>
        public static List<Tuple<int, String>> LoadBestResults()
        {
            try
            {
                List<Tuple<int, String>> results = new List<Tuple<int, String>>();
                StreamReader sr = new StreamReader("../../record.csv");
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    string[] values = line.Split(';');
                    if (!string.IsNullOrEmpty(line))
                    {
                        results.Add(new Tuple<int, String>(int.Parse(values[1]), values[0]));
                    }
                }

                results.Sort();
                results.Reverse();
                sr.Close();
                return results;
            }
            catch(FileNotFoundException)
            {
                return null;
            }
        } 

        /// <summary>
        /// Add a new score to the internal list
        /// </summary>
        /// <param name="t">The new result</param>
        private void AddAndOrderResult(Tuple<int, String> t)
        {
            if (this._results != null)
            {
                this._results.Add(t);
                this._results.Sort();
                this._results.Reverse();
            }
            else
            {
                this._results = new List<Tuple<int, String>>();
                this._results.Add(t);
            }
        }

        /// <summary>
        /// Write results to csv file
        /// </summary>
        private void AddResultToFile()
        {
            StreamWriter sw = new StreamWriter("../../record.csv", false);
            foreach (Tuple<int, String> t in this._results)
            {
                sw.WriteLine(t.Item2 + ";" + t.Item1);
            }
            sw.Close();
        }
    }
}


