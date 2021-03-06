﻿using System.Drawing;

namespace MasterDrums.Model
{
    /// <summary>
    /// Class that represents a pause note 
    /// </summary>
    class PauseNote : INote
    {
        public PauseNote(notePosition pos)
        {
            this._position = pos;
        }

        public override Image Image => null;

        /// <summary>
        /// The pause note cannot be hit and therefore has 0 point if theoretically hit
        /// </summary>
        public override int HitPoint => 0;
    }
}
