﻿namespace MasterDrums.Model
{
    /// <summary>
    /// Interface for a class tha will take care of implementing the game mechanics
    /// such as score keeping and bpm increasing in time.
    /// </summary>
    public interface IGame
    {
        /// <summary>
        /// The player name
        /// </summary>
        string PlayerName { get; set; }

        /// <summary>
        /// The bpm at witch the user is currently playing
        /// </summary>
        int Bpm { get; }

        /// <summary>
        /// The score totalized by the user
        /// </summary>
        int Score { get; }

        /// <summary>
        /// The user has hit a note, the score is augmented based on bpm and note type.
        /// </summary>
        /// <param name="note">The note that has been hitted</param>
        /// <param name="deltaT">The delay in ms from the perfect hit spot</param>
        void Hit(INote note, double deltaT);

        /// <summary>
        /// Called when an empty hit is performed
        /// </summary>
        void Hit();

        /// <summary>
        /// Saves the score of the user in the .csv file
        /// </summary>
        void SerializeScore();

        /// <summary>
        /// Ms after which a note is considered as wasted
        /// </summary>
        int NoteWastedMs
        {
            get;
        }

        /// <summary>
        /// Wrong hits remaining until the game ends
        /// </summary>
        int WrongHitsRemaining
        {
            get;
        }
    }
}
